using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    class Program {
        static void Main(string[] args) {
            var loadGroups = !args.Contains("skip_groups");
            var loadManual = !args.Contains("skip_manual");
            var overrideDB = !args.Contains("dont_erase_db"); 
            InitDB.InitProductsCollection(loadGroups, loadManual, overrideDB);
            
        }
    }
}
