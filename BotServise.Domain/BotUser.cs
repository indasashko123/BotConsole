using System;

namespace BotService.Domain
{
    public class BotUser
    {
        public Guid Id { get; set; }
        public Guid BotId { get; set; }
        public string Status { get; set; }
    }
}
