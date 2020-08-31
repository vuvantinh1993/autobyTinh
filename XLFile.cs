using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public static class XLFile
    {
        public static BindingList<ModelAccount> DocFileTaiKhoan(string url)
        {
            var listAcc = new BindingList<ModelAccount>();
            var urlUserAgent = "config/userAgent.txt";
            if (!Directory.Exists("config"))
            {
                Directory.CreateDirectory("config");
            }
            if (!File.Exists(urlUserAgent))
            {
                var file = File.Create(urlUserAgent);
                file.Close();
                AddUserAgent(urlUserAgent);
            }
            if (!File.Exists(url))
            {
                var file = File.Create(url);
                file.Close();
                AddDemoAccount(url);
            }

            var accounts = File.ReadAllLines(url);
            var listUserAgent = File.ReadAllLines("config/userAgent.txt");
            foreach (var account in accounts)
            {
                var item = account.Split('|');
                if (item.Count() == 16)
                {
                    listAcc.Add(new ModelAccount
                    {
                        Stt = item[0],
                        Id = item[1],
                        Pass = item[2],
                        Fa = item[8],
                        Cookie = item[9],
                        Name = item[3],
                        Golike = item[4],
                        PassGolike = item[5],
                        Hana = item[6],
                        PassHana = item[7],
                        RunHana = bool.Parse(item[10]),
                        RunGolike = bool.Parse(item[11]),
                        An = bool.Parse(item[12]),
                        Stop = bool.Parse(item[13]),
                        Action = item[14],
                        DKhana = "Đăng kí",
                        UserAgent = Convert.ToInt32(item[15]) <= listUserAgent.Count() ? listUserAgent[Convert.ToInt32(item[15]) - 1] : listUserAgent[0],
                        BackUp = "BackUp"
                    });
                }
            }
            return listAcc;
        }
        public static void AddUserAgent(string url)
        {
            var str = "Mozilla/5.0 (Linux; Android 10; SM-G975U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.93 Mobile Safari/537.36\n" +
                "Mozilla/5.0 (Linux; Android 9; LM-Q720) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.116 Mobile Safari/537.36\n" +
                "Mozilla/5.0 (Linux; Android 9; SM-N950U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.93 Mobile Safari/537.36\n" +
                "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1\n" +
                "Mozilla/5.0 (iPad; CPU OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/79.0.3945.73 Mobile/15E148 Safari/604.1\n";
            File.WriteAllText(url, str);
        }

        public static void AddDemoAccount(string url)
        {
            var str = "0|id|pass|name|golike|passgolike|hana|passhana|2Fa|Cookie|False|False|False|False|Bắt đầu|1\n";
            File.WriteAllText(url, str);
        }
    }

}
