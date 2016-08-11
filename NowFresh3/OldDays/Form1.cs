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
            InitializeComponent();
            AddSomeProducts();
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox2.TextChanged += TextBox2_TextChanged;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) {
            var collection = _database.GetCollection<BsonDocument>("products");
            MainCategoryTypes res;
            if (!Enum.TryParse(((TextBox)sender).Text, out res))
                return;
            var filter = Builders<BsonDocument>.Filter.Eq("_main", res);
            var result = collection.Find(filter).ToList();
            if (result.Count > 0) {
                pictureBox1.Image = Image.FromStream(new MemoryStream((BsonSerializer.Deserialize<Product>(result[0]).Image)));
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e) {
            if (((TextBox)sender).Text == string.Empty)
                return;
           var data =  new WebClient().DownloadString("http://localhost:51612/Api/Students/"+ ((TextBox)sender).Text);
            data = data.Replace("[",string.Empty);
            data = data.Replace("]", string.Empty);
            if (data == "null")
                return;
            dynamic parsed = JObject.Parse(data);
            string base64 = parsed.Image;
            if (base64 == null)
                return;
            base64 = base64.Replace("{", string.Empty);
            base64 = base64.Replace("}", string.Empty);
            var de = Convert.FromBase64String(base64);
            pictureBox1.Image = Image.FromStream(new MemoryStream(de));
        }

        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public static void AddSomeProducts() {
                var unit = new RestUnitOfWork();
                var lines = File.ReadAllLines(FolderPath + "Products_Table.csv").ToList();
               _database.DropCollection("products");

                lines.ForEach((line) => AddProduct(unit,line));
        }

        public static void AddProduct(RestUnitOfWork unit, string line) {
            var parts = line.Split(',');
            var collection = _database.GetCollection<BsonDocument>("products");
            unit.Products.Add(GetProduct(parts));
        }

        public static Product GetProduct(string[] parts) {
            var main = (MainCategoryTypes)Enum.Parse(typeof(MainCategoryTypes), parts[0]);
            var second = (SecondaryCategoryTypes)Enum.Parse(typeof(SecondaryCategoryTypes), parts[1]);
            var imgPath = FolderPath + parts[2] + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            return new Product() { MainCategory = main, SecondaryCategory = second, SpeciesName = parts[2], Image = imgBytes};
        }
    }
}
