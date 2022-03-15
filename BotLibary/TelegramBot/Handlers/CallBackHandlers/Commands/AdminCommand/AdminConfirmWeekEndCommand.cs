using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminConfirmWeekEndCommand : ICallBackCommand
    {
        DataBaseConnector context;
        BotOptions options;
        CallBack callBack;
        DataBase.Models.User admin;
        public AdminConfirmWeekEndCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            List<Appointment> apps = await context.db.FindAppointmentsAsync(callBack.EntityId);
            Day day = await context.db.FindDayAsync(callBack.EntityId);
            foreach (Appointment app in apps)
            {
                if (!app.IsEmpty)
                {
                    DataBase.Models.User user = await context.db.FindUserAsync(app.User);
                    await bot.SendTextMessageAsync(user.ChatId, options.personalConfig.Messages["USERAPPISCANCEL"]);
                }
            }
            day.IsWorkDay = false;
            await context.db.UpdateDayAsync(day);
            await bot.SendTextMessageAsync(admin.ChatId, $"День {day.Date}.{day.MonthNumber} теперь выходной\n", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            return null;
        }
    }
}
