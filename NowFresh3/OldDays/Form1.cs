using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) {
            var collection = _database.GetCollection<BsonDocument>("products");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ((TextBox)sender).Text);
            var result = collection.Find(filter).ToList();
            if (result.Count > 0) {
                pictureBox1.Image = Image.FromStream(new MemoryStream((BsonSerializer.Deserialize<Product>(result[0]).Image)));
            }
        }

        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public void AddSomeProducts() {
            var unit = new RestUnitOfWork();

            var lines = File.ReadAllLines(FolderPath + "Products_Table.csv").ToList();
            lines.ForEach((line) => unit.Products.Add(GetProduct(line)));
        }

        public static Product GetProduct(string dataLine) {
            var parts = dataLine.Split(',');
            var main = (MainCategoryTypes)Enum.Parse(typeof(MainCategoryTypes), parts[0]);
            var second = (SecondaryCategoryTypes)Enum.Parse(typeof(SecondaryCategoryTypes), parts[1]);
            var imgPath = FolderPath + parts[2] + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            return new Product() { MainCategory = main, SecondaryCategory = second, SpeciesName = parts[2], Image = imgBytes };
        }
    }
}
