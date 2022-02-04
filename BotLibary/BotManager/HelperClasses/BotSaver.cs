using BotLibary.BotManager.Interfaces;
using BotLibary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Options;

namespace BotLibary.BotManager.HelperClasses
{
    class BotSaver : IBotSaver
    {
        public List<Bot> FindAllBots()
        {
            throw new NotImplementedException();
        }

        public void Update(Bot SelectedBot)
        {
            
        }

        public void UpdateAll(List<Bot> bots)
        {
            throw new NotImplementedException();
        }
    }
}
