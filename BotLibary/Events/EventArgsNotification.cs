using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary.Events
{
    public class EventArgsNotification
    {
        public long Admin { get; set; }
        public string Text { get; set; }
        public IReplyMarkup Keyboard { get; set; }
        public EventArgsNotification()
        {

        }
        public EventArgsNotification(long admin,string text)
        {
            Admin = admin;
            Text = text;
        }
        public EventArgsNotification(long admin, string text, IReplyMarkup keyboard)
        {
            Admin = admin;
            Text = text;
            Keyboard = keyboard;
        }
    }
}
