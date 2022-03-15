using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands
{
    class UserChoiseAppointmentCommand : ICallBackCommand
    {
        CallBack callBack;
        DataBaseConnector context;
        DataBase.Models.User currentUser;
        DataBase.Models.User admin;
        BotOptions options;
        public UserChoiseAppointmentCommand(DataBase.Models.User admin, DataBase.Models.User currentUser, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.currentUser = currentUser;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            Appointment app = await context.db.FindAppointmentAsync(callBack.EntityId);
            if (!app.IsEmpty)
            {
                return await bot.SendTextMessageAsync(currentUser.ChatId, 
                                                      options.personalConfig.Messages["SORRYTAKEN"], 
                                                      replyMarkup: KeyBoards.GetStartKeyboard(options));                
            }
            app.IsConfirm = false;
            app.IsEmpty = false;
            app.User = currentUser.UserId;
            Day day = await context.db.FindDayAsync(app.Day);
            string message = $"Новая запись - \n " +
                $"{currentUser.FirstName} {currentUser.LastName} @{currentUser.Username}\n " +
                $"в {day.DayOfWeek}, {day.Date}.{day.MonthNumber} числа\n" +
                $" на время - {app.AppointmentTime}";
            await context.db.UpdateAppAsync(app);
            await bot.SendTextMessageAsync(currentUser.ChatId, "Заявка принята. Ожидайте подтверждения мастера", 
                                           replyMarkup: KeyBoards.GetStartKeyboard(options));
            await bot.SendTextMessageAsync(admin.ChatId, message,
                                           replyMarkup: KeyBoards.GetConfirmKeyboard(app.AppointmentId,
                                                                                     options,
                                                                                     Codes.AdminConfirm, 
                                                                                     currentUser.UserId));
            return null;
        }
    }
}
