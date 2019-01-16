using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord
{
	public delegate void ConsoleChangedHandler(CodeSelBot bot, ActionType action, object args);

	public class CodeSelBot
	{
		private const string BOT_TOKEN = "MzE3MjYxMzgyOTAwOTA4MDMz.Dge31g.pf82L15h1Ng7tI4iYcDHhryARFs";

		public event ConsoleChangedHandler ConsoleChanged;

		public CodeSelBot RunBot()
		{
			new Action(async () =>
			{
				Global.Client = new DiscordSocketClient();
				Global.Commands = new CommandService();

				Global.Services = new ServiceCollection()
					.AddSingleton(Global.Client)
					.AddSingleton(Global.Commands)
					.BuildServiceProvider();
				
				Global.Client.Log += (log) => { Log(ActionType.Add, log); return Task.CompletedTask; };
				Global.Client.ReactionAdded += (s, e, t) =>
				{
					return Task.CompletedTask;
				};

				await RegisterCommandsAsync();

				await Global.Client.LoginAsync(TokenType.Bot, BOT_TOKEN);

				Global.Data = SQLHandler.GetBotData();
				Global.Data.UserData = new List<UserData>(SQLHandler.GetUserData());
				Global.Data.NotifiedItems = new List<string>(SQLHandler.GetBotNotifications());

            var games = SQLHandler.GetUserGames();
				var acolytes = SQLHandler.GetUsersAcolytes();
				var wfSubs = SQLHandler.GetUserWarframeSubs();

				foreach (var dat in Global.Data.UserData)
				{
					dat.Tracking.Games = new List<Game>(games.Where(x => x.Item1 == dat.ID).Convert(x => x.Item2));
					dat.Preferences.Acolytes = new List<string>(acolytes.Where(x => x.Item1 == dat.ID).Convert(x => x.Item2));
					dat.WarframeData.WarframeSubs = wfSubs.Where(x => x.UserId == dat.ID).ToList();
				}

				await Global.Client.StartAsync();

				Periodic.Start();
			}).RunInBackground();

			return this;
		}

		private async Task RegisterCommandsAsync()
		{
			Global.Client.MessageReceived += MessageReceived;

			await Global.Commands.AddModulesAsync(Assembly.GetEntryAssembly());
		}

		public void Reconnect()
		{
			new Action(async () =>
			{
				if (Global.Client.ConnectionState.AnyOf(global::Discord.ConnectionState.Connected, global::Discord.ConnectionState.Connecting))
					await Global.Client.LogoutAsync();

				await Global.Client.LoginAsync(TokenType.Bot, BOT_TOKEN);
				await Global.Client.StartAsync();
				await Global.Client.SetStatusAsync(UserStatus.Online);

				Periodic.Resume();
			}).RunInBackground();
		}

		public void Disconnect()
		{
			new Action(async () =>
			{
				await Global.Client.SetStatusAsync(UserStatus.Invisible);
				System.Threading.Thread.Sleep(1000);
				await Global.Client.LogoutAsync();
				await Global.Client.StopAsync();

				Periodic.Stop();
			}).RunInBackground();
		}

		private async Task MessageReceived(SocketMessage arg)
		{
			var message = arg as SocketUserMessage;

			if (arg.Channel.Id == Global.ServerDictionary.Channels["RythmMix"].ID)
				arg.SelfDestruct(arg.Author.Username == "Rythm" ? 120000 : 30000);
			else if (arg.Channel.Id == Global.ServerDictionary.Channels["WarframeText"].ID)
				if (!(arg.Author.Id == Global.ServerDictionary.BotID || arg.Content.StartsWith("-warframe")))
					arg.SelfDestruct(5000);

			if (message is null || message.Author.IsBot) return;

			var argPos = 0;

			if (message.HasStringPrefix("-", ref argPos) || message.HasMentionPrefix(Global.Client.CurrentUser, ref argPos))
			{
				var context = new SocketCommandContext(Global.Client, message);

				var result = await Global.Commands.ExecuteAsync(context, argPos, Global.Services, MultiMatchHandling.Best);

				if (!result.IsSuccess)
				{
					Log(ActionType.Add, LogSeverity.Error, "Commands", result.ErrorReason);
					await GetFailedCommandResponce(message);
				}
				else
					Tracking.OnUserCommand(message);
			}

			Tracking.OnUserMessage(message);
		}

		private async Task GetFailedCommandResponce(SocketUserMessage message)
		{
			var commandName = System.Text.RegularExpressions.Regex.Match(message.Content, "-(.+?) ").Groups[1].Value;
			var commands = Global.Commands.Commands;
			var command = commands.Convert(x => new Tuple<CommandInfo, int>(x, Modules.Help.GetHelpScore(x, commandName))).Where(x => x.Item2 < 4).OrderBy(x => x.Item2).FirstOrDefault();

			if (command == null)
				await message.Channel.SendMessageAsync($"There doesn't seem to be a command named **{commandName}** :cry: \n\nList all commands by not adding any parameters to your `-help` command.");
			else
			{
				foreach (var cmd in commands.Where(x => x.Module.Name == command.Item1.Module.Name))
				{
					var embed = new EmbedBuilder();

					embed
						.WithColor(Color.Red)
						.WithFooter($"Requested by {message.Author.Username}")
						.WithCurrentTimestamp()
						.WithDescription("**You appear to have mistakenly used a command**\n\n" + (cmd.Summary ?? "No Description"))
						.WithTitle($"{cmd.Module.Name.FormatWords(true)} Command")
						.AddField(":capital_abcd: Aliases", cmd.Aliases.ListStrings(", "))
						.AddInlineField(":symbols: Parameters", cmd.Parameters.Count)
						.AddInlineField(":exclamation: Remarks", cmd.Remarks ?? "None");

					foreach (var par in cmd.Parameters)
						embed.AddField($"{(cmd.Parameters.ToList().IndexOf(par) + 1).ToEmoji()} **{par.Name.FormatWords(true)}** Parameter",
							par.Summary ?? $"\n" +
							$"`Value Type{" ".Copy(13 - 10)}:` {(par.IsRemainder ? "Text" : Modules.Help.TypeName(par.Type))}\n" +
							$"`Is Optional{" ".Copy(13 - 11)}:` {par.IsOptional.YesNo()}\n" +
							(par.IsOptional ? $"`Default{" ".Copy(13 - 7)}:` {par.DefaultValue}\n" : "") +
							$"`Is Multiple{" ".Copy(13 - 11)}:` {par.IsMultiple.YesNo()}");

					await message.Channel.SendMessageAsync(string.Empty, false, embed.Build());
				}
			}
		}

		internal void Log(ActionType action, LogSeverity severity, string source, string text)
			=> ConsoleChanged?.Invoke(this, action, new LogMessage(severity, source, text));

		internal void Log(ActionType action, LogMessage log)
			=> ConsoleChanged?.Invoke(this, action, log);
	}
}
