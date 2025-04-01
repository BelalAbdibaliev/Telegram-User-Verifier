using AntiBotVerifier.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace AntiBotVerifier;

public class BotService : IBotService
{
    private readonly TelegramBotClient _bot;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BotService> _logger;
    private readonly CancellationTokenSource _cts = new();

    public BotService(TelegramBotClient bot, IServiceProvider serviceProvider, ILogger<BotService> logger)
    {
        _bot = bot;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Start()
    {
        try
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery }
            };

            _bot.StartReceiving(
                async (botClient, update, cancellationToken) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
                    if (updateHandler == null)
                    {
                        _logger.LogError("IUpdateHandler could not be resolved.");
                        return;
                    }

                    await updateHandler.HandleUpdateAsync(botClient, update, cancellationToken);
                },
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            _logger.LogInformation("Bot is up and running...");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during bot initialization: {ExceptionMessage}", ex.Message);
        }

        Console.ReadLine();
        _cts.Cancel();
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => 
                $"Telegram API Error: [Error Code: {apiRequestException.ErrorCode}] Message: {apiRequestException.Message}",
            _ => $"An unexpected error occurred: {exception.Message}"
        };

        _logger.LogError("Exception occurred: {ExceptionType}, Error Message: {ErrorMessage}", exception.GetType().Name, errorMessage);

        return Task.CompletedTask;
    }
}