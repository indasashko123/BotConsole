using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.Commands
{
    class AppointmentCommand : ICommand
    {
        private DataBase.Models.User admin;
        private DataBase.Models.User currentUser;
        private BotOptions options;
        private DataBaseConnector context;
        public AppointmentCommand(DataBase.Models.User admin, DataBase.Models.User currentUser, BotOptions options, DataBaseConnector context)
        {
            this.context = context;
            this.admin = admin;
            this.currentUser = currentUser;
            this.options = options;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, Message message)
        {
                if (admin == null)
                {
                     return await bot.SendTextMessageAsync(currentUser.ChatId, "Бот пока не активирован");               
                }
                return await bot.SendTextMessageAsync(currentUser.ChatId, "Для записи необходимо выбрать месяц", replyMarkup: KeyBoards.GetMonthButtons(await context.db.Reader.GetMonthsAsync(), Codes.UserChoise, currentUser));             
        }
    }
 }
