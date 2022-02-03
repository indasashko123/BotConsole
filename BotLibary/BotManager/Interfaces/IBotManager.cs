using BotLibary.Interfaces;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotManager
    {        
        public void CreateBot(BotName Name, string Path);
        public void BotUpdate();
        public void SelectBot(BotName Name, List<Bot> bots);
        public void ShowBots(List<Bot> BotList);
        public void BotStart();
        public void BotStop();
        public string GetCurrentBotName();
    }
}
