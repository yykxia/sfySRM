using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace SFYWebApi
{
    public class SqlSel
    {
        static string getconn()
        {
            string ConnectionString = "server=121.40.38.95;uid=SFY2MBH;pwd=mlily_4689;database=SFY2MLILY;";
            return ConnectionString;
        }
        public static string getcipher()
        {
            string cipher = "MBH3Q2W876EG422";
            return cipher;
        }
        public static bool GetSqlSel(ref DataTable ODT, string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                SqlDataAdapter sda = new SqlDataAdapter(sc);
                ODT = new DataTable();
                sda.Fill(ODT);
                if (ODT.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static object GetSqlScale(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始填充
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }

        public static int ExeSql(string SQL)
        {
            string ConnectionString = getconn();
            SqlConnection _SqlConnection1 = new SqlConnection();
            SqlCommand sc = new SqlCommand();
            try
            {
                if (_SqlConnection1.State != ConnectionState.Open)
                {
                    _SqlConnection1.ConnectionString = ConnectionString;
                    _SqlConnection1.Open();
                }
                //开始执行
                string sqlCmd = SQL;
                sc.Connection = _SqlConnection1;
                sc.CommandText = sqlCmd;
                return sc.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
            finally
            {
                _SqlConnection1.Close();
            }
        }
    }
}
