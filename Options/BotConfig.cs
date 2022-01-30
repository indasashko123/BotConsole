using System.Collections.Generic;

namespace Options
{
    public class BotConfig
    {
        public string token { get; }
        public string password { get; }
        public string dataBaseName { get; }
        public int appointmentStandartCount { get;}
        public List<string> appointmentStandartTimes { get; }
    }
}