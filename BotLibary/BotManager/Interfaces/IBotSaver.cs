using BotLibary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotSaver
    {
        void Update(Bot SelectedBot);
        List<Bot> FindAllBots();
        void UpdateAll(List<Bot> bots);
    }
}
