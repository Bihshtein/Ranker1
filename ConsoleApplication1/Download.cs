using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1 {
    public class Download {
        public static string LoginUrl = "https://url.publishedprices.co.il/login/user";
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\prices\";

        public static List<long> GetIDS(string str, string idFieldName) {
            var aleg = new Stopwatch();
            aleg.Start();
            var reader = XmlReader.Create(new StringReader(str));
            List<long> ids = new List<long>();
            while (reader.ReadToFollowing(idFieldName)) {
                reader.Read();
                ids.Add(reader.ReadContentAsLong());
            }
            aleg.Stop();
            Console.WriteLine(string.Format("Generating  id's for : {0}, calcualtion time {1} seconds ", idFieldName, aleg.Elapsed.TotalSeconds));
            return ids;
        }

        public static void DownloadFullPriceFile(CompanyEnum company, long storeNum) {
            Console.WriteLine(string.Format("Downloading for company {0} store num {1}", company, storeNum));
            var client = new CookieAwareWebClient();
            client.Connect(company);

            var loginData = DataStructures.LoginDetails[company];
            string loginStr = string.Format("&username={0}&password={1}&", loginData.Usermame, loginData.Password);

            var shopNumStr = storeNum.ToString().PadLeft(3, '0');
            var date = DateTime.Today.ToString("yyyyMMdd");

            var fileUrl = string.Format("https://url.publishedprices.co.il/file/d/PriceFull{0}-{1}-{2}0010.gz", loginData.PriceFileID, shopNumStr, date);

            try {
                var readStream = new MemoryStream(client.DownloadData(fileUrl));
                GZipStream uncompressed = new GZipStream(readStream, CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int nRead;
                var writeStream = new FileStream(Download.FolderPath + string.Format("{0}_shopnum_{1}_date_{2}.xml", company, shopNumStr, date), FileMode.Create);
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
            var loginData = DataStructures.LoginDetails[company];
            var client = new CookieAwareWebClient();
            client.Connect(company);

            var date = DateTime.Today.ToString("yyyyMMdd");
            if (company == CompanyEnum.Keshet)
                date = "20150614";

            var fileUrl = string.Format("https://url.publishedprices.co.il/file/d/Stores{0}-{1}{2}.xml", loginData.StoreFileID, date, loginData.StoreFileSuffix);
            string data;
            try {
                data = client.DownloadString(fileUrl);
            }
            catch (WebException) {// stupid retry logic look at -1 day better to have some retry
                date = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
                fileUrl = string.Format("https://url.publishedprices.co.il/file/d/Stores{0}-{1}{2}.xml", loginData.StoreFileID, date, loginData.StoreFileSuffix);
                data = client.DownloadString(fileUrl);
            }
            return Download.GetIDS(data, "StoreId");
        }
        
        public static Dictionary<long, ItemData> GetData(List<long> ids, string str) {
            File.WriteAllText(Download.FolderPath + "matching_names.txt", ids.Count + "matching");
            var dataDict = new Dictionary<long, ItemData>();
            var reader = XmlReader.Create(new StringReader(str));
            while (reader.ReadToFollowing("ItemCode")) {
                reader.Read();
                var id = reader.ReadContentAsLong();
                if (ids.Contains(id)) {
                    reader.ReadToFollowing("ItemName");
                    reader.Read();
                    var name = reader.ReadContentAsString();

                    reader.ReadToFollowing("Quantity");
                    reader.Read();
                    var quantity = reader.ReadContentAsDouble();


                    reader.ReadToFollowing("ItemPrice");
                    reader.Read();
                    var price = reader.ReadContentAsDouble();
                    if (dataDict.ContainsKey(id))
                        File.AppendAllText(Download.FolderPath + "debug.txt", string.Format("hmm already has ::  {0}, is  :: {1} the same? /n/r", dataDict[id].Name, name));
                    else
                        dataDict.Add(id, new ItemData(name, quantity, price));
                }
            }
            return dataDict;
        }
    }



    public class CookieAwareWebClient : WebClient {

        public CookieAwareWebClient(CookieContainer container) {
            CookieContainer = container;
        }


        public void Connect(CompanyEnum company) {
            var loginData = DataStructures.LoginDetails[company];
            string loginStr = string.Format("&username={0}&password={1}&", loginData.Usermame, loginData.Password);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            UploadString(Download.LoginUrl, "POST", loginStr);
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
   
}
