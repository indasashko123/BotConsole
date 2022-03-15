using BotLibary.TelegramBot;
using System.Collections.Generic;

namespace BotLibary.BotManager.Interfaces
{
    interface IBotSaver
    {
        void Update(Bot SelectedBot);

        void UpdateAll(List<Bot> bots);
        void SetNewConfig(Bot SelectedBot);
    }
}
