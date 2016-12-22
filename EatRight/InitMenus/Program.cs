﻿using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using InitDB;
using System.Text.RegularExpressions;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace InitRecipes {
    class Program {
      
        public static int LoadMealsNum = 1000;

        static void Main(string[] args) {
            ParseRecipes.PopulateMealsDB(LoadMealsNum); // index file has 20,000 meals, load the first 200
            MatchProductsInRecipes.AddActualProductsToMealsDB();
        }
       
    }

}
