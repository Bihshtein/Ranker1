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
using System.Globalization;

namespace ConsoleApplication1 {
    
    class Program {
     
        public enum ArgsOptions {
            download
        }

        static void Main(string[] args) {
            if (args.Length > 0)
                if (args[0] == ArgsOptions.download.ToString())
                    DownloadAll();
            var files = new DirectoryInfo(Download.FolderPath).GetFiles().OrderByDescending(x => x.Length).Take(38).ToList<FileInfo>();
            var fileCount = 0;
            var storesData = Logic.GetData(files);
            while (storesData.Count > 1) { 
                var equals = Logic.GetEquals<long>(storesData);
                var list = new List<List<string>>();
                for (int i = 0; i < equals.Count; i++) {
                    list.Add(new List<string>());
                    equals[i].ForEach(item => list[i].Add(item.Name));
                    list[i].Sort();
                    for (int j = 0; j < list[i].Count; j++)
                        File.WriteAllLines(Download.FolderPath + fileCount + "_last_matching.txt", list[i]);
                    fileCount++;
                }
                storesData.RemoveRange(0, 2);
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

