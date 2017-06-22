using System;
using System.Configuration;

namespace McwdService
{
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString_mysql
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString_mysql"];
                return _connectionString;
            }
        }

        public static string ConnectionString_ora
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString_ora"];
                return _connectionString;
            }
        }
    }
}
