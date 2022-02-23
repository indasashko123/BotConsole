using BotLibary.Bots.Interfaces;
using Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.Bots.Masters
{
    internal class CreaterMasterBot
    {
        public static IBot Create(BotOptions options)
        {
            return new MasterBot(options);
        }
    }
}
