using BotLibary.Bots;
using BotLibary.Bots.Interfaces;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotFinder
    {
        IBot FindByName(BotName Name,List<IBot> bots);
        void ShowBots(List<IBot> bots);
        void ShowCurrent(IBot currentBot);
        List<IBot> FindAllBots();
    }
}
