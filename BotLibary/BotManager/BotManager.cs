using BotLibary.BotManager.HelperClasses;
using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
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
        public virtual void CreateBot(string Data)
        {
            BotCreater.Create(Data, BotList);
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
