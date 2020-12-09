using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using LeafBot.Commands;
using LeafBot.Data;
using LeafBot.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LeafBot
{
  class Program
  {
    public const string VERSION = "0.5";

    public static string BasePath { get; private set; }
    public static string DataPath { get; private set; }

    public static EventId BotEventId { get; private set; } = new EventId(420, "LeafBot");

    public DiscordClient Client { get; set; }
    public CommandsNextExtension Commands { get; set; }
    private Timer SaveTimer, PresenceTimer;
    private static string configPath;

    static void Main(string[ ] args)
    {
      // Setup all our bot's paths
      BasePath = AppDomain.CurrentDomain.BaseDirectory;
      DataPath = Path.Combine(BasePath, "Data");
      configPath = Path.Combine(BasePath, "config.json");

      // initialise bot stats
      Stats.Initialise();

      // pass execution through async method
      var program = new Program();
      program.RunBotAsync().GetAwaiter().GetResult();
    }

    public async Task RunBotAsync()
    {
      // load config file and set up client
      var cfgJson = await ReadConfigFile(configPath);
      var cfgDiscord = ConfigureDiscord(cfgJson);
      Client = new DiscordClient(cfgDiscord);
      ConfigureCommands(cfgJson);
      ConfigureEvents();

      // connect, baby!
      Debug.WriteLine("Connecting to discord...");
      await Client.ConnectAsync();

      // start the save timer to backup state every hour
      SaveTimer = new Timer(60 * 60 * 1000) { AutoReset = true, Enabled = true };
      SaveTimer.Elapsed += (object sender, ElapsedEventArgs e) => StaticEvents.SaveTimer_Elapsed(sender, e, Client);

      // Start a timer to update the bot's status every hour
      PresenceTimer = new Timer(60 * 60 * 1000) { AutoReset = true, Enabled = true };
      PresenceTimer.Elapsed += (object sender, ElapsedEventArgs e) => StaticEvents.PresenceTimer_Elapsed(Client);

      // prevent premature quitting
      await Task.Delay(-1);
    }

    private async Task<ConfigJson> ReadConfigFile(string path)
    {
      Debug.WriteLine("Reading config file...");
      var data = "";
      try
      {
        using(FileStream fs = File.OpenRead(path))
        using(StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
        {
          data = await sr.ReadToEndAsync();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"Error reading config file: {e.GetType()}: {e.Message}");
        Console.Read();
        Environment.Exit(1);
      }
      ConfigJson cfgjson = JsonConvert.DeserializeObject<ConfigJson>(data);

      return cfgjson;
    }

    private DiscordConfiguration ConfigureDiscord(ConfigJson config)
    {
      Debug.WriteLine("Setting up discord configuration...");
      // Setup loglevel based on config file string
      LogLevel ll = new LogLevel();
      switch (config.LogLevel)
      {
        case "debug":
          ll = LogLevel.Debug;
          break;
        case "info":
          ll = LogLevel.Information;
          break;
        case "critical":
          ll = LogLevel.Critical;
          break;
        case "error":
          ll = LogLevel.Error;
          break;
        case "warning":
          ll = LogLevel.Warning;
          break;
        default:
          ll = LogLevel.Information;
          break;
      }

      var cfg = new DiscordConfiguration
      {
        Token = config.Token,
        TokenType = TokenType.Bot,

        AutoReconnect = true,
        MinimumLogLevel = ll
      };

      return cfg;
    }

    private void ConfigureCommands(ConfigJson config)
    {
      Client.Logger.LogInformation(Program.BotEventId, $"Setting up commands...");
      var ccfg = new CommandsNextConfiguration()
      {
        StringPrefixes = config.CommandPrefix,
        CaseSensitive = false,
        EnableDms = true,
        EnableMentionPrefix = true
      };
      Commands = Client.UseCommandsNext(ccfg);

      // Add event callbacks
      Commands.CommandExecuted += StaticEvents.Commands_CommandExecuted;
      Commands.CommandErrored += StaticEvents.Commands_CommandErrored;

      // Register commands
      Commands.RegisterCommands<Bunnies>();
      Commands.RegisterCommands<Commands.Utilities>();
      Commands.RegisterCommands<Games>();
      Commands.RegisterCommands<Searches>();
      Commands.RegisterCommands<Counters>();
      Commands.RegisterCommands<Help>();

      // Set custom helpformatter
      Commands.SetHelpFormatter<HelpFormatter>();
    }

    private void ConfigureEvents()
    {
      Client.Logger.LogInformation(Program.BotEventId, "Setting up events...");
      Client.Ready += StaticEvents.Client_Ready;
      Client.ClientErrored += StaticEvents.Client_ClientError;
    }
  }
}
