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
            if (!System.IO.File.Exists(urlUserAgent))
            {
                var file = System.IO.File.Create(urlUserAgent);
                file.Close();
                AddUserAgent(urlUserAgent);
            }
            if (!System.IO.File.Exists(url))
            {
                var file = System.IO.File.Create(url);
                file.Close();
                var str = "0|id|pass|name|golike|passgolike|hana|passhana|2Fa|Cookie|True|True|True|True|Bắt đầu\n";
                System.IO.File.WriteAllText(url, str);
            }

            var accounts = System.IO.File.ReadAllLines(url);
            var listUserAgent = System.IO.File.ReadAllLines("config/userAgent.txt");
            foreach (var account in accounts)
            {
                var item = account.Split('|');

                listAcc.Add(new ModelAccount
                {
                    Stt = item[(int)VitriGhiEnum.stt],
                    Id = item[(int)VitriGhiEnum.tendangnhap],
                    Pass = item[(int)VitriGhiEnum.matkhau],
                    Fa = item[(int)VitriGhiEnum.matKhau2Fa],
                    Cookie = item[(int)VitriGhiEnum.cookie],
                    Name = item[(int)VitriGhiEnum.tennguoidung],
                    An = bool.Parse(item[(int)VitriGhiEnum.AnChrome]),
                    Stop = bool.Parse(item[(int)VitriGhiEnum.TamDung]),
                    Action = item[(int)VitriGhiEnum.TrangThai],
                    NameTDS = item[(int)VitriGhiEnum.tenTDS],
                    PassTDS = item[(int)VitriGhiEnum.passTDS],
                    RunTDS = bool.Parse(item[(int)VitriGhiEnum.RunTDS])
                    //Golike = item[(int)VitriGhiEnum.g],
                    //PassGolike = item[5],
                    //Hana = item[6],
                    //PassHana = item[7],
                    //RunHana = bool.Parse(item[10]),
                    //RunGolike = bool.Parse(item[11]),
                    //DKhana = "Đăng kí",
                    //UserAgent = Convert.ToInt32(item[15]) <= listUserAgent.Count() ? listUserAgent[Convert.ToInt32(item[15]) - 1] : listUserAgent[0],
                    //BackUp = "BackUp"
                });
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
            System.IO.File.WriteAllText(url, str);
        }
    }

}
