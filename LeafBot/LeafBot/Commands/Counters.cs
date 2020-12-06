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
    public async Task EddieShowering(CommandContext ctx)
    {
      // Increment stat
      Stats.EddieShowerCount++;

      await ctx.Channel.SendMessageAsync($"Eddie has stopped playing to go shower {Stats.EddieShowerCount} times {DiscordEmoji.FromName(ctx.Client, ":shower:")}" +
        $"{Environment.NewLine}{Formatter.Italic("(he must be hella stanky)")}");
    }
  }
}
