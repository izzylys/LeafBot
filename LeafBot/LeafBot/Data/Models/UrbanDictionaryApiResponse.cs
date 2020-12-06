using System;
using System.Collections.Generic;
using System.Text;

namespace LeafBot.Data.Models
{
  /// <summary>
  /// Json model for Urban Dictionary api responses
  /// </summary>
  public class UrbanDictionaryApiResponse
  {
    public UrbanDictionaryItem[] List { get; set; }
  }
  public class UrbanDictionaryItem
  {
    public string Word;
    public string Definition;
    public string PermaLink;
    public string Example;
  }
}
