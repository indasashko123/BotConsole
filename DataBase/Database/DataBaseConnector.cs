using DataBase.Database.Interface;

namespace DataBase.Database
{
    public class DataBaseConnector
    {
        public IRepository db;
        public DataBaseConnector(IRepository context)
        {
            db = context;
        }
    }
}
