using BotLibary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.Interfaces
{
    interface ISerializator
    {
        public void AddExample(EventArgsUpdate e);

    }
}
