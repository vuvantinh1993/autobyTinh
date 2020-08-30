using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public static class XLFile
    {
        public static List<ModelAccount> DocFileTaiKhoan(string url)
        {
            var listAcc = new List<ModelAccount>();
            if (File.Exists(url) && File.Exists("config/userAgent.txt"))
            {
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
            MessageBox.Show("bạn phải cấu hình 2 file userAgent và danh sách tài khoản");
            return listAcc;
        }
    }

}
