using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LeafBot.Commands
{
  public class Games : BaseCommandModule
  {
    private Random random = new Random();

    [Command("roll")]
    [Description("Instructs LeafBot to roll a lettice shaped dice")]
    [Aliases("rtd", "dice", "rollthedice")]
    public async Task RollTheDice(CommandContext ctx)
    {
      DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":game_die:");

      await ctx.RespondAsync($"{ctx.Member.Mention} rolls {random.Next(0, 101)} {emoji}");
    }

    [Command("flip")]
    [Description("Instructs LeafBot to flip a cabbage leaf")]
    [Aliases("coin", "coinflip")]
    public async Task FlipCoin(CommandContext ctx)
    {
      DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":coin:");

      string outcome = "";
      if (random.Next(0, 1) == 1)
      {
        outcome = "Heads";
      } 
      else
      {
        outcome = "Tails";
      }

      await ctx.RespondAsync($"{ctx.Member.Mention} flips a cabbage leaf... {outcome}! ");
    }
  }
}
