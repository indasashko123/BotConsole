using BotLibary.BotManager.HelperClasses;
using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using BotLibary.Interfaces;
using Options;
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

        public virtual void CreateBot(BotName Name, string Token, string DataBaseName)
        {
            BotCreater.Create(Name,  BotList, Token, DataBaseName);
        }
        public virtual void CreateBot(BotConfig config)
        {
            BotCreater.Create(config);
        }
        public virtual void SelectBot(BotName Name)
        {
           SelectedBot = BotFinder.FindByName(Name, this.BotList);
        }

        public virtual void ShowBots()
        {
            BotFinder.ShowBots(this.BotList);
        }
        public virtual void ShowCurrent()
        {
            BotFinder.ShowCurrent(SelectedBot);
        }
        public void FindAllBots()
        {
            this.BotList = BotFinder.FindAllBots();
        }
        public void UpdateAll()
        {
            this.BotSaver.UpdateAll(this.BotList);
        }
    }
}
