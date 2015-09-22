using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyBookShop.APIclass
{
    public class serviceAPI
    {
        private static string strconn;
        private static SqlConnection conn;
        private static SqlCommand sc;
        private static SqlDataReader sda;

        public void sqlConnect()
        {
            strconn ="Data Source=CHU-LIAM\\CHULIAM;database=bookshop;Integrated Security=true";
            try
            {
                conn = new SqlConnection(strconn);//新建数据库连接
            }
            catch (System.Data.SqlClient.SqlException ee)
            {
                throw new Exception(ee.Message);//抛出异常
            }
        }
        public int getRow(string strsql)
        {
            int Row = 0;
            sqlConnect();
            conn.Open();
            sc = conn.CreateCommand();
            sc.CommandText = strsql;
            sda = sc.ExecuteReader();
            if (sda.HasRows)
            {
                Row++;
            }
            conn.Close();
            return Row;
        }

        public int dataProcessor(string strsql)
        {
            int result = 0;
            sqlConnect();
            conn.Open();
            sc = conn.CreateCommand();
            sc.CommandText = strsql;
            result = sc.ExecuteNonQuery();
            conn.Close();
            return result;
        }
    }
}