using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public class InformationForm
    {
        DataGridView dgvAccounts;
        int rowIndex;
        IWebDriver chromeDriver;
        public InformationForm(DataGridView dgvAccounts, int rowIndex, IWebDriver chromeDriver)
        {
            this.dgvAccounts = dgvAccounts;
            this.rowIndex = rowIndex;
            this.chromeDriver = chromeDriver;
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
    }
}
