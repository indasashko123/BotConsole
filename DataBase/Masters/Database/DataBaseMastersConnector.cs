using DataBase.Masters.Database.Interface;

namespace DataBase.Masters.Database
{
    public class DataBaseMastersConnector
    {
        public IUpdaterEntity Updater;
        public ICreaterEntity Creater;
        public IEntityFinder Finder;
        public DataBaseMastersConnector(IUpdaterEntity Updater, ICreaterEntity Creater, IEntityFinder Finder)
        {
            this.Finder = Finder;
            this.Creater = Creater;
            this.Updater = Updater;
        }
    }
}
