using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autohana
{
    public static class DocGhiFile
    {
        //public static string teamplate = ""

        public static void Write(string nameFolder, string content)
        {
            var path = $"{Environment.CurrentDirectory}\\log\\{nameFolder}.txt";

            if (!System.IO.File.Exists(path))
            {
                var file = System.IO.File.Create(path);
                file.Close();
            }
            var stringWrite = $"{DateTime.Now} {content} \n";
            System.IO.File.AppendAllText(path, stringWrite);
        }

        public static void GhiFileBackUpFromString(string uid, string content, string namefile, string typeFile, bool isImageOneFriend = false, bool isWriteContinue = true)
        {
            if (!Directory.Exists("BackUp"))
            {
                Directory.CreateDirectory("BackUp");
            }
            if (!Directory.Exists($"BackUp/{uid}"))
            {
                Directory.CreateDirectory($"BackUp/{uid}");
            }
            if (isImageOneFriend)
            {
                if (!Directory.Exists($"BackUp/{uid}/anhbanbe"))
                {
                    Directory.CreateDirectory($"BackUp/{uid}/anhbanbe");
                }
                if (!System.IO.File.Exists($"BackUp/{uid}/{namefile}/anhbanbe/{uid}.{typeFile}"))
                {
                    var file = System.IO.File.Create($"BackUp/{uid}/{namefile}/anhbanbe/{uid}.{typeFile}");
                    file.Close();
                }
                System.IO.File.WriteAllText($"BackUp/{uid}/{namefile}/anhbanbe/{uid}.{typeFile}", content);
            }
            else
            {
                if (!System.IO.File.Exists($"BackUp/{uid}/{namefile}.{typeFile}"))
                {
                    var file = System.IO.File.Create($"BackUp/{uid}/{namefile}.{typeFile}");
                    file.Close();
                }
                System.IO.File.WriteAllText($"BackUp/{uid}/{namefile}.{typeFile}", content);
            }
        }

        // Backup các UId của bạn bè và ghi tiếp vào file listfriend
        public static void GhiFileBackUpListUid(string namFolder, string namefile, string typeFile, List<string> content)
        {
            if (!Directory.Exists($"{namFolder}"))
            {
                Directory.CreateDirectory($"{namFolder}");
            }
            if (!System.IO.File.Exists($"{namFolder}/{namefile}.{typeFile}"))
            {
                var file = System.IO.File.Create($"{namFolder}/{namefile}.{typeFile}");
                file.Close();
            }
            System.IO.File.WriteAllLines($"{namFolder}/{namefile}.{typeFile}", content);
        }

        public static void GhiFileBackUpListImageFriends(string namFolder, string namefile, string typeFile, List<string> content, bool isWriteContinue = true)
        {
            if (!Directory.Exists($"{namFolder}"))
            {
                Directory.CreateDirectory($"{namFolder}");
            }
            if (!System.IO.File.Exists($"{namFolder}/{namefile}.{typeFile}"))
            {
                var file = System.IO.File.Create($"{namFolder}/{namefile}.{typeFile}");
                file.Close();
            }
            System.IO.File.WriteAllLines($"{namFolder}/{namefile}.{typeFile}", content);
        }

        public static void GhiFileHTMLMoCheckPoint(string namefile, string uid, List<string> listUidNewBackUp)
        {
            if (!System.IO.File.Exists($"{namefile}"))
            {
                var file = System.IO.File.Create($"{namefile}");
                file.Close();
                System.IO.File.WriteAllText($"{namefile}", TemplateBackUp());
            }
            var teamplate = System.IO.File.ReadAllText($"{namefile}");
            var listNameFileAnhbanbe = Directory.GetFiles($"BackUp/{uid}/anhbanbe");
            var listFileNewAnhbanbe = listNameFileAnhbanbe.Where(x => listUidNewBackUp.Contains(x.Split('_')[1])).ToList();
            foreach (var nameFileAnhBanBe in listFileNewAnhbanbe)
            {
                teamplate = teamplate.Replace("noidungtiep", TempplateOneFriend(nameFileAnhBanBe));
            }
            System.IO.File.WriteAllText($"{namefile}", teamplate);
        }

        public static List<string> DocFileDSUid(string urlFile)
        {
            if (!System.IO.File.Exists($"{urlFile}"))
            {
                var file = System.IO.File.Create($"{urlFile}");
                file.Close();
            }
            return File.ReadAllLines($"{urlFile}").ToList();
        }

        public static void CreadFolder(string pathFolder)
        {
            if (!Directory.Exists($"{pathFolder}"))
            {
                Directory.CreateDirectory($"{pathFolder}");
            }
        }

        private static string TemplateBackUp(string name = "", string Uid = "")
        {
            var str = "<!DOCTYPE html>\n <html lang = \"en\" >\n" +
                "<head>\n <meta charset = \"UTF-8\" ><meta name = \"viewport\" content = \"width=device-width, initial-scale=1.0\"> \n" +
                "<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\" integrity = \"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm\" crossorigin = \"anonymous\" > \n" +
                "<script src = \"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js\"></script>\n" +
                $" <title> BackUp</title>" +
                "</head>\n";
            str = str + "<body>" +
                " <div class=\"row mb-3 align-items-center\">" +
                 "<textarea id=\"userName\" rows=\"4\" cols=\"50\"  class=\"ml-5 mr-5\"> </textarea> \n" +
                 "<button onclick=\"laygiatri()\">Tìm kiếm</button>\n" +
                 "</div>\n";

            str += "noidungtiep\n";


            // ddoanj scrip
            str = str + "<script>" +
                "function laygiatri() {\n" +
                "var input = $('#userName').val();" + "\n" +
                "if (bodau(input) != '') {" + "\n" +
                "var listUserFind = input.split('\\n');" + "\n" +
                "var listUserFindTrim = [];" + "\n" +
                "for (let i = 0; i < listUserFind.length; i++) {" + "\n" +
                "listUserFindTrim.push(bodau(listUserFind[i]).toUpperCase(0));" + "\n" +
                "}" + "\n" +
                "var listUser = $('.user').attr(\"hidden\", true);" + "\n" +
                "var list = $('.user').find('.tinh');" + "\n" +
                "for (let i = 0; i < list.length; i++) {" + "\n" +
                "if (listUserFindTrim.includes(bodau(list[i].textContent).toUpperCase())) {" + "\n" +
                "listUser[i].removeAttribute('hidden')" + "\n" +
                "}" + "\n" +
                "}" + "\n" +
                "} else {" + "\n" +
                "var listUser = $('.user').removeAttr(\"hidden\");" + "\n" +
                "}" + "\n" +
                "}" + "\n";

            str = str + "function bodau(str) {" + "\n" +
                "str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, \"a\");" + "\n" +
                " str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, \"e\");" + "\n" +
                " str = str.replace(/ì|í|ị|ỉ|ĩ/g, \"i\");" + "\n" +
                "str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, \"o\");" + "\n" +
                "str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, \"u\");" + "\n" +
                "str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, \"y\");" + "\n" +
                "str = str.replace(/đ/g, \"d\");" + "\n" +
                "str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, \"A\");" + "\n" +
                " str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, \"E\");" + "\n" +
                "str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, \"I\");" + "\n" +
                "str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, \"O\");" + "\n" +
                "str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, \"U\");" + "\n" +
                "str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, \"Y\");" + "\n" +
                "str = str.replace(/Đ/g, \"D\");" + "\n" +
                "str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, \"\");" + "\n" +
                "str = str.replace(/\u02C6|\u0306|\u031B/g, \"\");" + "\n" +
                " str = str.replace(/\\s+/g, '');" + "\n" +
                "return str;" + "\n" +
                " }" + "\n" +
                " </script>" + "\n";
            str = str + "</body>\n" +
               "</html>";
            return str;
        }
        private static string TempplateOneFriend(string nameFileAnhBanBe)
        {
            Common.DelayMiliSeconde(1);
            var uid = nameFileAnhBanBe.Split('_')[1];
            var name = nameFileAnhBanBe.Split('_')[2].Replace(".txt", "");
            var str = "<div class=\"user row mb-3 align-items-center\">\n" +
                "<div class=\"col-1\">\n" +
                $"<a href=\"https://www.facebook.com/{uid}\" target=\"_blank\" class=\"tinh\">{name}</a>\n " +
                "</div>\n" +
                "<div class=\"col-1\">\n" +
                $"<img src=\"http://graph.facebook.com/{uid}/picture?type=large\" width=\"100%\">\n" +
                "</div>\n" +
                "<div class=\"col-10\">";
            var listImg = System.IO.File.ReadAllLines(nameFileAnhBanBe);
            foreach (var img in listImg)
            {
                str += $"<img src=\"{img}\" height=\"150px\" class=\"mb-2\">\n";
            }
            str += "</div>\n";
            str += "</div>\n";
            str += "noidungtiep\n";
            return str;
        }

        //private void ConvertData_Click(object sender, EventArgs e)
        //{
        //    var accounts = System.IO.File.ReadAllLines("config/fileold.txt");
        //    List<string> listAccountNew = new List<string>();
        //    foreach (var item in accounts)
        //    {
        //        var splitItem = item.Split('|');
        //        if (splitItem.Count() == 8)
        //        {
        //            var c0 = splitItem[0];
        //            var c1 = splitItem[1];
        //            var c2 = splitItem[2];
        //            var c3 = splitItem[3];
        //            var c4 = splitItem[4]; // cookie
        //            var c5 = splitItem[5];
        //            var c6 = splitItem[6];
        //            var c7 = splitItem[7];
        //            var c8 = this.uidAddHana.Text;
        //            var c9 = this.passAddHana.Text;
        //            var c10 = "True";
        //            var c11 = "False";
        //            var c12 = "False";
        //            var c13 = "False";
        //            var c14 = "Bắt đầu";

        //            var str = c0 + '|' + c1 + '|' + c2 + '|' + c3 + '|' + c4 + '|' + c5 + '|' + c6 + '|' + c7 + '|' + c8 + '|' + c9 + '|' + c10 + '|' + c11 + '|' + c12 + '|' + c13 + '|' + c14;
        //            listAccountNew.Add(str);
        //        }
        //    }
        //    if (!System.IO.File.Exists("config/accounts.txt"))
        //    {
        //        var file = System.IO.File.Create("config/accounts.txt");
        //        file.Close();
        //        System.IO.File.WriteAllLines("config/accounts.txt", listAccountNew);
        //    }
        //    else
        //    {
        //        System.IO.File.AppendAllLines("config/accounts.txt", listAccountNew);
        //    }
        //}


        public static void OpenFileBackUp(string uid)
        {
            Process.Start("explorer", $"{Environment.CurrentDirectory}\\BackUp\\{uid}");
        }

    }

    public class UidAndName
    {
        public string Uid { get; set; }
        public string Name { get; set; }
    }
}
