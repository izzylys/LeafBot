using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using Newtonsoft.Json;

using LeafBot.Data.Models;


namespace LeafBot.Commands
{
  public class Searches : BaseCommandModule
  {
    [Command("wiki")]
    [Description("Instructs leafbot to search for a term on wikipedia (you smart person you)")]
    [Aliases("search", "wikipedia", "w")]
    // Based off: https://gitlab.com/Kwoth/nadekobot/-/blob/1.9/NadekoBot.Core/Modules/Searches/Searches.cs#L650
    public async Task Wiki(CommandContext ctx, [RemainingText] string query = null)
    {
      // trim and sanity check query string
      query = query?.Trim();
      if (string.IsNullOrWhiteSpace(query))
        return;

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
          await ctx.Channel.SendMessageAsync($"Could not find anything on Wikipedia for {query} {emoji}");
        }
        else
        {
          // Display first result
          DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, ":books:");
          await ctx.Channel.SendMessageAsync($"{emoji} {data.Query.Pages.First().FullUrl}");
        }
      }
    }
  }
}
