using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace autohana
{
    public enum ActionFb
    {
        [Display(Name = "like bài viết")]
        LikePost = 2,
        [Display(Name = "like Page")]
        Likepage = 4,
        [Display(Name = "Theo dõi")]
        Follow = 5,
        [Display(Name = "Comment")]
        Comment,
        [Display(Name = "Angry")]
        Angry,
        [Display(Name = "Haha")]
        Haha,
        [Display(Name = "Love")]
        Love,
        [Display(Name = "Care")]
        Care,
        [Display(Name = "Wow")]
        Wow,
        [Display(Name = "Sad")]
        Sad,
    }

    public static class Common
    {
        public static int _valueActionDelayFrom = Auto._delayFrom;
        public static int _valueActionDelayTo = Auto._delayTo;

        public static void Delay(int seconde)
        {
            while (seconde > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                seconde--;
            }
        }

        public static void DelayMiliSeconde(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static int RandomValue(int valueFrom, int valueTo)
        {
            int randomTime = (new Random()).Next(valueFrom, valueTo);
            return randomTime;
        }
        public static int DalayTimeActionRamdom()
        {
            int randomTime = (new Random()).Next(_valueActionDelayFrom, _valueActionDelayTo);
            return randomTime;
        }


        public static ReadOnlyCollection<IWebElement> WaitGetElement(IWebDriver chromeDriver, By xpath, int thoiGianDoi)
        {
            WebDriverWait webDriverWait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds((double)thoiGianDoi));
            ReadOnlyCollection<IWebElement> result;
            try
            {
                webDriverWait.Until<IWebElement>(ExpectedConditions.ElementIsVisible(xpath));
                result = chromeDriver.FindElements(xpath);
            }
            catch (NoSuchElementException)
            {
                result = null;
            }
            catch (WebDriverTimeoutException)
            {
                result = null;
            }
            catch (TimeoutException)
            {
                result = null;
            }
            return result;
        }

        // Thời gian chờ để click nut follow hoặc like.......
        public static void ChoClickButtonFB()
        {
            var randomTime = (new Random()).Next(_valueActionDelayFrom, _valueActionDelayTo);
            var dem = 0;
            while (dem < randomTime)
            {
                Thread.Sleep(1000);
                dem++;
                randomTime--;
            }
        }

        public static bool FindAndClickEle(string xpath, IWebDriver chromeDriver, int solantim = 10, int timemilisecondsdelay = 2)
        {
            int solantimkiem = 0;
            while (solantimkiem < solantim)
            {
                try
                {
                    var ele = chromeDriver.FindElement(By.XPath(xpath));
                    ele.Click();
                    return true;
                }
                catch (Exception e)
                {
                    DelayMiliSeconde(timemilisecondsdelay);
                    solantimkiem++;
                    continue;
                }
            }
            return false;
        }

        public static bool FindEle(string xpath, IWebDriver chromeDriver, int solantim = 2, int timemilisecondsdelay = 2)
        {
            int solantimkiem = 0;
            while (solantimkiem < solantim)
            {
                try
                {
                    var ele = chromeDriver.FindElement(By.XPath(xpath));
                    return true;
                }
                catch (Exception)
                {
                    DelayMiliSeconde(timemilisecondsdelay);
                    solantimkiem++;
                    continue;
                }
            }
            return false;
        }

        public static List<bool> CheckElesExits(List<string> xpaths, IWebDriver chromeDriver)
        {
            Task[] tasks = new Task[xpaths.Count()];
            var listResult = new List<bool>();
            for (int i = 0; i < xpaths.Count(); i++)
            {
                int j = i;
                tasks[i] = Task<bool>.Run(() => checkElement(xpaths[j], chromeDriver));
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < xpaths.Count(); i++)
            {
                //listResult.Add(listtask[i].Result);
            }

            var listtask = new List<Task<bool>>();

            return listResult;
        }

        private static async Task<bool> checkElement(string xpath, IWebDriver chromeDriver)
        {
            try
            {
                chromeDriver.FindElement(By.XPath(xpath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckTextInHTMLCurrent(IWebDriver chromeDriver, string text)
        {
            if (chromeDriver.PageSource.ToString().Contains(text))
            {
                return true;
            }
            return false;
        }

    }


}
