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
           
        static void Main(string[] args) {
            if (args.Length > 0)
                if (args[0] == ArgsOptions.download.ToString())
                    DownloadAll();
            var files = new DirectoryInfo(Download.FolderPath).GetFiles().OrderByDescending(x => x.Length).Take(50).ToList<FileInfo>();
            List<List<long>> ids = new List<List<long>>();
            files.ForEach((file) => ids.Add(Download.GetIDS(File.ReadAllText(file.FullName), "ItemCode")));
            var equals = Logic.GetEquals(ids, files.Count);
            var storesData = Logic.GetData(files, equals);
            File.WriteAllText(Download.FolderPath + "last_matching.txt", string.Format("matching :: {0}", storesData[0].Values.Count));

            foreach (var item in storesData[0].Values) {
                File.AppendAllText(Download.FolderPath + "last_matching.txt", string.Format("{0} :: {1} :: {2} \n", item.Name, item.Price, item.Quantity));

            }
        }

        public static void DownloadCompanyFullPrices(CompanyEnum company) {
            Download.GetShopIds(company).ForEach((id) => Download.DownloadFullPriceFile(company, id));
        }

        public static void DownloadAll() {
            foreach (var company in DataStructures.LoginDetails.Keys) {
                DownloadCompanyFullPrices(company);
            }
        }
    }
}

