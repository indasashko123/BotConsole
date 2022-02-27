using BotLibary.Bots.Interfaces;
using BotLibary.Bots.Masters;
using Options;
using Options.MasterBotConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.Bots
{
    public class BotDestinationCreater
    {
        public static IBot Create<T,K>(BotOptions<T,K> options, string Direction)
        {
            switch(Direction)
            {
                case "Nails":
                    {
                        try
                        {
                            var config = options as BotOptions<MasterBotConfig, PersonalMasterBotConfig>;
                            return new MasterBot(config);
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                default:
                    {
                        return null;
                    }
            }    
        }
    }
}
