using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Handler;

namespace ThornBot.Modules; 

public class UserModule : InteractionModuleBase<SocketInteractionContext> {

    private readonly IServiceProvider _services;

    public UserModule(IServiceProvider services) {
        _services = services;
    }

    [SlashCommand("info", "Get statistic info about the bot")]
    public async Task InfoAsync() {
        var platform = System.Environment.OSVersion.Platform;
        var version = System.Environment.OSVersion.Version;
        var uptime = DateTime.Now - ThornBot.StartTime;

        await RespondAsync(embed: await EmbedHandler.CreateBasicEmbed("ThornBot",
            ".A personal utility bot made by https://github.com/GuildedThorn\n\n" +
            "**Bot Version:** 1.0.0" +
            "**Bot Platform:** " + platform + "\n" +
            "**Bot OS Version:** " + version + "\n" +
            "**Bot Uptime:** " + uptime.ToString(@"dd\.hh\:mm\:ss")));
    }
}