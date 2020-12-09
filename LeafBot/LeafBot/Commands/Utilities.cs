using System;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using LeafBot.Data;

namespace LeafBot.Commands
{
  public class Utilities : BaseCommandModule
  {
    [Command("status")]
    [Description("How you doing leafbot")]
    public async Task Status(CommandContext ctx)
    {
      DiscordEmoji rabbitEmoji = DiscordEmoji.FromName(ctx.Client, ":rabbit2:");
      DiscordEmoji toolsEmoji = DiscordEmoji.FromName(ctx.Client, ":tools:");

      var embedThumbnail = new DiscordEmbedBuilder.EmbedThumbnail
      {
        Url = Links.STATUS_THUMBNAIL_BUN
      };
      var embedFooter = new DiscordEmbedBuilder.EmbedFooter
      {
        Text = "Brought to you with <3 by Kubi, Genghis & pals"
      };

      var embed = new DiscordEmbedBuilder
      {
        Title = $"{rabbitEmoji} LeafBot Status {toolsEmoji}",
        ImageUrl = Links.STATUS_BUN,
        Thumbnail = embedThumbnail,
        Footer = embedFooter,
        Color = DiscordColor.SpringGreen,
      };
      embed.AddField("Version", Program.VERSION, true);
      embed.AddField("DSharp Version", ctx.Client.VersionString, true);
      embed.AddField("Ping", ctx.Client.Ping.ToString(), true);
      embed.AddField("Bunnies served", Stats.BunniesServed.ToString(), true);
      embed.AddField("Commands run", Stats.ExecutedCommands.ToString(), true);
      embed.AddField("Roles picked", Stats.RolesPicked.ToString(), true);
      embed.AddField("Uptime", (DateTime.Now - Stats.StartTime).ToString() + " hours");
      embed.AddField("Connected to", ctx.Guild.Name.ToString(), true);
      embed.AddField("Running on", Stats.PCName, true);

      await ctx.Channel.SendMessageAsync(embed: embed);
    }

    [Command("commands")]
    public async Task ListCommands(CommandContext ctx)
    {
      var embed = new DiscordEmbedBuilder
      {
        Title = $"LeafBot commands ",
        Color = DiscordColor.SpringGreen,
      };

    }

    [Command("about")]
    public async Task About(CommandContext ctx)
    {

    }

    [Command("error")]
    [Description("Prints last command error details (oh nuuuu)")]
    [Aliases("printerror", "lasterror", "exception")]
    public async Task LastCommandError(CommandContext ctx)
    {
      if (Stats.LastCommandException == null)
      {
        DiscordEmoji tadaEmoji = DiscordEmoji.FromName(ctx.Client, ":tada:");
        await ctx.RespondAsync($"No errors! Yaaaay {tadaEmoji}! ");
        return;
      }

      DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":skull_crossbones:");

      string type = Stats.LastCommandException.GetType().ToString();
      string msg = Stats.LastCommandException.Message;
      string stacktrace = Stats.LastCommandException.StackTrace;

      var embed = new DiscordEmbedBuilder
      {
        Title = $"{emoji} Last Error:",
        Color = DiscordColor.DarkRed,
      };
      embed.AddField("Type", Formatter.Bold(type));
      embed.AddField("Message", Formatter.Italic(msg));
      embed.AddField("Trace", Formatter.BlockCode(string.IsNullOrWhiteSpace(stacktrace) ? "No stacktrace attached" : stacktrace));

      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
