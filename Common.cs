﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{

    public class TypeFile
    {
        public static string Txt = "txt";
        public static string Html = "html";
    }

    public static class Common
    {


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

        public static List<T> RandomList<T>(this IList<T> list, int number)
        {
            var listNew = new List<T>();
            Random rng = new Random();
            if (number < list.Count())
            {
                while (number > 0)
                {
                    int n = list.Count;
                    int k = rng.Next(0, n);
                    listNew.Add(list[k]);
                    list.Remove(list[k]);
                    number--;
                }
            }
            return listNew;
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

        public static void ChangeValueCheckBoxDGV(DataGridViewCell dataGridViewCell, bool valueOld)
        {
            dataGridViewCell.Value = !valueOld;
        }

        public static void ChangeValueCheckBoxForm(CheckBox dataGridViewCell, bool valueOld)
        {
            dataGridViewCell.Checked = !valueOld;
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
                catch (Exception)
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
                tasks[i] = Task<bool>.Run(() => CheckElement(xpaths[j], chromeDriver));
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < xpaths.Count(); i++)
            {
                //listResult.Add(listtask[i].Result);
            }

            var listtask = new List<Task<bool>>();

            return listResult;
        }

        private static async Task<bool> CheckElement(string xpath, IWebDriver chromeDriver)
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
