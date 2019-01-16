using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSelBot.Discord.Classes;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord
{
	public static class Global
	{
		private static DiscordSocketClient _client;
		private static CommandService _commands;
		private static IServiceProvider _services;

		public static CodeSelBot Bot { get; set; }

		public static DiscordSocketClient Client { get => _client; set => _client = value; }

		public static CommandService Commands { get => _commands; set => _commands = value; }

		public static IServiceProvider Services { get => _services; set => _services = value; }

		public static Data Data;

		public static ServerDictionary ServerDictionary = JsonConvert.DeserializeObject<ServerDictionary>(System.IO.File.ReadAllText("DiscordDictionary.json"));

		public static Func<string[]> ConsoleLines;

		public static bool AutoMove = true;
	}
}
