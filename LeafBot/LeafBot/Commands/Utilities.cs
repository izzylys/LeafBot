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
      DiscordEmoji rabbit2Emoji = DiscordEmoji.FromName(ctx.Client, ":rabbit:");
      DiscordEmoji toolsEmoji = DiscordEmoji.FromName(ctx.Client, ":tools:");
      DiscordEmoji tadaEmoji = DiscordEmoji.FromName(ctx.Client, ":tada:");
      DiscordEmoji sadEmoji = DiscordEmoji.FromName(ctx.Client, ":cry:");

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

      var uptime = (DateTime.Now - Stats.StartTime);
      decimal upDays = uptime.Days;
      decimal bunYears = Math.Round((upDays / 365) * 7.5M, 6);

      embed.AddField("Version", Program.VERSION, true);
      embed.AddField("DSharp Version", ctx.Client.VersionString, true);
      embed.AddField("Ping", ctx.Client.Ping.ToString(), true);
      embed.AddField("Bunnies served", Stats.BunniesServed.ToString(), true);
      embed.AddField("Commands run", Stats.ExecutedCommands.ToString(), true);
      embed.AddField("Roles picked", Stats.RolesPicked.ToString(), true);
      embed.AddField("Command errors", Stats.CommandErrors == 0 ? $"{tadaEmoji} {Stats.CommandErrors.ToString()} {tadaEmoji}" : Stats.CommandErrors.ToString() + $" {sadEmoji}", true);
      embed.AddField("Guilds", ctx.Client.Guilds.Count.ToString(), true);
      embed.AddField("Uptime", $"{uptime.Days} days {uptime.Hours} hours {uptime.Minutes} minutes {uptime.Seconds} seconds");
      embed.AddField("Age", $"{bunYears} bunny years {rabbit2Emoji}");
      embed.AddField("Connected to", ctx.Guild.Name.ToString(), true);
      embed.AddField("Running on", Stats.PCName, true);

      // Update bunnies served stat (there is a pic of a bunny in the status in bed silly)
      Stats.BunniesServed++;

      await ctx.Channel.SendMessageAsync(embed: embed);
    }

    [Command("ping")]
    [Description("Leafbot's ping (don't ping it too hard)")]
    public async Task Ping(CommandContext ctx)
    {
      await ctx.Channel.SendMessageAsync($"{DiscordEmoji.FromName(ctx.Client, ":ping_pong:")} Pong! Ping: {ctx.Client.Ping}ms");
    }

    [Command("uptime")]
    [Description("See how long leafbot has been awake for")]
    public async Task Uptime(CommandContext ctx)
    {
      DiscordEmoji rabbitEmoji = DiscordEmoji.FromName(ctx.Client, ":rabbit:");

      var uptime = (DateTime.Now - Stats.StartTime);
      decimal upDays = uptime.Days;
      decimal bunYears = Math.Round((upDays / 365) * 7.5M, 6);
      await ctx.Channel.SendMessageAsync($"LeafBot has been awake for {uptime.Days} days {uptime.Hours} hours {uptime.Minutes} minutes {uptime.Seconds} seconds (or {bunYears} bunny years {rabbitEmoji})");
    }

    [Command("error")]
    [Description("Prints last command error details (oh nuuuu)")]
    [Aliases("printerror", "lasterror", "exception")]
    public async Task LastCommandError(CommandContext ctx)
    {
      if (Stats.LastCommandException == null)
      {
        DiscordEmoji tadaEmoji = DiscordEmoji.FromName(ctx.Client, ":tada:");
        await ctx.RespondAsync($"No errors! Yaaaay {tadaEmoji} ");
        return;
      }

      DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":skull_crossbones:");

      string type = Stats.LastCommandException.GetType().ToString();
      string msg = Stats.LastCommandException.Message;
      string stacktrace = Stats.LastCommandException.StackTrace;

      var embed = new DiscordEmbedBuilder
      {
        Title = $"{emoji} Last Command Error {emoji}",
        Color = DiscordColor.DarkRed,
      };
      embed.AddField("Type", Formatter.Bold(type));
      embed.AddField("Message", Formatter.Italic(msg));
      embed.AddField("Trace", Formatter.BlockCode(string.IsNullOrWhiteSpace(stacktrace) ? "No stacktrace attached" : stacktrace));

      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
