using autohana.Otp;
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
using System.Windows.Forms;

namespace autohana
{
    public partial class FaceBook
    {
        private string _urlMLogin { get => "https://m.facebook.com/login.php"; }
        private string _urlLogin { get => "https://facebook.com/login.php"; }
        private string _urlBasicLogin { get => "https://mbasic.facebook.com/login"; }
        private string _urlhomeFb { get => "https://www.facebook.com/"; }
        private string _urlmesChuadoc { get => "https://mbasic.facebook.com/messages/?folder=unread"; }
        private string _urlhomeMFb { get => "https://m.facebook.com/home.php"; }
        private string _urlChangeInfor { get => "https://m.facebook.com/profile/intro/edit/public"; }
        private string _urlChangeMota { get => "https://mbasic.facebook.com/profile/basic/intro/bio"; }
        private DataGridView dgvAccounts;
        private int rowIndex;
        private IWebDriver chromeDriver;
        private static int _delayFrom;
        private static int _delayTo;
        private static bool _chiBackupnguoimoi = true;
        public int _timeDelayAddFriendFrom = 3;
        public int _timeDelayAddFriendTo = 10;
        public int _timeDelayBackupFrom = 10;
        public int _timeDelayBackupTo = 35;

        public FaceBook(DataGridView dgvAccounts, int rowIndex, IWebDriver chromeDriver)
        {
            this.dgvAccounts = dgvAccounts;
            this.rowIndex = rowIndex;
            this.chromeDriver = chromeDriver;
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
        public (bool rs, Modelfb data) DangNhap()
        {
            dgvAccounts["status", rowIndex].Value = "Đi đăng nhập Facebook";
            chromeDriver.Url = _urlLogin;
            if (LaCheckPoint())
            {
                return (false, Modelfb.isCheckpoint);
            }
            if (!LaTrangDangNhap())
            {
                dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                return (true, Modelfb.isLoginOk);
            }
            else
            {
                if (dgvAccounts.Rows[rowIndex].Cells["cookie"].Value != null)
                {
                    dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng cookie";
                    DangNhapVoiCookie(dgvAccounts.Rows[rowIndex].Cells["cookie"].Value.ToString());
                    chromeDriver.Url = _urlLogin;
                    if (LaCheckPoint())
                    {
                        return (false, Modelfb.isCheckpoint);
                    }
                    if (!LaTrangDangNhap())
                    {
                        dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                        return (true, Modelfb.isLoginOk);
                    }
                }

                dgvAccounts["status", rowIndex].Value = "Đăng nhập Fb bằng Uid và Pass";
                DangNhapVoiUidVaPass(dgvAccounts.Rows[rowIndex].Cells["id"].Value.ToString(), dgvAccounts.Rows[rowIndex].Cells["pass"].Value.ToString());
                chromeDriver.Url = _urlLogin;
                if (LaCheckPoint())
                {
                    return (false, Modelfb.isCheckpoint);
                }
                if (!LaTrangDangNhap())
                {
                    dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thành công";
                    var cookie = GetCookieFb(chromeDriver);
                    dgvAccounts.Rows[rowIndex].Cells["cookie"].Value = cookie.ToString();
                    return (true, Modelfb.isLoginOk);
                }
            }
            dgvAccounts["status", rowIndex].Value = "Đăng nhập faceBook thất bại";
            return (false, Modelfb.isloginNotOk);
        }
        private bool LaTrangDangNhap()
        {
            if (chromeDriver.Url.Contains("login"))
            {
                return true;
            }
            return false;
        }
        private bool LaCheckPoint()
        {
            if (chromeDriver.Url.Contains("checkpoint"))
            {
                return true;
            }
            return false;
        }
        public void DangNhapVoiCookie(string cookies)
        {
            cookies = cookies.Replace(" ", "");
            foreach (string item in cookies.Split(';'))
            {
                if (item.Split('=').Count() == 2)
                {
                    chromeDriver.Manage().Cookies.AddCookie(new Cookie(item.Split('=')[0], item.Split('=')[1]));
                }
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
        public string GetUserIdFromCookie()
        {
            return new Regex(@"c_user=\d*").Match(dgvAccounts["cookie", rowIndex].Value.ToString()).Value.Replace("c_user=", "");
        }
        public void DangNhapVoiUidVaPass(string userId, string pass)
        {
            chromeDriver.Url = _urlBasicLogin;
            chromeDriver.FindElement(By.Name("email")).SendKeys(userId);
            chromeDriver.FindElement(By.Name("pass")).SendKeys(pass);
            chromeDriver.FindElement(By.Name("login")).Click();
            // neu co 2Fa thif dang nhap
            try
            {
                if (chromeDriver.PageSource.Contains("Đăng nhập bằng một lần nhấn"))
                {
                    chromeDriver.FindElement(By.XPath("//a[contains(@href,'login')]")).Click();
                }
                var _2fa = dgvAccounts["Fa", rowIndex].Value.ToString();
                if (_2fa != null)
                {
                    var code = _2Fa.Lay2FaFB(_2fa);
                    dgvAccounts["status", rowIndex].Value = $"Nhập mã 2Fa là {code}";
                    Thread.Sleep(1500);
                    chromeDriver.FindElement(By.Name("approvals_code")).SendKeys(code);
                    chromeDriver.FindElement(By.Name("submit[Submit Code]")).Click();
                    chromeDriver.FindElement(By.Name("submit[Continue]")).Click();
                }
            }
            catch (Exception e)
            {

            }

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
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        #endregion

        #region tuong tác newfeed
        public void TrithongMinh(int rowIndex, IWebDriver chromeDriver)
        {
            var ramdom = new Random().Next(1, 28);
            int[] luotnewfeed = { 1, 2, 3, 4, 5, 6, 7, 25, 26, 27, 28 };
            int[] like = { 8, 9, 10, 11, 23, };
            int[] camxuc = { 12, 13, 14, 24 };
            int[] xemthongbao = { 14, 16 };
            int[] xemprofilebanbe = { 19, 17, 18 };
            int[] xemgroup = { 20, 21, 22 };


            if (chromeDriver.Url != "https://m.facebook.com/home.php")
            {
                chromeDriver.Navigate().Back();
                if (chromeDriver.Url != "https://m.facebook.com/home.php")
                {
                    chromeDriver.Url = "https://m.facebook.com/home.php";
                }
            }
            Thread.Sleep(1000);

            if (luotnewfeed.Contains(ramdom))
            {
                MActionLuotNewFeed(chromeDriver);
            }
            else if (like.Contains(ramdom))
            {
                dgvAccounts["status", rowIndex].Value = "Tự động Like ngẫu nhiên bài viết trên tường";
                MActionLikePost(chromeDriver);
            }
            else if (camxuc.Contains(ramdom))
            {
                var ramCX = new Random().Next(0, Enum.GetNames(typeof(ActionNuoiFbEnum)).Length);
                MActionCamXuc(chromeDriver, (ActionNuoiFbEnum)ramCX);
            }
            else if (xemthongbao.Contains(ramdom))
            {
                MCheckThongBao(chromeDriver);
            }
            else if (xemprofilebanbe.Contains(ramdom))
            {
                MActionViewProfile(chromeDriver);
            }
            else if (xemgroup.Contains(ramdom))
            {
                MActionViewGroup(chromeDriver);
            }
        }
        private bool MActionViewProfile(IWebDriver chromeDriver)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                var profiles = chromeDriver.FindElements(By.XPath("//a[contains(@href,'/groups/')"));
                var index = Common.RandomValue(0, profiles.Count() - 1);
                dgvAccounts["status", rowIndex].Value = $"Tự động xem Profile của {profiles[index].Text}";
                profiles[index].Click();
                Thread.Sleep(2000);
                var i = new Random().Next(1, 5);
                while (i > 0)
                {
                    js.ExecuteScript("window.scrollBy(0,700)");
                    Thread.Sleep(1000);
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
        private bool MActionViewGroup(IWebDriver chromeDriver)
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
                    Thread.Sleep(1000);
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
                var j = new Random().Next(0, 4);
                if (j == 0)
                {
                    dgvAccounts["status", rowIndex].Value = "Tự động tải lại newfeed";
                    chromeDriver.Url = _urlhomeMFb;
                }
                dgvAccounts["status", rowIndex].Value = "Tự động lướt newfeed";
                var i = new Random().Next(2, 7);
                while (i > 0)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                    js.ExecuteScript("window.scrollBy(0,700)");
                    i--;
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
        private bool MCheckThongBao(IWebDriver chromeDriver)
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
                dgvAccounts["status", rowIndex].Value = "Tự động like ngẫu nhiên bài viết";
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                var likes = chromeDriver.FindElements(By.XPath("//a[@class='_15ko _77li touchable']"));
                var index = Common.RandomValue(0, likes.Count());
                ((IJavaScriptExecutor)chromeDriver).ExecuteScript("arguments[0].scrollIntoView({block: \"center\",inline: \"center\",behavior: \"smooth\"});", likes[index]);
                Thread.Sleep(2000);
                likes[index].Click();
                Thread.Sleep(500);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionCamXuc(IWebDriver chromeDriver, ActionNuoiFbEnum actionFb)
        {
            try
            {
                dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc ngẫu nhiên";
                var likes = chromeDriver.FindElements(By.ClassName("_1ekf"));
                var index = Common.RandomValue(0, likes.Count());
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                ((IJavaScriptExecutor)chromeDriver).ExecuteScript("arguments[0].scrollIntoView({block: \"center\",inline: \"center\",behavior: \"smooth\"});", likes[index]);
                Thread.Sleep(1000);
                js.ExecuteScript($"var a = document.getElementsByClassName('_1ekf'); a[{index}].click();");
                switch (actionFb)
                {
                    case ActionNuoiFbEnum.Love:
                        dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc LOVE";
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Yêu thích']")).Click();
                        break;
                    case ActionNuoiFbEnum.Care:
                        dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc THƯƠNG THƯƠNG";
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Thương thương']")).Click();
                        break;
                    case ActionNuoiFbEnum.Wow:
                        dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc WOW";
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Wow']")).Click();
                        break;
                    case ActionNuoiFbEnum.Haha:
                        dgvAccounts["status", rowIndex].Value = "Tự động thả cảm xúc HAHA";
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@aria-label='Haha']")).Click();
                        break;
                    default:
                        return false;
                }
                Thread.Sleep(1000);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
            // báo lỗi
        }
        private bool MActionAddFriend(IWebDriver chromeDriver)
        {
            dgvAccounts["status", rowIndex].Value = "Tự động kết bạn";
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
            dgvAccounts["status", rowIndex].Value = "Tự động Gửi lời mời kết bạn";
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
            dgvAccounts["status", rowIndex].Value = "Tự động kiểm tra tin nhắn";
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
                            contain = noidung.FindElement(By.XPath("//span")).Text
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
            catch (Exception)
            {
            }
            return null;
        }
        private bool BackUpImageOneFriend(IWebDriver chromeDriver, string uidProfile, string uidOneFriend)
        {
            try
            {
                var imagesUid = BackUpImageFromUId(chromeDriver, uidOneFriend);
                if (imagesUid.error)
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

                DocGhiFile.GhiFileBackUpListImageFriends($"BackUp/{uidProfile}/anhbanbe", $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}_{uidOneFriend}_{imagesUid.nameUid}", TypeFile.Txt, imagesUid.images);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        private (List<string> images, string nameUid, bool error) BackUpImageFromUId(IWebDriver chromeDriver, string uidFb)
        {
            var listSrc = GetSrcsImgFromUID(chromeDriver, uidFb);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));
            return (listSrc.images, listSrc.nameUid, listSrc.error);
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

        #region get information user
        public ModelProfile getProfileCopy(IWebDriver chromeDriver, string uid)
        {
            try
            {
                var modelprofile = new ModelProfile();

                chromeDriver.Url = $"https://mbasic.facebook.com/{uid}";
                var linkImage = GetLinkAvatarAndBiafromUID(chromeDriver, uid);
                modelprofile.linkAvata = linkImage.linkAvata;
                modelprofile.linkAnhBia = linkImage.linkAnhBia;
                modelprofile.motaBanThan = GetMotaFromAboutAcc(chromeDriver);
                modelprofile.listimage = GetLinksImagefromUID(chromeDriver, uid);

                chromeDriver.Url = $"https://m.facebook.com/profile.php?id={uid}&v=info";
                modelprofile.listCongVIec = GetCongViecFromAboutAcc(chromeDriver);
                modelprofile.listHocVans = GetHocVanFromAboutAcc(chromeDriver);
                modelprofile.listNoiSongs = GetNoiSongFromAboutAcc(chromeDriver);
                modelprofile.gioiTinh = GetGioiTinhFromAboutAcc(chromeDriver);
                return modelprofile;
            }
            catch (Exception e)
            {
            }
            return new ModelProfile();

        }
        private (string linkAvata, string linkAnhBia) GetLinkAvatarAndBiafromUID(IWebDriver chromeDriver, string uid)
        {
            try
            {
                var urlAvata = Guid.NewGuid().ToString();
                var urlAnhbia = Guid.NewGuid().ToString();
                var srcAvata = chromeDriver.FindElement(By.XPath("//img[contains(@alt,'profile picture')]")).GetAttribute("src");
                var srcAnhbia = chromeDriver.FindElement(By.CssSelector("#profile_cover_photo_container img")).GetAttribute("src");
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.DownloadFile($"{srcAvata}", $"img\\{urlAvata}.png");
                    webClient.DownloadFile($"{srcAnhbia}", $"img\\{urlAnhbia}.png");
                }
                Directory.GetCurrentDirectory();
                return ($"{Directory.GetCurrentDirectory()}\\img\\{urlAvata}.png", $"{Directory.GetCurrentDirectory()}\\img\\{urlAnhbia}.png");
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
        private List<string> GetLinksImagefromUID(IWebDriver chromeDriver, string uid)
        {
            try
            {
                var listsrc = GetSrcsImgFromUID(chromeDriver, uid);
                var listLinks = ConvertSrcToLink(listsrc.images);
                return listLinks;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetGioiTinhFromAboutAcc(IWebDriver chromeDriver)
        {
            try
            {
                return chromeDriver.FindElement(By.XPath("//div[text()='Nam' or text()='Nữ']")).Text;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private List<ModelNoiSong> GetNoiSongFromAboutAcc(IWebDriver chromeDriver)
        {
            var listNoiSong = new List<ModelNoiSong>();
            try
            {
                var noisongs = chromeDriver.FindElements(By.XPath("//div[@class='_4g34 _5b6q _5b6p _5i2i _52we']"));
                for (int i = 0; i < noisongs.Count(); i++)
                {
                    var diachiAndType = chromeDriver.FindElements(By.XPath("//div[@class='_4g34 _5b6q _5b6p _5i2i _52we']"))[i].Text;
                    var diachi = diachiAndType.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var loai = diachiAndType.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var loaidiachi = loai == "Tỉnh/Thành phố hiện tại" ? TypeDiaChi.ThanhPhoHienTai : TypeDiaChi.QueQuan;
                    listNoiSong.Add(new ModelNoiSong { name = diachi, type = loaidiachi });
                }
            }
            catch (Exception)
            {
            }
            return listNoiSong;
        }
        private List<ModelHocVan> GetHocVanFromAboutAcc(IWebDriver chromeDriver)
        {
            var listHocVans = new List<ModelHocVan>();
            try
            {
                var hocvans = chromeDriver.FindElements(By.CssSelector("#education ._5cds._2lcw"));
                for (int j = 0; j < hocvans.Count(); j++)
                {
                    var onehocvan = new ModelHocVan();
                    var capbac = chromeDriver.FindElements(By.CssSelector("#education ._52jc._52ja"))[j].Text;
                    if (capbac == "Trường trung học")
                    {
                        onehocvan.type = TypeHocVan.trunghoc;
                    }
                    else
                    {
                        onehocvan.type = TypeHocVan.daihoc;
                    }
                    onehocvan.isToNow = false;
                    onehocvan.name = chromeDriver.FindElements(By.CssSelector("#education ._52jd._52jb._3-8_"))[j].Text;
                    try
                    {
                        var time = chromeDriver.FindElements(By.CssSelector("#education ._52jc._52j9"))[j].Text;
                        var times = time.Split('-');
                        if (times.Count() == 2)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                var ngay = 1;
                                var thang = 1;
                                var nam = 1970;
                                var regex = new Regex(@"\d{1,}").Matches(times[i]);
                                if (regex.Count == 3)
                                {
                                    ngay = Convert.ToInt32(regex[0].Value);
                                    thang = Convert.ToInt32(regex[1].Value);
                                    nam = Convert.ToInt32(regex[2].Value);
                                }
                                else if (regex.Count == 2)
                                {
                                    thang = Convert.ToInt32(regex[0].Value);
                                    nam = Convert.ToInt32(regex[1].Value);
                                }
                                else if (regex.Count == 1)
                                {
                                    nam = Convert.ToInt32(regex[0].Value);
                                }
                                if (i == 0)
                                {
                                    onehocvan.timeFrom = new DateTime(nam, thang, ngay);
                                }
                                else
                                {
                                    onehocvan.timeTo = new DateTime(nam, thang, ngay);
                                }
                            }

                        }
                        if (time.Contains("Hiện tại"))
                        {
                            onehocvan.isToNow = true;
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    listHocVans.Add(onehocvan);
                }
            }
            catch (Exception e)
            {
            }
            return listHocVans;
        }
        private List<ModelCongViec> GetCongViecFromAboutAcc(IWebDriver chromeDriver)
        {
            var listHocVans = new List<ModelCongViec>();
            try
            {
                var hocvans = chromeDriver.FindElements(By.CssSelector("#work ._5cds._2lcw"));
                for (int j = 0; j < hocvans.Count(); j++)
                {
                    var onehocvan = new ModelCongViec();
                    onehocvan.isToNow = false;
                    onehocvan.name = chromeDriver.FindElements(By.CssSelector("#work ._52jd._52jb._3-8_"))[j].Text;
                    try
                    {
                        var time = chromeDriver.FindElements(By.CssSelector("#work ._5cds._2lcw"))[j].FindElement(By.CssSelector("._52jc._52j9")).Text;
                        var times = time.Split('-');
                        if (times.Count() == 2)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                var ngay = 1;
                                var thang = 1;
                                var nam = 1970;
                                var regex = new Regex(@"\d{1,}").Matches(times[i]);
                                if (regex.Count == 3)
                                {
                                    ngay = Convert.ToInt32(regex[0].Value);
                                    thang = Convert.ToInt32(regex[1].Value);
                                    nam = Convert.ToInt32(regex[2].Value);
                                }
                                else if (regex.Count == 2)
                                {
                                    thang = Convert.ToInt32(regex[0].Value);
                                    nam = Convert.ToInt32(regex[1].Value);
                                }
                                else if (regex.Count == 1)
                                {
                                    nam = Convert.ToInt32(regex[0].Value);
                                }
                                if (i == 0)
                                {
                                    onehocvan.timeFrom = new DateTime(nam, thang, ngay);
                                }
                                else
                                {
                                    onehocvan.timeTo = new DateTime(nam, thang, ngay);
                                }
                            }

                        }
                        if (time.Contains("Hiện tại"))
                        {
                            onehocvan.isToNow = true;
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    listHocVans.Add(onehocvan);
                }
            }
            catch (Exception e)
            {
            }
            return listHocVans;
        }
        private string GetMotaFromAboutAcc(IWebDriver chromeDriver)
        {
            try
            {
                return chromeDriver.FindElement(By.CssSelector(".bv.bw.bx>div:nth-child(2)")).Text;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private List<string> ConvertSrcToLink(List<string> srcimgs)
        {
            var listLinks = new List<string>();
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                foreach (var srcimg in srcimgs)
                {
                    var link = Guid.NewGuid().ToString();
                    webClient.DownloadFile($"{srcimg}", $"img\\{link}.png");
                    listLinks.Add($"{Directory.GetCurrentDirectory()}\\img\\{link}.png");
                }
            }
            return listLinks;
        }
        private (List<string> images, string nameUid, bool error) GetSrcsImgFromUID(IWebDriver chromeDriver, string uidFb)
        {
            chromeDriver.Url = UrlAlbulmImageMFa(uidFb);
            var tenBanBe = chromeDriver.FindElement(By.XPath("//a[@data-sigil='MBackNavBarClick']")).Text;
            var listAlbumEles = chromeDriver.FindElements(By.XPath("//div[@class='item _50lb tall acw abb']"));
            var listLinkImg = new List<string>();
            for (int i = 0; i < listAlbumEles.Count() && i < 2; i++)
            {
                chromeDriver.FindElements(By.XPath("//div[@class='item _50lb tall acw abb']"))[i].Click();

                listLinkImg.AddRange(chromeDriver.FindElements(By.XPath("//img[@class='_8brl']")).Where((x, index) => index < 7).Select(x => x.GetAttribute("src")).ToList());
                if (i < listAlbumEles.Count() && i < 2 - 1)
                {
                    chromeDriver.Navigate().Back();
                }
            }
            if (chromeDriver.PageSource.Contains("Để bảo vệ cộng đồng khỏi spam, chúng tôi giới hạn tần suất bạn đăng bài, bình luận hoặc làm các việc khác trong khoảng thời gian nhất định.") || chromeDriver.Url.Contains("checkpoint"))
            {
                return (null, null, true);
            }
            return (listLinkImg, tenBanBe, false);
        }
        #endregion

        #region changeinfor
        public bool ChangeThongtinFollowUid(IWebDriver chromeDriver, string uid)
        {
            var dataProfileCopy = getProfileCopy(chromeDriver, uid);

            ChangeAvartar(chromeDriver, dataProfileCopy.linkAvata);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            ChangeAnhBia(chromeDriver, dataProfileCopy.linkAnhBia);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            ChangeMoTaBanThan(chromeDriver, dataProfileCopy.motaBanThan);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            ChangeGioiTinh(chromeDriver, dataProfileCopy.gioiTinh);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            AddThanhPhoHienTaiVaQueQuan(chromeDriver, dataProfileCopy.listNoiSongs);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            AddCongViec(chromeDriver, dataProfileCopy.listCongVIec);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            AddHocVans(chromeDriver, dataProfileCopy.listHocVans);
            Common.Delay((new Random()).Next(_timeDelayBackupFrom, _timeDelayBackupTo));

            return true;
        }
        private bool ChangeAvartar(IWebDriver chromeDriver, string urlimg)
        {
            chromeDriver.Url = _urlChangeInfor;
            try
            {
                chromeDriver.FindElement(By.XPath("//img[@id='u_0_7']")).Click();
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//input[@id='nuxPicFileInput']")).SendKeys(urlimg);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[@value='Đặt làm ảnh đại diện']")).Click();
            }
            catch (Exception e)
            {

            }
            return true;
        }
        private bool ChangeAnhBia(IWebDriver chromeDriver, string urlimg)
        {
            chromeDriver.Url = _urlChangeInfor;
            try
            {
                chromeDriver.FindElement(By.XPath("//img[@id='u_0_j']")).Click();
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//h1[contains(., 'Tải ảnh lên')]/parent::*/parent::*/parent::*")).Click();
                //chromeDriver.FindElement(By.XPath("//i[@class='_4q9b img sp_gOzSA1_YjT5_3x sx_38276f']")).Click();
                chromeDriver.FindElement(By.XPath("//input[@id='nuxPicFileInput']")).SendKeys(urlimg);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[@value='Đặt làm ảnh bìa']")).Click();
            }
            catch (Exception)
            {

            }
            return true;
        }
        private bool ChangeMoTaBanThan(IWebDriver chromeDriver, string urlimg)
        {
            chromeDriver.Url = _urlChangeMota;
            try
            {
                chromeDriver.FindElement(By.XPath("//textarea[@name='bio']")).Clear();
                chromeDriver.FindElement(By.XPath("//textarea[@name='bio']")).SendKeys(urlimg);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//input[@value='Lưu']")).Click();
            }
            catch (Exception)
            {

            }
            return true;
        }
        private bool ChangeGioiTinh(IWebDriver chromeDriver, string gender)
        {
            chromeDriver.Url = "https://m.facebook.com/profile/edit/infotab/section/forms/?section=basic-info";
            try
            {
                if (gender == "Nam")
                {
                    chromeDriver.FindElement(By.XPath("//span[text()='Nam']")).Click();
                }
                else
                {
                    chromeDriver.FindElement(By.XPath("//span[text()='Nữ']")).Click();
                }
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
            }
            chromeDriver.FindElement(By.XPath("//button[@name='save']")).Click();
            return true;
        }

        private bool AddHocVans(IWebDriver chromeDriver, List<ModelHocVan> hocVans)
        {
            foreach (var hocVan in hocVans)
            {
                if (hocVan.type == TypeHocVan.daihoc)
                {
                    AddHocVanDaiHoc(chromeDriver, hocVan);
                }
                else
                {
                    AddHocVanTrungHoc(chromeDriver, hocVan);
                }
            }
            return true;
        }
        private bool AddHocVanDaiHoc(IWebDriver chromeDriver, ModelHocVan hocvan)
        {
            chromeDriver.Url = "https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=education&experience_type=2004";
            try
            {
                chromeDriver.FindElement(By.XPath("//input[@name='college_school_text']")).Clear();
                chromeDriver.FindElement(By.XPath("//input[@name='college_school_text']")).SendKeys(hocvan.name);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//div[@id='u_0_5']")).Click();
                var id = chromeDriver.FindElement(By.XPath("//div[@data-sigil=' jx-result']")).GetAttribute("rel");
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript($"document.getElementById('u_0_2').setAttribute('value', '{id}')");
                if (hocvan.timeFrom != null)
                {
                    SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                    dropdown.SelectByText(hocvan.timeFrom?.Year.ToString());
                    dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_month']")));
                    dropdown.SelectByText($"Tháng {hocvan.timeFrom?.Month.ToString()}");
                    dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_day']")));
                    dropdown.SelectByText($"{hocvan.timeFrom?.Day.ToString()}");
                }
                else
                {
                    SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                    dropdown.SelectByText("Năm");
                }
                if (hocvan.isToNow == true)
                {
                    var checkbox = chromeDriver.FindElement(By.XPath("//input[@name='graduated']"));
                    if (!checkbox.Selected)
                        chromeDriver.FindElement(By.XPath("//span[text()='Đã tốt nghiệp']")).Click();
                }
                else
                {
                    if (hocvan.timeTo != null)
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                        dropdown.SelectByText(hocvan.timeTo?.Year.ToString());
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_month']")));
                        dropdown.SelectByText($"Tháng {hocvan.timeTo?.Month.ToString()}");
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_day']")));
                        dropdown.SelectByText($"{hocvan.timeTo?.Day.ToString()}");
                    }
                    else
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                        dropdown.SelectByText("Năm");
                    }
                }
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[@name='save']")).Click();
                Thread.Sleep(1500);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private bool AddHocVanTrungHoc(IWebDriver chromeDriver, ModelHocVan hocvan)
        {
            chromeDriver.Url = "https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=education&experience_type=2003";
            try
            {
                chromeDriver.FindElement(By.XPath("//input[@name='hs_school_text']")).Clear();
                chromeDriver.FindElement(By.XPath("//input[@name='hs_school_text']")).SendKeys(hocvan.name);
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//div[@id='u_0_h']")).Click();
                var id = chromeDriver.FindElement(By.XPath("//div[@data-sigil=' jx-result']")).GetAttribute("rel");
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript($"document.getElementById('u_0_2').setAttribute('value', '{id}')");
                if (!chromeDriver.FindElement(By.XPath("//input[@name='junk']")).Selected)
                {
                    // đây ko phải địa điem thực tế
                    chromeDriver.FindElement(By.XPath("//span[@class='_2pir _592p _3fc5']")).Click();
                }

                if (hocvan.timeFrom != null)
                {
                    SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                    dropdown.SelectByText(hocvan.timeFrom?.Year.ToString());
                    dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_month']")));
                    dropdown.SelectByText($"Tháng {hocvan.timeFrom?.Month.ToString()}");
                    dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_day']")));
                    dropdown.SelectByText($"{hocvan.timeFrom?.Day.ToString()}");
                }
                else
                {
                    SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                    dropdown.SelectByText("Năm");
                }
                if (hocvan.isToNow == true)
                {
                    var checkbox = chromeDriver.FindElement(By.XPath("//input[@name='graduated']"));
                    if (!checkbox.Selected)
                        chromeDriver.FindElement(By.XPath("//span[text()='Đã tốt nghiệp']")).Click();
                }
                else
                {
                    if (hocvan.timeTo != null)
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                        dropdown.SelectByText(hocvan.timeTo?.Year.ToString());
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_month']")));
                        dropdown.SelectByText($"Tháng {hocvan.timeTo?.Month.ToString()}");
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_day']")));
                        dropdown.SelectByText($"{hocvan.timeTo?.Day.ToString()}");
                    }
                    else
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                        dropdown.SelectByText("Năm");
                    }
                }
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[@name='save']")).Click();
                Thread.Sleep(1500);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private bool AddThanhPhoHienTaiVaQueQuan(IWebDriver chromeDriver, List<ModelNoiSong> listNoiSongs)
        {
            chromeDriver.Url = "https://m.facebook.com/profile/edit/infotab/section/forms/?info_surface=info&section=living";
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                var dem = 0;
                foreach (var noiSong in listNoiSongs)
                {
                    if (noiSong.type == TypeDiaChi.ThanhPhoHienTai)
                    {
                        chromeDriver.FindElement(By.XPath("//input[@name='current_city_text']")).Clear();
                        chromeDriver.FindElement(By.XPath("//input[@name='current_city_text']")).SendKeys(noiSong.name);
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@role='heading']")).Click();
                        var id = chromeDriver.FindElement(By.XPath("//div[@data-sigil=' jx-result']")).GetAttribute("rel");
                        js.ExecuteScript($"document.getElementsByName('current_city')[0].setAttribute('value', '{id}')");
                        dem = chromeDriver.FindElements(By.XPath("//div[@data-sigil=' jx-result']")).Count();
                    }
                    else
                    {
                        chromeDriver.FindElement(By.XPath("//input[@name='hometown_text']")).Clear();
                        chromeDriver.FindElement(By.XPath("//input[@name='hometown_text']")).SendKeys(noiSong.name);
                        Thread.Sleep(1000);
                        chromeDriver.FindElement(By.XPath("//div[@role='heading']")).Click();
                        var id = chromeDriver.FindElements(By.XPath("//div[@data-sigil=' jx-result']"))[dem].GetAttribute("rel");
                        js.ExecuteScript($"document.getElementsByName('hometown')[0].setAttribute('value', '{id}')");

                    }
                }

                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("//button[@name='save']")).Click();
            }
            catch (Exception)
            {
            }
            return true;
        }
        private bool AddCongViec(IWebDriver chromeDriver, List<ModelCongViec> congViecs)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
            try
            {
                foreach (var congViec in congViecs)
                {
                    chromeDriver.Url = "https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=work&experience_type=2002";
                    chromeDriver.FindElement(By.XPath("//input[@name='employer_text']")).Clear();
                    chromeDriver.FindElement(By.XPath("//input[@name='employer_text']")).SendKeys(congViec.name);
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//div[@role='heading']")).Click();
                    var id = chromeDriver.FindElement(By.XPath("//div[@data-sigil=' jx-result']")).GetAttribute("rel");
                    js.ExecuteScript($"document.getElementById('u_0_2').setAttribute('value', '{id}')");
                    if (!chromeDriver.FindElement(By.XPath("//input[@name='junk']")).Selected)
                    {
                        chromeDriver.FindElement(By.XPath("//span[text()='Đây không phải là địa điểm thực tế']")).Click();
                    }
                    if (congViec.timeFrom != null && congViec.timeFrom != new DateTime(1970, 1, 1))
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                        dropdown.SelectByText(congViec.timeFrom?.Year.ToString());
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_month']")));
                        dropdown.SelectByText($"Tháng {congViec.timeFrom?.Month.ToString()}");
                        dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_day']")));
                        dropdown.SelectByText($"{congViec.timeFrom?.Day.ToString()}");
                    }
                    else
                    {
                        SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='start_year']")));
                        dropdown.SelectByText("Năm");
                    }
                    if (congViec.isToNow == true)
                    {
                        if (!chromeDriver.FindElement(By.XPath("//input[@name='current']")).Selected)
                        {
                            chromeDriver.FindElement(By.XPath("//span[text()='Tôi hiện đang làm việc ở đây']")).Click();
                        }
                    }
                    else
                    {
                        if (congViec.timeTo != null && congViec.timeTo != new DateTime(1970, 1, 1))
                        {
                            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                            dropdown.SelectByText(congViec.timeTo?.Year.ToString());
                            dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_month']")));
                            dropdown.SelectByText($"Tháng {congViec.timeTo?.Month.ToString()}");
                            dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_day']")));
                            dropdown.SelectByText($"{congViec.timeTo?.Day.ToString()}");
                        }
                        else
                        {
                            SelectElement dropdown = new SelectElement(chromeDriver.FindElement(By.XPath("//select[@name='end_year']")));
                            dropdown.SelectByText("Năm");
                        }
                    }
                    Thread.Sleep(1000);
                    chromeDriver.FindElement(By.XPath("//button[@name='save']")).Click();
                    Thread.Sleep(1500);
                }
            }
            catch (Exception)
            {

            }
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
        public void BackUpFacebookAll(IWebDriver chromeDriver, int rowIndex)
        {
            var uidFb = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            BackupThongTinCoBan(chromeDriver, uidFb);
            DocGhiFile.CreadFolder($"BackUp/{uidFb}/anhbanbe");
            BackUpAnhBanBe(chromeDriver, uidFb);
            BackUpBaoMat(chromeDriver, uidFb);
        }
        public void BackUpFacebookOnlyImageFriend(IWebDriver chromeDriver, int rowIndex)
        {
            var uidFb = GetCookieFb(chromeDriver).FirstOrDefault(x => x.Name == "c_user").Value;
            DocGhiFile.CreadFolder($"BackUp/{uidFb}/anhbanbe");
            BackUpAnhBanBe(chromeDriver, uidFb);
            BackUpBaoMat(chromeDriver, uidFb);
        }
        public void QuetThanhVienGroup(IWebDriver chromeDriver, int rowIndex)
        {
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
            var rsLogin = DangNhap();
            if (rsLogin.rs == false) { return; }
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

    public enum GenderEnum
    {
        Man,
        Women
    }
}
