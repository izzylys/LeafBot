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

    public static string BasePath { get; private set; }
    public static string StorePath { get; private set; }

    public static async void Initialise()
    {
      // save session stats
      StartTime = DateTime.Now;
      PCName = Environment.MachineName;

      // check if store exists and if not, create it
      BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
      StorePath = Path.Combine(BasePath, @"Data\stats_store.json");
      if (!File.Exists(StorePath))
      {
        File.Create(StorePath);
        return;
      }

      // get bunny count from store
      using(StreamReader file = File.OpenText(StorePath))
      {
        var json = await file.ReadToEndAsync();
        dynamic s = JObject.Parse(json);

        BunniesServed = s.bunnies_served;
      }
    }
  }
}
