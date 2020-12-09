using System;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using LeafBot.Data;

namespace LeafBot.Commands
{
  class Help : BaseCommandModule
  {
    [Command("about")]
    [Description("Who is this LeafBot I have heard so much about?")]
    [Aliases("whoami", "newnumberwhodis", "whodis")]
    public async Task About(CommandContext ctx)
    {
      DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":leaves:");
      var embed = new DiscordEmbedBuilder
      {
        Title = $"Heyya! I'm LeafBot {emoji}",
        Color = DiscordColor.SpringGreen,
        Description = $"Guardian of this discord server {DiscordEmoji.FromName(ctx.Client, ":shield:")}, protector of bunnies {DiscordEmoji.FromName(ctx.Client, ":rabbit:")} and back street dealer of cute animal pictures {DiscordEmoji.FromName(ctx.Client, ":otter:")}.",
        Footer = new DiscordEmbedBuilder.EmbedFooter
        {
          Text = "Brought to you with <3 by Kubi, Genghis & pals"
        },
      };

      embed.AddField($"Code {DiscordEmoji.FromName(ctx.Client, ":wrench:")}", "My code (whatever that is) can be found on " + Formatter.MaskedUrl("github", new Uri("https://github.com/izzylys/LeafBot"), "click me, you know you want to") +
        $". You can also submit any feature ideas, suggestions, feelings or bugs on the {Formatter.MaskedUrl("issues page", new Uri("https://github.com/izzylys/LeafBot/issues"))}.");
      embed.AddField($"Help {DiscordEmoji.FromName(ctx.Client, ":books:")}", $"If you want to see a list of my commands, type {Formatter.InlineCode("!commands")}. For any information about a command type {Formatter.InlineCode("!help <command>")}");
      embed.AddField($"Have fun!", $"and don't forget to throw all your games (looking at you Eddie {DiscordEmoji.FromName(ctx.Client, ":see_no_evil:")})");

      await ctx.Channel.SendMessageAsync(embed: embed);
    }

    [Command("commands")]
    [Description("Lists all the amazing commands that LeafBot can do")]
    [Aliases("list", "command")]
    public async Task ListCommands(CommandContext ctx)
    {
      var embed = new DiscordEmbedBuilder
      {
        Title = $"LeafBot commands ",
        Color = DiscordColor.SpringGreen,
        Footer = new DiscordEmbedBuilder.EmbedFooter
        {
          Text = $"Type '!help <command>' to get more info about a command"
        }
      };

      DiscordEmoji rabbitEmoji = DiscordEmoji.FromName(ctx.Client, ":rabbit:");
      DiscordEmoji counterEmoji = DiscordEmoji.FromName(ctx.Client, ":slot_machine:");
      DiscordEmoji gamesEmoji = DiscordEmoji.FromName(ctx.Client, ":joystick:");
      DiscordEmoji searchEmoji = DiscordEmoji.FromName(ctx.Client, ":mag:");
      DiscordEmoji utilityEmoji = DiscordEmoji.FromName(ctx.Client, ":gear:");
      DiscordEmoji helpEmoji = DiscordEmoji.FromName(ctx.Client, ":books:");

      // So there is a way to list all commands attached to the client's CommandNext 
      // but that list contains ALL aliases of commands so I am just going to add them manually
      // here unless we come up with a better way to do this. Also the !help command with 
      // no arguments basically does this but I wanted something with catagories

      embed.AddField($"Bunnies {rabbitEmoji}",
        $"-> {Formatter.InlineCode("sleepy")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("curious")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("angry")} {Environment.NewLine}", true);
      embed.AddField($"Counters {counterEmoji}",
        $"-> {Formatter.InlineCode("eddieshower")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("buns")} {Environment.NewLine}", true);
      embed.AddField($"Games {gamesEmoji}",
        $"-> {Formatter.InlineCode("rollthedice")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("coinflip")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("pickrole")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("bunnyage")} {Environment.NewLine}", true);
      embed.AddField($"Searches {searchEmoji}",
        $"-> {Formatter.InlineCode("wikipedia")} {Environment.NewLine}" +
        $"-> {Formatter.InlineCode("urbandictionary")} {Environment.NewLine}", true);
      embed.AddField($"Utilities {utilityEmoji}",
         $"-> {Formatter.InlineCode("ping")} {Environment.NewLine}" +
         $"-> {Formatter.InlineCode("uptime")} {Environment.NewLine}" +
         $"-> {Formatter.InlineCode("lasterror")} {Environment.NewLine}", true);
      embed.AddField($"Help {helpEmoji}",
         $"-> {Formatter.InlineCode("help")} {Environment.NewLine}" +
         $"-> {Formatter.InlineCode("about")} {Environment.NewLine}" +
         $"-> {Formatter.InlineCode("commands")} {Environment.NewLine}", true);

      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
