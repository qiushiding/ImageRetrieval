using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Lucene
{
    public class SQLHelper
    {
        SqlConnection myConn;
        public SQLHelper(string Connectionstring) //带参数的构造函数
        {
            myConn = new SqlConnection(Connectionstring);
        }
        //执行SQL命令
        public void RunSQL(string cmdText)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            myCommand.ExecuteNonQuery();
        }
        //关闭连接
        public void Close()
        {
            myConn.Close();
        }
        public void Dispose()
        {
            myConn.Dispose();
        }
        //执行SQL命令并输出数据集
        public void RunSQL(string cmdText, ref DataSet ds)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            SqlDataAdapter sda = new SqlDataAdapter(myCommand);
            DataSet ds1 = new DataSet();
            sda.Fill(ds1);
            ds = ds1;
        }
        //执行SQL命令并输出数据集
        public DataTable RunSQLToTable(string cmdText)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            SqlDataAdapter sda = new SqlDataAdapter(myCommand);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        //执行SQL命令，并执行数据读取
        public void RunSQL(string cmdText, out SqlDataReader sdr)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            SqlDataReader sdr1 = myCommand.ExecuteReader();
            sdr = sdr1;
            sdr1.Dispose();
        }
        //创建参数
        public SqlParameter CreateParam(string ParamName, SqlDbType sqlType, int size, ParameterDirection direction, object value)
        {
            SqlParameter sp = new SqlParameter();
            sp.ParameterName = ParamName;
            sp.SqlDbType = sqlType;
            sp.Size = size;
            sp.Direction = direction;
            sp.Value = value;
            return sp;
        }
        //创建输入参数
        public SqlParameter CreateInParam(string ParamName, SqlDbType sqlType, int size, object value)
        {
            SqlParameter sp = new SqlParameter();
            sp.ParameterName = ParamName;
            sp.SqlDbType = sqlType;
            sp.Size = size;
            sp.Direction = ParameterDirection.Input;
            sp.Value = value;
            return sp;
        }
        public SqlParameter CreateOutParam(string ParamName, SqlDbType sqlType, int size)
        {
            SqlParameter sp = new SqlParameter();
            sp.ParameterName = ParamName;
            sp.SqlDbType = sqlType;
            sp.Size = size;
            sp.Direction = ParameterDirection.Output;
            return sp;
        }
        public SqlParameter CreateReturnParam(string ParamName, SqlDbType sqlType, int size)
        {
            SqlParameter sp = new SqlParameter();
            sp.ParameterName = ParamName;
            sp.SqlDbType = sqlType;
            sp.Size = size;
            sp.Direction = ParameterDirection.ReturnValue;
            return sp;
        }
        //运行带参数的SQL语句
        public void RunSQL(string cmdText, SqlParameter[] sp)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            myCommand.CommandType = CommandType.Text;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            myCommand.ExecuteNonQuery();
        }
        //运行带参数的SQL语句，并输出结果集
        public void RunSQL(string cmdText, SqlParameter[] sp, ref DataSet ds)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            myCommand.CommandType = CommandType.Text;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            SqlDataAdapter sda = new SqlDataAdapter(myCommand);
            DataSet ds1 = new DataSet();
            sda.Fill(ds1);
            ds = ds1;
        }
        //运行带参数的SQL语句，并输出数据读取集
        public void RunSQL(string cmdText, SqlParameter[] sp, out SqlDataReader sdr)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            myCommand.CommandType = CommandType.Text;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            SqlDataReader sdr1 = myCommand.ExecuteReader();
            sdr = sdr1;
        }
        //运行带参数的存储过程
        public void RunProc(string procName)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(procName, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.ExecuteNonQuery();
        }
        public void RunProc(string procName, ref DataSet ds)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(procName, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(myCommand);
            DataSet ds1 = new DataSet();
            sda.Fill(ds1);
            ds = ds1;
        }
        //运行存储过程
        public void RunProc(string procName, out SqlDataReader sdr)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(procName, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr1 = myCommand.ExecuteReader();
            sdr = sdr1;
            sdr1.Dispose();
        }
        //运行带参数的SQL存储过程
        public void RunProc(string procName, SqlParameter[] sp)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(procName, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            myCommand.ExecuteNonQuery();
        }
        //运行带参数的SQL语句，并输出结果集
        public void RunProc(string cmdText, SqlParameter[] sp, ref DataSet ds)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(cmdText, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            SqlDataAdapter sda = new SqlDataAdapter(myCommand);
            DataSet ds1 = new DataSet();
            sda.Fill(ds1);
            ds = ds1;
        }
        //运行带参数的SQL语句，并输出数据读取集
        public void RunProc(string procName, SqlParameter[] sp, out SqlDataReader sdr)
        {
            myConn.Open();
            SqlCommand myCommand = new SqlCommand(procName, myConn);
            myCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sp.Length; i++)
            {
                myCommand.Parameters.Add(sp[i]);
            }
            SqlDataReader sdr1 = myCommand.ExecuteReader();
            sdr = sdr1;
        }
    }
}
