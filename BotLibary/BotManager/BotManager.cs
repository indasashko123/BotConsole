using BotLibary.BotManager.HelperClasses;
using BotLibary.BotManager.Interfaces;
using BotLibary.Interfaces;
using System;
using System.Collections.Generic;

namespace BotLibary.BotManager
{
    public class BotManager : AbstractBotManager,IBotManager
    { 
        public BotManager()
        {                 
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
            BotCreater.CreateBot(Name, Path);
        }

        public virtual void SelectBot(string Name, List<Bot> bots)
        {
            BotFinder.FindByName(Name, bots)
        }

        public virtual void ShowBots(List<Bot> bots)
        {
            BotFinder.ShowBots(bots)
        }
    }
}
