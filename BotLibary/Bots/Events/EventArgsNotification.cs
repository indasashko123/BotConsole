﻿
using Telegram.Bot.Types.ReplyMarkups;

namespace BotLibary.Bots.Events
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
