using Discord;
using Discord.Commands;
using ThornBot.Services;

namespace ThornBot.Modules; 

public class MusicModule : ModuleBase<SocketCommandContext> {
    
    private MusicService MusicService { get; set; }

    [Command("join")]
    public async Task Join()
        => await ReplyAsync(embed: await MusicService.JoinAsync(Context.Guild, Context.User as IVoiceState,
            Context.Channel as ITextChannel));

    
}