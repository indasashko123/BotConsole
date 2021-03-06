using BotLibary.Events;
using BotLibary.Interfaces;
using Options;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotManager
    {
        public void CreateBot(string config);
        public void BotUpdate();
        public void SelectBot(BotName Name);
        public void ShowBots();
        public void BotStart();
        public void BotStop();
        public void ShowCurrent();
        public void FindAllBots();
        public void UpdateAll();
    }
}
