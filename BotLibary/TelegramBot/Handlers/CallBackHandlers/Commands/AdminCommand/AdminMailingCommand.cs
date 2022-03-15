using BotLibary.TelegramBot.CallBackData;
using BotLibary.TelegramBot.ReplyMarkups;
using DataBase.Database;
using Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotLibary.TelegramBot.Handlers.CallBackHandlers.Commands.AdminCommand
{
    class AdminMailingCommand : ICallBackCommand
    {
        Message lastMessage;
        DataBase.Models.User admin;
        BotOptions options;
        DataBaseConnector context;
        CallBack callBack;
        public AdminMailingCommand(DataBase.Models.User admin, DataBaseConnector context, BotOptions options, Message lastMessage, CallBack callBack)
        {
            this.admin = admin;
            this.context = context;
            this.options = options;
            this.lastMessage = lastMessage;
            this.callBack = callBack;
        }
        public async Task<Message> ReturnCommand(ITelegramBotClient bot, CallbackQuery callBackQuery)
        {
            if (lastMessage == null)
            {
                return await bot.SendTextMessageAsync(admin.ChatId,
                                                            "Не выбрано сообщение",
                                                            replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            }

            await Task.Run(() => admin.Status = "");
            await context.db.UpdateUserAsync(admin);
            if (callBack.Stage == Stage.Yes)
            {
                var users = await context.db.FindUsersAsync();
                foreach (var user in users)
                {
                    try
                    {
                        await bot.ForwardMessageAsync(user.ChatId, admin.ChatId, lastMessage.MessageId);
                    }
                    catch
                    {
                        await bot.SendTextMessageAsync(admin.ChatId, $"Замучен {user}");
                    }
                }
            }
            else
            {
                await bot.SendTextMessageAsync(admin.ChatId, "Отменено", replyMarkup: KeyBoards.GetKeyboardAdmin(options));
            }
            lastMessage = null;
            return null;
        }
    }
}
