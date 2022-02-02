using BotLibary.BotManager.HelperClasses;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    public class AbstractBotManager
    {
        internal Bot SelectedBot { get; set; }
        protected List<Bot> BotList { get; set; }
        protected private BotCreater BotCreater { get; set; }
        protected private BotSaver BotSaver { get; set; }
        protected private BotFinder BotFinder { get; set; }
        public AbstractBotManager()
        {
            BotCreater = new BotCreater();
            BotSaver = new BotSaver();
            BotFinder = new BotFinder();
            BotList = new List<Bot>();
        }

    }
}
