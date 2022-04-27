using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Handler;
using ThornBot.Services;
using Victoria;

namespace ThornBot;

public class ThornBot {
    
    private readonly IConfigurationRoot _config;
    private readonly DiscordSocketClient _client;
    private readonly DiscordRpcClient _rpc;
    private readonly LoggingService _loggingService;
    private readonly CommandHandler _commandHandler;
    private readonly InteractionService _commands;
    private readonly LavaNode _lavaNode;
    public static DateTime StartTime;

    public ThornBot() {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Resources"))
            .AddJsonFile("config.json", false).Build();
        var services = ConfigureServices(config);
        _config = services.GetRequiredService<IConfigurationRoot>();
        _client = services.GetRequiredService<DiscordSocketClient>();
        _rpc = services.GetRequiredService<DiscordRpcClient>();
        _loggingService = services.GetRequiredService<LoggingService>();
        _commands = services.GetRequiredService<InteractionService>();
        _commandHandler = services.GetRequiredService<CommandHandler>();
        _lavaNode = services.GetRequiredService<LavaNode>();
    }

    public async Task StartThornBot() {
        _client.Ready += OnReadyAsync;

        // Login to the bot and start it
        await _client.LoginAsync(TokenType.Bot, _config["Token"]);
        await _client.StartAsync();
        StartTime = DateTime.Now;
        await _client.SetActivityAsync(new Game("Thorn's apartment.", ActivityType.Watching));

        await _commandHandler.InitializeAsync();


        _rpc.Initialize();
        _rpc.SetPresence(new RichPresence() {
            Details = "Waiting for instructions...",
            State = "From Thorn.",
            Buttons = new [] {
                new Button {
                    Label = "Github",
                    Url = "https://github.com/GuildedThorn"
                },
                new Button {
                    Label = "Twitter",
                    Url = "https://twitter.com/GuildedThorn"
                }
            }
            //TODO Create images for each rich presence
        });

        // Delay this task infinitely so the bot never shuts down
        await Task.Delay(Timeout.Infinite);
    }

    private async Task OnReadyAsync() {
        if (!_lavaNode.IsConnected) {
            await _lavaNode.ConnectAsync();
        }
    }
    
    private static ServiceProvider ConfigureServices(IConfiguration config) {
        return new ServiceCollection()
            .AddSingleton(new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Resources"))
                .AddJsonFile("config.json", false).Build())
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true, 
                MessageCacheSize = 1000,
            }))
            .AddSingleton(new DiscordRpcClient(config["AppID"]))
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<CommandHandler>()
            .AddSingleton<LoggingService>()
            .AddSingleton<MusicService>()
            .AddSingleton<LavaNode>()
            .AddSingleton<LavaConfig>()
            .AddLavaNode(x => {
                x.Hostname = config.GetSection("LavaLink").GetValue<string>("Host");
                x.Port = config.GetSection("LavaLink").GetValue<ushort>("Port");
                x.Authorization = config.GetSection("LavaLink").GetValue<string>("Authorization");
                x.SelfDeaf = config.GetSection("LavaLink").GetValue<bool>("AutoDeafen");
            })
            .BuildServiceProvider();
    }
}