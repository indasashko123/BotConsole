using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BotLibary.Bots.Events
{
    public class EventArgsUpdate
    {
        public IBot bot { get; set; }
        public string fileId { get; set; }
        public EventArgsUpdate(IBot bot, string fileId = null)
        {
            this.bot = bot;
            this.fileId = fileId;
        }
    }
}
