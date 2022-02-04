using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using BotLibary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses
{
    class BotFinder : IBotFinder
    {
        public Bot FindByName(BotName Name, List<Bot> bots)
        {
            throw new NotImplementedException();
        }

        public void ShowBots(List<Bot> bots)
        {
            throw new NotImplementedException();
        }

        public void ShowCurrent(Bot currentBot)
        {
            throw new NotImplementedException();
        }
    }
}
