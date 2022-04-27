using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ThornBot.Handler;

public class CommandHandler {

    private readonly IServiceProvider _services;
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactions;

    public CommandHandler(IServiceProvider services) {
        _services = services;
        _client = services.GetRequiredService<DiscordSocketClient>();
        _interactions = services.GetRequiredService<InteractionService>();
    }

    public async Task InitializeAsync() {
        await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        
        _client.InteractionCreated += HandleInteraction;
        //_client.Ready += () => _interactions.RegisterCommandsToGuildAsync(ulong.Parse("887516061266755585"), true);
    }

    private async Task HandleInteraction(SocketInteraction arg) {
        try {
            var ctx = new SocketInteractionContext(_client, arg);
            await _interactions.ExecuteCommandAsync(ctx, _services);
        } catch (Exception ex) { 
            Console.WriteLine(ex);
            
            if (arg.Type == InteractionType.ApplicationCommand) {
                await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}