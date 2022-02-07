using Discord;
using ThornBot.Handler;
using Victoria;

namespace ThornBot.Services; 

public class MusicService {

    private readonly LavaNode _lavaNode;
    
    public MusicService(LavaNode lavaNode) => _lavaNode = lavaNode;
    
    public async Task<Embed> JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel textChannel) {
        if (_lavaNode.HasPlayer(guild)) {
            return await EmbedHandler.CreateBasicEmbed("ThornBot", "I'm already connected to a voice channel.", Color.Red);
        }

        if (voiceState.VoiceChannel is null) {
            return await EmbedHandler.CreateBasicEmbed("ThornBot", "You must be in a voice channel.", Color.Red);
        }

        try {
            await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
            return await EmbedHandler.CreateBasicEmbed("ThornBot", "Successfully joined the voice channel.", Color.Red);
        }
        catch (Exception e) {
            return await EmbedHandler.CreateBasicEmbed("ThornBot", e.Message, Color.Red);
        }
    }
    
    
    
}