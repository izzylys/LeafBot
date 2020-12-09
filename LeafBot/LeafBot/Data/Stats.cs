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
    public static int ExecutedCommands;
    public static int RolesPicked;

    public static string FilePath { get; private set; }

    public static async void Initialise()
    {
      // save session stats
      StartTime = DateTime.Now;
      PCName = Environment.MachineName;

      FilePath = Path.Combine(Program.DataPath, "stats_store.json");

      if (!File.Exists(FilePath))
      {
        if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        }

        CreateStatsFile(FilePath);
        return;
      }

      using(StreamReader file = File.OpenText(FilePath))
      {
        var json = await file.ReadToEndAsync();
        dynamic s = JObject.Parse(json);

        BunniesServed = s.bunnies_served;
        EddieShowerCount = s.eddie_shower_count;
        ExecutedCommands = s.executed_commands;
        RolesPicked = s.roles_picked;
      }
    }

    public static async void CreateStatsFile(string path)
    {
      // NOTE: This should be wrapped in a try but because we don't have accessible logging at the moment
      // we don't have a way of reporting any errors and addressing them. So let it error for now, revisit this...

      // Create file and save some blank stats to it
      using (StreamWriter sw = File.CreateText(path))
      {
        await sw.WriteAsync(
          $"{{" +
          $"\"bunnies_served\": 0," +
          $"\"eddie_shower_count\": 0," +
          $"\"roles_picked\": 0," +
          $"\"executed_commands\": 0" +
          $"}}");
      }
    }

    public static async Task Save(DiscordClient client = null)
    {
      var backupPath = Path.Combine(Program.DataPath, "stats_store_backup.json");

      // backup the store first to avoid corruption if something goes bang
      try
      {
        using (FileStream store = File.Open(FilePath, FileMode.Open))
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
        using (StreamWriter sw = new StreamWriter(FilePath))
        {
          await sw.WriteAsync(
            $"{{" +
            $"\"bunnies_served\": {Stats.BunniesServed}," +
            $"\"eddie_shower_count\": {Stats.EddieShowerCount}," +
            $"\"roles_picked\": {Stats.RolesPicked}," +
            $"\"executed_commands\": {Stats.ExecutedCommands}" +
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
