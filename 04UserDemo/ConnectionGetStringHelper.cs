using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04UserDemo
{
    public class ConnectionGetStringHelper
    {
        /// <summary>
        /// 根据名称从配置文件获取连接字符串
        /// </summary>
        /// <param name="name">连接字符串名称</param>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString(string name)
        {
            /// <summary>
            /// 从配置文件中获取指定名称的连接字符串
            /// </summary>
            /// <param name="name">要获取的连接字符串的名称</param>
            /// <returns>返回指定名称的连接字符串，如果未找到则返回null</returns>
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
        }
    }
}
