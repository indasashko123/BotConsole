using BotLibary.TelegramBot.Handlers.Commands;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using DataBase.Models;
using Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.AdminCommands
{
    class AddAppointmentTimeCommand : ICommand
    {
        readonly DataBase.Models.User admin;
        readonly DataBaseConnector context;
        readonly BotOptions options;
        string data;
        public AddAppointmentTimeCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, string data)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.data = data;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
            if (Int32.TryParse(admin.Status.Split('/')[1], out int appId))
            {
                admin.Status = "";
                await context.db.UpdateUserAsync(admin);
                Appointment app = await context.db.FindAppointmentAsync(appId);
                if (data.Length == 5)
                {
                    app.AppointmentTime = data;
                    await context.db.UpdateAppAsync(app);
                    return await bot.SendTextMessageAsync(admin.ChatId, $"Добавилась запись на {app.AppointmentTime}", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                    
                }
                else
                {
                    await context.db.DeleteAppAsync(app);
                    return await bot.SendTextMessageAsync(admin.ChatId, $" запись удалена", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
                    
                }
            }
            else
            {
                return await bot.SendTextMessageAsync(admin.ChatId, $"Запись не добавилась. Ошибка ", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            }
        }
    }
}
