using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OCR
{
    public class OCRProc
    {
        static int i = 0;
        public static int successCnt = 0;
        public static int errorCnt = 0;

        string successList = @"D:\JiangTao\Project\data\successList.txt";
        string errorList = @"D:\JiangTao\Project\data\errorList.txt";
        public string successDir = @"D:\JiangTao\Project\data\Success";
        public string errorDir = @"D:\JiangTao\Project\data\Failure";

        public List<string> GetAllPath()
        {
            List<string> allPath = new List<string>();
            string dir = @"D:\Flow_Chart_Recogntion\";
            string picData = @"D:\JiangTao\Project\data\All_data.txt";

            FileStream fs = new FileStream(picData, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            StringBuilder path = new StringBuilder();

            string line = reader.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                path.Append(dir);
                path.Append(line.Replace(",", "\\"));
                allPath.Add(path.ToString());

                line = reader.ReadLine();
                path.Clear();
            }

            return allPath;
        }

        public void OcrProcess(string fileName)
        {
            if (String.IsNullOrEmpty(fileName.Trim()))
                return;

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

                string txtName = successDir + "\\" + picName + ".txt";

                StreamWriter writeFile = new StreamWriter(txtName, false);
                writeFile.Write(picName + "\r\n" + result);
                writeFile.Flush();
                writeFile.Close();

                StreamWriter writeResult = new StreamWriter(successList, true);
                writeResult.Write(fileName + "\r\n");
                writeResult.Flush();
                writeResult.Close();

                successCnt++;
                Console.WriteLine(++i);
            }
            catch (Exception e)
            {
                if (md != null)
                    md.Close();
                result = "error";

                string txtName = errorDir + "\\" + picName + ".txt";
                StreamWriter writeFile = new StreamWriter(txtName, false);
                writeFile.Write(picName + "\r\n" + result);
                writeFile.Flush();
                writeFile.Close();

                StreamWriter writeResult = new StreamWriter(errorList, true);
                writeResult.Write(fileName + "\r\n");
                writeResult.Flush();
                writeResult.Close();

                errorCnt++;
                Console.WriteLine(++i);
            }
        }

        public static string CharProcess(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            string result = null;
            //统一用UTF-8的方式编码：非ASCII字符是由多字节组成，每个字节的第一位都是1，因此每字节都是大于128。
            Byte[] encodedBytes = Encoding.UTF8.GetBytes(text);
            //选出ASCII字符
            //Byte[] resultBytes = encodedBytes.Where(x => x < 128).ToArray();
            //提取出大小写字母，数字，空格
            Byte[] resultBytes = encodedBytes.Where(x => (x >= 48 && x <= 57) || (x >= 97 && x <= 122) || (x >= 65 && x <= 90) || x == 32).ToArray();

            if (resultBytes.Length > 0)
            {
                //用ASCII编码将字节反编成字符串，此时已经剔除非ASCII字符
                result = Encoding.ASCII.GetString(resultBytes);
            }

            return result;
        }

    }
}
