using System.Collections.Generic;

namespace Options
{
    public class BotConfig
    {
        public string token { get;protected set; }
        public string password { get; protected set; }
        public string dataBaseName { get; protected set; }
        public int appointmentStandartCount { get; protected set; }
        public List<string> appointmentStandartTimes { get; protected set; }
       public BotConfig()
        {
            appointmentStandartTimes = new List<string>();
        }
    }
}