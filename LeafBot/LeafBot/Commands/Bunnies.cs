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
        Color = DiscordColor.PhthaloBlue,
        ImageUrl = Links.SLEEPY_BUNS[rng.Next(Links.SLEEPY_BUNS.Length)]
      };
      await ctx.Channel.SendMessageAsync(embed: embed);
    }

    [Command("curious")]
    [Description("blesses you with a photo of a curious bun")]
    public async Task Curious(CommandContext ctx)
    {
      var embed = new DiscordEmbedBuilder
      {
        Color = DiscordColor.HotPink,
        ImageUrl = Links.CURIOUS_BUNS[rng.Next(Links.CURIOUS_BUNS.Length)]
      };
      await ctx.Channel.SendMessageAsync(embed: embed);
    }

    [Command("angry")]
    [Description("curses you with a photo of an angry bun")]
    public async Task Angry(CommandContext ctx)
    {
      var embed = new DiscordEmbedBuilder
      {
        Color = DiscordColor.Red,
        ImageUrl = Links.ANGRY_BUNS[rng.Next(Links.ANGRY_BUNS.Length)]
      };
      await ctx.Channel.SendMessageAsync(embed: embed);
    }
  }
}
