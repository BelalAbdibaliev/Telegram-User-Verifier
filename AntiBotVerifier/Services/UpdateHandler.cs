using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace AntiBotVerifier
{
    public class UpdateHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                var message = update.Message;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"HELLLLLLOOOOOO {message.From.Username}");
            }
            else if (update.ChatMember != null)
            {
                await botClient.SendTextMessageAsync(update.ChatMember.Chat.Id, 
                    $"Welcome new user: {update.ChatMember.NewChatMember.User.Username}");
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error occurred: {exception.Message}");
            await Task.CompletedTask;
        }
    }
}