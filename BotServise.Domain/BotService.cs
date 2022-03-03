using BotLibary.BotManager;

namespace BotService.Domain
{
    public class BotService
    {
        private static BotService Instance;
        private BotService()
        {
            Manager = new BotManager();
        }
        public readonly BotManager Manager;
        public static BotService GetBotservice()
        {
            if (Instance == null)
            {
                Instance = new BotService();
            }
            return Instance;
        }
    }
}
