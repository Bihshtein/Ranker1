using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InitMenus {
    class Program {
        static void Main(string[] args) {
            var setting = new XmlReaderSettings();
            setting.DtdProcessing = DtdProcessing.Parse;
            var page = new WebClient().DownloadString("http://allrecipes.com/recipe/240744/");
            var parts = page.Split(new string[1] { "itemprop=\"ingredients\">"}, StringSplitOptions.None);
            var ingredients = new List<string>();
            for (int i = 1; i < parts.Length-1; i++) {
                var chars = parts[i].TakeWhile(a => a != '<');
                ingredients.Add(new String(chars.ToArray()));
            }
            ingredients.ForEach(a => Console.WriteLine(a));



        }
    }
}
