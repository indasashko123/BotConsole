
using System.Collections.Generic;


namespace BotLibary.BotManager.Interfaces
{
    interface IBotCreater
    {        
        public void Create(string config, List<Bot> bots);
    }
}
