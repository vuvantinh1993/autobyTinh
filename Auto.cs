using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public static int ldelayFrom = 0;
        public static int ldelayTo = 0;
        public static int ljobMaxOfDay = 0;
        public bool lchiBackupnguoimoi1 = true;

        public Auto()
        {
            InitializeComponent();
            dgvAccounts.AutoGenerateColumns = false;
            this.dgvAccounts.DataSource = XLFile.DocFileTaiKhoan("config/accounts.txt");
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
                        if ((bool)nuoinick.Checked)
                        {
                            NuoiNick(e.RowIndex);
                        }
                        else
                        {
                            ChayJobHana(e.RowIndex);
                        }
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
                if (e.ColumnIndex == 14 || e.ColumnIndex == 15 || e.ColumnIndex == 16 || e.ColumnIndex == 17)
                {
                    Common.ChangeValueCheckBoxDGV(dgvAccounts[e.ColumnIndex, e.RowIndex], (bool)dgvAccounts[e.ColumnIndex, e.RowIndex].Value);
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

                    menu_dgv.Show(dgvAccounts, new Point(e.X, e.Y));
                    if (dgvAccounts.SelectedRows.Count == 1)
                    {
                        dgvAccounts.ClearSelection();
                    }
                    dgvAccounts.Rows[positionClick].Selected = true;
                    menu_dgv.ItemClicked += new ToolStripItemClickedEventHandler(my_menu_ItemChicked);
                }
            }
        }
        private void my_menu_ItemChicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvAccounts.SelectedRows)
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "Mở chrome":
                        RunMenu(ActionMenu.OpenChrome, row.Index);
                        break;
                    case "m.facebook":
                        RunMenu(ActionMenu.OpenFacebook, row.Index);
                        break;
                    case "BackUp ảnh bạn bè":
                        RunMenu(ActionMenu.BackUpFacebookOnlyImageFriend, row.Index);
                        break;
                    case "BackUp toàn bộ":
                        RunMenu(ActionMenu.BackUpFacebookAll, row.Index);
                        break;
                    case "Mở file BackUp":
                        DocGhiFile.OpenFileBackUp(dgvAccounts.Rows[row.Index].Cells["id"].Value.ToString());
                        break;
                    case "Đăng kí Hana":
                        ChayDangKiHana(row.Index);
                        break;
                    case "Delete":
                        dgvAccounts.Rows.Remove(row);
                        break;
                    case "Quét thành viên Group":
                        RunMenu(ActionMenu.QuetThanhVienGroup, row.Index);
                        break;
                    case "Comment Group":
                        CommentGroup(row.Index);
                        break;
                }
            }
        }
        #endregion

        private bool TaoChrome(int rowIndex)
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeOptions = new ChromeOptions();
            var chrome = new Chrome(dgvAccounts, rowIndex, chromeDriverService, chromeOptions);
            if (chrome.SetUpChrome((bool)this.checkLoadImage.Checked, ref chromeDriver[rowIndex]))
            {
                return true;
            }
            return false;
        }
        private void RunMenu(ActionMenu actionMenu, int rowIndex)
        {
            Task t = new Task(() =>
            {
                if (!TaoChrome(rowIndex)) { return; }
                var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDriver[rowIndex]);
                if (actionMenu == ActionMenu.OpenChrome)
                {
                    return;
                }
                else
                {
                    var rsLogin = facebook.DangNhap();
                    if (!rsLogin.rs)
                    {
                        return;
                    }
                    switch (actionMenu)
                    {
                        case ActionMenu.OpenFacebook:
                            break;
                        case ActionMenu.BackUpFacebookOnlyImageFriend:
                            facebook.BackUpFacebookOnlyImageFriend(chromeDriver[rowIndex], rowIndex);
                            break;
                        case ActionMenu.BackUpFacebookAll:
                            facebook.BackUpFacebookAll(chromeDriver[rowIndex], rowIndex);
                            break;
                        case ActionMenu.ChayDangKiHana:
                            break;
                        case ActionMenu.QuetThanhVienGroup:
                            facebook.QuetThanhVienGroup(chromeDriver[rowIndex], rowIndex);
                            break;
                        case ActionMenu.CommentGroup:
                            break;
                    }
                }
            });
            t.Start();
        }


        private void ChayJobHana(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                var chrome = new Chrome(dgvAccounts, rowIndex, chromeDriverService, chromeOptions);
                if (chrome.SetUpChrome((bool)this.checkLoadImage.Checked, ref chromeDriver[rowIndex])) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex, chromeDriver[rowIndex]);
                var rsLogin = facebook.DangNhap();
                if (rsLogin.rs == false) { return; }

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
                                else if (rsLamJob.isFinishTotalJob == true || (int)dgvAccounts.Rows[rowIndex].Cells["total"].Value >= ljobMaxOfDay)
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
                var chrome = new Chrome(dgvAccounts, rowIndex, chromeDriverService, chromeOptions);
                if (chrome.SetUpChrome((bool)this.checkLoadImage.Checked, ref chromeDriver[rowIndex])) return;

                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex, chromeDriver[rowIndex]);
                var rsLogin = facebook.DangNhap();
                if (rsLogin.rs == false) { return; }

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
                            //var urlbaiviet = facebook.ActionDangBai(chromeDriver[rowIndex], codeHana);
                            var urlbaiviet = "";
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

        private void NuoiNick(int rowIndex)
        {
            Task t = new Task(() =>
            {
                if (!TaoChrome(rowIndex)) { return; }
                var facebook = new FaceBook(dgvAccounts, rowIndex, chromeDriver[rowIndex]);
                var rsLogin = facebook.DangNhap();
                if (!rsLogin.rs)
                {
                    dgvAccounts["status", rowIndex].Value = "Đăng nhập thất bại";
                    return;
                }
                dgvAccounts["status", rowIndex].Value = "Đi tương tác";

                for (int i = 0; i < Convert.ToInt32(numberAction.Text); i++)
                {
                    CheckStopAppAuto(rowIndex);
                    facebook.TrithongMinh(1, chromeDriver[rowIndex]);
                    dgvAccounts["status", rowIndex].Value = $"Xong tương tác số {i + 1}";
                    ChoClickButtonFB(rowIndex, $"thao tác  {i + 2}");
                }
            });
            t.Start();

        }

        public void ChoClickButtonFB(int rowIndex, string nameJob = "thao tác")
        {
            var randomTime = (new Random()).Next(Convert.ToInt32(delayFrom.Value), Convert.ToInt32(delayTo.Value));
            while (randomTime > 0)
            {
                dgvAccounts["status", rowIndex].Value = $"Click {nameJob} sau {randomTime} giây";
                Thread.Sleep(1000);
                randomTime--;
            }
        }





        private void CommentGroup(int rowIndex)
        {
            Task t = new Task(() =>
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                ChromeOptions chromeOptions = new ChromeOptions();
                var chrome = new Chrome(dgvAccounts, rowIndex, chromeDriverService, chromeOptions);
                if (chrome.SetUpChrome((bool)this.checkLoadImage.Checked, ref chromeDriver[rowIndex])) return;
                FaceBook facebook = new FaceBook(dgvAccounts, rowIndex, chromeDriver[rowIndex]);
                var rsLogin = facebook.DangNhap();
                if (rsLogin.rs == false) { return; }
                facebook.MActionJobComment(chromeDriver[rowIndex]);
            });
            t.Start();
        }
        private void locthanhvien_Click(object sender, EventArgs e)
        {
        }

        private void checkLoadImage_CheckedChanged(object sender, EventArgs e)
        {
            Common.ChangeValueCheckBoxForm(checkLoadImage, (bool)checkLoadImage.Checked);
        }

        private void isCheckBackUpFriendNew_CheckedChanged(object sender, EventArgs e)
        {
            Common.ChangeValueCheckBoxForm(isCheckBackUpFriendNew, (bool)isCheckBackUpFriendNew.Checked);
        }

        private void nuoinick_CheckedChanged(object sender, EventArgs e)
        {
            Common.ChangeValueCheckBoxForm(isCheckBackUpFriendNew, (bool)nuoinick.Checked);
        }
    }
}
