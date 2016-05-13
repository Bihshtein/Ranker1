using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Logic {
        public static List<long> GetEquals(List<List<long>> ids,int numOfFiles) {
            var equals = new List<long>();
            for (int i = 0; i < ids[0].Count; i++) {
                var allEquals = true;
                for (int j = 1; j < numOfFiles; j++) {
                    if (!ids[j].Contains(ids[0][i]))
                        allEquals = false;
                }
                if (allEquals)
                    equals.Add(ids[0][i]);
            }
            Console.WriteLine(string.Format("Matching id's : {0}", equals.Count.ToString()));
            return equals;
        }

        public static List<Dictionary<long, ItemData>> GetData(List<FileInfo> files, List<long> equals) {
            var storesData = new List<Dictionary<long, ItemData>>();
            foreach (var file in files) {
                var data = Download.GetData(equals, File.ReadAllText(file.FullName));
                storesData.Add(data);
                double total = 0;
                foreach (var value in data.Values) {
                    total += value.Price;
                }
                Console.WriteLine(Path.GetFileName(file.FullName) + "  :  " + total);
            }
            return storesData;
        }
    }
}
