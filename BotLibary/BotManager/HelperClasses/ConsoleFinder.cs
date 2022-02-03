using BotLibary.BotManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleFinder : IBotFinder
    {
        public Bot FindByName(BotName Name, List<Bot> bots)
        {
            return bots.Where(bot => bot.BotName.Name == Name.Name).FirstOrDefault();
        }

        public void ShowBots(List<Bot> bots)
        {
            throw new NotImplementedException();
        }
    }
}
