using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO.Compression;
using System.Threading;

namespace ConsoleApplication1 {
    
    class Program {
     
        public enum ArgsOptions {
            download
        }
      
        public static void DownloadCompanyFullPrices(CompanyEnum company) {
           Download.GetShopIds(company).ForEach((id) => Download.DownloadFullPriceFile(company, id));
        }

        public static void DownloadAll() {
            foreach (var company in DataStructures.LoginDetails.Keys) {
                DownloadCompanyFullPrices(company);
            }
        }

        static void Main(string[] args) {
            if (args.Length > 0)
                if (args[0] == ArgsOptions.download.ToString())
                    DownloadAll();
            List<Dictionary<long, ItemData>> storesData = new List<Dictionary<long, ItemData>>();

            List<string> files = Directory.EnumerateFiles(Download.FolderPath, "*.xml").ToList<string>();
            List<List<long>> ids = new List<List<long>>();
            files.ForEach((file) => ids.Add(Download.GetIDS(File.ReadAllText(file), "ItemCode")));
            List<long> equals = new List<long>();
            for (int i = 0; i < ids[0].Count; i++) {
                var allEquals = true;
                for (int j = 1; j < files.Count; j++) {
                    if (!ids[j].Contains(ids[0][i]))
                        allEquals = false;
                }
                if (allEquals)
                    equals.Add(ids[0][i]);
            }
            Console.WriteLine(string.Format("Matching id's : {0}", equals.Count.ToString()));
            foreach (var file  in files) {
                var data = Download.GetData(equals, File.ReadAllText(file));
                storesData.Add(data);
                double total = 0;
                foreach (var value in data.Values) {
                    total += value.Price;
                }
                Console.WriteLine(Path.GetFileName(file) +"  :  " + total);
            }
        }
      
    }
}

