using AntiBotVerifier;
using AntiBotVerifier.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices(ConfigureServices)
    .Build();

var botService = host.Services.GetRequiredService<IBotService>();
botService.Start();
        
await host.RunAsync();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{

    var botToken = context.Configuration.GetValue<string>("TelegramBotApiKey");
    services.AddSingleton<TelegramBotClient>(new TelegramBotClient(botToken));
    
    services.AddSingleton<IUpdateHandler, UpdateHandler>();
    services.AddSingleton<IBotService, BotService>();
    
}