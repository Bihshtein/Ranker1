﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static string path =  Assembly.GetExecutingAssembly().Location + @"\..\..\..\prices\";
        static void Main(string[] args)
        {
            
            List<string> files = new List<string>() { path + "prices_koop_herzelia_08_05_2016.xml" ,
                                                      path + "prices_shufer_raanana_09_05_2017.xml",
                                                      path + "prices_koop_ashdod_09_05_2017.xml"};


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
            Console.WriteLine(string.Format("\n\nMatching id's :  {0} \n\nWe still don't know who they are yet we know they are matching!!!\n\n", equals.Count));

            PrintNames(equals,File.ReadAllText(files[0]));

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

        public static void  PrintNames(List<long> ids, string str)
        {
            File.WriteAllText(path + "matching_names.txt",  ids.Count + "matching" );
            var reader = XmlReader.Create(new StringReader(str));
            while (reader.ReadToFollowing("ItemCode"))
            {
                reader.Read();
                var id = reader.ReadContentAsLong();
                if (ids.Contains(id)) {
                    reader.ReadToFollowing("ItemName");
                    reader.Read();
                    Console.WriteLine();
                    File.AppendAllText(path + "matching_names.txt ", reader.ReadContentAsString()+"\n");
                }
            }
         
        }
    }
}

