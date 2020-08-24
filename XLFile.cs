using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autohana
{
    public static class XLFile
    {
        public static List<ModelAccount> DocFileTaiKhoan(string url)
        {
            var listAcc = new List<ModelAccount>();
            var accounts = File.ReadAllLines(url);
            foreach (var account in accounts)
            {
                var item = account.Split('|');
                if (item.Count() == 15)
                {
                    listAcc.Add(new ModelAccount
                    {
                        Stt = item[0],
                        Id = item[1],
                        Pass = item[2],
                        Fa = item[3],
                        Cookie = item[4],
                        Name = item[5],
                        Golike = item[6],
                        PassGolike = item[7],
                        Hana = item[8],
                        PassHana = item[9],
                        RunHana = bool.Parse(item[10]),
                        RunGolike = bool.Parse(item[11]),
                        An = bool.Parse(item[12]),
                        Stop = bool.Parse(item[13]),
                        Action = item[14],
                        DKhana = "Đăng kí",
                    });
                }
            }

            return listAcc;
        }
    }

}
