using BotLibary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotCreater
    {
        void Create(BotName Name, string Path, List<Bot> bots);
    }
}
