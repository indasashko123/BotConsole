
using Options;

namespace BotLibary.Interfaces
{
    internal interface IBot
    {
        void  BotStart();
        void BotStop();
        void GetNewConfig(BotOptions options);
    }
}
