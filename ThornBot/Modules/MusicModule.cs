using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Services;
using Victoria;

namespace ThornBot.Modules;

public class MusicModule : InteractionModuleBase<SocketInteractionContext> {
    
    private readonly DiscordSocketClient _client;
    private readonly LavaNode _lavaNode;
    private readonly MusicService _musicService;

    public MusicModule(IServiceProvider services) {
        _client = services.GetRequiredService<DiscordSocketClient>();
        _lavaNode = services.GetRequiredService<LavaNode>();
        _musicService = services.GetRequiredService<MusicService>();
    }

    [SlashCommand("join", "Join the voice channel.")]
    public async Task JoinAsync() => 
        await RespondAsync(embed: await _musicService.JoinAsync(Context.Guild, (Context.User as IVoiceState)!, (Context.Channel as ITextChannel)!));

    [SlashCommand("play", "Play a song or radio stream.")]
    public async Task PlayAsync([Remainder] string search) =>
        await RespondAsync(embed: await _musicService.PlayAsync((Context.User as SocketGuildUser)!, Context.Guild, search));
    
    [SlashCommand("pause", "Pause the current song.")]
    public async Task PauseAsync() =>
        await RespondAsync(embed: await _musicService.PauseAsync(Context.Guild));
    
    [SlashCommand("resume", "Resume the current song.")]
    public async Task ResumeAsync() =>
        await RespondAsync(embed: await _musicService.ResumeAsync(Context.Guild));
    
    [SlashCommand("stop", "Stop the current song.")]
    public async Task StopAsync() =>
        await RespondAsync(embed: await _musicService.StopAsync(Context.Guild));
    
    [SlashCommand("skip", "Skip the current song.")]
    public async Task SkipAsync() =>
        await RespondAsync(embed: await _musicService.SkipAsync(Context.Guild));
    
    [SlashCommand("queue", "Show the current queue.")]
    public async Task QueueAsync() =>
        await RespondAsync(embed: await _musicService.QueueAsync(Context.Guild));
    
    [SlashCommand("volume", "Set the volume of the current song.")]
    public async Task VolumeAsync([Remainder] string volume) =>
        await RespondAsync(embed: await _musicService.VolumeAsync(Context.Guild, volume));
    
    [SlashCommand("leave", "Leave the voice channel.")]
    public async Task LeaveAsync() =>
        await RespondAsync(embed: await _musicService.LeaveAsync(Context.Guild));
    
    [SlashCommand("np", "Show the current song.")]
    public async Task NowPlayingAsync() =>
        await RespondAsync(embed: await _musicService.NowPlayingAsync(Context.Guild));
    
    [SlashCommand("shuffle", "Shuffle the current queue.")]
    public async Task ShuffleAsync() =>
        await RespondAsync(embed: await _musicService.ShuffleAsync(Context.Guild));
    
    [SlashCommand("clear", "Clear the current queue.")]
    public async Task ClearAsync() =>
        await RespondAsync(embed: await _musicService.ClearAsync(Context.Guild));
}