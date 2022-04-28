using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using ThornBot.Handler;

namespace ThornBot.Modules;

public class AdminModule : InteractionModuleBase<SocketInteractionContext> {

    private readonly DiscordSocketClient _client;
    private readonly DiscordRpcClient _rpc;
    private readonly InteractionService _interactionService;

    public AdminModule(IServiceProvider services) {
        _client = services.GetRequiredService<DiscordSocketClient>();
        _rpc = services.GetRequiredService<DiscordRpcClient>();
        _interactionService = services.GetRequiredService<InteractionService>();
    }

    [SlashCommand("deploy", "Deploy all slash commands to a specific guild")]
    public async Task DeployAsync(string type) {
        switch (type) {
            case "global":
                await _interactionService.RegisterCommandsGloballyAsync();
                await RespondAsync(embed: await EmbedHandler.CreateBasicEmbed("ThornBot", "Succesfully registered commands globally."));
                break;
            case "guild":
                await _interactionService.RegisterCommandsToGuildAsync(Context.Guild.Id);
                await RespondAsync(embed: await EmbedHandler.CreateBasicEmbed("ThornBot", "Succesfully registered commands to the guild."));
                break;
        }
    }

    [SlashCommand("rpc", "Sets Thorn's rich presence as work, sleeping, driving")]
    [Command("rpc", false)]
    public async Task RpcAsync(string rpc) {
        switch (rpc.ToLower()) {
            case "coding":
                _rpc.SetPresence(new RichPresence() {
                    Details = "Coding",
                    State = "with a coffee in hand",
                    Buttons = new [] {
                        new Button() {
                            Label = "Github",
                            Url = "https://github.com/GuildedThorn"
                        },
                        new Button() {
                            Label = "Get Some help",
                            Url = "https://chicagosleepcenter.com/"
                        }
                    }
                });
                break;
            case "work":
                _rpc.SetPresence(new RichPresence() {
                    Details = "Working",
                    State = "in the murder state",
                    Buttons = new [] {
                        new Button {
                            Label = "Leave a message",
                            Url = "https://guildedthorn.com/contact"
                        },
                        new Button {
                            Label = "Github",
                            Url = "https://github.com/GuildedThorn"
                        }
                    },
                    Timestamps = new Timestamps() {
                        Start = DateTime.Now
                    }
                });
                break;
            case "sleeping":
                _rpc.SetPresence(new  RichPresence() {
                    Details = "Sleeping",
                    State = "with one eye open",
                    Buttons = new [] {
                        new Button {
                            Label = "Leave a message",
                            Url = "https://guildedthorn.com/contact"
                        },
                        new Button {
                            Label = "Linkedin",
                            Url = "https://www.linkedin.com/in/jamieduddleston/"
                        }
                    },
                    Timestamps = new Timestamps() {
                        Start = DateTime.Now
                    }
                });
                break;
            case "driving":
                _rpc.SetPresence(new RichPresence() {
                    Details = "Driving",
                    State = "in a state where noone can",
                    Buttons = new [] {
                        new Button {
                            Label = "Illinois Driving School",
                            Url = "https://www.ilsos.gov/departments/drivers/driver_education/home.html"
                        },
                        new Button {
                            Label = "Get some help",
                            Url = "https://chicagocompasscounseling.com/alcohol-moderation-counseling/"
                        }
                    }
                });
                break;
        }
        await RespondAsync(embed: await EmbedHandler.CreateBasicEmbed("ThornBot", $"Succesfully set RPC as {rpc}"));
    }

    [SlashCommand("status", "Sets The bot's status as online, invisible, idle, or dnd")]
    [Command("status", false)]
    public async Task StatusAsync(string status) {
        switch (status.ToLower()) {
            case "online":
                await _client.SetStatusAsync(UserStatus.Online);
                break;
            case "invisible":
                await _client.SetStatusAsync(UserStatus.Invisible);
                break;
            case "idle":
                await _client.SetStatusAsync(UserStatus.Idle);
                break;
            case "dnd":
                await _client.SetStatusAsync(UserStatus.DoNotDisturb);
                break;
        }
        await RespondAsync(embed: await EmbedHandler.CreateBasicEmbed("ThornBot", "Status set to " + status.ToLower()));
    }
}
