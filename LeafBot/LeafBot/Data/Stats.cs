using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LeafBot.Data
{
  public static class Stats
  {
    public static DateTime StartTime;
    public static string PCName;
    public static int BunniesServed;

    public async static void Initialise()
    {
      StartTime = DateTime.Now;
      PCName = Environment.MachineName;

      var path = @"Data\stats_store.json";
      if (!File.Exists(path))
      {
        File.Create(path);
        return;
      }

      using(StreamReader file = File.OpenText(path))
      {
        var json = await file.ReadToEndAsync();
        dynamic s = JObject.Parse(json);

        BunniesServed = s.bunnies_served;
      }
    }
  }
}
