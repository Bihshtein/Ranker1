using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldDays {
    public partial class Form1 : Form {
        static MongoClient _client = new MongoClient();
        static IMongoDatabase _database = _client.GetDatabase("test");
        public Form1() {
            var aleg = new WebClient();
            var ndbno = 12061;
            var apiKey = "fo9SmQOhJvGXLkWqXbFUGFEb89SYgVgTA1CkEbua";
            var format = "json";
            var str = "http://api.nal.usda.gov/ndb/reports/?ndbno={0}&type=f&format={1}&api_key={2}";
            str = string.Format(str, ndbno,format, apiKey);
            var data = aleg.DownloadString(str);
            
            InitializeComponent();
            AddSomeProducts();
        }


        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public static void AddSomeProducts() {
                var unit = new RestDBInterface();
                var lines = File.ReadAllLines(FolderPath + "Products_IDS.csv").ToList();
               _database.DropCollection("products");

                lines.ForEach((line) => AddProduct(unit,line));
        }

        public static void AddProduct(RestDBInterface unit, string line) {
            var parts = line.Split(',');
            var collection = _database.GetCollection<BsonDocument>("products");
            unit.Products.Add(GetProduct(parts[0]));
        }

        public static Product GetProduct(string name) {
            var imgPath = FolderPath + name + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            return new Product() {  Name = name, Image = imgBytes};
        }
    }
}
