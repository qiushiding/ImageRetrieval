using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XML;
using System.IO;

namespace Test
{
    class Xml
    {
        public void testXml()
        {
            string file = @"E:\fchartXML.xml";
            string label = XMLParser.readXml(file);
            Console.WriteLine(label);
        }

        public void testNxml()
        {
            //StreamWriter writer = new StreamWriter(@"D:\JiangTao\Project\ImageRetrieval\DOI_all_Unordered.txt", true);
            StreamWriter writer = new StreamWriter(@"D:\JiangTao\Project\ImageRetrieval\DOI_fc.txt", true);

            List<string> paths = NxmlParser.GetPaths();
            if (paths.Count > 0)
            {
                List<string> dois = new List<string>();
                int i = 0;
                foreach (string path in paths)
                {
                    Console.WriteLine("正在处理第"+(++i)+"个文件");
                    dois.AddRange(NxmlParser.readNxml(path));
                }
                //排序
                //dois.Sort();
                //List<string> ddd = new List<string>();
                //去重
                //ddd.AddRange(dois.Distinct());

                if (dois.Count > 0)
                {
                    foreach (string doi in dois)
                    {
                        writer.WriteLine(doi);
                    }
                }
            }
            writer.Close();
            
        }
    }
}
