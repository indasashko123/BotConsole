
namespace Options
{
    public class BotOptions<T,PT>
    {
        public T botConfig { get; set; }
        public PT personalConfig { get; set; }
        public BotOptions (T bot, PT pers)
        {
            botConfig = bot;
            personalConfig = pers;
        }
    }
}
