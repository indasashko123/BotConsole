using BotLibary.TelegramBot;

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
