﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net;

namespace TestTheMongo {
    [TestClass]
    public class UnitTest1 {
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        [TestMethod]
        public void TestRest() {
        }
        [TestMethod]
        public void AddSomeProducts() {
            var unit = new RestDBInterface();            
            
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
            return new Product() { MainCategory = main, SecondaryCategory = second, Name = parts[2], Image = imgBytes};
        }
    }
}
