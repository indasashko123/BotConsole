using BotLibary.Interfaces;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotManager
    {        
        public void CreateBot(BotName Name, string Path);
        public void BotUpdate();
        public void SelectBot(string Name, List<Bot> BotList);
        public void ShowBots(List<Bot> BotList);
        public void BotStart();
        public void BotStop();
    }
}
