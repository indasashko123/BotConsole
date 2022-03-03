using BotLibary.Bots.Interfaces;
using Options;
using Options.MasterBotConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.Bots.Masters
{
    internal class CreaterMasterBot
    {
        public static IBot Create(BotOptions<MasterBotConfig, PersonalMasterBotConfig> options)
        {
            return new MasterBot(options);
        }
    }
}
