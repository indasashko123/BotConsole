
using BotLibary.Bots.Events;
using BotLibary.Bots.Interfaces;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    public class AbstractBotManager
    {
        public ChangesLog SystemMessage { get; set; }
        public IBot SelectedBot { get; set; }
        public List<IBot> BotList { get; set; }
        protected private IBotCreater BotCreater { get; set; }
        protected private IBotSaver BotSaver { get; set; }
        protected private IBotFinder BotFinder { get; set; }
        public AbstractBotManager()
        {
            BotList = new List<IBot>();
        }

    }
}
