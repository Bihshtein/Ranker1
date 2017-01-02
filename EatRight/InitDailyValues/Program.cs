using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitDB;
namespace InitDailyValuesDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var loadGroups = !args.Contains("skip_groups");
            var loadManual = !args.Contains("skip_manual");
            var overrideDB = !args.Contains("dont_erase_db");

            InitDailyValuesDB.InitDailyValuesCollection(overrideDB);
        }
    }
}
