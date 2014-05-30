using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace OCR
{
    public class SQLHelper
    {
         string connectionString = string.Format("Server={0};Database={1};uid={2};pwd={3}",
                                    "XP-201109141459\\SQLSERVER", "flowchart", "sa", "sql");
        SqlConnection myConn;
        SqlCommand sqlCmd;

        public SQLHelper()
        {
            myConn = new SqlConnection(connectionString);
            myConn.Open();
        }
        //插入数据
        public int InsertData(string figname, string imagexmltext)
        {
            //myConn.Open();
            //string sqlStr = "insert into fc_inform(figname, imagetext) values('"+figname+"','"+imagetext+"' )";
            string sqlStr = "insert into fc_inform(figname, imagexmltext) values('" + figname + "','" + imagexmltext + "' )";
            sqlCmd = new SqlCommand(sqlStr, myConn);
            return sqlCmd.ExecuteNonQuery();
        }

        //更新fc_inform表,返回更新的表项数量
        public int UpdateData(string figname, string imagexmltext)
        {
            string sqlStr = "UPDATE fc_inform SET imagexmltext='" + imagexmltext+"'WHERE figname='"+figname+"'";
            sqlCmd = new SqlCommand(sqlStr, myConn);
            return sqlCmd.ExecuteNonQuery();
        }

        //删除fc_inform表中所有的数据
        public int DeleteData()
        {
            //myConn.Open();
            string sql = "delete from fc_inform";
            sqlCmd = new SqlCommand(sql, myConn);
            return sqlCmd.ExecuteNonQuery();
        }

        //插入数据
        public void InsertData(string cmdText)
        {
            //myConn.Open();
            sqlCmd = new SqlCommand(cmdText, myConn);
            sqlCmd.ExecuteNonQuery();
        }
        //删除数据
        public int DeleteData(string cmdText)
        {
            //myConn.Open();
            sqlCmd = new SqlCommand(cmdText, myConn);
            return sqlCmd.ExecuteNonQuery();
        }

        public void Close()
        {
            myConn.Close();
        }
    }
}
