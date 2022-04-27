using System.Text;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Handler;
using Victoria;
using Victoria.Enums;
using Victoria.Responses.Search;

namespace ThornBot.Services; 

public class MusicService {

    private readonly LavaNode _lavaNode;

    public MusicService(IServiceProvider services) {
        _lavaNode = services.GetRequiredService<LavaNode>();
    }
    
    public async Task<Embed> JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel textChannel) {
        
        if (_lavaNode.HasPlayer(guild)) {
            return await EmbedHandler.CreateErrorEmbed("I'm already connected to a voice channel.");
        }
        
        if (voiceState.VoiceChannel == null) {
            return await EmbedHandler.CreateErrorEmbed("You must be in a voice channel to run this command.");
        }

        try {
            await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
            return await EmbedHandler.CreateBasicEmbed("ThornBot", $"I've connected to {voiceState.VoiceChannel.Name} successfully.");
            
        } catch (Exception exception) {
            return await EmbedHandler.CreateErrorEmbed(exception.Message);
        }
    }

    public async Task<Embed> PlayAsync(SocketGuildUser user, IGuild guild, string query) {
        
        if (user.VoiceChannel == null) {
            return await EmbedHandler.CreateErrorEmbed("You must be in a voice channel to run this command.");
        }

        if (!_lavaNode.HasPlayer(guild)) {
            return await EmbedHandler.CreateErrorEmbed("I'm not connected to a voice channel.");
        }

        try {
            var player = _lavaNode.GetPlayer(guild);

            var search = Uri.IsWellFormedUriString(query, UriKind.Absolute)
                ? await _lavaNode.SearchAsync(SearchType.Direct, query)
                : await _lavaNode.SearchYouTubeAsync(query);

            if (search.Status == SearchStatus.NoMatches) {
                return await EmbedHandler.CreateErrorEmbed($"I wasn't able to find anything for {query}.");
            }

            var track = search.Tracks.FirstOrDefault();

            if (player.Track != null && player.PlayerState is PlayerState.Playing ||
                player.PlayerState is PlayerState.Paused) {
                
                player.Queue.Enqueue(track);
                return await EmbedHandler.CreateBasicEmbed("ThornBot", $"Queued: {track.Title}\nUrl: {track.Url}");
            }

            await player.PlayAsync(track);
            return await EmbedHandler.CreateBasicEmbed("ThornBot", $"Queued: {track.Title}\nUrl: {track.Url}");
        } catch (Exception exception) {
            return await EmbedHandler.CreateErrorEmbed(exception.Message);
        }
    }
    
    public async Task<Embed> SkipAsync(IGuild guild) {
        if (!_lavaNode.HasPlayer(guild)) {
            return await EmbedHandler.CreateErrorEmbed("I'm not connected to a voice channel.");
        }

        var player = _lavaNode.GetPlayer(guild);
        await player.SkipAsync();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Skipped the current song.");
    }
    
    public async Task<Embed> StopAsync(IGuild guild) {
        if (!_lavaNode.HasPlayer(guild)) {
            return await EmbedHandler.CreateErrorEmbed("I'm not connected to a voice channel.");
        }

        var player = _lavaNode.GetPlayer(guild);
        await player.StopAsync();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Stopped the current song.");
    }

    public async Task<Embed> PauseAsync(IGuild guild) { 
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        await player.PauseAsync();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Paused the current song.");
    }
    
    public async Task<Embed> ResumeAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Paused)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently paused.");
        await player.ResumeAsync();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Resumed the current song.");
    }
    
    public async Task<Embed> VolumeAsync(IGuild guild, string volume) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        await player.UpdateVolumeAsync(ushort.Parse(volume));
        return await EmbedHandler.CreateBasicEmbed("ThornBot", $"Set the volume to {volume}%.");
    }
    
    public async Task<Embed> NowPlayingAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        var track = player.Track;
        return await EmbedHandler.CreateBasicEmbed("ThornBot", $"Now Playing: {track.Title}\nUrl: {track.Url}");
    }
    
    public async Task<Embed> QueueAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        var queue = player.Queue.ToList();
        var builder = new StringBuilder();
        foreach (var track in queue) {
            builder.AppendLine($"{track.Title}\nUrl: {track.Url}\n");
        }
        return await EmbedHandler.CreateBasicEmbed("ThornBot", builder.ToString());
    }
    
    public async Task<Embed> LeaveAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        await player.StopAsync();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Left the voice channel.");
    }

    public async Task<Embed> ShuffleAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        player.Queue.Shuffle();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Shuffled the queue.");
    }
    
    public async Task<Embed> ClearAsync(IGuild guild) {
        var player = _lavaNode.GetPlayer(guild);
        if (player.PlayerState is not PlayerState.Playing)
            return await EmbedHandler.CreateErrorEmbed("I'm not currently playing anything.");
        player.Queue.Clear();
        return await EmbedHandler.CreateBasicEmbed("ThornBot", "Cleared the queue.");
    }

}