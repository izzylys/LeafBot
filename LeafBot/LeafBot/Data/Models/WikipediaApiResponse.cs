using System;

namespace LeafBot.Data.Models
{
  /// <summary>
  /// Json model structure for a Wikipedia api query response
  /// </summary>
  public class WikipediaApiResponse
  {
    public WikipediaQuery Query { get; set; }
  }

  public class WikipediaQuery
  {
    public WikipediaNormalisedText[] Normalized { get; set; }
    public WikipediaPage[] Pages { get; set; }
  }

  public class WikipediaPage
  {
    public string Title { get; set; }
    public bool Missing { get; set; } = false;
    public string PageLanguage { get; set; }
    public int Length { get; set; }
    public string FullUrl { get; set; }
  }

  public class WikipediaNormalisedText
  {
    public bool FromEncoded { get; set; } = false;
    public string From { get; set; }
    public string To { get; set; }
  }
}
