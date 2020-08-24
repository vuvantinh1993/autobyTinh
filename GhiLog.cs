using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autohana
{
    public static class GhiLog
    {
        public static void Write(string nameFolder, string content)
        {
            var path = $"{Environment.CurrentDirectory}\\log\\{nameFolder}.txt";

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }
            var stringWrite = $"{DateTime.Now} {content} \n";
            File.AppendAllText(path, stringWrite);
        }
    }
}
