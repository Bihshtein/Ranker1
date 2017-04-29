using RestModel;
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
using System.Text.RegularExpressions;
using log4net;
using System.Globalization;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace InitRecipes {
    class Program {
      
        

        static void Main(string[] args) {
            var start = DateTime.Now;
            var offline = args.Contains("OFFLINE");
            var dropTable = args.Contains("CLEAR");
            ParseRecipes.CreateDB(offline, dropTable, int.Parse(args[0])); 
            Console.WriteLine(new TimeSpan(DateTime.Now.Ticks - start.Ticks).ToString());
        }
       
    }

}
