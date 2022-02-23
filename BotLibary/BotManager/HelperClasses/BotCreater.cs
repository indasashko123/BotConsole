using BotLibary.BotManager.Interfaces;
using BotLibary.Bots;
using BotLibary.Bots.Interfaces;
using Options;
using System.Collections.Generic;

namespace BotLibary.BotManager.HelperClasses
{
    internal class BotCreater : IBotCreater
    {
        public void Create(BotName Name, List<IBot> bots, string Token, string DataBaseName)
        {
            throw new System.NotImplementedException();
        }
       public void Create(BotConfig config, List<IBot> bots)
        {
            throw new System.NotImplementedException();
        }
    }
}
