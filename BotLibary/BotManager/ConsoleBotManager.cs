using BotLibary.BotManager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.BotManager
{
    public class ConsoleBotManager: BotManager
    {
        
        public override void InitBotManager()
        {
            this.SystemMessage = ((string text) => { Console.WriteLine(text); });
            this.BotCreater = new ConsoleCreator(SystemMessage);
            this.BotSaver = new BotSaver();
            this.BotFinder = new ConsoleFinder();
        }
        
    }
}
