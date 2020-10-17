using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public class Chrome
    {
        DataGridView dgvAccounts;
        int rowIndex;
        ChromeOptions chromeOptions;
        ChromeDriverService chromeDriverService;
        public Chrome(DataGridView dgvAccounts, int rowIndex, ChromeDriverService chromeDriverService, ChromeOptions chromeOptions)
        {
            this.dgvAccounts = dgvAccounts;
            this.rowIndex = rowIndex;
            this.chromeOptions = chromeOptions;
            //this.chromeDriver = chromeDriver;
            this.chromeDriverService = chromeDriverService;
        }

        public bool SetUpChrome(bool ischeckLoadImage, ref IWebDriver chromeDriver)
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
            chromeOptions.AddArgument($"--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
            //chromeOptions.AddArgument($"--user-agent={this.dgvAccounts["userAgent", rowIndex].Value.ToString()}");
            if (ischeckLoadImage)
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
                CheckAndAddProfile(ref chromeOptions, rowIndex, dgvAccounts);
                try
                {
                    chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);
                    chromeDriver.Manage().Window.Size = new Size(600, 600);
                }
                catch (Exception)
                {
                    dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Hãy update chromedrive mới, hoặc trình duyệt cùng profile đang bật tắt nó đi";
                    dgvAccounts.Rows[rowIndex].Cells["Action"].Value = "Bắt đầu";
                    return false;
                }
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                return true;
            }
            catch (Exception)
            {
                this.dgvAccounts.Rows[rowIndex].Cells["status"].Value = "Có lỗi khi add arguments, tắt đi chạy lại";
            }
            return false;
        }

        public void CheckAndAddProfile(ref ChromeOptions chromeOptions, int rowIndex, DataGridView dgvAccounts)
        {
            if (!Directory.Exists("Profile"))
            {
                Directory.CreateDirectory("Profile");
            }
            if (Directory.Exists("Profile"))
            {
                chromeOptions.AddArguments("user-data-dir=" + "Profile" + "\\" + dgvAccounts.Rows[rowIndex].Cells["id"].Value);
            }
        }


    }
}
