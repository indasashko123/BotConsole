using DataBase.Database.Interface;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Database.Context.MySQL
{
    public class SQLContext : ICRUD
    {
        string DataBaseName { get; set; }
        public SQLContext(string dbName)
        {
            DataBaseName = dbName;
        }
       
    }
}
