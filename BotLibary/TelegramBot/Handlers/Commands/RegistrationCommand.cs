using System;
using DataBase.Database;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Generic;
using DataBase.Models;
using Options;
using BotLibary.Events;
using BotLibary.TelegramBot.ReplyMarkups;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class RegistrationCommand : ICommand
    {
        public event ChangesLog ConsoleMessage;

        private readonly DateFunction dateFunction;
        private readonly DataBaseConnector context;
        private readonly BotOptions options;
        private readonly DataBase.Models.User currentUser;
        private DataBase.Models.User admin;
        public event ChangesLog consoleMessage;
        public event AdminMessage adminMessage;
        public RegistrationCommand(
            DateFunction dateFunction,
            DataBaseConnector context,
            BotOptions options,
            DataBase.Models.User currentUser,
            DataBase.Models.User admin,
            ChangesLog consoleMessage,
            AdminMessage adminMessage
            )
        {
            this.dateFunction = dateFunction;
            this.context = context;
            this.options = options;
            this.currentUser = currentUser;
            this.admin = admin;
            this.consoleMessage += (string text) => consoleMessage?.Invoke(text);
            this.adminMessage += (EventArgsNotification e) => adminMessage?.Invoke(e);
        }      
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            await dateFunction.CreateMonthsAsync(DateTime.Now);
            await context.db.Creater.AddMonthAsync(dateFunction.CurrentMonth);
            await context.db.Creater.CreateDaysAsync(dateFunction.CurrentDay, dateFunction.CurrentMonth, dateFunction.DayNames);
            List<Day> daysCurrentMonth = await context.db.Reader.FindDaysAsync(dateFunction.CurrentMonth.MonthId);
            foreach (Day day in daysCurrentMonth)
            {
                for (int i = 0; i < options.botConfig.appointmentStandartTimes.Count; i++)
                {
                    Appointment app = new Appointment(options.botConfig.appointmentStandartTimes[i], day.DayId);
                    await context.db.Creater.AddAppointmentAsync(app);
                }
            }
            await context.db.Creater.AddMonthAsync(dateFunction.NextMonth);
            await context.db.Creater.CreateDaysAsync(1, dateFunction.NextMonth, dateFunction.DayNames);
            List<Day> daysNextMonth = await context.db.Reader.FindDaysAsync(dateFunction.NextMonth.MonthId);
            foreach (Day day in daysNextMonth)
            {
                for (int i = 0; i < options.botConfig.appointmentStandartTimes.Count; i++)
                {
                    Appointment app = new Appointment(options.botConfig.appointmentStandartTimes[i], day.DayId);
                    await context.db.Creater.AddAppointmentAsync(app);
                }
            }
            currentUser.IsAdmin = true;
            await context.db.Updater.UpdateUserAsync(currentUser);
            admin = await context.db.Reader.FindAdminAsync();
            if (admin == null)
            {        
                return null;
            }
            consoleMessage?.Invoke($"Зарегестрирован {admin.FirstName}");
            adminMessage?.Invoke(new EventArgsNotification(admin.ChatId, "Зарегестрирован!", KeyBoards.GetKeyboardAdmin(options)));
            return null;
        }
    
    }
}
