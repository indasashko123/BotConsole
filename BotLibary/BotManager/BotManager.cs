using BotLibary.BotManager.HelperClasses;
using BotLibary.BotManager.Interfaces;
using BotLibary.TelegramBot;

namespace BotLibary.BotManager
{
    public class BotManager : AbstractBotManager,IBotManager
    { 
        public virtual void InitBotManager()
        {
            BotCreater = new BotCreater();
            BotSaver = new BotSaver();
            BotFinder = new BotFinder();
        }
        public BotManager()
        {
            InitBotManager();
        }
        public virtual void BotStart()
        {
            if(SelectedBot != null)
            SelectedBot.BotStart();
        }
        public virtual void BotStop()
        {
            if (SelectedBot != null)
                SelectedBot.BotStop();
        }
        public virtual void BotUpdate()
        {
            if (SelectedBot != null)
            BotSaver.Update(SelectedBot);
        }
        public virtual void CreateBot(string config)
        {
            BotCreater.Create(config, BotList);
        }
        public virtual void SelectBot(BotName Name)
        {
           SelectedBot = BotFinder.FindByName(Name, BotList);
        }
        public virtual void ShowBots()
        {
            BotFinder.ShowBots(BotList);
        }
        public virtual void ShowCurrent()
        {
            BotFinder.ShowCurrent(SelectedBot);
        }
        public void FindAllBots()
        {
            BotList = BotFinder.FindAllBots();
        }
        public void UpdateAll()
        {
            BotSaver.UpdateAll(BotList);
        }
        public void UpdateCurrent()
        {
            BotSaver.SetNewConfig(SelectedBot);
        }
    }
}
