
using DataBase.Database.Interface;

namespace DataBase.Database.Context.MySQL
{
    public class MySqlRepository : IRepository
    {
        public IReader Reader { get; set; }
        public ICreater Creater { get; set; }
        public IUpdater Updater { get; set; }
        public IErraiser Erraiser { get; set; }
        public MySqlRepository()
        {
        }
        public void Init(IReader reader, ICreater creater, IUpdater updater, IErraiser erraiser)
        {
            this.Reader = reader;
            this.Creater = creater;
            this.Updater = updater;
            this.Erraiser = erraiser;
        }
    }
}
