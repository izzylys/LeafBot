using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LeafBot.Data;

namespace LeafBot.Commands
{
  public class Bunnies : BaseCommandModule
  {
    private Random rng = new Random();

    [Command("sleepy")]
    [Description("blesses you with a photo of a sleepy bun")]
    public async Task Sleepy(CommandContext ctx)
    {
      var embed = new DiscordEmbedBuilder
      {
        ImageUrl = Links.SLEEPY_BUNS[rng.Next(Links.SLEEPY_BUNS.Length)]
      };
      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
