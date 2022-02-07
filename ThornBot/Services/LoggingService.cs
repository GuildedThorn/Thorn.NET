using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ThornBot.Services;

public class LoggingService {
    
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly InteractionService _interactionService;
    
    public LoggingService(IServiceProvider services) {
        _commands = services.GetRequiredService<CommandService>();
        _client = services.GetRequiredService<DiscordSocketClient>();
        _interactionService = services.GetRequiredService<InteractionService>();
        
        _client.Log += OnLogAsync;
        _commands.Log += OnLogAsync;
        _interactionService.Log += OnLogAsync;
        
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