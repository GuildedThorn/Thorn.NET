using Discord;
using Discord.WebSocket;

namespace ThornBot.Commands; 

public class StatusCommand {

    public static async void RegisterSlashCommand(SocketGuild? guild) {
        if (guild != null) {
            var statusCommand = new SlashCommandBuilder();
            statusCommand.WithName("status");
            statusCommand.WithDescription("Check the status of the bot");

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
}