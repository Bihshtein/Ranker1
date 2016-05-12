﻿using System;
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
        
        public static Dictionary<CompanyEnum, CompanyData> LoginDetails = new Dictionary<CompanyEnum, CompanyData>() {
            { CompanyEnum.TivTaam, new CompanyData(7290873255550,"TivTaam", string.Empty, 7290873255550,"2005") },
            { CompanyEnum.RamiLevi, new CompanyData(7290058140886,"RamiLevi", string.Empty, 7290058140886,"2005") },
            { CompanyEnum.FreshMarket, new CompanyData(7290876100000,"freshmarket", "f_efrd", 7290876100000 ,"0900") }

        };
        static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\prices\";
        public static string LoginUrl = "https://url.publishedprices.co.il/login/user";
        public class CompanyData {
            public long PriceFileID { get; set; }
            public long StoreFileID { get; set; }
            public string StoreFileSuffix { get; set; }
            public string Usermame { get; set; }
            public string Password { get; set; }
            public CompanyData(long priceFileID, string usermame, string password, long storeFileID, string storeFileSuffix) {
                this.PriceFileID = priceFileID;
                this.Usermame = usermame;
                this.Password = password;
                this.StoreFileID = storeFileID;
                this.StoreFileSuffix = storeFileSuffix;
            }
        }
        public class CookieAwareWebClient : WebClient {        

            public CookieAwareWebClient(CookieContainer container) {
                CookieContainer = container;
            }


            public void Connect(CompanyEnum company) {
                var loginData = LoginDetails[company];
                string loginStr = string.Format("&username={0}&password={1}&", loginData.Usermame, loginData.Password);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                UploadString(LoginUrl, "POST", loginStr);
            }
            public CookieAwareWebClient()
              : this(new CookieContainer()) { }

            public CookieContainer CookieContainer { get; private set; }

            protected override WebRequest GetWebRequest(Uri address) {
                var request = (HttpWebRequest)base.GetWebRequest(address);
                request.CookieContainer = CookieContainer;
                return request;
            }
        }

        public enum CompanyEnum {
            TivTaam,
            RamiLevi, 
            FreshMarket
        }

    public static void DownloadFullPriceFile(CompanyEnum company, long storeNum) {
            Console.WriteLine(string.Format("Downloading for company {0} store num {1}",company, storeNum));
            var client = new CookieAwareWebClient();
            client.Connect(company);

            var loginData = LoginDetails[company];
            string loginStr = string.Format("&username={0}&password={1}&", loginData.Usermame, loginData.Password);
            
            var shopNumStr = storeNum.ToString().PadLeft(3, '0');
            var date = DateTime.Today.ToString("yyyyMMdd");
            
            var fileUrl = string.Format("https://url.publishedprices.co.il/file/d/PriceFull{0}-{1}-{2}0010.gz", loginData.PriceFileID, shopNumStr, date);
            
            try {
                var readStream = new MemoryStream(client.DownloadData(fileUrl));
                GZipStream uncompressed = new GZipStream(readStream, CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int nRead;
                var writeStream = new FileStream(FolderPath + string.Format("{0}_shopnum_{1}_date_{2}.xml", company, shopNumStr, date), FileMode.Create);
                while ((nRead = uncompressed.Read(buffer, 0, buffer.Length)) > 0) {
                    writeStream.Write(buffer, 0, nRead);
                }
                uncompressed.Flush();
                uncompressed.Close();
                writeStream.Dispose();
                readStream.Dispose();
            }
            catch (WebException) {
                Console.WriteLine((string.Format("Skipping for company {0} store num {1}", company, storeNum)));
            }
       

    }


        public static List<long> GetShopIds(CompanyEnum company) {
            var loginData = LoginDetails[company];
            var client = new CookieAwareWebClient();
            client.Connect(company);
            var date = DateTime.Today.ToString("yyyyMMdd");
            var fileUrl = string.Format("https://url.publishedprices.co.il/file/d/Stores{0}-{1}{2}.xml", loginData.StoreFileID, date, loginData.StoreFileSuffix);
            return GetIDS(client.DownloadString(fileUrl), "StoreId");
          

        }

        public static void DownloadCompanyFullPrices(CompanyEnum company) {
            GetShopIds(company).ForEach((id) => DownloadFullPriceFile(company, id));
        }

        public static void DownloadAll() {
            foreach (var company in LoginDetails.Keys) {
                DownloadCompanyFullPrices(company);
            }
        }

        static void Main(string[] args) {
            //DownloadAll();
            foreach (var company in LoginDetails.Keys) {
                List<string> files = Directory.EnumerateFiles(FolderPath, company.ToString() + "*.xml").ToList<string>();
                List<List<long>> ids = new List<List<long>>();
                files.ForEach((file) => ids.Add(GetIDS(File.ReadAllText(file), "ItemCode")));
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
                string str = string.Format("Matching id's : {0}, company {1}", equals.Count.ToString(), company.ToString());
                Console.WriteLine(str);
                PrintNames(equals, File.ReadAllText(files[0]),company);
            }

        }

        public static List<long> GetIDS(string str, string idFieldName) {
            var reader = XmlReader.Create(new StringReader(str));
            List<long> ids = new List<long>();
            while (reader.ReadToFollowing(idFieldName)) {
                reader.Read();
                ids.Add(reader.ReadContentAsLong());
            }
            return ids;
        }

        public static void PrintNames(List<long> ids, string str, CompanyEnum company) {
            File.WriteAllText(FolderPath + "matching_names.txt", ids.Count + "matching");
            var reader = XmlReader.Create(new StringReader(str));
            while (reader.ReadToFollowing("ItemCode")) {
                reader.Read();
                var id = reader.ReadContentAsLong();
                if (ids.Contains(id)) {
                    reader.ReadToFollowing("ItemName");
                    reader.Read();
                    File.AppendAllText(FolderPath + company.ToString()  + ".txt ", reader.ReadContentAsString() + "\r\n");
                }
            }

        }
    }
}

