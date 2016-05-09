using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> files = new List<string>() { @"C:\Users\USER\Downloads\prices\prices_koop_herzelia_08_05_2016.xml" ,
                                                      @"C:\Users\USER\Downloads\prices\prices_shufer_raanana_09_05_2017.xml",
                                                      @"C:\Users\USER\Downloads\prices\prices_koop_ashdod_09_05_2017.xml"};
            List<List<long>> ids = new List<List<long>>();
            files.ForEach((file) => ids.Add(GetIDS(File.ReadAllText(file))));
            List<long> equals = new List<long>();
            for (int i = 0; i < ids[0].Count; i++)
            {
                var allEquals = true;
                for (int j = 1; j < files.Count; j++)
                {
                    if (!ids[j].Contains(ids[0][i]))
                        allEquals = false;
                }
                if (allEquals)
                    equals.Add(ids[0][i]);
            }

        }

        public static List<long> GetIDS(string str)
        {
            var reader = XmlReader.Create(new StringReader(str));
            List<long> ids = new List<long>();
            while (reader.ReadToFollowing("ItemCode"))
            {
                reader.Read();
                ids.Add(reader.ReadContentAsLong());
            }
            return ids;
        }
    }
}

