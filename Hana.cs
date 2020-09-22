using Anticaptcha_example.Api;
using Anticaptcha_example.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace autohana
{
    public class Hana : Auto
    {
        private string _userName;
        private string _pass;
        private string _urlLoginHana = "https://fb.vieclamonline.org/login";
        private string _urlHomeHana = "https://fb.vieclamonline.org/";
        private string _urlListJobFb = "https://fb.vieclamonline.org/jobs/facebook";
        private string _urlListTaiKhoanFB = "https://fb.vieclamonline.org/account/manager/facebook";
        private DataGridView _dgvAccounts;
        private int _rowIndex;
        private int soLanLayItJobLienTiep = 0; // mục đích nếu đếm số job sau khi giả capcha <=2 job trong 2 lần thì dừng lại 20 phút đợi job rồi làm

        public Hana(string userName, string pass, DataGridView dgvAccounts, int rowIndex)
        {
            _userName = userName;
            _pass = pass;
            _dgvAccounts = dgvAccounts;
            _rowIndex = rowIndex;
        }
        public string LoginHana(IWebDriver chromeDriver)
        {
            //Auto.dgvAccounts.
            //a.form.dgvAccounts
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Hana";
            chromeDriver.Url = _urlHomeHana;
            Common.DelayMiliSeconde(2000);
            if (chromeDriver.Url == _urlHomeHana)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Hana thành công, chọn tài khoản làm việc";
                return "asdsad";
            }
            else if (chromeDriver.Url == _urlLoginHana)
            {
                var token = LoginUidAndPass(chromeDriver);
                if (token != null)
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Hana thành công, chọn tài khoản làm việc";
                    return token;
                }
            }
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đăng nhập Hana thất bại";
            return null;
        }
        private string LoginUidAndPass(IWebDriver chromeDriver)
        {
            chromeDriver.FindElement(By.Name("username")).SendKeys(_userName);
            chromeDriver.FindElement(By.Name("password")).SendKeys(_pass);
            chromeDriver.FindElement(By.TagName("button")).Click();
            Common.Delay(2);
            if (Common.FindEle("//*[@id=\"app\"]/div/div[1]/header/div/div[1]", chromeDriver, 5, 2))
            {
                return "asdsad";
            }
            return null;
        }

        public bool LayMotJobAndClick(IWebDriver chromeDriver, ref int socapchadagiai, ref int socapchadagiaikhongthanh)
        {
            var demGiaiJob = 0;
            int checkTokenfalse = 0;
        GOTO_LAY_JOB:
            chromeDriver.Url = _urlListJobFb;
            TatLopPhuVaDoiLoadHana(chromeDriver);
            if (demGiaiJob > 2)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Số lần giải capcha khong thanh {demGiaiJob}";
                return false;
            }


            if (Common.FindEle("//div[@data-v-67e4251b=''and@class='col col-12']", chromeDriver, 2, 1))
            {
                checkTokenfalse = 2;
                // click Job dau tien
                chromeDriver.FindElements(By.XPath("//div[@data-v-67e4251b=''and@class='col col-12']"))[0].Click();
                TatLopPhuVaDoiLoadHana(chromeDriver);
                if (chromeDriver.Url == _urlListJobFb)
                {
                    goto GOTO_LAY_JOB;
                }
                return true;
            }
            else if (Common.FindEle("/html/body/div[1]/div/div/div/main/div/div/div/div/div/div/div[2]/div/div/div/div[1]/div/iframe", chromeDriver, 1, 0))
            {
                if (soLanLayItJobLienTiep > 2)
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Hana ít job quá, dừng lại!";
                    return false;
                }

                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Giải capcha thứ: {socapchadagiai + 1}";
                if (checkTokenfalse == 1)
                {
                    socapchadagiaikhongthanh++;
                }
                else
                {
                    checkTokenfalse = 1;
                }
                // tạo 1 task mới đi nuôi nick facebook
                Task noinickTask = new Task(() =>
                {
                    Common.DelayMiliSeconde(100);
                });
                noinickTask.Start();
                noinickTask.Wait();

                // check giải capcha tay
                try
                {
                    var m = chromeDriver.FindElements(By.XPath("//iframe[contains(@src,'google.com')]")).ToArray();
                    m[0].Click();
                    Common.DelayMiliSeconde(1000);
                    if (!chromeDriver.PageSource.Contains("<iframe title=\"recaptcha challenge\""))
                    {
                        TatLopPhuVaDoiLoadHana(chromeDriver);
                        goto GOTO_LAY_JOB;
                    }
                    else
                    {
                        chromeDriver.Navigate().Refresh();
                        TatLopPhuVaDoiLoadHana(chromeDriver);
                    }
                }
                catch (Exception)
                {
                }

                // cos capcha Giải capcha ở đây
                var checkresolveCapcha = ExampleNoCaptchaProxyless("https://fb.vieclamonline.org/jobs/facebook", chromeDriver, ref socapchadagiai);
                if (checkresolveCapcha.value)
                {
                    try
                    {
                        var danhsachJob = chromeDriver.FindElements(By.XPath("//*[@id=\"app\"]/div/main/div/div/div/div/div/div/div[2]/div/div/div/div[2]/div[4]/div")).ToArray();
                        if (danhsachJob.Count() > 0)
                        {
                            if (danhsachJob.Count() <= 2)
                            {
                                soLanLayItJobLienTiep++;
                            }
                            else
                            {
                                soLanLayItJobLienTiep = 0;
                            }
                        }
                    }
                    catch (Exception) { }

                    demGiaiJob = 0;
                    // giai capcha thanh cong
                    goto GOTO_LAY_JOB;
                }
                else
                {
                    demGiaiJob++;
                    // giai cap cha khong thanh cong load trang giai lai
                    if (checkresolveCapcha.errors.Contains("Không thể load được trang sau khi giai capcha"))
                    {
                        return false;
                    }
                    LayMotJobAndClick(chromeDriver, ref socapchadagiai, ref socapchadagiaikhongthanh);
                }
            }

            // hết job rồi bỏ qua
            if (chromeDriver.PageSource.Contains("Bảo trì"))
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Hana đang bảo trì";
                return false;
            }
            try
            {
                if (chromeDriver.FindElement(By.XPath("/html/body/div[1]/div/div/div/main/div/div/div/div/div/div/div[2]/div/div/div/div[2]/div[4]")).Text.Length < 10)
                {
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Hana hết job rồi!";
                    return false;
                    //var seconde = 3600;
                    //while (seconde > 0)
                    //{
                    //    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Hana hết job đợi {seconde} giây load lại";
                    //    Thread.Sleep(TimeSpan.FromSeconds(1));
                    //    seconde--;
                    //}
                    //goto GOTO_LAY_JOB;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private (bool value, string errors) ExampleNoCaptchaProxyless(string pageurl, IWebDriver chromeDriver, ref int socapchadagiai)
        {
            DebugHelper.VerboseMode = true;
            var api = new NoCaptchaProxyless
            {
                ClientKey = "c35c7cd7c3653e886a9e578c07166a06",
                WebsiteUrl = new Uri($"{pageurl}"),
                WebsiteKey = "6LdfCvAUAAAAAP-flt3eC-0pRnbbvRTUIeaa9rT9"
            };

            if (!api.CreateTask())
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Giải cap cha không thành công {api.ErrorMessage}";
                return (false, api.ErrorMessage);
            }
            else if (!api.WaitForResult())
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Giải cap cha không thành công - Could not solve the captcha.";
                return (false, "Could not solve the captcha.");
            }
            else
            {
                var resolveCapcha = api.GetTaskSolution().GRecaptchaResponse;
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                socapchadagiai++;
                var noidung = js.ExecuteScript(@"function TIMHAMCALLBACK(capcha, j = 0){
                                                  var query = '';
                                                  var demsolan = j;
                                                  var listKeys = Object.keys(capcha);
                                                  var listValues = Object.values(capcha);
                                                  for (var i = 0; i < listKeys.length; i++) {
                                                    if(listKeys[i] === 'callback'){
                                                         console.log('callback');
                                                         return '.callback('
                                                       }else if(listValues[i] === undefined ||listValues[i] == null ){
                                                         continue;
                                                       }else if(demsolan > 2){
                                                          return 'dasd';
                                                       }
                                                       else{
                                                         var rs = TIMHAMCALLBACK(listValues[i], demsolan+1);
                                                         if(rs && rs.includes('callback')){
                                                           query += '.'+ listKeys[i] + rs;
                                                           return query;
                                                         }
                                                       }
                                                  }
                                                };
                                                return TIMHAMCALLBACK(___grecaptcha_cfg.clients[0]);");
                if (noidung.ToString().Contains("callback"))
                {
                    var query = "___grecaptcha_cfg.clients[0]" + noidung.ToString() + $"'{resolveCapcha}')";
                    js.ExecuteScript("document.getElementById('g-recaptcha-response').innerHTML='" + resolveCapcha + "';");
                    js.ExecuteScript(query);
                    if (chromeDriver.PageSource.Contains("opacity: 0.46; background-color: rgb(33, 33, 33)"))
                    {
                        while (chromeDriver.PageSource.Contains("opacity: 0.46; background-color: rgb(33, 33, 33)"))
                        {
                            Common.DelayMiliSeconde(1000);
                        }
                    }
                    _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Giải capcha thành công";
                    return (true, "Giải capcha thành công!");
                }

                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Không thể load được trang sau khi giai capcha";
                return (false, "Không thể load được trang sau khi giai capcha 2");
            }
        }
        public bool SelectAccountLeanJob(IWebDriver chromeDriver, string nameAccount)
        {
            chromeDriver.Url = _urlListJobFb;
            TatLopPhuVaDoiLoadHana(chromeDriver);
            Common.Delay(1);
            var jobLamHonNay = 0;
            try
            {
                chromeDriver.FindElement(By.XPath("//span[contains(text(), 'Chọn tài khoản')]")).Click();
                Common.DelayMiliSeconde(1000);
                var listuser = chromeDriver.FindElements(By.XPath("//div[contains(@class, 'v-list-item__content')]"));
                for (int i = 0; i < listuser.Count(); i++)
                {
                    if (listuser[i].Text.Contains(nameAccount))
                    {
                        var b = listuser[i].Text;
                        Regex regex2 = new Regex(@"[0-9]{1,4}");
                        jobLamHonNay = Convert.ToInt32(regex2.Match(b).Value);
                        listuser[i].Click();
                        if (jobLamHonNay >= ljobMaxOfDay)
                        {
                            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"đã hoàn thành {jobLamHonNay} Job";
                            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = jobLamHonNay;
                            return false;
                        }
                        else
                        {
                            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Chọn tài khoản làm việc thành công.";
                            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = jobLamHonNay;
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Không tồn tại tài khoản trong hana";
            return false;
        }

        public string LayKeyThemTaiKhoanHana(IWebDriver chromeDriver)
        {
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đi lấy mã Code Hana";
            try
            {
                chromeDriver.Url = _urlListTaiKhoanFB;
                Common.DelayMiliSeconde(1000);
                chromeDriver.FindElement(By.XPath("//span[text()= 'Thêm tài khoản']")).Click();
                Common.DelayMiliSeconde(1000);
                chromeDriver.FindElement(By.XPath("//span[text()= 'Copy']")).Click();
                Common.DelayMiliSeconde(1000);
                var noidung = chromeDriver.FindElement(By.XPath("//input[@id= 'verify-code']")).GetProperty("value");
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = $"Mã code Hana là {noidung}";
                return noidung;
            }
            catch (Exception)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Lỗi, không lấy được mã Code Hana";
            }
            return null;
        }

        public bool NhapMaBaiFBChuaCodeHana(IWebDriver chromeDriver, string urlBaiviet)
        {
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Nhập Link bài viết";
            try
            {
                chromeDriver.FindElement(By.XPath("//div[@class= 'allow paste_data']")).SendKeys(urlBaiviet);
                Common.DelayMiliSeconde(1000);
                chromeDriver.FindElement(By.XPath("//div[@class= 'text-center confirm white--text pa-3 v-card v-card--flat v-sheet theme--light']")).Click();
                TatLopPhuVaDoiLoadHana(chromeDriver);
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Thêm vào Hana thành công";
                return true;
            }
            catch (Exception)
            {
                _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Không thêm được Hana";
            }
            return false;
        }


        private void DoiLoadHana(IWebDriver chromeDriver)
        {
            _dgvAccounts.Rows[_rowIndex].Cells["status"].Value = "Đợi load trang Hana";
            while (chromeDriver.PageSource.Contains("opacity: 0.46; background-color: rgb(33, 33, 33); border-color: rgb(33, 33, 33);"))
            {
                Common.DelayMiliSeconde(1000);
            }
        }

        private void TatLopPhuVaDoiLoadHana(IWebDriver chromeDriver)
        {
            try
            {
                DoiLoadHana(chromeDriver);
                var nut = chromeDriver.FindElement(By.XPath("//div[@class='v-overlay__scrim']"));
                IJavaScriptExecutor js = (IJavaScriptExecutor)chromeDriver;
                js.ExecuteScript("return document.getElementsByClassName('v-overlay v-overlay--active theme--dark')[0].remove();");
                js.ExecuteScript("return document.getElementsByClassName('white--text v-navigation-drawer v-navigation-drawer--fixed v-navigation-drawer--is-mobile v-navigation-drawer--open theme--light deep-purple darken-4')[0].remove();");
            }
            catch (Exception)
            {
            }
        }
    }


}


public class StartWorkModel
{
    public bool success = false;
    public string data;
    public string message;
}

public class BaocaoHanaModel
{
    public bool success = false;
    public string data;
    public string message;
}


public class OneJobHanaModel
{
    public bool success = false;
    public OneJobHana data;
    public string message;

    public class OneJobHana
    {
        public int id; // la job_id
        public string post_id;
        public string phone_post_link;
        public string web_post_link;
        public string basic_post_link;
        //public ActionFb seeding_type;
        public string comment_need;
        public int num_seeding_remain;
        public string note;
        public string days_remain;
        public string time_remain;
        public int coin;
        public string title;
        public string content;
        public string updated_at;
        public string created_at;
        public string end_at;
        public int? fix_coin_job;
        public string icon;
        public string seeding_title;
        public int? user_likes_info; // la user_likes_info
        public int? user_following_info; // la user_following_info
        public int? fanpage_likes_info; // la fanpage_likes_info
    }
}


// danh sách idfb so sánh với jdFBHana
public class ListUserFbModel
{
    public bool success = false;
    public Model1 data;
    public class Model1
    {
        public Model2 data;
    }

    public class Model2
    {
        public int id;
        public Model3 farmer;
    }
    public class Model3
    {
        public List<Model4> accounts;
    }

    public class Model4
    {
        public int id;
        public string id_account;
    }
}


// danh sách job
public class JobHanasModel
{
    public bool success = false;
    public List<Job> data;
    public string message;
    public class Job
    {
        public int id; // la job_id
        public string post_id;
    }
}
