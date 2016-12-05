﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InitRecipes {
    class Program {
        static void Main(string[] args) {

            for (int i = 220000; i < 300000; ++i) {
                ParseRecipe(i.ToString());
            }
        }

        public static void ParseRecipe(string id) {
            string page = string.Empty;
            try {
                page = new WebClient().DownloadString("http://allrecipes.com/recipe/" + id);
            }
            catch { return; }
            var nameParts = page.Split(new string[2] { "<title>", "</title>" }, StringSplitOptions.None);
            Console.WriteLine("Num : " + id);
            Console.WriteLine("Title : " + nameParts[1]);
            var ingredientParts = page.Split(new string[1] { "itemprop=\"ingredients\">" }, StringSplitOptions.None);
            var ingredients = new List<string>();
            for (int i = 1; i < ingredientParts.Length; i++) { 
                if (ingredientParts[i].Contains("<")) {
                    var chars = ingredientParts[i].TakeWhile(a => a != '<');
                    ingredients.Add(new String(chars.ToArray()));
                }
            }
            ingredients.ForEach(a => Console.WriteLine(a));
            Console.WriteLine();
        }
        
    }
}
