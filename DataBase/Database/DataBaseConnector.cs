using DataBase.Database.Interface;

namespace DataBase.Database
{
    public class DataBaseConnector
    {
        public ICRUD db;
        public DataBaseConnector(ICRUD context)
        {
            db = context;
        }
    }
}
