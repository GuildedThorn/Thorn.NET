using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Modules;

namespace ThornBot.Services; 

public class CommandHandler {

    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;
    private readonly IConfigurationRoot _config;
    
    public CommandHandler(IServiceProvider services) {
        _commands = services.GetRequiredService<CommandService>();
        _client = services.GetRequiredService<DiscordSocketClient>();
        _config = services.GetRequiredService<IConfigurationRoot>();
        _services = services;
    }

    public async Task InitializeAsync() {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        await _commands.AddModuleAsync<AdminModule>(_services);
        _client.MessageReceived += HandleCommandAsync;
        _client.SlashCommandExecuted += HandleSlashCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam) {
        // Don't process the command if it was a system message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        var argPos = 0;

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (!(message.HasCharPrefix('!', ref argPos) || 
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        // Create a WebSocket-based command context based on the message
        var context = new SocketCommandContext(_client, message);

        // Execute the command with the command context we just
        // created, along with the service provider for precondition checks.
        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: null);
    }

    private async Task HandleSlashCommandAsync(SocketSlashCommand command) {
        await command.RespondAsync($"You executed {command.Data.Name}");
    }
}