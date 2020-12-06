using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace LeafBot.Data
{
  public static class Stats
  {
    public static DateTime StartTime;
    public static string PCName;
    public static int BunniesServed;

    public static async void Initialise()
    {
      // save session stats
      StartTime = DateTime.Now;
      PCName = Environment.MachineName;

      // check if store exists and if not, create it
      var path = @"Data\stats_store.json";
      if (!File.Exists(path))
      {
        File.Create(path);
        return;
      }

      // get bunny count from store
      using(StreamReader file = File.OpenText(path))
      {
        var json = await file.ReadToEndAsync();
        dynamic s = JObject.Parse(json);

        BunniesServed = s.bunnies_served;
      }
    }
  }
}
