using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace Lucene
{
    class LuceneProgram
    {
        static void Main(string[] args)
        {
            //Index index = new Index();
            //index.CreateIndex();

            //Console.Read();
            LuceneProgram lp = new LuceneProgram();
            lp.RetrieveFromIndex(@"D:\JiangTao\Project\DataIndex");
        }

        public void RetrieveFromIndex(string indexLocation)
        {
            Directory dir = FSDirectory.GetDirectory(indexLocation, false);
            IndexSearcher searcher = new IndexSearcher(dir, true);
            QueryParser parser = new QueryParser("figure", new StandardAnalyzer());
            Query query = parser.Parse("figure:1471-2105-10-S12-S1-6 09-1006-F2");

            Hits hits = searcher.Search(query);
            if (hits != null && hits.Length() > 0)
            {
                Console.WriteLine(hits.Length().ToString() + "\n");
                for (int i = 0; i < hits.Length(); i++)
                {
                    Document doc = hits.Doc(i);
                    //Console.WriteLine("dirname: " + doc.Get("dirname"));
                    //Console.WriteLine("nxmlname: " + doc.Get("nxmlname"));
                    //Console.WriteLine("pdfname: " + doc.Get("pdfname"));
                    //Console.WriteLine("journal: " + doc.Get("journal"));
                    //Console.WriteLine("article: " + doc.Get("article"));
                    //Console.WriteLine("authors: " + doc.Get("authors"));
                    //Console.WriteLine("adresss: " + doc.Get("adresss"));
                    //Console.WriteLine("abscontent: " + doc.Get("abscontent"));
                    //Console.WriteLine("keyword: " + doc.Get("keyword"));
                    //Console.WriteLine("xlink: " + doc.Get("xlink"));
                    //Console.WriteLine("figure: " + doc.Get("figure"));
                    //Console.WriteLine("caption: " + doc.Get("caption"));
                    //Console.WriteLine("context: " + doc.Get("context"));
                    Console.WriteLine("imagetext: " + doc.Get("imagetext"));
                    Console.WriteLine("imagexmltext: " + doc.Get("imagexmltext"));
                }
            }
            searcher.Close();
            Console.Read();

        }

    }
}
