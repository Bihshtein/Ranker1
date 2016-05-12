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


namespace ConsoleApplication1 {
    
    class Program {
        
        public static Dictionary<CompanyEnum, CompanyData> LoginDetails = new Dictionary<CompanyEnum, CompanyData>() {
            { CompanyEnum.TivTaam, new CompanyData(7290873255550,"TivTaam", string.Empty) },
            { CompanyEnum.RamiLevi, new CompanyData(7290058140886,"RamiLevi", string.Empty) },
            { CompanyEnum.FreshMarket, new CompanyData(7290873255550,"freshmarket", "f_efrd") }

        };
        public class CompanyData {
            public long FileID { get; set; }
            public string Usermame { get; set; }
            public string Password { get; set; }
            public CompanyData(long fileID, string usermame, string password) {
                this.FileID = fileID;
                this.Usermame = usermame;
                this.Password = password;
            }
        }
        public class CookieAwareWebClient : WebClient {        

            public CookieAwareWebClient(CookieContainer container) {
                CookieContainer = container;
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

    public static void GetFile(CompanyEnum company, int shopNum) {
            var loginData = LoginDetails[company];
            string loginData1 = string.Format("username={0}&password={1}", loginData.Usermame, loginData.Password);
            
            var shopNumStr = shopNum.ToString().PadLeft(3, '0');
            var date = DateTime.Today.ToString("yyyyMMdd");
            
            var fileUrl = string.Format("https://url.publishedprices.co.il/file/d/PriceFull{0}-{1}-{2}0010.gz", loginData.FileID, shopNumStr, date);
            var client = new CookieAwareWebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            client.UploadString("https://url.publishedprices.co.il/login/user", "POST", loginData1);            
            var writeStream = new FileStream(path + string.Format("{0}_shopnum_{1}_date_{2}.xml", company, shopNumStr, date), FileMode.Create);
            var readStream = new MemoryStream(client.DownloadData(fileUrl));
            GZipStream uncompressed = new GZipStream(readStream, CompressionMode.Decompress);
            byte[] buffer = new byte[1024];
            int nRead;
            while ((nRead = uncompressed.Read(buffer, 0, buffer.Length)) > 0) {
                writeStream.Write(buffer, 0, nRead);
            }
            uncompressed.Flush();
            uncompressed.Close();

            writeStream.Dispose();
            readStream.Dispose();

        }

        static string path = Assembly.GetExecutingAssembly().Location + @"\..\..\..\prices\";
        static void Main(string[] args) {


            GetFile(CompanyEnum.TivTaam, 2);
            GetFile(CompanyEnum.RamiLevi, 2);
           // GetFile(CompanyEnum.FreshMarket, 2);
            Console.OutputEncoding = new UTF8Encoding();
            List<string> files = new List<string>() { path + "prices_koop_herzelia_08_05_2016.xml" ,
                                                      path + "prices_shufer_raanana_09_05_2017.xml",
                                                      path + "prices_koop_ashdod_09_05_2017.xml"};


            List<List<long>> ids = new List<List<long>>();
            files.ForEach((file) => ids.Add(GetIDS(File.ReadAllText(file))));
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
            Console.WriteLine(string.Format("\n\nMatching id's :  {0} \n\nWe still don't know who they are yet we know they are matching!!!\n\n", equals.Count));

            PrintNames(equals, File.ReadAllText(files[1]));

        }

        public static List<long> GetIDS(string str) {
            var reader = XmlReader.Create(new StringReader(str));
            List<long> ids = new List<long>();
            while (reader.ReadToFollowing("ItemCode")) {
                reader.Read();
                ids.Add(reader.ReadContentAsLong());
            }
            return ids;
        }

        public static void PrintNames(List<long> ids, string str) {
            File.WriteAllText(path + "matching_names.txt", ids.Count + "matching");
            var reader = XmlReader.Create(new StringReader(str));
            while (reader.ReadToFollowing("ItemCode")) {
                reader.Read();
                var id = reader.ReadContentAsLong();
                if (ids.Contains(id)) {
                    reader.ReadToFollowing("ItemName");
                    reader.Read();
                    File.AppendAllText(path + "matching_names.txt ", reader.ReadContentAsString() + "\r\n");
                }
            }

        }
    }
}

