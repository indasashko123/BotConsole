using BotLibary.BotManager.HelperClasses;
using BotLibary.BotManager.Interfaces;
using BotLibary.Interfaces;
using System;
using System.Collections.Generic;

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
            BotSaver.Update(SelectedBot);
        }

        public virtual void CreateBot(BotName Name, string Path)
        {
            BotCreater.Create(Name, Path, BotList);
        }

        public virtual void SelectBot(BotName Name, List<Bot> bots)
        {
           SelectedBot = BotFinder.FindByName(Name, bots);
        }

        public virtual void ShowBots(List<Bot> bots)
        {
            BotFinder.ShowBots(bots);
        }
        public virtual string GetCurrentBotName()
        {
            return this.SelectedBot.BotName.Name;
        }
    }
}
