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
            var files = new DirectoryInfo(Download.FolderPath).GetFiles().OrderByDescending(x => x.Length).Take(6).ToList<FileInfo>();
            List<List<long>> ids = new List<List<long>>();
            files.ForEach((file) => ids.Add(Download.GetIDS(File.ReadAllText(file.FullName), "ItemCode")));
            var equals = Logic.GetEquals(ids, files.Count);
            var storesData = Logic.GetData(files, equals);
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

