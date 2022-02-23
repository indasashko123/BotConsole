using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using Options;

namespace BotLibary.Bots
{
    public class BotDestinationCreater
    {

        public static IBot Create(BotOptions options)
        {
            switch(options.botConfig.Direction)
            {
                case "Nails":
                    {
                        return CreaterMasterBot.Create(options);
                    }
                default:
                    {
                        return null;
                    }
            }
            
        }
    }
}
