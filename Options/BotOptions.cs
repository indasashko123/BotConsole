using System;

namespace Options
{
    public class BotOptions
    {
        public BotConfig botConfig { get; set; }
        public PersonalConfig personalConfig { get; set; }
        public BotOptions (BotConfig bot, PersonalConfig pers)
        {
            botConfig = bot;
            personalConfig = pers;
        }
    }
}
