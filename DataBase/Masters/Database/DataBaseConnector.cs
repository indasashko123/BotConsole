using DataBase.Masters.Database.Interface;

namespace DataBase.Masters.Database
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
