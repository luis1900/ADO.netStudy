using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05ConfigDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            //获取配置文件中的AppSetting中的数据
            //Console.WriteLine(ConfigurationManager.AppSettings["Version"]);

            //Console.WriteLine(ConfigurationManager.AppSettings["SqlCo"]);

            Console.WriteLine(ConfigurationManager.ConnectionStrings["Sql1"].ConnectionString);

            Console.ReadKey();
        }
    }
}
