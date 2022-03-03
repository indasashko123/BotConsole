using BotLibary.Bots.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibary.TestingMock.Events
{
    public class EventsArgNotificationMoq : EventArgsNotification
    {
        public string Message { get; set; }
        public EventsArgNotificationMoq(string Message)
        {
            this.Message = Message;
        }
    }
}
