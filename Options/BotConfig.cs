namespace Options
{
    
    public class BotConfig
    {       
        public  string Token { get;set; }
        public string Password { get; set; }
        public string DataBaseName { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string Direction { get; set; }   
        public BotConfig ()
        {

        }
        public BotConfig(string Token, string Password, string DataBaseName, string Name, string CustomerName, string Direction)
        {
            this.Token = Token;
            this.Password = Password;
            this.DataBaseName = DataBaseName;
            this.Name = Name;
            this.CustomerName = CustomerName;
            this.Direction = Direction;
        }
    }
}