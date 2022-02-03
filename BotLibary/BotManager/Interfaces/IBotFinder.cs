using BotLibary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotFinder
    {
        Bot FindByName(BotName Name,List<Bot> bots);
        void ShowBots(List<Bot> bots);
    }
}
