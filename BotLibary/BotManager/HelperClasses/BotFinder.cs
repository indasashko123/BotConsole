using BotLibary.BotManager.Interfaces;
using BotLibary.TelegramBot;
using System;
using System.Collections.Generic;

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
        public List<Bot> FindAllBots()
        {
            throw new NotImplementedException();
        }
    }
}
