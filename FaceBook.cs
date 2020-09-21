using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private string _urlmesChuadoc { get => "https://mbasic.facebook.com/messages/?folder=unread"; }
        private string _urlLogin { get => "https://facebook.com/login"; }
        private string _urlhomeMFb { get => "https://m.facebook.com/home.php"; }
        private DataGridView dgvAccounts;
        private int rowIndex;
        private static int _delayFrom;
        private static int _delayTo;
        private static bool _chiBackupnguoimoi = true;
        public int _timeDelayAddFriendFrom = 3;
        public int _timeDelayAddFriendTo = 10;
        public int _timeDelayBackupFrom = 6;
        public int _timeDelayBackupTo = 30;

        public FaceBook(DataGridView dgvAccounts, int rowIndex)
        {
            this.dgvAccounts = dgvAccounts;
            this.rowIndex = rowIndex;
        }

        #region trả về 1 số url Facebook
        private string UrlInfomationMFa(string Uid)
        {
            return $"https://m.facebook.com/profile.php?v=info&lst={Uid}%3A{Uid}%3A1598986297";
        }
        private string UrlFrienfListMFa(string Uid)
        {
            return $"https://m.facebook.com/profile.php?v=friends&lst={Uid}%3A{Uid}%3A1598977239";
            //return $"{urlProfile}/friends?lst={Uid}%3A{Uid}%3A1598723261";
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
            dgvAccounts["status", rowIndex].Value = "Đi đăng nhập Facebook";
            chromeDriver.Url = _urlLogin;
            Task.Delay(1000);
            if (GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user") != null)
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                return GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            }
            else
            {
                if (dgvAccounts.Rows[rowIndex].Cells["cookie"].Value != null)
                {
                    dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng cookie";
                    var uid = LoginWithCookie(dgvAccounts.Rows[rowIndex].Cells["cookie"].Value.ToString(), chromeDriver);
                    if (uid != null)
                    {
                        dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                        return GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
                    }
                }
                else
                {
                    dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                    var uid = LoginFbWithUidAndPass(dgvAccounts.Rows[rowIndex].Cells["id"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString(), chromeDriver);
                    if (uid != null)
                    {
                        dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                        var cookie = GetCookieFb(chromeDriver);
                        dgvAccounts.Rows[rowIndex].Cells["cookie"].Value = cookie.ToString();
                        return cookie.FirstOrDefault(x => x.Name == "c_user").Value;
                    }
                }
            }
            dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
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
            var cookieUid = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user");
            if (cookieUid != null)
            {
                return cookieUid.Value;
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
                                dgvAccounts["status", rowIndex].Value = "Copy nội dung thành công";
                                noidungDanhgiaComment = true;
                            }
                            catch (Exception)
                            {
                                dgvAccounts["status", rowIndex].Value = "Không thể copy được nội dung";
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
                        dgvAccounts["status", rowIndex].Value = $"Page không tồn tại";
                        goto PageKhongTonTai_GOTO;
                    }

                    dgvAccounts["status", rowIndex].Value = $"Làm Job: {tenJob}";
                    if (tenJob.ToUpper().Contains("TĂNG FOLLOW"))
                    {
                        check = MActionJobFollow(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("TĂNG LIKE FANPAGE"))
                    {
                        check = MActionJobLikePage(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("TĂNG LIKE BÀI VIẾT"))
                    {
                        check = MActionJobLikePost(chromeDriver);
                    }
                    else if (tenJob.ToUpper().Contains("THƯƠNG THƯƠNG"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Care);
                    }
                    else if (tenJob.ToUpper().Contains("LOVE"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Love);
                    }
                    else if (tenJob.ToUpper().Contains("HA HA"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Haha);
                    }
                    else if (tenJob.ToUpper().Contains("BUỒN"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Sad);
                    }
                    else if (tenJob.ToUpper().Contains("WOW"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Wow);
                    }
                    else if (tenJob.ToUpper().Contains("GIẬN DỮ"))
                    {
                        check = MActionJobCamXuc(chromeDriver, ActionFb.Angry);
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
                dgvAccounts["status", rowIndex].Value = $"Đi báo cáo Job lỗi";
                BaoJobLoi(chromeDriver);
                dgvAccounts.Rows[rowIndex].Cells["reWork"].Value = ((int)dgvAccounts.Rows[rowIndex].Cells["reWork"].Value + 1).ToString();
                dgvAccounts.Rows[rowIndex].Cells["error"].Value = ((int)dgvAccounts.Rows[rowIndex].Cells["error"].Value + 1);
            }
            else
            {
                dgvAccounts["status", rowIndex].Value = $"Đi báo cáo hoàn thành Job";
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
                    dgvAccounts.Rows[rowIndex].Cells["total"].Value = jobLamHonNay;
                }
                else if (pageSource.Contains("Đổi gió với Instagram và Tiktok thôi nào"))
                {
                    // làm đủ 100 Job;
                    return (new ModelLamJob() { isFinishTotalJob = true });
                }

                if (chromeDriver.Url.Contains("detail"))
                {
                    dgvAccounts["status", rowIndex].Value = $"Báo cáo không thành công, cáo cáo lỗi";
                    dgvAccounts.Rows[rowIndex].Cells["reWork"].Value = ((int)dgvAccounts.Rows[rowIndex].Cells["reWork"].Value + 1).ToString();
                    if ((int)dgvAccounts.Rows[rowIndex].Cells["reWork"].Value > 5)
                    {
                        // loi 5 lan lien tiep
                        return (new ModelLamJob() { isError5Finish = true });
                    }

                    BaoJobLoi(chromeDriver);
                }
                else
                {
                    dgvAccounts.Rows[rowIndex].Cells["reWork"].Value = 0;
                    dgvAccounts.Rows[rowIndex].Cells["done"].Value = ((int)dgvAccounts.Rows[rowIndex].Cells["done"].Value + 1);
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

        #region check thoong tin
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
        #endregion

        #region action lamJob
        private bool MActionJobFollow(IWebDriver chromeDriver)
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
        private bool MActionJobLikePage(IWebDriver chromeDriver)
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
        private bool MActionJobLikePost(IWebDriver chromeDriver)
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
        private bool MActionJobCamXuc(IWebDriver chromeDriver, ActionFb actionFb)
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
        public bool MActionJobComment(IWebDriver chromeDriver)
        {
            var random = Common.RandomValue(3, 7);
            chromeDriver.Url = "https://m.facebook.com/groups/508388105950882?ref=m_notif&notif_t=group_highlights";
            IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
            while (random > 0)
            {
                js.ExecuteScript("window.scrollBy(0,700)");
                random--;
            }
            var listHref = new List<string>();
            try
            {
                ReadOnlyCollection<IWebElement> binhluan;
                try
                {
                    binhluan = chromeDriver.FindElements(By.XPath("//a[text()='Bình luận']"));
                }
                catch
                {
                    js.ExecuteScript("window.scrollBy(0,700)");
                    binhluan = chromeDriver.FindElements(By.XPath("//a[text()='Bình luận']"));
                }
                var a = new List<int>();
                for (int i = 0; i < binhluan.Count; i++)
                {
                    a.Add(i);
                }
                var b = a.RandomList(5);
                foreach (var number in b)
                {
                    listHref.Add(binhluan[number].GetAttribute("href"));
                }
                foreach (var item in listHref)
                {
                    ChoClickButtonFB("BÀI VIẾT MỚI");
                    chromeDriver.Url = item;
                    chromeDriver.FindElement(By.XPath("//a[text()='Bình luận']")).Click();
                    chromeDriver.FindElement(By.XPath("//textarea[@id='composerInput']")).SendKeys("Báo giá mẫu này cho mình với ạ.");
                    chromeDriver.FindElement(By.XPath("//input[@accept='image/*, image/heif, image/heic']")).SendKeys("C:\\Users\\TINHVU\\Desktop\\a.jpg");
                    ChoClickButtonFB("ĐĂNG BÀI");
                    chromeDriver.FindElement(By.XPath("//button[@aria-label='Đăng']")).Click();
                }
            }
            catch (Exception e)
            {
            }
            return false;
            // báo lỗi
        }
        #endregion

        #region tuong tác newfeed
        private void TrithongMinh(int rowIndex, IWebDriver chromeDriver)
        {
            var ramdom = new Random().Next(1, 30);
            int[] luotnewfeed = { 1, 2, 3, 4, 5, 6, 7 };
            int[] like = { 8, 9, 10, 11, 23 };
            int[] camxuc = { 12, 13, 14, 24 };
            int[] xemthongbao = { 14, 16 };
            int[] xemprofilebanbe = { 19, 17, 18 };
            int[] xemgroup = { 20, 21, 22 };

            if (ramdom <= 24)
            {
                if (chromeDriver.Url != "https://m.facebook.com/home.php")
                {
                    chromeDriver.Navigate().Back();
                    if (chromeDriver.Url != "https://m.facebook.com/home.php")
                    {
                        chromeDriver.Url = "https://m.facebook.com/home.php";
                    }
                }
            }
            Thread.Sleep(2000);

            if (luotnewfeed.Contains(ramdom))
            {
                dgvAccounts["status", rowIndex].Value = "Tự động lướt newfeed";
                MActionLuotNewFeed(chromeDriver);
            }
            else if (like.Contains(ramdom))
            {
                dgvAccounts["status", rowIndex].Value = "Tự động Like ngẫu nhiên bài viết trên tường";
                MActionLikePost(chromeDriver);
            }
            else if (camxuc.Contains(ramdom))
            {
                var ramCX = new Random().Next(1, 4);
                dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc ngẫu nhiên bài viết trên tường";
                MActionCamXuc(chromeDriver, (ActionFb)ramCX);
            }
            else if (xemthongbao.Contains(ramdom))
            {
                MCheckThongBao(chromeDriver, rowIndex);
            }
            else if (xemprofilebanbe.Contains(ramdom))
            {
                MActionViewProfile(chromeDriver, rowIndex);
            }
            else if (xemprofilebanbe.Contains(ramdom))
            {
                MActionViewGroup(chromeDriver, rowIndex);
            }
        }
        private bool MActionViewProfile(IWebDriver chromeDriver, int rowIndex)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,400)");
                var profiles = chromeDriver.FindElements(By.XPath("//a[contains(@href,'/groups/')"));
                var index = Common.RandomValue(0, profiles.Count() - 1);
                dgvAccounts["status", rowIndex].Value = $"Tự động xem Profile của {profiles[index].Text}";
                profiles[index].Click();
                Thread.Sleep(2000);
                var i = new Random().Next(1, 5);
                while (i > 0)
                {
                    js.ExecuteScript("window.scrollBy(0,700)");
                    Thread.Sleep(1500);
                    i--;
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionViewGroup(IWebDriver chromeDriver, int rowIndex)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                var profiles = chromeDriver.FindElements(By.XPath("//a[contains(@href,'/groups/') and contains(@href,'C-R')]"));
                var index = Common.RandomValue(0, profiles.Count() - 1);
                dgvAccounts["status", rowIndex].Value = $"Tự động xem group: {profiles[index].Text}";
                profiles[index].Click();
                Thread.Sleep(1500);
                var i = new Random().Next(3, 7);
                while (i > 0)
                {
                    js.ExecuteScript("window.scrollBy(0,700)");
                    Thread.Sleep(1700);
                    i--;
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionLuotNewFeed(IWebDriver chromeDriver)
        {
            try
            {
                var i = new Random().Next(2, 7);
                while (i > 0)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                    js.ExecuteScript("window.scrollBy(0,700)");
                    i--;
                    Thread.Sleep(1500);
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MCheckThongBao(IWebDriver chromeDriver, int rowIndex)
        {
            dgvAccounts["status", rowIndex].Value = "Tự động kiểm tra thông báo";
            try
            {
                var like = chromeDriver.FindElement(By.XPath("//div[@id='notifications_jewel']"));
                like.Click();
                Thread.Sleep(2500);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionLikePost(IWebDriver chromeDriver)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("window.scrollBy(0,400)");
                var likes = chromeDriver.FindElements(By.XPath("//a[@class='_15ko _77li touchable']"));
                var index = Common.RandomValue(0, likes.Count() - 1);
                likes[index].Click();
                Thread.Sleep(2500);
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
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("var a = document.getElementsByClassName('_1ekf'); var ran =Math.floor(Math.random() * a.length); a[1].click();");
                Thread.Sleep(2000);
                switch (actionFb)
                {
                    case ActionFb.Love:
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Yêu thích']")).Click();
                        break;
                    case ActionFb.Care:
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Thương thương']")).Click();
                        break;
                    case ActionFb.Wow:
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Wow']")).Click();
                        break;
                    default:
                        return false;
                }
                Thread.Sleep(1000);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionAddFriend(IWebDriver chromeDriver)
        {
            chromeDriver.Url = "https://m.facebook.com/friends/center/requests";
            var ran = new Random().Next(1, 3);
            try
            {
                for (int i = 0; i < ran; i++)
                {
                    Thread.Sleep(1000);
                    var likes = chromeDriver.FindElements(By.XPath("//button[@value='Xác nhận']"));
                    var index = Common.RandomValue(0, likes.Count() - 1);
                    likes[index].Click();
                    Thread.Sleep(1000);
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionSendFriend(IWebDriver chromeDriver)
        {
            chromeDriver.Url = "https://m.facebook.com/friends/center";
            var ran = new Random().Next(1, 3);
            try
            {
                for (int i = 0; i < ran; i++)
                {
                    Thread.Sleep(1000);
                    var likes = chromeDriver.FindElements(By.XPath("//button[@value='Thêm bạn bè']"));
                    var index = Common.RandomValue(0, likes.Count() - 1);
                    likes[index].Click();
                    Thread.Sleep(1000);
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MCheckMess(IWebDriver chromeDriver)
        {
            var listContain = new List<ModelMess>();
            try
            {
                while (true)
                {
                    chromeDriver.Url = _urlmesChuadoc;
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//h3[@class='bz ca cb']")).Click();

                    var noidungs = chromeDriver.FindElements(By.XPath("//div[@class='bt']"));
                    foreach (var noidung in noidungs)
                    {
                        listContain.Add(new ModelMess
                        {
                            name = noidung.FindElement(By.XPath("//strong[@class='bv']")).Text,
                            link = noidung.FindElement(By.XPath("//a[@class='bu']")).GetAttribute("href"),
                            contain = noidung.FindElement(By.XPath("//span")).Text,
                        });
                    }

                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//textarea[@id='composerInput']")).SendKeys("Chào bạn");
                    chromeDriver.FindElement(By.XPath("//h3[@type='submit']")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//textarea[@id='composerInput']")).SendKeys("Bạn có thể nhắn cho mình vào tài khoản: http....");
                    chromeDriver.FindElement(By.XPath("//h3[@type='submit']")).Click();
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//textarea[@id='composerInput']")).SendKeys("Cảm ơn bạn.");
                    chromeDriver.FindElement(By.XPath("//h3[@type='submit']")).Click();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {
            }
            return true;
            // báo lỗi
        }
        #endregion

        #region backUp Facebook
        private bool BackupThongTinCoBan(IWebDriver chromeDriver, string uidFb)
        {
            dgvAccounts["status", rowIndex].Value = $"BackUp thông tin cơ bản";
            try
            {
                chromeDriver.Url = UrlInfomationMFa(uidFb);
                var listContent = chromeDriver.FindElements(By.XPath("//div[@class='_55wo _2xfb _1kk1']"));
                var str = "";
                for (int i = 0; i < 10; i++)
                {
                    str += listContent[i].Text + "</br></br>";
                    var a = listContent[i].Text;
                }
                str = str.Replace("\r\n", "</br>");
                DocGhiFile.GhiFileBackUpFromString(uidFb, str, "information", TypeFile.Html);
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
            dgvAccounts["status", rowIndex].Value = $"BackUp ảnh bạn bè";
            try
            {
                // Đọc file listfriend trong máy, tìm tới khi hết những người chưa backup
                var listUidBackuped = DocGhiFile.DocFileDSUid($"BackUp/{uidFb}/listfriend.txt");
                // trả về danh sách uid đã backup
                if (_chiBackupnguoimoi)
                {
                    chromeDriver.Url = UrlFriendNewListMfa(uidFb);
                }
                else
                {
                    chromeDriver.Url = UrlFrienfListMFa(uidFb);
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
                var listUidNewBackUp = new List<string>();
                var dem = 0;
                foreach (var uidFriendNew in listUid)
                {
                    if (!BackUpImageOneFriend(chromeDriver, uidFb, uidFriendNew))  // tạo file backup Ảnh user đó nếu trả về false thì bị lỗi haowcj fb chặn spam
                    {
                        break;
                    }
                    listUidNewBackUp.Add(uidFriendNew);
                    dem++;
                    if (listUidNewBackUp.Count() % 5 == 0 || listUid.Count() == dem)
                    {
                        // dảo ngược danh sach Uid sắp xếp theo từ mới tới cũ
                        listUidBackuped.Reverse();
                        listUidNewBackUp.Reverse();
                        listUidBackuped.AddRange(listUidNewBackUp);
                        listUidBackuped.Reverse();
                        DocGhiFile.GhiFileBackUpListUid($"BackUp/{uidFb}", "listfriend", TypeFile.Txt, listUidBackuped); // ghi đè uid chưua backup vào file
                        DocGhiFile.GhiFileHTMLMoCheckPoint($"BackUp/{uidFb}/anhbanbe.html", uidFb, listUidNewBackUp);
                        listUidNewBackUp = new List<string>(); // ghi 5 bản ghi rồi reset về 0
                        dgvAccounts["status", rowIndex].Value = $"BackUp hoàn thành {dem}/{listUid.Count()}";
                    }
                }
                return listUidNewBackUp;
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
                    Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));
                    chromeDriver.FindElements(By.XPath("//div[@class='item _50lb tall acw abb']"))[i].Click();

                    Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));
                    listLinkImg.AddRange(chromeDriver.FindElements(By.XPath("//img[@class='_8brl']")).Where((x, index) => index < 7).Select(x => x.GetAttribute("src")).ToList());
                    if (i < listAlbumEles.Count() && i < 2 - 1)
                    {
                        chromeDriver.Navigate().Back();
                    }
                }
                if (chromeDriver.PageSource.Contains("Để bảo vệ cộng đồng khỏi spam, chúng tôi giới hạn tần suất bạn đăng bài, bình luận hoặc làm các việc khác trong khoảng thời gian nhất định.") || chromeDriver.Url.Contains("checkpoint"))
                {
                    return false;
                }
                // xoá file ảnh đã backup bị trùng
                var listfileDaBackUp = Directory.GetFiles($"BackUp/{uidProfile}/anhbanbe");
                var listfileBackUptrung = listfileDaBackUp.Where(x => x.Contains(uidOneFriend)).ToList();
                foreach (var item in listfileBackUptrung)
                {
                    System.IO.File.Delete(item);
                }
                // end

                DocGhiFile.GhiFileBackUpListImageFriends($"BackUp/{uidProfile}/anhbanbe", $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}_{uidOneFriend}_{tenBanBe}", TypeFile.Txt, listLinkImg);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }

        private bool BackUpBaoMat(IWebDriver chromeDriver, string uidFb)
        {
            dgvAccounts["status", rowIndex].Value = $"BackUp bảo mật";

            var a = "https://m.facebook.com/ntdelegatescreen/?params=%7B%22entry-point%22%3A%22settings%22%7D&path=%2Fcontacts%2Fmanagement%2F";
            chromeDriver.Url = a;
            var list = chromeDriver.FindElements(By.XPath("//div[@style='flex-grow:0;flex-shrink:1;padding:4px 0 4px 12px']"));
            var str = "<p2><b>QUẢN LÝ THÔNG TIN LIÊN HỆ</b></p2></br></br>\n";
            foreach (var item in list)
            {
                str += $"{item.Text} </br>\n";
            }
            str += "</br></br></br><p2><b>BẢO MẬT VÀ ĐĂNG NHẬP</b></p2></br></br>\n";
            chromeDriver.Url = "https://m.facebook.com/settings/security_login/sessions/";
            list = chromeDriver.FindElements(By.XPath("//div[@class='_4n7b c']"));
            foreach (var item in list)
            {
                str += $"{item.Text} </br>\n";
            }
            if (str.Length > 50)
            {
                DocGhiFile.GhiFileBackUpFromString(uidFb, str, "security", TypeFile.Html);
                dgvAccounts["status", rowIndex].Value = $"Hoàn thất BackUp";
            }
            chromeDriver.Quit();
            return true;
        }
        #endregion

        public void ChoClickButtonFB(string nameJob = "thao tác")
        {
            var randomTime = (new Random()).Next(_delayFrom, _delayTo);
            while (randomTime > 0)
            {
                dgvAccounts["status", rowIndex].Value = $"Click {nameJob} sau {randomTime} giây";
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


        #region Menu
        public void OpenFacebook(IWebDriver chromeDriver, int rowIndex)
        {
            string userIdFb = Login(chromeDriver);
            return;
        }
        public void BackUpFacebookAll(IWebDriver chromeDriver, int rowIndex)
        {
            string userIdFb = Login(chromeDriver);
            if (userIdFb == null) { return; }
            var uidFb = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            BackupThongTinCoBan(chromeDriver, uidFb);
            DocGhiFile.CreadFolder($"BackUp/{uidFb}/anhbanbe");
            BackUpAnhBanBe(chromeDriver, uidFb);
            BackUpBaoMat(chromeDriver, uidFb);
        }
        public void BackUpFacebookOnlyImageFriend(IWebDriver chromeDriver, int rowIndex)
        {
            string userIdFb = Login(chromeDriver);
            if (userIdFb == null) { return; }
            var uidFb = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            DocGhiFile.CreadFolder($"BackUp/{uidFb}/anhbanbe");
            BackUpAnhBanBe(chromeDriver, uidFb);
            BackUpBaoMat(chromeDriver, uidFb);
        }
        public void QuetThanhVienGroup(IWebDriver chromeDriver, int rowIndex)
        {
            string userIdFb = Login(chromeDriver);
            if (userIdFb == null) { return; }
            dgvAccounts["status", rowIndex].Value = $"Đi quét group";
            try
            {
                chromeDriver.Url = "https://www.facebook.com/groups/j2team.community/members/";
                for (int i = 0; i < 500; i++)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                    js.ExecuteScript("window.scrollBy(0,800)");
                    Thread.Sleep(500);
                }

                var listContent = chromeDriver.FindElements(By.XPath("//div[contains(@id,'recently_joined_')]"));
                var listUid = new List<string>();
                foreach (var item in listContent)
                {
                    var uid = item.GetAttribute("id").Replace("recently_joined_", "");
                    listUid.Add(uid);
                }
                System.IO.File.WriteAllLines("tinh.txt", listUid);
                return;
            }
            catch (Exception)
            {
            }
            return;
        }
        public void LocNguoiDung(IWebDriver chromeDriver, int rowIndex)
        {
            string userIdFb = Login(chromeDriver);
            if (userIdFb == null) { return; }
            dgvAccounts["status", rowIndex].Value = $"Loc thành viên tương tác thật";
            try
            {
                chromeDriver.Url = "https://www.facebook.com/100003915540430";
                for (int i = 0; i < 5; i++)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                    js.ExecuteScript("window.scrollBy(0,800)");
                    Thread.Sleep(500);
                }
                var listContent = chromeDriver.FindElements(By.XPath("//div[contains(@aria-label,'đã bày tỏ cảm xúc')]"));
                var tong = 0;
                if (listContent.Count() > 5)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        tong += Convert.ToInt32(listContent[i].Text);
                    }
                }
                var trungbinh = tong / 5;
                return;
            }
            catch (Exception)
            {
            }
            return;
        }
        #endregion
    }

    public class ModelFriendFb
    {
        public string id { get; set; }
    }

    public class ModelMess
    {
        public string name { get; set; }
        public string contain { get; set; }
        public string link { get; set; }
    }
}
