using DataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Database
{
    public class DataBaseConnector
    {
        public ICRUD db;
        public DataBaseConnector(DataBaseType type)
        {
            switch(type)
            {
                case DataBaseType.MySQL:
                    {
                        db = new MySQL.Context.Context();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
                

            
        }
    }
}
