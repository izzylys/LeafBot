﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using LeafBot.Data;

namespace LeafBot.Events
{
  public static class StaticEvents
  {
    public static Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
      sender.Logger.LogInformation(Program.BotEventId, "Client is ready to process events.");

      return Task.CompletedTask;
    }

    public static Task Client_ClientError(DiscordClient sender, ClientErrorEventArgs e)
    {
      sender.Logger.LogError(Program.BotEventId, e.Exception, $"Exception occurred: {e.Exception.Message}");

      return Task.CompletedTask;
    }

    public static Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
    {
      e.Context.Client.Logger.LogInformation(
        Program.BotEventId,
        "{0} successfully executed '{1}' {2}",
        e.Context.User.Username,
        e.Command.QualifiedName,
        (e.Context.RawArguments.Count > 0 ? "with arguments '" + e.Context.RawArgumentString + "'" : string.Empty)
      );

      return Task.CompletedTask;
    }

    public static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
      e.Context.Client.Logger.LogError(
        Program.BotEventId, 
        "{0} tried executing '{1}' but it errored: {2}: {3}",
        e.Context.User.Username,
        e.Command?.QualifiedName ?? "<unknown command>",
        e.Exception.GetType(),
        e.Exception.Message ?? "<no message>"
      );

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

    public static async void SaveTimer_Elapsed(object sender, ElapsedEventArgs e, DiscordClient client)
    {
      client.Logger.LogInformation("Save timer elapsed. Attempting to save stats to local store", DateTime.Now);

      // Save our stats object to file
      Stats.Save(client);
    }

  }
}
