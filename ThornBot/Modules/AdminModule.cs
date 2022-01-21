using System.Diagnostics;
using Discord;
using Discord.Commands;
using ThornBot.Commands;

namespace ThornBot.Modules; 

[Group("!admin")]
public class AdminModule : ModuleBase<SocketCommandContext> {
    
    [Command("deploy")]
    [Summary("Deploys the bot's slash commands to the guild")]
    public Task DeployAsync() {
        var guild = Context.Guild;
        StatusCommand.RegisterSlashCommand(guild);
        
        return Task.CompletedTask;
    }


    [Command("status")]
    [Summary("Sets the bot's status as online, offline, idle, or dnd")]
    public async Task StatusAsync(string arg) {
        Debug.Assert(ThornBot.Client != null, "ThornBot.Client != null");
        switch (arg.ToLower()) {
            case "online":
                await ThornBot.Client.SetStatusAsync(UserStatus.Online);
                break;
            case "offline":
                await ThornBot.Client.SetStatusAsync(UserStatus.Offline);
                break;
            case "idle":
                await ThornBot.Client.SetStatusAsync(UserStatus.Idle);
                break;
            case "dnd":
                
                await ThornBot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
                break;
        }
        await ReplyAsync("Status set to " + arg);
    }
}