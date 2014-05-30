using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Index;
using System.Data;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Common;

namespace Lucene
{
    public class Index
    {
        string indexLoc = @"D:\JiangTao\Project\DataIndex";
        string connectionString = string.Format("Server={0};Database={1};uid={2};pwd={3}",
                                    "XP-201109141459\\SQLSERVER", "flowchart", "sa", "sql");
        public void CreateIndex()
        {
            CreateIndex(indexLoc);
        }

        public void CreateIndex(string indexLocation)
        {
            if (IndexReader.IndexExists(indexLocation))
            {
                IndexReader reader = IndexReader.Open(indexLocation);
                for (int i = 0; i < reader.MaxDoc(); i++)
                {
                    reader.DeleteDocument(i);
                }
                reader.Close();
            }

            Console.WriteLine("---Begin create index---");
            int recordCount = 0;
            DateTime startTime = DateTime.Now;

            IndexWriter writer = new IndexWriter(indexLocation, new StandardAnalyzer(), true);
            DataTable table = RetrieveData();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (IndexDocument(writer,
                                        row["dirname"].ToString(), row["nxmlname"].ToString(),
                                        row["pdfname"].ToString(), row["journal"].ToString(),
                                        row["article"].ToString(), row["authors"].ToString(),
                                        row["adresss"].ToString(), row["abscontent"].ToString(),
                                        row["keyword"].ToString(), row["xlink"].ToString(),
                                        row["figure"].ToString(), row["caption"].ToString(),
                                        row["context"].ToString(), row["imagetext"].ToString(),
                                        row["imagexmltext"].ToString()
                                        )
                        )
                        recordCount++;
                }
                writer.Optimize();
                writer.Close();
                double searchTime = (DateTime.Now - startTime).TotalSeconds;

                Console.WriteLine("Have Indexed {0} pictures with {1}seconds.", recordCount, searchTime);
                Console.WriteLine("---End create index---");
            }
        }

        /// <summary>
        /// 2012-10-15 从数据库中取出所有flowchart数据，imagexmltext是xml格式，需要解析提取内容
        /// </summary>
        private DataTable RetrieveData()
        {
            SQLHelper sqlHelper = new SQLHelper(connectionString);
            //string sql = "SELECT dirname,nxmlname,pdfname,journal,article,authors,adresss,abstract as abscontent,keyword,xlink,figure,caption,context,imagetext"
            //    + "  FROM fc_inform INNER JOIN figure_inform ON fc_inform.figname=figure_inform.figure INNER JOIN dir_inform ON figure_inform.dir_name=dir_inform.dirname";
            //return sqlHelper.RunSQLToTable(sql);

            //imagexmltext
            string sql = "SELECT dirname,nxmlname,pdfname,journal,article,authors,adresss,abstract as abscontent,keyword,xlink,figure,caption,context,imagetext,imagexmltext"
                + "  FROM fc_inform INNER JOIN figure_inform ON fc_inform.figname=figure_inform.figure INNER JOIN dir_inform ON figure_inform.dir_name=dir_inform.dirname";
            DataTable table = sqlHelper.RunSQLToTable(sql);
            if (table != null && table.Rows.Count > 1)
            {
                foreach (DataRow row in table.Rows)
                {
                    string xmlText = row["imagexmltext"].ToString();
                    xmlText = XmlHelper.GetNodesContentByXpath(xmlText, "ro_ot/no_de");
                    row["imagexmltext"] = xmlText;
                }
            }
            return table;
        }

        private bool IndexDocument(IndexWriter writer, string dirname, string nxmlname, string pdfname, string journal, string article, string authors,
                                    string adresss, string abscontent, string keyword, string xlink, string figure, string caption, string context,
                                    string imagetext, string imagexmltext)
        {
            try
            {
                Document doc = new Document();
                doc.Add(new Field("dirname", dirname, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("nxmlname", nxmlname, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("pdfname", pdfname, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("journal", journal, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("article", article, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("authors", authors, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("adresss", adresss, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("abscontent", abscontent, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("keyword", keyword, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("xlink", xlink, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("figure", figure, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("caption", caption, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("context", context, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("imagetext", imagetext, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("imagexmltext", imagexmltext, Field.Store.YES, Field.Index.TOKENIZED));
                writer.AddDocument(doc);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

    }
}
