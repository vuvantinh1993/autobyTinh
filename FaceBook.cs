using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace autohana
{
    public partial class FaceBook
    {
        private string _urlhomeFb { get => "https://www.facebook.com/"; }
        private string _urlhomembasicFb { get => "https://mbasic.facebook.com/"; }
        private string _urlLogin { get => "https://facebook.com/login"; }
        private string _urlhomeMFb { get => "https://m.facebook.com/home.php"; }
        private DataGridView _dgvAccounts;
        private int _rowIndex;
        private static int _delayFrom;
        private static int _delayTo;
        private static bool _chiBackupnguoimoi = true;

        public FaceBook(DataGridView dgvAccounts, int rowIndex)
        {
            _dgvAccounts = dgvAccounts;
            _rowIndex = rowIndex;
        }

        #region trả về 1 số url Facebook
        private string UrlInfomationMFa(string urlProfile, string Uid)
        {
            return $"{urlProfile}/about?lst={Uid}%3A{Uid}%3A1598723261";
        }
        private string UrlFrienfListMFa(string urlProfile, string Uid)
        {
            return $"{urlProfile}/friends?lst={Uid}%3A{Uid}%3A1598723261";
        }
        private string UrlFriendNewListMfa(string UidProfile)
        {
            return $"https://www.facebook.com/{UidProfile}/friends_recent";
        }
        private string UrlAlbulmImageMFa(string Uid)
        {
            return $"https://m.facebook.com/profile.php?v=photos&lst=100027294830101%3A{Uid}%3A1598778805&id={Uid}";
        }
        private string UrlProfileMFa(string Uid)
        {
            return $"https://m.facebook.com/{Uid}";
        }
        #endregion

        #region Login Facebook
        // trả vể uId
        public string Login(IWebDriver chromeDriver)
        {
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đi đăng nhập Facebook";
            chromeDriver.Url = _urlLogin;
            Task.Delay(1000);
            if (chromeDriver.Url.Contains(_urlhomeMFb))
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập faceBook thành công";
                return GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            }
            else
            {
                if (_dgvAccounts.Rows[_rowIndex].Cells["cookie"].Value.ToString() != null)
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Fb bằng cookie";
                    var uid = LoginWithCookie(_dgvAccounts.Rows[_rowIndex].Cells["cookie"].Value.ToString(), chromeDriver);
                    if (uid != null)
                    {
                        _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập faceBook thành công";
                        var cookie = GetCookieFb(chromeDriver);
                        _dgvAccounts.Rows[_rowIndex].Cells["cookie"].Value = cookie;
                        return cookie.FirstOrDefault(x => x.Name == "c_user").Value;
                    }
                }
                else
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Fb bằng Uid và Pass";
                    var uid = LoginFbWithUidAndPass(_dgvAccounts.Rows[_rowIndex].Cells["id"].Value.ToString(), _dgvAccounts.Rows[_rowIndex].Cells["pass"].Value.ToString(), chromeDriver);
                    if (uid != null)
                    {
                        _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập faceBook thành công";
                        var cookie = GetCookieFb(chromeDriver);
                        _dgvAccounts.Rows[_rowIndex].Cells["cookie"].Value = cookie;
                        return cookie.FirstOrDefault(x => x.Name == "c_user").Value;
                    }
                }
            }
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập faceBook thất bại";
            return null;
        }
        public string LoginWithCookie(string cookies, IWebDriver chromeDriver)
        {
            chromeDriver.Url = _urlhomeMFb;
            cookies = cookies.Replace(" ", "");
            foreach (string item in cookies.Split(';'))
            {
                if (item.Split('=').Count() == 2)
                {
                    chromeDriver.Manage().Cookies.AddCookie(new Cookie(item.Split('=')[0], item.Split('=')[1]));
                }
            }
            chromeDriver.Url = _urlhomeMFb;
            Task.Delay(1000);
            if (chromeDriver.Url.Contains(_urlhomeMFb))
            {
                return "oke";
            }
            else
            {
                return null;
            }
        }
        public ReadOnlyCollection<Cookie> GetCookieFb(IWebDriver chromeDriver)
        {
            var cookie = chromeDriver.Manage().Cookies.AllCookies;
            //Cookie listCookie = cookie.Where(x=>x.sp)
            var str = "";
            foreach (var item in cookie)
            {
                str += item.ToString().Split(';')[0] + ";";
            }
            return cookie;
        }
        // truyên trang home fb
        public string GetUserIdFromHome(IWebDriver chromeDriver)
        {
            try
            {
                var element = chromeDriver.FindElement(By.XPath("//li[@data-type=\"type_user\"]"));
                var userId = element.GetAttribute("data-nav-item-id");
                return userId;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string LoginFbWithUidAndPass(string userId, string pass, IWebDriver chromeDriver)
        {
            chromeDriver.Url = _urlLogin;
            if (chromeDriver.Url != _urlLogin)
            {
                chromeDriver.FindElement(By.Name("email")).SendKeys(userId);
                chromeDriver.FindElement(By.Name("pass")).SendKeys(pass);
                chromeDriver.FindElement(By.Name("login")).Click();
                if (chromeDriver.Url == _urlhomeFb)
                {
                    return "oke";
                }
            }
            return null;
        }
        #endregion

        #region Hana
        public ModelLamJob LamJob(IWebDriver chromeDriver, ref int sotiendalam, ref int solankhonggiaiduoctien)
        {
            var tienjob = "";
            bool check = false;
            string originalWindow = chromeDriver.CurrentWindowHandle;
            bool noidungDanhgiaComment = false;

            if (Common.FindEle("/html/body/div[1]/div/div/div/main/div/div/div[1]/div[1]/div[2]/div/div[2]/div/span[1]", chromeDriver))
            {
                var tenJob = "";
                while (tenJob == "")
                {
                    tenJob = chromeDriver.FindElement(By.XPath("//span[@class='text-uppercase primary--text font-weight-bold hidden-md-and-up body-2']")).Text;
                    try
                    {
                        tienjob = chromeDriver.FindElement(By.XPath("//span[@class='hold-prices']")).Text;
                        if (tenJob.ToUpper().Contains("ĐÁNH GIÁ FANPAGE") || tenJob.ToUpper().Contains("BÌNH LUẬN"))
                        {
                            try
                            {
                                chromeDriver.FindElement(By.XPath("//span[contains(text(), 'copy')]")).Click();
                                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Copy nội dung thành công";
                                noidungDanhgiaComment = true;
                            }
                            catch (Exception)
                            {
                                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Không thể copy được nội dung";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        solankhonggiaiduoctien++;
                    }
                }

                foreach (string window in chromeDriver.WindowHandles)
                {
                    if (originalWindow != window)
                    {
                        chromeDriver.SwitchTo().Window(window);
                        chromeDriver.Close();
                    }
                }
                chromeDriver.SwitchTo().Window(originalWindow);

                if (Common.FindAndClickEle("//span[contains(text(),'Làm việc bằng ứng dụng Facebook trên điện thoại.')]", chromeDriver))
                {
                    Common.DelayMiliSeconde(3000);

                    chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());

                    if (!CheckPageExits(chromeDriver))
                    {
                        _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Page không tồn tại";
                        goto PageKhongTonTai_GOTO;
                    }

                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Làm Job: {tenJob}";
                    if (tenJob.ToUpper().Contains("TĂNG FOLLOW"))
                    {
                        check = MActionFollow(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("TĂNG LIKE FANPAGE"))
                    {
                        check = MActionLikePage(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("TĂNG LIKE BÀI VIẾT"))
                    {
                        check = MActionLikePost(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("THƯƠNG THƯƠNG"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Care);
                    }
                    else if (tenJob.ToUpper().Contains("LOVE"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Love);
                    }
                    else if (tenJob.ToUpper().Contains("HA HA"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Haha);
                    }
                    else if (tenJob.ToUpper().Contains("BUỒN"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Sad);
                    }
                    else if (tenJob.ToUpper().Contains("WOW"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Wow);
                    }
                    else if (tenJob.ToUpper().Contains("GIẬN DỮ"))
                    {
                        check = MActionCamXuc(chromeDriver, ActionFb.Angry);
                    }
                    else if (tenJob.ToUpper().Contains("ĐÁNH GIÁ FANPAGE") && noidungDanhgiaComment)
                    {
                        check = ActionDanhGia(chromeDriver);
                    }
                    if (CheckFBBlockAction(chromeDriver))
                    {
                        return (new ModelLamJob() { isBlockaction = true });
                    }
                    if (CheckFBCheckPoint(chromeDriver))
                    {
                        return (new ModelLamJob() { isCheckpoint = true });
                    }
                PageKhongTonTai_GOTO:
                    chromeDriver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    MessageBox.Show("Không click được làm job basic");
                }
            }
            else
            {
                // lỗi ko thấy có job hoặc trang hana Bảo trì
                return (new ModelLamJob() { });
            }
            if (check == false)
            {
                // báo lỗi
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Đi báo cáo Job lỗi";
                BaoJobLoi(chromeDriver);
                _dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value = ((int)_dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value + 1).ToString();
                _dgvAccounts.Rows[_rowIndex].Cells["error"].Value = ((int)_dgvAccounts.Rows[_rowIndex].Cells["error"].Value + 1);
            }
            else
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Đi báo cáo hoàn thành Job";
                sotiendalam += Convert.ToInt32(tienjob);
                BAoCaoHoanThanhJob(chromeDriver);

                if (chromeDriver.PageSource.Contains("opacity: 0.46; background-color: rgb(33, 33, 33)"))
                {
                    while (chromeDriver.PageSource.Contains("opacity: 0.46; background-color: rgb(33, 33, 33)"))
                    {
                        Common.DelayMiliSeconde(1000);
                    }
                }
                var pageSource = chromeDriver.PageSource;
                if (pageSource.Contains("Đã gửi thông tin lên hệ thống xét duyệt ! Bạn đã thực"))
                {
                    // lấy số Job hoàn thành
                    Regex regex = new Regex(@"Bạn đã thực hiện.*hôm nay");
                    var b = regex.Match(pageSource).Value;
                    Regex regex2 = new Regex(@"[0-9]{1,4}");
                    var jobLamHonNay = regex2.Match(b).Value;
                    _dgvAccounts.Rows[_rowIndex].Cells["total"].Value = jobLamHonNay;
                }
                else if (pageSource.Contains("Đổi gió với Instagram và Tiktok thôi nào"))
                {
                    // làm đủ 100 Job;
                    return (new ModelLamJob() { isFinishTotalJob = true });
                }

                if (chromeDriver.Url.Contains("detail"))
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Báo cáo không thành công, cáo cáo lỗi";
                    _dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value = ((int)_dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value + 1).ToString();
                    if ((int)_dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value > 5)
                    {
                        // loi 5 lan lien tiep
                        return (new ModelLamJob() { isError5Finish = true });
                    }

                    BaoJobLoi(chromeDriver);
                }
                else
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["reWork"].Value = 0;
                    _dgvAccounts.Rows[_rowIndex].Cells["done"].Value = ((int)_dgvAccounts.Rows[_rowIndex].Cells["done"].Value + 1);
                }
            }
            return (new ModelLamJob() { isFinishOneJob = check });
        }
        private void BAoCaoHoanThanhJob(IWebDriver chromeDriver)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,500)");
                chromeDriver.FindElement(By.XPath("//span[contains(text(),'Hoàn thành')]")).Click();
                Common.Delay(1);
            }
            catch (Exception)
            {

            }
        }
        private void BaoJobLoi(IWebDriver chromeDriver)
        {
            try
            {
                chromeDriver.FindElements(By.XPath("//span[contains(text(),'Báo lỗi')]"))[0].Click();
                Common.Delay(1);
                chromeDriver.FindElement(By.XPath("//span[contains(text(), 'Gửi báo cáo ')]")).Click();
                Common.Delay(4);
            }
            catch (Exception)
            {

            }
        }
        #endregion


        private bool CheckPageExits(IWebDriver chromeDriver)
        {
            var pageSource = chromeDriver.PageSource;
            if (pageSource.Contains("Liên kết bạn đã theo dõi có thể đã hết hạn hoặc trang có thể chỉ hiển thị với đối tượng không bao gồm bạn.") || pageSource.Contains("Nguyên nhân có thể là trang tạm thời không hoạt động, liên kết bạn nhấp bị hỏng, hết hạn") || pageSource.Contains("The page you requested cannot be displayed right now") || pageSource.Contains("Không tìm thấy nội dung") || pageSource.Contains("Sorry, this content isn't available right now") || pageSource.Contains("Trang này không khả dụng") || pageSource.Contains("This page isn't available"))
            {
                return false;
            }
            return true;
        }
        private bool CheckFBCheckPoint(IWebDriver chromeDriver)
        {
            if (chromeDriver.Url.Contains("checkpoint"))
            {
                return true;
            }
            return false;
        }
        private bool CheckFBBlockAction(IWebDriver chromeDriver)
        {
            var pageSource = chromeDriver.PageSource;
            if (pageSource.Contains("This action was blocked. Please try again later.") || pageSource.Contains("Thao tác này đã bị chặn. Vui lòng thử lại sau.") || pageSource.Contains("Tạm thời, bạn không thể làm một số việc nhất định trên Facebook."))
            {
                return true;
            }
            return false;
        }
        private void ConvertFBPhoneToWeb(IWebDriver chromeDriver)
        {
            var url = chromeDriver.Url;
            if (url.Contains("://m.facebook.com"))
            {
                var urlnew = url.Replace("//m.", "//www.");
                chromeDriver.Url = urlnew;
            }
            return;
        }

        #region action Facebook Web window
        private bool ActionLikePage(IWebDriver chromeDriver)
        {
            ReadOnlyCollection<IWebElement> webElements;
            try
            {
                chromeDriver.FindElement(By.XPath("//span[contains(.,'Đã thích')]")); // cu
            }
            catch (Exception)
            {
                try
                {
                    chromeDriver.FindElement(By.XPath("//span[contains(.,'Liked')]")); // tieng Anh cu
                }
                catch (Exception)
                {
                    try
                    {
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Đã thích']")); // web Moi
                    }
                    catch (Exception)
                    {
                        webElements = Common.WaitGetElement(chromeDriver, By.XPath("//button[contains(text(),'Thích')]"), 2); // cu
                        if (webElements != null)
                        {
                            ChoClickButtonFB();
                            webElements[0].Click();
                            AlertLikePage(chromeDriver);
                            return true;
                        }
                        else
                        {
                            webElements = Common.WaitGetElement(chromeDriver, By.XPath("//button[contains(text(),'Like')]"), 2); // TA cu
                            if (webElements != null)
                            {
                                ChoClickButtonFB();
                                webElements[0].Click();
                                // AlertLikePage(chromeDriver); // phai viet bang tieng anh
                                return true;
                            }
                            else
                            {
                                webElements = Common.WaitGetElement(chromeDriver, By.XPath("//div[@aria-label='Thích']"), 2); // Moi
                                if (webElements != null)
                                {
                                    ChoClickButtonFB();
                                    webElements[0].Click();
                                    AlertLikePage(chromeDriver);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool ActionFollow(IWebDriver chromeDriver)
        {
            try
            {
                chromeDriver.FindElement(By.XPath("//button[contains(.,'Đang theo dõi')]"));
            }
            catch (Exception)
            {
                try
                {
                    chromeDriver.FindElement(By.XPath("//button[contains(.,'Following')]"));
                }
                catch (Exception)
                {
                    try
                    {// giao dien moi
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Bỏ theo dõi']"));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[contains(text(),'Theo dõi')]"), 2);
                            if (webElements != null)
                            {
                                ChoClickButtonFB();
                                webElements[0].Click();
                                return true;
                            }
                            else
                            {
                                webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[contains(text(),'Follow')]"), 2);
                                if (webElements != null)
                                {
                                    ChoClickButtonFB();
                                    webElements[0].Click();
                                    return true;
                                }
                                else
                                {
                                    try
                                    { // giao dien moi
                                        ChoClickButtonFB();
                                        chromeDriver.FindElement(By.XPath("//img[contains(@src, 'oABzID6cE5f.png')]")).Click();
                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return false;
        }
        private bool ActionLikePost(IWebDriver chromeDriver)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,300)");
            }
            catch (Exception e)
            {
                GhiFile.Write("123", $"Javascrip----------- {e.ToString()}");
            }

            ReadOnlyCollection<IWebElement> webElements;
            try
            {
                chromeDriver.FindElement(By.XPath("//div[contains(@aria-label,'Thay đổi cảm xúc')]")); // cu và mới
            }
            catch (Exception)
            {
                try
                {
                    chromeDriver.FindElement(By.XPath("//div[contains(@aria-label,'React')]")); // TA cu
                }
                catch (Exception)
                {
                    webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[@aria-label='Thích' and contains(@class,'_18vj')]"), 2); // lam job cu
                    if (webElements != null)
                    {
                        ChoClickButtonFB();
                        webElements[webElements.Count() - 1].Click();
                        return true;
                    }
                    else
                    {
                        webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[@aria-label='Like' and contains(@class,'_18vj')]"), 2); // lam TA job cu
                        if (webElements != null)
                        {
                            ChoClickButtonFB();
                            webElements[webElements.Count() - 1].Click();
                            return true;
                        }
                        else
                        {
                            webElements = Common.WaitGetElement(chromeDriver, By.XPath("//span[contains(@class,'_18vi')]"), 2); // lam job mơi
                            if (webElements != null)
                            {
                                ChoClickButtonFB();
                                webElements[0].Click();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool ActionCamXuc(IWebDriver chromeDriver, ActionFb actionFb)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,300)");
            }
            catch (Exception e)
            {
                GhiFile.Write("123", $"Javascrip----------- {e.ToString()}");
            }
            ReadOnlyCollection<IWebElement> webElements;
            Actions actionProvider = new Actions(chromeDriver);
            try
            {
                chromeDriver.FindElement(By.XPath("//div[contains(@aria-label,'Thay đổi cảm xúc')]")); // cu vs moi
            }
            catch (Exception)
            {
                try
                {
                    chromeDriver.FindElement(By.XPath("//div[contains(@aria-label,'Change') and contains(@aria-label,'reaction')] ")); // TA cu vs moi
                }
                catch (Exception)
                {
                    webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[@aria-label='Thích' and contains(@class,'_18vj')]"), 2); // lam job cu
                    if (webElements == null)
                    {
                        webElements = Common.WaitGetElement(chromeDriver, By.XPath("//a[@aria-label='Like' and contains(@class,'_18vj')]"), 2); // lam job cu
                        if (webElements == null)
                        {
                            webElements = Common.WaitGetElement(chromeDriver, By.XPath("//span[contains(@class,'_18vi')]"), 2); // lam job moi
                            if (webElements == null)
                            {
                                return false; // vẫn không thấy bão lỗi
                            }
                            else
                            {
                                try
                                {
                                    actionProvider.MoveToElement(webElements[0]).Build().Perform();
                                    Common.DelayMiliSeconde(3000);
                                    switch (actionFb)
                                    {
                                        case ActionFb.Angry:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Phẫn nộ']")).Click();
                                            break;
                                        case ActionFb.Haha:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Haha']")).Click();
                                            break;
                                        case ActionFb.Love:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Yêu thích']")).Click();
                                            break;
                                        case ActionFb.Care:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Thương thương']")).Click();
                                            break;
                                        case ActionFb.Wow:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Wow']")).Click();
                                            break;
                                        case ActionFb.Sad:
                                            chromeDriver.FindElement(By.XPath("//span[@aria-label='Buồn']")).Click();
                                            break;
                                        default:
                                            return false;
                                    }
                                    return true;
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    try
                    {
                        ChoClickButtonFB();
                        actionProvider.MoveToElement(webElements[webElements.Count() - 1]).Build().Perform();
                        Common.DelayMiliSeconde(3000);
                        switch (actionFb)
                        {
                            case ActionFb.Angry:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Phẫn nộ']")).Click();
                                break;
                            case ActionFb.Haha:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Haha']")).Click();
                                break;
                            case ActionFb.Love:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Yêu thích']")).Click();
                                break;
                            case ActionFb.Care:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Thương thương']")).Click();
                                break;
                            case ActionFb.Wow:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Wow']")).Click();
                                break;
                            case ActionFb.Sad:
                                chromeDriver.FindElement(By.XPath("//span[@aria-label='Buồn']")).Click();
                                break;
                            default:
                                return false;
                        }
                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return false;
        }
        private bool ActionDanhGia(IWebDriver chromeDriver)
        {
            try
            {
                chromeDriver.FindElement(By.XPath("//button[contains(@value,'Có')]")).Click();
                Common.DelayMiliSeconde(1000);
                chromeDriver.FindElement(By.XPath("//textarea[contains(@class,'composerInput')]")).SendKeys(System.Windows.Forms.Keys.Control + "v");
                chromeDriver.FindElement(By.XPath("//button[contains(@value,'Đăng')]")).Click();
                ChoClickButtonFB();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public string ActionDangBai(IWebDriver chromeDriver, string content)
        {
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Đi đăng bài Facebook";
            try
            {
                chromeDriver.Url = _urlhomembasicFb;

                ReadOnlyCollection<IWebElement> webElement5 = Common.WaitGetElement(chromeDriver, By.XPath("//textarea[contains(@name,'xc_message')]"), 2);
                if (webElement5 != null)
                {
                    webElement5[0].SendKeys(content);
                }
                try
                {
                    Common.DelayMiliSeconde(1000);
                    var privacy = chromeDriver.FindElement(By.XPath("//input[@name= 'view_privacy']")).GetAttribute("value");
                    if (privacy != "Công khai")
                    {
                        chromeDriver.FindElement(By.XPath("//input[@name= 'view_privacy']")).Click();
                        Common.DelayMiliSeconde(1000);
                        chromeDriver.FindElement(By.XPath("//a[contains(@aria-label,'Công khai')]")).Click();
                        Common.DelayMiliSeconde(1000);
                    }
                }
                catch (Exception)
                {
                }
                Common.WaitGetElement(chromeDriver, By.XPath("//input[contains(@name,'view_post')]"), 2)[0].Click();
                Common.DelayMiliSeconde(1000);
                chromeDriver.Url = _urlhomeFb;
                chromeDriver.FindElement(By.XPath("//div[@data-click = 'profile_icon']")).Click();
                Common.DelayMiliSeconde(1500);
                chromeDriver.FindElement(By.XPath("//span[@class = 'timestampContent']")).Click();
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Url bài đăng Fb là {chromeDriver.Url}";
                return chromeDriver.Url;
            }
            catch (Exception)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Lỗi, Không thể đăng bài viết";
            }
            return null;
        }
        #endregion

        // alert thong báo
        private void AlertLikePage(IWebDriver chromeDriver)
        {
            Common.DelayMiliSeconde(1000);
            try
            {
                if (chromeDriver.FindElement(By.XPath("//div[contains(@class,' _2ien')]")).Text.Contains("Khi thích một Trang, bạn sẽ thấy các cập nhật từ Trang đó trong Bảng tin của mình."))
                {
                    IAlert alert = chromeDriver.SwitchTo().Alert();
                    alert.Accept();
                }
            }
            catch (Exception)
            {
            }
        }


        #region action Facebook Phone
        private bool MActionFollow(IWebDriver chromeDriver)
        {
            if (!chromeDriver.PageSource.Contains("Đang theo dõi"))
            {
                try
                {
                    ChoClickButtonFB("FOLLOW");
                    chromeDriver.FindElement(By.XPath("//a[@aria-label='Khác']")).Click();
                    var like = chromeDriver.FindElement(By.XPath("//a[contains(@href,'subscriptions/add?')]"));
                    like.Click();
                    return true;
                }
                catch (Exception)
                {
                }
            }
            return false;
            // báo lỗi
        }
        private bool MActionLikePage(IWebDriver chromeDriver)
        {
            try
            {
                var lopNgoai = chromeDriver.FindElement(By.XPath("//div[contains(@class,'_a58 _a5t _9_7 _2rgt _1j-f _2rgt')]")); // là Lớp bao ngoài
                try
                {
                    lopNgoai.FindElement(By.XPath("//div[contains(@class,'_5u9t')]")).FindElement(By.XPath("//*[.='Đã thích']"));
                    // câu trên có nghĩa là trang đó chưa thích
                    var like = lopNgoai.FindElement(By.XPath("//*[.='Thích']"));
                    ChoClickButtonFB("LIKE PAGE");
                    like.Click();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        private bool MActionLikePost(IWebDriver chromeDriver)
        {

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,400)");
                var like = chromeDriver.FindElement(By.XPath("//a[@class='_15ko _77li touchable']"));
                ChoClickButtonFB("LIKE POST");
                like.Click();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionCamXuc(IWebDriver chromeDriver, ActionFb actionFb)
        {
            chromeDriver.Url = chromeDriver.Url.Replace("/m.", "/mbasic.");
            Common.DelayMiliSeconde(3000);
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,200)");
                var like = chromeDriver.FindElement(By.XPath("//img[contains(@src,'kq4YlzJKoP0.png')]"));
                ChoClickButtonFB("CẢM XÚC");
                like.Click();
                Common.DelayMiliSeconde(1000);
                switch (actionFb)
                {
                    case ActionFb.Angry:
                        chromeDriver.FindElement(By.XPath("//span[text()='Phẫn nộ']")).Click();
                        break;
                    case ActionFb.Haha:
                        chromeDriver.FindElement(By.XPath("//span[text()='Haha']")).Click();
                        break;
                    case ActionFb.Love:
                        chromeDriver.FindElement(By.XPath("//span[text()='Yêu thích']")).Click();
                        break;
                    case ActionFb.Care:
                        chromeDriver.FindElement(By.XPath("//span[text()='Thương thương']")).Click();
                        break;
                    case ActionFb.Wow:
                        chromeDriver.FindElement(By.XPath("//span[text()='Wow']")).Click();
                        break;
                    case ActionFb.Sad:
                        chromeDriver.FindElement(By.XPath("//span[text()='Buồn']")).Click();
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        #endregion


        private void Nuoi_NhanKetBan(IWebDriver chromeDriver)
        {
            ReadOnlyCollection<IWebElement> webElements;
            chromeDriver.Url = "https://www.facebook.com/";
            try
            {
                chromeDriver.FindElement(By.XPath("//div[contains(@class,'uiToggle _4962 _3nzl _24xk')]")).Click();
                webElements = Common.WaitGetElement(chromeDriver, By.XPath("//button[contains(@aria-label,'Xác nhận lời mời kết bạn của')]"), 2);
                if (webElements != null)
                {
                    webElements[Common.RandomValue(0, webElements.Count() - 1)].Click();
                }
                chromeDriver.FindElement(By.XPath("//div[contains(@class,'uiToggle _4962 _3nzl _24xk')]")).Click();
            }
            catch (Exception)
            {
            }
        }
        private void Nuoi_GuiKetBan(IWebDriver chromeDriver)
        {
            ReadOnlyCollection<IWebElement> webElements;
            chromeDriver.Url = "https://www.facebook.com/";
            try
            {
                chromeDriver.FindElement(By.XPath("//div[contains(@class,'uiToggle _4962 _3nzl _24xk')]")).Click();
                webElements = Common.WaitGetElement(chromeDriver, By.XPath("//button[contains(@aria-label,'Xác nhận lời mời kết bạn của')]"), 2);
                if (webElements != null)
                {
                    webElements[Common.RandomValue(0, webElements.Count() - 1)].Click();
                }
                chromeDriver.FindElement(By.XPath("//div[contains(@class,'uiToggle _4962 _3nzl _24xk')]")).Click();
            }
            catch (Exception)
            {
            }
        }



        #region backUp Facebook
        public void BackUpFacebook(IWebDriver chromeDriver)
        {
            var uidFb = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            BackupThongTinCoBan(chromeDriver, uidFb);
            BackUpAnhBanBe(chromeDriver, uidFb);
            BackUpBaoMat(chromeDriver, uidFb);
        }
        private bool BackupThongTinCoBan(IWebDriver chromeDriver, string uidFb)
        {
            try
            {
                chromeDriver.Url = UrlProfileMFa(uidFb);
                chromeDriver.Url = UrlInfomationMFa(chromeDriver.Url.Replace("?_rdr", ""), uidFb);
                var listContent = chromeDriver.FindElements(By.XPath("//div[@class='_55wo _2xfb _1kk1']"));
                var str = "";
                for (int i = 0; i < 10; i++)
                {
                    str += listContent[i].Text + "</br></br>";
                    var a = listContent[i].Text;
                }
                str = str.Replace("\r\n", "</br>");
                GhiFile.GhiFileBackUpFromString(uidFb, str, "information", TypeFile.Html);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        // vào trang danh sách bạn bè, tìm các tài khoản chưa backup 
        // trả về danh sách UID chưa được backup
        // mặc định nếu file listfriend.txt không có dữ liệu thì backup hết, nếu file có dữ liệu thì backUp những bạn mới
        private List<string> BackUpAnhBanBe(IWebDriver chromeDriver, string uidFb)
        {
            Thread.Sleep(1);
            try
            {
                // Đọc file listfriend trong máy, tìm tới khi hết những người chưa backup
                var listUidBackuped = GhiFile.DocFileDSUid($"BackUp/{uidFb}/listfriend.txt");
                // trả về danh sách uid đã backup
                if (_chiBackupnguoimoi)
                {
                    chromeDriver.Url = UrlFriendNewListMfa(uidFb);
                }
                else
                {
                    chromeDriver.Url = UrlProfileMFa(uidFb);
                    chromeDriver.Url = UrlFrienfListMFa(chromeDriver.Url.Replace("?_rdr", ""), uidFb);
                }
                while (chromeDriver.PageSource.Contains("id=\"m_more_friends\" data-sigil=\"marea\""))
                {
                    ((IJavaScriptExecutor)chromeDriver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                    Common.DelayMiliSeconde(1000);
                }
                var listIdElement = chromeDriver.FindElements(By.XPath("//div[@class='_5d0x right']"));
                // Tìm những Uid chưa BackUp
                var listUid = new List<string>();
                foreach (var item in listIdElement)
                {
                    var dataStore = item.GetAttribute("data-store").ToString();
                    var uid = JsonConvert.DeserializeObject<ModelFriendFb>(dataStore);
                    if (!listUidBackuped.Contains(uid.id))
                    {
                        listUid.Add(uid.id);
                    }
                }
                var listUidNewBachUp = new List<string>();
                foreach (var uidFriendNew in listUid)
                {
                    if (!BackUpImageOneFriend(chromeDriver, uidFb, uidFriendNew))  // tạo file backup Ảnh user đó nếu trả về false thì bị lỗi haowcj fb chặn spam
                    {
                        break;
                    }
                    listUidNewBachUp.Add(uidFriendNew);
                }
                // dảo ngược danh sach Uid sắp xếp theo từ mới tới cũ
                listUidBackuped.Reverse();
                listUidNewBachUp.Reverse();
                listUidBackuped.AddRange(listUidNewBachUp);
                listUidBackuped.Reverse();
                GhiFile.GhiFileBackUpListUid($"BackUp/{uidFb}", "listfriend", TypeFile.Txt, listUidBackuped); // ghi đè uid chưua backup vào file
                GhiFile.GhiFileHTMLMoCheckPoint($"BackUp/{uidFb}/anhbanbe.html", uidFb, listUidNewBachUp);
                return listUidNewBachUp;
            }
            catch (Exception e)
            {
            }
            return null;
        }
        private bool BackUpImageOneFriend(IWebDriver chromeDriver, string uidProfile, string uidOneFriend)
        {
            try
            {
                chromeDriver.Url = UrlAlbulmImageMFa(uidOneFriend);
                var tenBanBe = chromeDriver.FindElement(By.XPath("//a[@data-sigil='MBackNavBarClick']")).Text;
                var listAlbumEles = chromeDriver.FindElements(By.XPath("//div[@class='item _50lb tall acw abb']"));
                var listLinkImg = new List<string>();
                for (int i = 0; i < listAlbumEles.Count() && i < 2; i++)
                {
                    Thread.Sleep(5000);
                    chromeDriver.FindElements(By.XPath("//div[@class='item _50lb tall acw abb']"))[i].Click();
                    Thread.Sleep(5000);
                    listLinkImg.AddRange(chromeDriver.FindElements(By.XPath("//img[@class='_8brl']")).Where((x, index) => index < 7).Select(x => x.GetAttribute("src")).ToList());
                    if (i < listAlbumEles.Count() && i < 2 - 1)
                    {
                        chromeDriver.Navigate().Back();
                    }
                }
                if (chromeDriver.PageSource.Contains("Để bảo vệ cộng đồng khỏi spam, chúng tôi giới hạn tần suất bạn đăng bài, bình luận hoặc làm các việc khác trong khoảng thời gian nhất định."))
                {
                    return false;
                }
                GhiFile.GhiFileBackUpListImageFriends($"BackUp/{uidProfile}/anhbanbe", $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}_{uidOneFriend}_{tenBanBe}", TypeFile.Txt, listLinkImg);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }


        private bool BackUpBaoMat(IWebDriver chromeDriver, string uidFb)
        {
            //chromeDriver.Url = UrlInfomationMFa("", "100027294830101");
            return true;
        }
        #endregion

        public void ChoClickButtonFB(string nameJob = "thao tác")
        {
            var randomTime = (new Random()).Next(_delayFrom, _delayTo);
            while (randomTime > 0)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Click {nameJob} sau {randomTime} giây";
                Thread.Sleep(1000);
                randomTime--;
            }
        }

        public void ChangeValueWithForm(int delayFrom, int delayTo, bool chiBackupnguoimoi)
        {
            _delayFrom = delayFrom;
            _delayTo = delayTo;
            _chiBackupnguoimoi = chiBackupnguoimoi;
        }
    }

    public class ModelFriendFb
    {
        public string id { get; set; }
    }
}
