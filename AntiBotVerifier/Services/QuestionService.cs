using AntiBotVerifier.Entities;
using AntiBotVerifier.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace AntiBotVerifier;

public class QuestionService: IQuestionService
{
    private readonly ILogger<QuestionService> _logger;
    private List<Question> Questions;

    public QuestionService(ILogger<QuestionService> logger)
    {
        _logger = logger;
    }
    
    public async Task SendQuestionAsync(ITelegramBotClient botClient ,Question question)
    {
        throw new NotImplementedException();
    }
}