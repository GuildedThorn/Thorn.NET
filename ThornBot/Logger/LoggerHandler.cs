using Discord;

namespace ThornBot.Logger;

public class LoggerHandler {

    public static Task InitLogger(LogMessage message) {
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
        Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
    
}