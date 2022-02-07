using Discord;

namespace ThornBot.Handler; 

public class EmbedHandler {

    public static async Task<Embed> CreateBasicEmbed(string title, string description, Color color) {
        var embed = await Task.Run(() => (new EmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(color)
            .WithCurrentTimestamp().Build()));
        return embed;
    }

    public static async Task<Embed> CreateBasicEmbedWithFields(string title, string description, EmbedFieldBuilder[] fields, Color color) {
        var embed = await Task.Run(() => (new EmbedBuilder()
            .WithTitle(title)
            .WithFields(fields)
            .WithDescription(description)
            .WithColor(color)
            .WithCurrentTimestamp().Build()));
        return embed;
    }
}