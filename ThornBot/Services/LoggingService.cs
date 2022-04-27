using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ThornBot.Services;

public class LoggingService {
    
    public LoggingService(IServiceProvider services) {
        var client = services.GetRequiredService<DiscordSocketClient>();
        var interactionService = services.GetRequiredService<InteractionService>();
        
        client.Log += OnLogAsync;
        interactionService.Log += OnLogAsync;
    }
    
    private static Task OnLogAsync(LogMessage message) {
        var msg = $"{DateTime.Now,-19} {message.Source}: {message.Message} {message.Exception}";
        
        switch (message.Severity) {
            case LogSeverity.Critical:
            case LogSeverity.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case LogSeverity.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case LogSeverity.Debug: 
            case LogSeverity.Verbose: 
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;
            case LogSeverity.Info:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
        }
        Console.WriteLine(msg);
        Console.ResetColor();
        return Task.CompletedTask;
    }
}