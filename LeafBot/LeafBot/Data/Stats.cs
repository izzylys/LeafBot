using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace LeafBot.Data
{
  public static class Stats
  {
    public static DateTime StartTime;
    public static string PCName;
    public static int BunniesServed;
    public static int EddieShowerCount;

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
        EddieShowerCount = s.eddie_shower_count;
      }
    }

    public static async void Save(DiscordClient client = null)
    {
      var storePath = @"Data\stats_store.json";
      var backupPath = @"Data\stats_store_backup.json";

      // backup the store first to avoid corruption if something goes bang
      try
      {
        using (FileStream store = File.Open(storePath, FileMode.Open))
        using (FileStream backup = File.Open(backupPath, FileMode.Create))
        {
          await store.CopyToAsync(backup);
        }
      }
      catch (Exception ex)
      {
        client?.Logger.LogError(Program.BotEventId, $"Could not backup stats store: {ex.GetType()}: {ex.Message}", DateTime.Now);
        return;
      }

      // write the state to the store
      try
      {
        using (StreamWriter sw = new StreamWriter(storePath))
        {
          await sw.WriteAsync(
            $"{{" +
            $"\"bunnies_served\": {Stats.BunniesServed}," +
            $"\"eddie_shower_count\": {Stats.EddieShowerCount}" +
            $"}}");
        }

      }
      catch (Exception ex)
      {
        client?.Logger.LogError(Program.BotEventId, $"Could not save stats to local store: {ex.GetType()}: {ex.Message}", DateTime.Now);
        return;
      }

      // delete the backup 
      try
      {
        await Task.Run(() => File.Delete(backupPath));
      }
      catch (Exception ex)
      {
        client?.Logger.LogError(Program.BotEventId, $"Could not delete stats backup: {ex.GetType()}: {ex.Message}", DateTime.Now);
        return;
      }

      client?.Logger.LogInformation(Program.BotEventId, "Successfully saved stats to the local store", DateTime.Now);
      return;
    }
  }
}
