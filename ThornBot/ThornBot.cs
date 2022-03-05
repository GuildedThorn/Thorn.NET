using System.Diagnostics;
using System.Runtime.InteropServices;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Services;
using Victoria;

namespace ThornBot;

public class ThornBot {

    private readonly ServiceProvider _services;
    private readonly IConfigurationRoot _config;
    private readonly DiscordSocketClient _client;
    private readonly LoggingService _loggingService;
    private readonly CommandHandler _commandHandler;
    private readonly LavaNode _lavaNode;

    public ThornBot() {
        _services = ConfigureServices();
        _config = _services.GetRequiredService<IConfigurationRoot>();
        _client = _services.GetRequiredService<DiscordSocketClient>();
        _loggingService = _services.GetRequiredService<LoggingService>();
        _commandHandler = _services.GetRequiredService<CommandHandler>();
        _lavaNode = _services.GetRequiredService<LavaNode>();
    }
    
    public async Task StartAsync() {
        await StartThornBot();
        await StartLavaLinkAsync();
    }

    private async Task StartThornBot() {
        // Login to the bot and start it
        await _client.LoginAsync(TokenType.Bot, _config["Token"]);
        await _client.StartAsync();
        
        // Run this function infinitely so the bot does not shutdown
        await Task.Delay(-1);
        
        await _commandHandler.InitializeAsync();
    }

    private static async Task StartLavaLinkAsync() {
        var processList = Process.GetProcessesByName("java");
        if (processList.Length == 0) {
            var lavalinkFile = Path.Combine(AppContext.BaseDirectory, "Lavalink", "Lavalink.jar");
            if (!File.Exists(lavalinkFile)) return;

            var process = new ProcessStartInfo {
                FileName = "java",
                Arguments = $"-jar \"{Path.Combine(AppContext.BaseDirectory, "Lavalink")}/Lavalink.jar\"",
                WorkingDirectory = Path.Combine(AppContext.BaseDirectory, "Lavalink"),
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Minimized
            };
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // Try to get the java exe path
                var exePath = Environment.GetEnvironmentVariable("PATH")
                    ?.Split(Path.PathSeparator)
                    .FirstOrDefault(x => File.Exists(Path.Combine(x, "java.exe")));

                if (exePath != null) {
                    process.FileName = Path.Combine(exePath, "java.exe");
                }
            }
            Process.Start(process);
            await Task.Delay(2000);
        }
    }

    private static ServiceProvider ConfigureServices() {
        return new ServiceCollection()
            .AddSingleton(new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Resources"))
                .AddJsonFile("config.json", false).Build())
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true, 
                MessageCacheSize = 1000,
            }))
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<LavaNode>()
            .AddSingleton(new LavaConfig())
            .AddSingleton<MusicService>()
            .AddSingleton<InteractionService>()
            .AddSingleton<LoggingService>()
            .BuildServiceProvider();
    }
}