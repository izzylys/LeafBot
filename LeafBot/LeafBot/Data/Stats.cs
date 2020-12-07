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

    public static string FilePath { get; private set; }

    public async static void Initialise()
    {
      StartTime = DateTime.Now;
      PCName = Environment.MachineName;

      FilePath = Path.Combine(Program.DataPath, "stats_store.json");

      if (!File.Exists(FilePath))
      {
        if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        }

        File.Create(FilePath);
        return;
      }

      using(StreamReader file = File.OpenText(FilePath))
      {
        var json = await file.ReadToEndAsync();
        dynamic s = JObject.Parse(json);

        BunniesServed = s.bunnies_served;
      }
    }
  }
}
