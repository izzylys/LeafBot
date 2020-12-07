using System;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using LeafBot.Data;

namespace LeafBot.Commands
{
  class Counters : BaseCommandModule
  {
    [Command("shower")]
    [Description("Track how many times Eddie showers... wait why do we do this again?")]
    [Aliases("eddieshowering", "eddieshower")]
    public async Task EddieShowering(CommandContext ctx)
    {
      // Increment stat
      Stats.EddieShowerCount++;

      // I mean this save is a bit redundant because the save function is on a timer,
      // but hopes for the moment while leafbot sometimes gets closed before the backup occurs
      Stats.Save(ctx.Client);

      await ctx.Channel.SendMessageAsync($"Eddie has stopped playing to go shower {Stats.EddieShowerCount} times {DiscordEmoji.FromName(ctx.Client, ":shower:")}" +
        $"{Environment.NewLine}{Formatter.Italic("(he must be hella stanky)")}");
    }
  }
}
