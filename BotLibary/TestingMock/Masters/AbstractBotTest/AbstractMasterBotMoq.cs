using BotLibary.Bots.Masters;
using DataBase.Masters.Database;


namespace BotLibary.TestingMock.Masters.AbstractBotTest
{
    public class AbstractMasterBotMoq : AbstractMasterBot
    {
        public DataBaseMastersConnector connector { get { return context; } set { context = value; } }
        public string DBName { get; set; }
        
    }
}
