
using DataBase.Database;
using DataBase.Database.Context.MySQL;
using System;
using UnutTesting.DbTestContext;

namespace UnutTesting
{
    public abstract class ConnectorTest : IDisposable
    {
        protected readonly DataBaseConnector connector;
        protected readonly MySqlDbContext context;
        public ConnectorTest()
        {
            var repository = new MySqlRepository();
            repository.Init(new Reader("TST"),
                            new Creater("TST"),
                            new Updater("TST"),
                            new Erraiser("TST"));
            connector = new DataBaseConnector(repository);    
        }
        public DataBaseConnector getConnector()
        {
            return this.connector;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
