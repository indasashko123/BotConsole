using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Config
{
    internal class BaseConfig
    {
        public string server { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string port { get; set; }
        public string host { get; set; }
        public string sslCa {get;set;}
        public string sslMode { get; set; }
        public MySQLVersion version { get; set; }
    }
}
