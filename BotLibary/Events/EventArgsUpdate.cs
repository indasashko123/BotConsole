using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BotLibary.Events
{
    public class EventArgsUpdate
    {
        public Bot bot { get; set; }
        public string fileId { get; set; }
        public EventArgsUpdate(Bot bot, string fileId = null)
        {
            this.bot = bot;
            this.fileId = fileId;
        }
    }
}
