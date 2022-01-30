using System.Collections.Generic;
namespace Options
{
    public class PersonalConfig
    {
        public Dictionary<string, string> Messages { get; set; }
        public Dictionary<string, string> Buttons { get; set; }
        public Dictionary<string, string> AdminButtons { get; set; }
        public Dictionary<string, string> Paths { get; set; }
        public List<string> Partfolio { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Dictionary<string,string> MediaLink { get; set; }
    }
}