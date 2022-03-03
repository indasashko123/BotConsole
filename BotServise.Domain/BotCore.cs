using System;

namespace BotService.Domain
{
    public class BotCore 
    {
        public Guid Id { get; set; }
        public string Options { get; set; }
        public string BotType { get; set; }
        public bool IsStarted { get; set; }
    }
}
