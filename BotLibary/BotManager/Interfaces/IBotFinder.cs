using BotLibary.TelegramBot;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotFinder
    {
        Bot FindByName(BotName Name,List<Bot> bots);
        void ShowBots(List<Bot> bots);
        void ShowCurrent(Bot currentBot);
        List<Bot> FindAllBots();
    }
}
