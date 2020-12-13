using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;

using Newtonsoft.Json;

using LeafBot.Data;
using LeafBot.Data.Models;


namespace LeafBot.Commands
{
  public class Searches : BaseCommandModule
  {
    [Command("wiki")]
    [Description("Instructs leafbot to search for a term on wikipedia (you smart person you)")]
    [Aliases("search", "wikipedia", "w")]
    // Based off: https://gitlab.com/Kwoth/nadekobot/-/blob/1.9/NadekoBot.Core/Modules/Searches/Searches.cs#L650
    public async Task Wiki(CommandContext ctx, [RemainingText, Description("Term/thingie to search for")] string query = null)
    {
      // trim and sanity check query string
      query = query?.Trim();
      if (string.IsNullOrWhiteSpace(query))
      {
        DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":thinking:");
        await ctx.Channel.SendMessageAsync($"Leafbot is pretty amazing but I can't search for nothing my dude {emoji}");
        return;
      }

      using (var webClient = new HttpClient())
      {
        // Query wikiedia
        string response = await webClient.GetStringAsync("https://en.wikipedia.org//w/api.php?action=query&format=json&prop=info&redirects=1&formatversion=2&inprop=url&titles=" + Uri.EscapeDataString(query));
        
        // Convert the json string response to an object 
        WikipediaApiResponse data = JsonConvert.DeserializeObject<WikipediaApiResponse>(response);

        // Display if there are any results
        if (data.Query.Pages.First().Missing || string.IsNullOrWhiteSpace(data.Query.Pages.First().FullUrl))
        {
          DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":cry:");
          await ctx.Channel.SendMessageAsync($"I could not find anything on Wikipedia for {query} {emoji}");
        }
        else
        {
          // Display first result
          DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":books:");
          await ctx.Channel.SendMessageAsync($"{emoji} {data.Query.Pages.First().FullUrl}");
        }
      }
    }

    [Command("urbandictionary")]
    [Description("Instructs leafbot to search for a term on urban dictionary (wow really high class humour you got there bud)")]
    [Aliases("ud", "lookup", "define")]
    public async Task UrbanDictionary(CommandContext ctx, [RemainingText, Description("Thingie to look up in dictionary")] string query = null)
    {
      query = query?.Trim();
      if (string.IsNullOrWhiteSpace(query))
      {
        DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":thinking:");
        await ctx.Channel.SendMessageAsync($"Leafbot is pretty amazing but I can't search for nothing my dude {emoji}");
        return;
      }

      using (var webClient = new HttpClient())
      {
        // Query UrbanDictionary api
        string response = await webClient.GetStringAsync("http://api.urbandictionary.com/v0/define?term=" + Uri.EscapeDataString(query));

        // Convert response to object
        var data = JsonConvert.DeserializeObject<UrbanDictionaryApiResponse>(response).List;

        if (data.Any())
        {
          // Build an embed to display the results
          var embed = new DiscordEmbedBuilder
          {
            Title = $"'{data.First().Word}'",

            // I think it looks better without the thumbnail for now
            //Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            //{
            //  Url = Links.URBAN_DICTIONARY_ICON
            //};
        };

          // Look I know that inlining is gross, don't judge
          embed.AddField("Definition", data.First().Definition.Replace("[", "").Replace("]", ""));
          embed.AddField("Example", Formatter.BlockCode(data.First().Example.Replace("[", "").Replace("]", "")));

          await ctx.Channel.SendMessageAsync(embed: embed);
        }
        else
        {
          DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":cry:");
          await ctx.Channel.SendMessageAsync($"I could not find anything on Urban Dictionary for {query} {emoji}");
        }
      }
    }
  }
}
