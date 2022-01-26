using System.Collections.Generic;

namespace Options
{
    public class BotConfig
    {
        public string token { get; set; }
        public string password { get; set; }
        public string dataBaseName { get; set; }
        public int appointmentStandartCount { get; set; }
        public List<string> appointmentStandartTimes { get; set; }
    }
}