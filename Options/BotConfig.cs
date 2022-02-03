using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;
namespace Options
{
    
    public class BotConfig
    {
        
        public string token { get;set; }
        public string password { get; set; }
        public string dataBaseName { get; set; }
        public int appointmentStandartCount { get;set; }
        public List<string> appointmentStandartTimes { get; set; }
       public BotConfig()
        {
            appointmentStandartTimes = new List<string>();
            token = "";
            password = "";
            dataBaseName = "";
            appointmentStandartCount = 3;
        }
        public BotConfig(string Path)
        {
            BotConfig config;
            string text = File.ReadAllText(Path, Encoding.UTF8);
            config = JsonConvert.DeserializeObject<BotConfig>(text);                   
            appointmentStandartTimes = config.appointmentStandartTimes;
            token = config.token;
            password = config.password;
            dataBaseName = config.dataBaseName;
            appointmentStandartCount = config.appointmentStandartCount;
        }
        public BotConfig CreatTemplate()
        {
            return new BotConfig()
            {
                appointmentStandartTimes = new List<string>()         
                   {  
                    "10:00",
                    "14:00",
                    "18:00"},
            token = "1973705386:AAECVqHeUF6GHIFOT0xnzD23mNu9HPL0eMs",
            password = "xxxx",
            dataBaseName = "testname",
            appointmentStandartCount = 3
        };           
        }
    }
}