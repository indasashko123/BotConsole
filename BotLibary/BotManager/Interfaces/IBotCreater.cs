using BotLibary.Bots;
using BotLibary.Bots.Interfaces;
using Options;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotCreater
    {
        void Create(BotName Name,  List<IBot> bots, string Token, string DataBaseName);
        public void Create(BotConfig config, List<IBot> bots);
    }
}
