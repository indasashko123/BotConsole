using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.HelperClasses
{
    class ConsoleFinder : IBotFinder
    {
        ChangesLog log { get; set; }
        public ConsoleFinder(ChangesLog log)
        {
            this.log = log;
        }
        public Bot FindByName(BotName Name, List<Bot> bots)
        {
            return bots.Where(bot => bot.BotName.Name == Name.Name).FirstOrDefault();
        }

        public void ShowBots(List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                string answer = $" bot name is{bot.BotName.Name}, customer is {bot.BotName.CustomerName}, direction is  {bot.BotName.Direction}\n";
                log?.Invoke(answer);
            }
        }
        public void ShowCurrent(Bot bot)
        {
            string answer = $" bot name is{bot.BotName.Name}, customer is {bot.BotName.CustomerName}, direction is  {bot.BotName.Direction}\n";
            log?.Invoke(answer);
        }
        
    }
}
