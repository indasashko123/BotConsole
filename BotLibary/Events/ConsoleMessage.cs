using System.Threading.Tasks;

namespace BotLibary.Events
{
    public delegate void ChangesLog(string text);
    public delegate Task AdminMessage(EventArgsNotification e);
    public delegate void SendMessage(long id, string text);
}
