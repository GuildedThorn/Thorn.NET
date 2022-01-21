using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Data;
using ThornBot.Logger;
using ThornBot.Modules;
using EventHandler = ThornBot.Events.EventHandler;

namespace ThornBot;

public class ThornBot {

    public static DiscordSocketClient? Client = null;
    
    private static void Main(string[] data) => new ThornBot().Main().GetAwaiter().GetResult();

    private async Task Main() {

        // Enable all intents for the socket client
        var config = new DiscordSocketConfig() {
            MessageCacheSize = 50,
            //GatewayIntents = GatewayIntents.All
        };
        
        // Create a new DiscordSocketClient
        Client = new DiscordSocketClient(config);
        
        // Handle the config object
        Config.InitConfig();
        
        // Login to the bot token
        await Client.LoginAsync(TokenType.Bot, Config._config["Token"]);
        // Start the bot software
        await Client.StartAsync();

        // Handle all events for the bot
        EventHandler.InitEvents().GetAwaiter().GetResult();

        // run this function infinitely so the bot does not shutdown
        await Task.Delay(-1);
    }
}