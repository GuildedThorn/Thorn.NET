using System.Diagnostics;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace ThornBot.Modules;

public class AdminModule : ModuleBase<SocketCommandContext> {
    
    [Command("deploy")]
    [Discord.Commands.Summary("Deploys the bot's slash commands to the guild")]
    public async Task DeployAsync() {
        var guild = Context.Guild;
        
        if (guild != null) {
            var statusCommand = new SlashCommandBuilder();
            statusCommand.WithName("stats");
            statusCommand.WithDescription("Check the stats of the bot");

            var cpu = new SlashCommandOptionBuilder() {
                Name = "cpu",
                Description = "Check the CPU usage of the bot",
                IsRequired = false
            };
            statusCommand.AddOption(cpu);

            var ram = new SlashCommandOptionBuilder() {
                Name = "ram",
                Description = "Check the RAM usage of the bot",
                IsRequired = false
            };
            statusCommand.AddOption(ram);

            var uptime = new SlashCommandOptionBuilder() {
                Name = "uptime",
                Description = "Check the uptime of the bot",
                IsRequired = false
            };
            statusCommand.AddOption(uptime);

            var threads = new SlashCommandOptionBuilder() {
                Name = "threads",
                Description = "Check the number of threads the bot is using",
                IsRequired = false
            };
            statusCommand.AddOption(threads);


            await guild.CreateApplicationCommandAsync(statusCommand.Build());
        }
    }

    [Command("status")]
    [SlashCommand("status", "Sets the bot's status as online, offline, idle, or dnd")]
    [Discord.Commands.Summary("Sets the bot's status as online, offline, idle, or dnd")]
    public async Task StatusAsync(DiscordSocketClient client, string arg) {
        switch (arg.ToLower()) {
            case "online":
                await client.SetStatusAsync(UserStatus.Online);
                break;
            case "offline":
                await client.SetStatusAsync(UserStatus.Offline);
                break;
            case "idle":
                await client.SetStatusAsync(UserStatus.Idle);
                break;
            case "dnd":
                await client.SetStatusAsync(UserStatus.DoNotDisturb);
                break;
        }
        await ReplyAsync("Status set to " + arg);
    }
}