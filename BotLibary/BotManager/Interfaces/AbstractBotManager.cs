using BotLibary.BotManager.HelperClasses;
using BotLibary.Events;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    public class AbstractBotManager
    {
        public ChangesLog SystemMessage { get; set; }
        public Bot SelectedBot { get; set; }
        public List<Bot> BotList { get; set; }
        protected private IBotCreater BotCreater { get; set; }
        protected private IBotSaver BotSaver { get; set; }
        protected private IBotFinder BotFinder { get; set; }
        public AbstractBotManager()
        {

            BotList = new List<Bot>();
        }

    }
}
