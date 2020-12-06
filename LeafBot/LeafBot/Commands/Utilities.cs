using System;
using System.Threading.Tasks;
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
      embed.AddField("Uptime", (DateTime.Now - Stats.StartTime).ToString() + " hours");
      embed.AddField("Connected to", ctx.Guild.Name.ToString(), true);
      embed.AddField("Running on", Stats.PCName, true);

      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
