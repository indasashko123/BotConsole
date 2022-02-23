using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
using BotLibary.Bots.Interfaces;
using System;
using System.Collections.Generic;

namespace BotLibary.BotManager.HelperClasses
{
    class BotFinder : IBotFinder
    {
        public IBot FindByName(BotName Name, List<IBot> bots)
        {
            throw new NotImplementedException();
        }

        public void ShowBots(List<IBot> bots)
        {
            throw new NotImplementedException();
        }

        public void ShowCurrent(IBot currentBot)
        {
            throw new NotImplementedException();
        }
        public List<IBot> FindAllBots()
        {
            throw new NotImplementedException();
        }
    }
}
