using System.Collections.Generic;
namespace Options.MasterBotConfig
{
    public class MasterBotConfig : BotConfig
    {
        public int appointmentStandartCount { get; set; }
        public List<string> appointmentStandartTimes { get; set; }
        public MasterBotConfig(string aptTimes, BotConfig config)
        {
            appointmentStandartTimes = new List<string>();
            appointmentStandartCount = 0;
            string[] times = aptTimes.Split('|');
            foreach (var time in times)
            {
                appointmentStandartTimes.Add(time);
                appointmentStandartCount++;
                Token = config.Token;
                Password = config.Password;
                Name = config.Name;
                Direction = config.Direction;
                DataBaseName = config.DataBaseName;
                CustomerName = config.CustomerName;
            }
        }       
    }
}
