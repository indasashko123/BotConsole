using BotLibary.Bots.Interfaces;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotSaver
    {
        void Update(IBot SelectedBot);
        void UpdateAll(List<IBot> bots);
        //TODO: Serializate config
    }
}
