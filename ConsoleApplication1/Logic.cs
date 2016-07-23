using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    public class NameComparer : IEqualityComparer<ItemData> {
        public bool Equals(ItemData x, ItemData y) {
            var words1 = x.Name.ToLower().Split(' ').Take(2).ToList();
            var words2 = y.Name.ToLower().Split(' ').Take(2).ToList();
           return  words1.Intersect(words2).ToList().Count == words1.Count;
        }

        public int GetHashCode(ItemData obj) {
            return string.Join("-",obj.Name.ToLower().Split(' ').Take(2).ToList()).GetHashCode();
        }
    }

    public class IDComparer : IEqualityComparer<ItemData> {
        public bool Equals(ItemData x, ItemData y) {
            return x.ID == y.ID;
        }

        public int GetHashCode(ItemData obj) {
            return (int)obj.ID;
        }
    }
    class Logic {
        public static List<List<ItemData>> GetEquals<T>(List<List<ItemData>> data) {
            var aleg = new Stopwatch();
            aleg.Start();
            int count = Math.Min(data[0].Count, data[1].Count);
            var mutualIDS1 = data[0].Intersect(data[1], new IDComparer()).ToList();
            var mutualIDS2 = data[1].Intersect(data[0], new IDComparer()).ToList();
            data[0].RemoveAll((i)=> mutualIDS1.Contains(i));
            data[1].RemoveAll((i) => mutualIDS2.Contains(i));
            var mutualNames1 = data[0].Intersect(data[1], new NameComparer()).ToList();
            var mutualNames2 = data[1].Intersect(data[0], new NameComparer()).ToList();            
            aleg.Stop();
            double percent =  ((double)(mutualIDS1.Count + mutualNames1.Count)) / count;
            Console.WriteLine(string.Format("Total : {0} \t Id's \t {1}\t  Names {2}\t  percent {3}\t calcualtion time {4} Milliseconds ",count, mutualIDS1.Count, mutualNames1.Count, percent, aleg.Elapsed.TotalMilliseconds));
            return new List<List<ItemData>>() { mutualNames1/*.Concat(mutualIDS1).ToList()*/, mutualNames2/*.Concat(mutualIDS2).ToList()*/ };
        }

        public static List<List<ItemData>> GetData(List<FileInfo> files) {
            var prices = new List<Tuple<double, string>>();
            var storesData = new List<List<ItemData>>();
            foreach (var file in files) {
                
                var data = Download.GetData(File.ReadAllText(file.FullName));                
                storesData.Add(data);
                double total = 0;
                data.ForEach(val => total += val.Price);
                prices.Add( new Tuple<double, string>(total, Path.GetFileName(file.FullName)));
            }
            foreach (var item in prices) {
                Console.WriteLine(item.Item2 + "  : \t\t " + item.Item1);

        }
            return storesData;
        }
    }
}
