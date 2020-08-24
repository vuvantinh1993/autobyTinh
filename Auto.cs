using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace autohana
{
    public partial class Auto : Form
    {
        public int _soCapchaDagiai = 0;
        public int _soCapchaDagiaiKhongthanh = 0;
        public int _solanKhonggiaiduocTien = 0;
        public int _soTienDalam = 0;
        delegate void Loadform_Delegate(int socapchadagiai, int solankhonggiaiduoctien);
        public List<Model> listUidRun = new List<Model>();
        public IWebDriver[] chromeDriver = new IWebDriver[1000];
        public static int _delayFrom = 0;
        public static int _delayTo = 0;
        public static int _JobMaxOfDay = 0;


        public Auto()
        {
            InitializeComponent();
            this.socapdagiai.Text = "0";
            this.soLanKhongGiaiDuoctien.Text = "0";
            this.socapgiaikhongthanh.Text = "0";
            this.sotiennhan.Text = "0";
            this.dgvAccounts.DataSource = XLFile.DocFileTaiKhoan("config/accounts.txt");
            _delayFrom = (int)this.delayFrom.Value;
            _delayTo = (int)this.delayTo.Value;
            _JobMaxOfDay = (int)this.JobMaxOfDay.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void UpdateSoCapDagiai(int socapchadagiai)
        {
            this.socapdagiai.Text = socapchadagiai.ToString();
        }
        private void UpdateSocapchadagiaikhongthanh(int socapchadagiaikhongthanh)
        {
            this.socapgiaikhongthanh.Text = socapchadagiaikhongthanh.ToString();
        }
        private void UpdateSotiendalam(int sotiendalam)
        {
            this.sotiennhan.Text = sotiendalam.ToString();
        }
        private void UpdateSolankhonggiaiduoctien(int solankhonggiaiduoctien)
        {
            this.soLanKhongGiaiDuoctien.Text = solankhonggiaiduoctien.ToString();
        }
        private void LoadForm()
        {
            this.socapdagiai.Invoke(new Action(() => UpdateSoCapDagiai(_soCapchaDagiai)));
            this.socapgiaikhongthanh.Invoke(new Action(() => UpdateSocapchadagiaikhongthanh(_soCapchaDagiaiKhongthanh)));
            this.sotiennhan.Invoke(new Action(() => UpdateSotiendalam(_soTienDalam)));
            this.soLanKhongGiaiDuoctien.Invoke(new Action(() => UpdateSolankhonggiaiduoctien(_solanKhonggiaiduocTien)));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 19)
                {
                    if (this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Bắt đầu")
                    {
                        ChayJobHana(e.RowIndex);
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Kết thúc";
                    }
                    else
                    {
                        // xử lý code
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Bắt đầu";
                    }
                }
                if (e.ColumnIndex == 20)
                {
                    if (this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Đăng kí")
                    {
                        ChayDangKiHana(e.RowIndex);
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Kết thúc";
                    }
                    else
                    {
                        // xử lý code
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Đăng kí";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void ChayJobHana(int rowIndex)
        {
            #region Load Job max
            this.JobMaxOfDay.Invoke(new Action(() =>
            {
                _JobMaxOfDay = (int)this.JobMaxOfDay.Value;
            }));
            #endregion
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex);
                try
                {
                    chromeDriver[rowIndex] = new ChromeDriver(chromeDriverService, chromeOptions);
                }
                catch (Exception)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Trình duyệt cùng profile đang bật, hãy tắt đi chạy lại";
                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                    return;
                }
                chromeDriver[rowIndex].Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Fb thất bại";
                }
                else
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Fb thành công!";
                }
                // nếu login được thì làm tiếp
                if (this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString() != null && this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString() != null && (bool)this.dgvAccounts.Rows[rowIndex].Cells["runhana"].Value)
                {
                    Hana hana = new Hana(this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString(),
                        this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString(), dgvAccounts, rowIndex);
                    string token = hana.LoginHana(chromeDriver[rowIndex]);
                    if (token != null)
                    {
                        dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Hana thành công, chọn tài khoản làm việc";
                        #region chọn tài khoản làm việc
                        var rsSelectAccount = hana.SelectAccountLeanJob(chromeDriver[rowIndex], dgvAccounts.Rows[rowIndex].Cells["name"].Value.ToString());
                        if (rsSelectAccount.jobLamHonNay >= _JobMaxOfDay)
                        {
                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = $"đã hoàn thành {rsSelectAccount.jobLamHonNay} Job";
                            dgvAccounts.Rows[rowIndex].Cells["total"].Value = rsSelectAccount.jobLamHonNay;
                            return;
                        }
                        else if (rsSelectAccount.rs)
                        {
                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = $"Chọn tài khoản làm việc.";
                            dgvAccounts.Rows[rowIndex].Cells["total"].Value = rsSelectAccount.jobLamHonNay;
                        }
                        else
                        {
                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = $"Không tồn tại tài khoản trong hana";
                            return;
                        }
                        #endregion
                        while (true)
                        {
                            #region điều chỉnh thời gian delay thao tác, và Job max
                            this.delayFrom.Invoke(new Action(() =>
                            {
                                _delayFrom = (int)this.delayFrom.Value;
                            }));
                            this.delayTo.Invoke(new Action(() =>
                            {
                                _delayTo = (int)this.delayTo.Value;
                            }));
                            this.JobMaxOfDay.Invoke(new Action(() =>
                            {
                                _JobMaxOfDay = (int)this.JobMaxOfDay.Value;
                            }));
                            #endregion

                            while ((bool)dgvAccounts.Rows[rowIndex].Cells["stop"].Value)
                            {
                                Common.DelayMiliSeconde(2000);
                            }

                            dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đi lấy Job Hana";
                            bool takeJob = hana.LayMotJobAndClick(chromeDriver[rowIndex], ref _soCapchaDagiai, ref _soCapchaDagiaiKhongthanh);
                            if (takeJob)
                            {
                                var rsLamJob = facebook.LamJob(chromeDriver[rowIndex], ref _soTienDalam, ref _solanKhonggiaiduocTien);
                                if (rsLamJob.isError5Finish == true)
                                {
                                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Hoàn thành Job lỗi liên tục 5 lần";
                                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                                    chromeDriver[rowIndex].Quit();
                                    return;
                                }
                                else if (rsLamJob.isBlockaction == true)
                                {
                                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Tài khoản bị chặn tương tác";
                                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                                    chromeDriver[rowIndex].Quit();
                                    return;
                                }
                                else if (rsLamJob.isCheckpoint == true)
                                {
                                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Tài khoản bị checkpoint";
                                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                                    chromeDriver[rowIndex].Quit();
                                    return;
                                }
                                else if (rsLamJob.isFinishTotalJob == true || (int)dgvAccounts.Rows[rowIndex].Cells["total"].Value >= _JobMaxOfDay)
                                {
                                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Hoàn thành tối đa lượng job 1 ngày";
                                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                                    chromeDriver[rowIndex].Quit();
                                    return;
                                }
                            }
                            else
                            {
                                dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                                chromeDriver[rowIndex].Quit();
                                return;
                            }
                            LoadForm();
                        }
                    }
                    else
                    {
                        dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Hana thất bại";
                        dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                        chromeDriver[rowIndex].Quit();
                        return;
                    }
                }
            });
            t.Start();
            Common.DelayMiliSeconde(1000);
        }


        private void ChayDangKiHana(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex);
                try
                {
                    chromeDriver[rowIndex] = new ChromeDriver(chromeDriverService, chromeOptions);
                }
                catch (Exception)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Trình duyệt cùng profile đang bật, hãy tắt đi chạy lại";
                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                    return;
                }
                chromeDriver[rowIndex].Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Fb thất bại";
                }
                else
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Fb thành công!";
                }

                // mả tab 2 Hana
                Actions actionProvider = new Actions(chromeDriver[rowIndex]);
                ((IJavaScriptExecutor)chromeDriver[rowIndex]).ExecuteScript("window.open();");
                string originalWindow = chromeDriver[rowIndex].CurrentWindowHandle;
                chromeDriver[rowIndex].SwitchTo().Window(chromeDriver[rowIndex].WindowHandles.Last());


                // nếu login được thì làm tiếp
                if (this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString() != null && this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString() != null)
                {
                    Hana hana = new Hana(this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString(),
                        this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString(), dgvAccounts, rowIndex);
                    string token = hana.LoginHana(chromeDriver[rowIndex]);
                    if (token != null)
                    {
                        dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Hana thành công, bắt đầu thêm tài khoản";
                        // lấy Keyhana
                        var codeHana = hana.LayKeyThemTaiKhoanHana(chromeDriver[rowIndex]);
                        if (codeHana != null)
                        {
                            // đăng bài code hana lên fb
                            chromeDriver[rowIndex].SwitchTo().Window(chromeDriver[rowIndex].WindowHandles.First());
                            var urlbaiviet = facebook.ActionDangBai(chromeDriver[rowIndex], codeHana);
                            if (urlbaiviet != null)
                            {
                                chromeDriver[rowIndex].SwitchTo().Window(chromeDriver[rowIndex].WindowHandles.Last());
                                // thêm vào hana
                                //hana.NhapMaBaiFBChuaCodeHana(chromeDriver[rowIndex], urlbaiviet);
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Đăng nhập Hana thất bại";
                        dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                        chromeDriver[rowIndex].Quit();
                        return;
                    }
                }
            });
            t.Start();
            Common.DelayMiliSeconde(1000);
        }

        public void PathStatusDaGirlView(int rowIndex, string noidung)
        {
            dgvAccounts.Rows[rowIndex].Cells["status"].Value = noidung;
        }

        public void SetUpChrome(ref ChromeDriverService chromeDriverService, ref ChromeOptions chromeOptions, int rowIndex)
        {
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            if ((bool)this.dgvAccounts.Rows[rowIndex].Cells["An"].Value)
            {
                chromeOptions.AddArgument("--headless");
            }
            chromeOptions.AddArguments(new string[]
            {
                    "--disable-blink-features=AutomationControlled"
            });
            chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4209.0 Safari/537.36");
            if (this.checkLoadImage.Checked)
            {
                chromeOptions.AddArguments(new string[]
                {
                    "--blink-settings=imagesEnabled=false"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--enable-automation"
                });
                chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            }

            try
            {
                chromeOptions.AddArguments(new string[]
                {
                    "--start-maximized"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-notifications"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-popup-blocking"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-geolocation"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--no-sandbox"
                });
                chromeOptions.AddArguments(new string[]
                {
                    "--disable-gpu"
                });
                CheckAndAddProfile(ref chromeOptions, rowIndex);
                //chromeOptions.AddArgument(string.Format("user-data-dir={0}", arg));
                //this.chromeDriver_0[int_64] = new ChromeDriver(chromeDriverService, chromeOptions);
            }
            catch (Exception)
            {
                this.dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Có lỗi khi add arguments";
            }
        }

        private void CheckAndAddProfile(ref ChromeOptions chromeOptions, int rowIndex)
        {
            if (!Directory.Exists("Profile"))
            {
                Directory.CreateDirectory("Profile");
            }
            if (Directory.Exists("Profile"))
            {
                chromeOptions.AddArguments("user-data-dir=" + "Profile" + "\\" + this.dgvAccounts.Rows[rowIndex].Cells["id"].Value);
            }
        }

        private void ConvertData_Click(object sender, EventArgs e)
        {
            var accounts = File.ReadAllLines("config/fileold.txt");
            List<string> listAccountNew = new List<string>();
            foreach (var item in accounts)
            {
                var splitItem = item.Split('|');
                if (splitItem.Count() == 8)
                {
                    var c0 = splitItem[0];
                    var c1 = splitItem[1];
                    var c2 = splitItem[2];
                    var c3 = splitItem[3];
                    var c4 = splitItem[4]; // cookie
                    var c5 = splitItem[5];
                    var c6 = splitItem[6];
                    var c7 = splitItem[7];
                    var c8 = this.uidAddHana.Text;
                    var c9 = this.passAddHana.Text;
                    var c10 = "True";
                    var c11 = "False";
                    var c12 = "False";
                    var c13 = "False";
                    var c14 = "Bắt đầu";

                    var str = c0 + '|' + c1 + '|' + c2 + '|' + c3 + '|' + c4 + '|' + c5 + '|' + c6 + '|' + c7 + '|' + c8 + '|' + c9 + '|' + c10 + '|' + c11 + '|' + c12 + '|' + c13 + '|' + c14;
                    listAccountNew.Add(str);
                }
            }
            if (!File.Exists("config/accounts.txt"))
            {
                var file = File.Create("config/accounts.txt");
                file.Close();
                File.WriteAllLines("config/accounts.txt", listAccountNew);
            }
            else
            {
                File.AppendAllLines("config/accounts.txt", listAccountNew);
            }
        }

        private void listAccounts_Click(object sender, EventArgs e)
        {
            Process.Start($"{Environment.CurrentDirectory}\\config\\accounts.txt");
        }

        private void clean_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
        }
    }




}
