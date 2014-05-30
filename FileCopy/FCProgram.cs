using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileCopy
{
    class FCProgram
    {
        static void Main(string[] args)
        {
            //string source = @"D:\JiangTao\Project\data\All_FlowChart_data.txt";
            string source = @"D:\JiangTao\FlowPic\Data1_1_FC.txt";
            //string destination = @"D:\JiangTao\Project\TCBVR\picData\";
            string destination = @"D:\JiangTao\FlowPic\FlowChart\";

            FileStream fs = new FileStream(source,FileMode.Open);
            StreamReader reader = new StreamReader(fs);

            //string pic1 = @"D:\Flow_Chart_Recogntion\temp\BMC_Med_Inform_Decis_Mak_2006_Apr_3_6_18\1472-6947-6-18-1.jpg";
            //string pic2 = @"D:\Flow_Chart_Recogntion\temp\BMC_Med_Inform_Decis_Mak_2006_Apr_3_6_18\1472-6947-6-18-2.jpg";
            //File.Copy(pic1,pic1.Substring(pic1.LastIndexOf("\\") + 1),true);
            //File.Copy(pic2, pic2.Substring(pic2.LastIndexOf("\\") + 1), true);

            string cc = reader.ReadToEnd();
            if (!string.IsNullOrEmpty(cc))
            {
                string[] files = cc.Split('\n');
                int length = files.Length;
                int count = 0;
                for (int i = 0; i < length;i++ )
                {
                    string pic = files[i].Trim();
                    if (!string.IsNullOrEmpty(pic))
                    {
                        string nameJPG = pic.Substring(pic.LastIndexOf("\\") + 1);
                        File.Copy(pic, destination + nameJPG, true);
                        //Console.WriteLine(++count);

                        //string fGIF = pic.Replace(".jpg", ".gif");
                        //string nameGIF = fGIF.Substring(fGIF.LastIndexOf("\\") + 1);
                        //File.Copy(fGIF, destination + nameGIF, true);
                        Console.WriteLine(++count);
                    }
                }
            }
            fs.Close();
            reader.Close();
        }
    }
}
