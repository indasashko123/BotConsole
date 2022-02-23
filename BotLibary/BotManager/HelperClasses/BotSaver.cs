using BotLibary.BotManager.Interfaces;
using BotLibary.Bots.Interfaces;
using System;
using System.Collections.Generic;

namespace BotLibary.BotManager.HelperClasses
{
    class BotSaver : IBotSaver
    {    
        public void Update(IBot SelectedBot)
        {
            
        }

        public void UpdateAll(List<IBot> bots)
        {
            throw new NotImplementedException();
        }
    }
}
