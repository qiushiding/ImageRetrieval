using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //第一步：OCR识别文字
            //p.ExecOCR();
            //第二步：存入数据库
            //p.SaveToDatabase();

            //p.ExecCharProcess();

            p.SaveXMLtoDB();
        }

        public void Test()
        {
            string dir = @"D:\JiangTao\FlowPic\FlowPicSource\BMC_Bioinformatics_2007_Jun_14_8_203\1471-2105-8-203-8.jpg";
            string[] pics = Directory.GetFiles(dir, "*.jpg");
            bool flag = true;
            string textFloder = @"D:\JiangTao\FlowPic";
            foreach (string pic in pics)
            {
                if (!OcrProcess(pic, textFloder))
                    flag = false;
            }
        }

        /// <summary>
        /// 2012-12-7
        /// OCR，识别所有流程图切割后的子图片，每个子图片存成一个txt
        /// 流程图切割后的子图片存放在D:\JiangTao\FlowPic\FlowPicSource
        /// 识别后的文字存放在D:\JiangTao\FlowPic\FlowPicText，每个图片对应一个文件夹
        /// </summary>
        public void ExecuteOCR()
        {
            string resFolder = @"D:\JiangTao\FlowPic\FlowPicText\";//识别的文字存放文件夹
            string recogList = @"D:\JiangTao\FlowPic\FlowPicText\recognizeList.txt";
            StreamWriter writer = new StreamWriter(recogList);
            int i = 0;
            //int folderCount = 0;

            string outFolder = @"D:\JiangTao\FlowPic\FlowPicSource"; //源图片文件夹
            string[] folders = Directory.GetDirectories(outFolder);
            int foldersLength=folders.Length;
            if (folders == null || folders.Length < 1) return;
            //foreach (string folder in folders)
            for (int f = 5454; f < foldersLength; f++)
            {
                if (f == 5601) return;
                //folder== D:\FlowPic\Psychosoc_Med_2006_Dec_11_3_Doc09
                string[] dirs = Directory.GetDirectories(folders[f]);
                if (dirs == null || dirs.Length < 1) continue;
                foreach (string dir in dirs)
                {
                    //dir== D:\FlowPic\Psychosoc_Med_2006_Dec_11_3_Doc09\PSM-03-09-g-001.jpg
                    string picName = dir.Substring(dir.LastIndexOf("\\") + 1); //picName==PSM-03-09-g-001.jpg
                    Console.WriteLine("正在处理第" + (++i) + "个图片：");
                    Console.WriteLine(dir);

                    //创建新文件夹，存放识别出来的文字
                    string textFolder = resFolder + picName;
                    if (Directory.Exists(textFolder))
                        Directory.Delete(textFolder, true);
                    Directory.CreateDirectory(textFolder);

                    string[] pics = Directory.GetFiles(dir, "*.jpg");
                    if (pics == null || pics.Length < 1) continue;
                    bool flag = true; //标示，该文件夹中的所有图片是否都识别
                    foreach (string pic in pics)
                    {
                        //pic== D:\FlowPic\QJM_2011_Sep_3_104(9)_747-760\hcr107f3.jpg\1.jpg
                        //if (!OcrProcess(pic, textFolder))
                        //    flag = false;
                        OcrProcess(pic, textFolder);
                    }
                    writer.WriteLine("第" + f + "个文件夹 " + i + ": " + dir);
                    writer.Flush();
                    //if (flag)
                    //{
                    //    writer.WriteLine("第" + f + "个文件夹 " + i + ": " + dir + "---" + "Success"); //记录识别的文件（路径）
                    //    writer.Flush();
                    //}
                    //else
                    //{
                    //    writer.WriteLine("第" + f + "个文件夹 " + i + ": " + dir + " ------ " + "Failure"); //记录识别的文件（路径）
                    //    writer.Flush();
                    //}
                }
            }
            //writer.Flush();
            writer.Close();
        }
        
        /// <summary>
        /// 识别filename所指的图片，并将识别的文字存放在textFolder目录下
        /// </summary>
        /// <param name="fileName">待识别的图片</param>
        /// <param name="textFolder">识别后的文字存放在该目录下，文字存在txt文档并以图片名字命名</param>
        /// <returns>图片能否进行OCR识别</returns>
        public bool OcrProcess(string fileName, string textFolder)
        {
            if (String.IsNullOrEmpty(fileName.Trim()))
                return false;
            bool flag = true;
            //Console.WriteLine(fileName);

            string result = string.Empty;
            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.LastIndexOf(".");
            int nameLen = endIndex - startIndex;
            string picName = fileName.Substring(startIndex, nameLen);

            MODI.Document md = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new Exception();
                }

                md = new MODI.Document();
                md.Create(fileName);
                md.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true);
                for (int j = 0; j < md.Images.Count; j++)
                {
                    MODI.Image image = (MODI.Image)md.Images[j];
                    result += image.Layout.Text + "\r\n";
                }
                md.Close();

                string txtName = textFolder + "\\" + picName + ".txt";

                StreamWriter writeFile = new StreamWriter(txtName, false);
                writeFile.Write(result);
                writeFile.Flush();
                writeFile.Close();

                flag = true;
            }
            catch (Exception e)
            {
                if (md != null)
                    md.Close();
                flag = false;
            }
            Console.WriteLine(fileName + " ------ " + flag);
            return flag;
        }

        public void ExecOCR()
        {
            DateTime start = DateTime.Now;
            OCRProc op = new OCRProc();
            List<string> allPath = op.GetAllPath();
            foreach (string path in allPath)
            {
                op.OcrProcess(path);
            }

            double dt = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine("Time Spend : " + dt);

            Console.WriteLine("Success: " + OCRProc.successCnt);
            Console.WriteLine("Error: " + OCRProc.errorCnt);
            Console.WriteLine("over!");

            Console.Read();
        }

        /// <summary>
        /// 2012-12-11：将OCR后的文字存成XML文件，每个节点对应一张切割后的子图片
        /// txtDir是OCR文字所在目录，每个文件夹对应一张图片，并以图片名字命名
        /// xmlDir是存放XML文件的目录
        /// </summary>
        public void SaveToXML()
        {
            string txtDir = @"D:\JiangTao\FlowPic\FlowPicText"; ;
            string xmlDir = @"D:\JiangTao\FlowPic\FlowPicXML\";
            //string folder = @"E:\LabProject\data\FlowPicText";
            string[] pics = Directory.GetDirectories(txtDir);
            if (pics == null || pics.Length < 1)
                return;

            int emptyFolder = 0;
            int count = 0;
            StreamReader reader = null;
            StreamWriter writer = null;
            foreach (string pic in pics)
            {
                string[] texts = Directory.GetFiles(pic, "*.txt");
                if (texts == null || texts.Length < 1)
                {
                    ++emptyFolder;
                    continue;
                }

                StringBuilder xmlStr = new StringBuilder();
                xmlStr.Append("<ro_ot>");
                foreach (string txt in texts)
                {
                    reader = new StreamReader(txt);
                    string cc = OCRProc.CharProcess(reader.ReadToEnd()); //字符过滤
                    if (cc!=null && !String.IsNullOrEmpty(cc.Trim()))
                    {
                        xmlStr.Append("<no_de>" + cc + "</no_de>");
                    }
                    reader.Close();
                }
                xmlStr.Append("</ro_ot>");

                //存储到数据库中   E:\LabProject\data\FlowPicText\01-0317-F1.jpg
                int first = pic.LastIndexOf("\\") + 1;
                int second = pic.LastIndexOf(".");
                string picName = pic.Substring(first, second - first);

                //string outputFolder = @"E:\LabProject\data\fct\";
                writer = new StreamWriter(xmlDir + picName + ".xml", false);
                writer.Write(xmlStr.ToString());
                Console.WriteLine(++count);
                writer.Flush();
                writer.Close();
            }
            Console.WriteLine("Empty folder has : " + emptyFolder);
        }

        /// <summary>
        /// 2012-12-12 把xml存到数据库中，主要是更新fc_inform已有的项
        /// </summary>
        public int SaveXMLtoDB()
        {
            string xmlDir = @"D:\JiangTao\FlowPic\FlowPicXML\";
            //string updatedXml = @"D:\JiangTao\FlowPic\FPXML\";
            string[] xmls = Directory.GetFiles(xmlDir, "*.xml");
            if (xmls == null || xmls.Length < 1) return 0;

            //string updatetxt = @"D:\JiangTao\FlowPic\update.txt";
            //StreamWriter writer = new StreamWriter(updatetxt);

            SQLHelper helper = new SQLHelper();
            int updateCount = 0;
            StreamReader reader = null;
            foreach (string xml in xmls)
            {
                //D:\JiangTao\FlowPic\FlowPicXML\1465-9921-4-13-10.xml
                reader = new StreamReader(xml);
                int first = xml.LastIndexOf("\\") + 1;
                int second = xml.LastIndexOf(".");
                string fig = xml.Substring(first, second - first);

                string xmlText = reader.ReadLine();
                reader.Close();
                if (!String.IsNullOrEmpty(xmlText))
                { 
                    //save to datebase, update the table
                    //int i = helper.UpdateData(fig, xmlText);

                    //save new to table
                    int i = helper.InsertData(fig, xmlText);

                    Console.WriteLine(++updateCount);
                    //if (i == 1)
                    //{
                        //File.Move(xml, updatedXml + fig + ".xml");
                        //writer.WriteLine(xml);
                        //writer.Flush();
                    //}
                }
            }
            //writer.Close();
            return updateCount;
        }

        public void SaveToDatabase()
        {
            OCRProc op = new OCRProc();
            SQLHelper helper = new SQLHelper();
            //int deletedCount = helper.DeleteData();
            //Console.WriteLine("{0} records have been deleted!", deletedCount);

            string[] files = (Directory.GetFiles(op.successDir)).Where(x => x.EndsWith(".txt")).ToArray();
            int i = 0;
            foreach (string f in files)
            {
                StreamReader reader = new StreamReader(f);
                string fileName = reader.ReadLine();
                string content = reader.ReadToEnd();
                //过滤字符
                content = OCRProc.CharProcess(content);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    helper.InsertData(fileName, content);
                }
                Console.WriteLine(++i);
                reader.Close();
            }
            helper.Close();
            Console.WriteLine("over!");
            Console.Read();
        }

        public void ExecCharProcess()
        {
            string f = @"D:\JiangTao\Project\data\1\pbio.0020055.t002.txt";
            FileStream fs = new FileStream(f, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            string name = reader.ReadLine();
            string content = reader.ReadToEnd();

            string newstr = OCRProc.CharProcess(content);
            StreamWriter writer = new StreamWriter(@"D:\JiangTao\Project\data\1\pbio.txt");
            writer.Write(newstr);

            reader.Close();
            writer.Close();
        }

    }
}
