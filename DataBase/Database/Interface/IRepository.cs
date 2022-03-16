namespace DataBase.Database.Interface
{
    public interface IRepository
    {
        IReader Reader { get; set; }
        ICreater Creater { get; set; }
        IUpdater Updater { get; set; }
        IErraiser Erraiser { get; set; }
        public void Init(IReader reader, ICreater creater, IUpdater updater, IErraiser erraiser);
    }
}
