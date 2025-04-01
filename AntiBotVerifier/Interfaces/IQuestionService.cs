using AntiBotVerifier.Entities;
using Telegram.Bot;

namespace AntiBotVerifier.Interfaces;

public interface IQuestionService
{
    Task SendQuestionAsync(ITelegramBotClient botClient ,Question question);
}