using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace XML
{
    public class NxmlParser
    {
        public static List<String> GetPaths()
        {
            //string file = @"D:\JiangTao\Project\ImageRetrieval\tt.txt";   //----IF
            string file = @"D:\JiangTao\Project\ImageRetrieval\fc.txt";
            string dir = "D:\\Flow_Chart_Recogntion\\";
            List<String> paths = new List<string>();

            FileStream fs = new FileStream(file, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            
            int i = 0;
            DateTime start = DateTime.Now;

            string line = reader.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                #region read from IF.txt
                //int firstpos = line.IndexOf(",");
                //int lastpos = line.LastIndexOf(",");
                ////string name = line.Substring(firstpos + 1, lastpos - firstpos - 1);
                ////string nameAA = name.Substring(0, name.IndexOf("."));
                //string pth = dir + line.Substring(0, lastpos).Replace(",", "\\").Replace("\\\\", "\\");
                ////string newpth = @"D:\JiangTao\Project\ImageRetrieval\nxmls\"+name.Replace(".nxml", ".xml");
                
                ////File.Copy(pth, newpth,true);
                ////paths.Add(newpth);

                //paths.Add(pth);
                //line = reader.ReadLine();
                #endregion

                #region read from FC
                int pos = line.IndexOf(",");
                string pth = line.Substring(0, pos);
                string directory = dir + pth;
                string[] nxmls = Directory.GetFiles(directory, "*.nxml");
                if (nxmls != null && nxmls.Length > 0)
                {
                    paths.AddRange(nxmls);
                }
                line = reader.ReadLine();
                #endregion

                Console.WriteLine(++i);
            }

            Console.WriteLine((DateTime.Now-start).TotalSeconds);
            fs.Close();
            reader.Close();
            return paths;
        }

        public static List<string> readNxml(string file)
        {
            StreamReader reader = new StreamReader(file);

            //用正则表达式匹配
            //string rex1 = "<pub-id pub-id-type=\"doi\">.*?</pub-id>";
            string rex2 = "<article-id pub-id-type=\"doi\">.*?</article-id>";
            string content = reader.ReadToEnd();

            List<string> results = new List<string>();

            if (!string.IsNullOrEmpty(content))
            {
                //MatchCollection mc1 = Regex.Matches(content, rex1, RegexOptions.IgnoreCase);
                MatchCollection mc2 = Regex.Matches(content, rex2, RegexOptions.IgnoreCase);
                //if (mc1 != null && mc1.Count > 0)
                //{
                //    foreach (Match m in mc1)
                //    {
                //        string doi = m.Value.Replace("<pub-id pub-id-type=\"doi\">", "").Replace("</pub-id>", "");
                //        results.Add(doi);
                //    }
                //}
                if (mc2 != null && mc2.Count > 0)
                {
                    foreach (Match m in mc2)
                    {
                        string doi = m.Value.Replace("<article-id pub-id-type=\"doi\">", "").Replace("</article-id>", "");
                        results.Add(doi);
                    }
                }
            }
            reader.Close();
            return results;
        }
    }
}
