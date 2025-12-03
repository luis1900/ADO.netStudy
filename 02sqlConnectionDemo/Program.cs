using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02sqlConnectionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 01第一个连接对象
            // 连接字符串
            //string connectionString = "server=pluto;uid=sa;pwd=123456;database=School"; // Replace with your actual connection string
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    Console.WriteLine("Connection opened successfully.");
            //    // Add your database operations here  
            //    connection.Close();
            //    Console.WriteLine("Connection closed successfully.");
            //}
            #endregion

            #region 02SqlCommand对象
            // 连接字符串
            string connectionString = "server=pluto;uid=sa;pwd=123456;database=School"; // Replace with your actual connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection opened successfully.");

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [dbo].[TblClass]([tClassName],[tClassDesc]) VALUES (@name, @desc)";
                        command.Parameters.AddWithValue("@name", "车辆工程");
                        command.Parameters.AddWithValue("@desc", "经济理论");

                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"{rows} row(s) inserted.");
                    }
                }
                catch (SqlException ex)
                {
                    // 记录异常日志
                    Console.WriteLine("数据库操作异常: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    Console.WriteLine("Connection closed successfully.");
                }
            }

            #endregion

            Console.ReadKey();
        }
    }
}
