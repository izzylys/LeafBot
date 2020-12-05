﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using LeafBot.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LeafBot
{
  class Program
  {
    public DiscordClient Client { get; set; }
    private static string configPath = "config.json";
    static void Main(string[ ] args)
    {
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

      // connect, baby!
      Debug.WriteLine("Connecting to discord...");
      await Client.ConnectAsync();

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
  }
}