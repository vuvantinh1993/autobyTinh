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
        public bool _chiBackupnguoimoi1 = true;

        public Auto()
        {
            InitializeComponent();
            this.socapdagiai.Text = "0";
            this.soLanKhongGiaiDuoctien.Text = "0";
            this.socapgiaikhongthanh.Text = "0";
            this.sotiennhan.Text = "0";
            this.dgvAccounts.DataSource = XLFile.DocFileTaiKhoan("config/accounts.txt");
        }

        public bool SetUpChrome(ref ChromeDriverService chromeDriverService, ref ChromeOptions chromeOptions, int rowIndex)
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
            chromeOptions.AddArgument($"--user-agent={this.dgvAccounts.Rows[rowIndex].Cells["userAgent"].Value.ToString()}");
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
                //chromeOptions.AddArguments(new string[]
                //{
                //    "--start-maximized"
                //});
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
                try
                {
                    chromeDriver[rowIndex] = new ChromeDriver(chromeDriverService, chromeOptions);
                    chromeDriver[rowIndex].Manage().Window.Size = new Size(400, 850);
                }
                catch (Exception e)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi";
                    //dgvAccounts.Rows[rowIndex].Cells["status"].Value = "2222" + e.ToString();
                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                    return false;
                }
                chromeDriver[rowIndex].Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                return true;
            }
            catch (Exception)
            {
                this.dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Có lỗi khi add arguments, tắt đi chạy lại";
            }
            return false;
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

        #region funtion vowis dataGirdView
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
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                // click Tạm dừng
                if (e.ColumnIndex == 14)
                {
                    if ((bool)this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                    {
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    }
                    else
                    {
                        // xử lý code
                        this.dgvAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    }
                }
            }
        }
        private void dgvAccounts_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu_dgv = new ContextMenuStrip();
                int positionClick = this.dgvAccounts.HitTest(e.X, e.Y).RowIndex;
                if (positionClick >= 0)
                {
                    menu_dgv.Items.Add("Mở chrome").Name = "Mở chrome";
                    menu_dgv.Items.Add("Mở m.facebook").Name = "m.facebook";
                    menu_dgv.Items.Add("BackUp ảnh bạn bè").Name = "BackUp ảnh bạn bè";
                    menu_dgv.Items.Add("BackUp toàn bộ").Name = "BackUp toàn bộ";
                    menu_dgv.Items.Add("Mở file BackUp").Name = "Mở file BackUp";
                    menu_dgv.Items.Add("Đăng kí Hana").Name = "Đăng kí Hana";
                    menu_dgv.Items.Add("Delete").Name = "Delete";
                    menu_dgv.Items.Add("Quét thành viên Group").Name = "Quét thành viên Group";
                    menu_dgv.Items.Add("Comment Group").Name = "Comment Group";
                }
                menu_dgv.Show(dgvAccounts, new Point(e.X, e.Y));
                if (dgvAccounts.SelectedRows.Count == 1)
                {
                    dgvAccounts.ClearSelection();
                }
                dgvAccounts.Rows[positionClick].Selected = true;
                menu_dgv.ItemClicked += new ToolStripItemClickedEventHandler(my_menu_ItemChicked);
            }
        }
        private void my_menu_ItemChicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "Mở chrome":
                        OpenChrome(row.Index);
                        break;
                    case "m.facebook":
                        OpenFacebook(row.Index);
                        break;
                    case "BackUp ảnh bạn bè":
                        BackUpFacebookOnlyImageFriend(row.Index);
                        break;
                    case "BackUp toàn bộ":
                        BackUpFacebookAll(row.Index);
                        break;
                    case "Mở file BackUp":
                        OpenFileBackUp(dgvAccounts.Rows[row.Index].Cells["id"].Value.ToString());
                        break;
                    case "Đăng kí Hana":
                        ChayDangKiHana(row.Index);
                        break;
                    case "Delete":
                        dgvAccounts.Rows.Remove(row);
                        break;
                    case "Quét thành viên Group":
                        QuetThanhVienGroup(row.Index);
                        break;
                    case "Comment Group":
                        CommentGroup(row.Index);
                        break;
                }
            }
        }
        private void OpenFileBackUp(string uid)
        {
            Process.Start("explorer", $"{Environment.CurrentDirectory}\\BackUp\\{uid}");
        }
        #endregion



        private void ChayJobHana(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }

                // nếu login fb được thì làm tiếp
                if (this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString() != null && this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString() != null && (bool)this.dgvAccounts.Rows[rowIndex].Cells["runhana"].Value)
                {
                    Hana hana = new Hana(this.dgvAccounts.Rows[rowIndex].Cells["hana"].Value.ToString(),
                        this.dgvAccounts.Rows[rowIndex].Cells["passhana"].Value.ToString(), dgvAccounts, rowIndex);
                    string token = hana.LoginHana(chromeDriver[rowIndex]);
                    if (token != null)
                    {
                        #region chọn tài khoản làm việc
                        if (!hana.SelectAccountLeanJob(chromeDriver[rowIndex], dgvAccounts.Rows[rowIndex].Cells["name"].Value.ToString())) return;
                        #endregion

                        while (true)
                        {
                            CheckStopAppAuto(rowIndex); // kiểm tra tạm dừng
                            UpdateValueFormForFb(ref facebook);

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
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }

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
        public void CheckStopAppAuto(int rowIndex)
        {
            var checkStop = (bool)dgvAccounts.Rows[rowIndex].Cells["stop"].Value;
            if (checkStop)
            {
                dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Dừng chạy";
                while ((bool)dgvAccounts.Rows[rowIndex].Cells["stop"].Value)
                {
                    Task.Delay(1000);
                }
                dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Huỷ tạm dừng tiếp tục chạy";
            }
        }
        public void UpdateValueFormForFb(ref FaceBook facebook)
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
            this.isCheckBackUpFriendNew.Invoke(new Action(() =>
            {
                _chiBackupnguoimoi1 = (bool)this.isCheckBackUpFriendNew.Checked;
            }));
            #endregion
            facebook.ChangeValueWithForm(_delayFrom, _delayTo, _chiBackupnguoimoi1);
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

        private void button1_Click(object sender, EventArgs e)
        {
            var accounts = File.ReadAllLines("config/accounts.txt");
            List<string> listAccountNew = new List<string>();
            foreach (var item in accounts)
            {
                var splitItem = item.Split('|');
                var str = $"{ splitItem[0] }|{ splitItem[1] }|{ splitItem[2] }|{ splitItem[5] }|{ splitItem[6] }|{ splitItem[7] }|{ splitItem[8] }|{ splitItem[9] }|{ splitItem[3] }|{ splitItem[4] }|{ splitItem[10] }|{ splitItem[11] }|{ splitItem[12] }|{ splitItem[13] }|{ splitItem[14]}|1";
                listAccountNew.Add(str);
            }
            File.WriteAllLines("config/account.txt", listAccountNew);
        }


        #region BackUp Facebook
        private void BackUpFacebookAll(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }
                facebook.BackUpFacebookAll(chromeDriver[rowIndex]);
            });
            t.Start();
        }
        private void BackUpFacebookOnlyImageFriend(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }
                facebook.BackUpFacebookOnlyImageFriend(chromeDriver[rowIndex]);
            });
            t.Start();
        }
        #endregion

        #region mở facebook
        private void OpenFacebook(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                return;
            });
            t.Start();
        }
        private void OpenChrome(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex);
                return;
            });
            t.Start();
        }
        #endregion

        #region crowl dữ liệu
        private void QuetThanhVienGroup(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }
                facebook.QuetThanhVienGroup(chromeDriver[rowIndex]);
            });
            t.Start();
        }

        private void LocNguoiDung(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }
                facebook.LocNguoiDung(chromeDriver[rowIndex]);
            });
            t.Start();
        }
        #endregion

        private void CommentGroup(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!SetUpChrome(ref chromeDriverService, ref chromeOptions, rowIndex)) return;
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex);
                UpdateValueFormForFb(ref facebook);
                string userIdFb = facebook.Login(chromeDriver[rowIndex]);
                if (userIdFb == null) { return; }
                facebook.MComment(chromeDriver[rowIndex]);
            });
            t.Start();
        }
        private void locthanhvien_Click(object sender, EventArgs e)
        {
            LocNguoiDung(2);
        }

    }
}
