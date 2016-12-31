using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MongoDB;
using InitDB;
using System.IO;
using System.Globalization;
using System.Threading;

namespace InitDailyValuesDB
{
    class InitDailyValuesDB : InitDB.InitDB
    {

        public static void InitDailyValuesCollection(bool overrideDB)
        {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            if (overrideDB)
                MongoData._database.DropCollection("dailyvalues");
            var unit = new RestDBInterface();
            AddDailyValuesFromCSV(unit);
        }

        private static void AddDailyValuesFromCSV(RestDBInterface unit)
        {
            var filePath = Path.Combine(FolderPath, "DailyValues.csv");

            unit.DailyValues.Empty();

            var dvList = DailyValueBuilder.FromFile(filePath, new NutritionGoalsCSV());
            dvList.ForEach(dv => unit.DailyValues.Add(dv));
        }
    }
}
