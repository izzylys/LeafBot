using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace LeafBot.Events
{
  public static class StaticEvents
  {
    public static EventId BotEventId = new EventId(420, "LeafBot");

    public static Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
      sender.Logger.LogInformation(BotEventId, "Client is ready to process events.");

      return Task.CompletedTask;
    }

    public static Task Client_ClientError(DiscordClient sender, ClientErrorEventArgs e)
    {
      sender.Logger.LogError(BotEventId, e.Exception, $"Exception occurred: {e.Exception.Message}");

      return Task.CompletedTask;
    }

    public static Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
    {
      e.Context.Client.Logger.LogInformation(BotEventId, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");

      return Task.CompletedTask;
    }

    public static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
      e.Context.Client.Logger.LogError(BotEventId, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

      if (e.Exception is CommandNotFoundException ex)
      {

        var emoji = DiscordEmoji.FromName(e.Context.Client, ":pleading_face:");

        var embed = new DiscordEmbedBuilder
        {
          Title = "i'm sorry, i don't know that command",
          Description = $"{emoji} please try a different command",
          ImageUrl = @"https://user-images.githubusercontent.com/7717434/101267251-8eeff780-374e-11eb-9885-a8236b882bff.jpg",
          Color = DiscordColor.DarkRed,
        };
        await e.Context.RespondAsync("", embed : embed);
      }
    }

  }
}
