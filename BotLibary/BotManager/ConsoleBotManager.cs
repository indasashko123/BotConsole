using BotLibary.BotManager.HelperClasses;
using BotLibary.Events;
namespace BotLibary.BotManager
{
    public class ConsoleBotManager: BotManager
    {
        public event ChangesLog onChangerLog;
        string Path { get; set; }
        public override void InitBotManager()
        {
            BotCreater = new ConsoleCreator(onChangerLog,Path);
            BotSaver = new ConsoleSaver(onChangerLog,Path);
            BotFinder = new ConsoleFinder(onChangerLog,Path);
        }
        public ConsoleBotManager(string Path, ChangesLog log)
        {
            this.Path = Path;
            onChangerLog += (string text) => log?.Invoke(text);
            InitBotManager();
        }
        public void TryLog(string log)
        {
            onChangerLog?.Invoke($"{log}");
        }

    }
}
