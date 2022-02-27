using BotLibary.Bots;
using Options;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotManager
    {
        public void CreateBot(string Data);
        public void SelectBot(BotName Name);
        public void BotUpdate();      
        public void ShowBots();
        public void BotStart();
        public void BotStop();
        public void ShowCurrent();
        public void FindAllBots();
        public void UpdateAll();
    }
}
