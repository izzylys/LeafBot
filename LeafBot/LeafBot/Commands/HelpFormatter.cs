using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;

namespace LeafBot.Commands
{
  // Custom formatter for any help commands
  class HelpFormatter : BaseHelpFormatter
  {
    private DiscordEmbedBuilder EmbedBuilder { get; }
    private DiscordEmoji HelpEmoji { get; }

    public HelpFormatter(CommandContext ctx) : base(ctx)
    {
      this.EmbedBuilder = new DiscordEmbedBuilder();
      this.HelpEmoji = DiscordEmoji.FromName(ctx.Client, ":books:");

      EmbedBuilder.Color = DiscordColor.SpringGreen;
    }
    public override BaseHelpFormatter WithCommand(Command command)
    {
      EmbedBuilder.Title = $"{HelpEmoji} '{command.Name}'";

      EmbedBuilder.AddField("Description", Formatter.Italic(command.Description));

      // Print any aliases if the command has any
      if (command.Aliases.Count != 0)
        EmbedBuilder.AddField("Aliases", string.Join(", ", command.Aliases.Select(x => Formatter.InlineCode(x))));

      // Print arguments list
      foreach (var overload in command.Overloads)
      {
        if (overload.Arguments.Count == 0)
        {
          continue;
        }

        EmbedBuilder.AddField("Arguments", string.Join(", ", overload.Arguments.Select(xarg => $"({xarg.Type.Name}) '{Formatter.Bold(Formatter.Underline(xarg.Name))}' - {xarg.Description}")));
      }

      return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
    {
      // NOTE: This assumes that the only command with subcommands is the main command !help, so if we ever add commands with subcommands this will have to change
      EmbedBuilder.AddField("Help", $"These are all the commands that LeafBot can do! For help and/or more info about LeafBot try {Formatter.InlineCode("!about")}.");
      EmbedBuilder.AddField("Commands", string.Join(", ", subcommands.Select(xc => Formatter.InlineCode(xc.Name))));

      return this;
    }

    public override CommandHelpMessage Build()
    {
      return new CommandHelpMessage(string.Empty, EmbedBuilder.Build());
    }

  }
}
