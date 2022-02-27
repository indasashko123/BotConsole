namespace Options
{
    public class ExampleBotConfig<T>: BotConfig
    {
        public T ExampleConfig { get; set; }
        public ExampleBotConfig(T exampleConfig)
        {
            ExampleConfig = exampleConfig;
        }
        public ExampleBotConfig<T> CreateTemplate(BotConfig config)
        {
            return new ExampleBotConfig<T>(ExampleConfig)
            {
                CustomerName = config.CustomerName,
                DataBaseName = config.DataBaseName,
                Direction = config.Direction,
                Name = config.Name,
                Password = config.Password,
                Token = config.Token
            };
        }
    }
}
