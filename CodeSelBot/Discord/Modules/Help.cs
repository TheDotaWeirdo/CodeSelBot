using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Extensions;

namespace CodeSelBot.Discord.Modules
{
	public class Help : ModuleBase<SocketCommandContext>
	{
		[Command("help"), Summary("Gets the list of possible commands supported by the bot")]
		public async Task HelpAsync()
		{
			var commands = Global.Commands.Commands;
			var sb = new StringBuilder();
			var embed = new EmbedBuilder();

			BuildCommandList(commands, ref sb, out var count);

			embed
				.WithColor(Color.Gold)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithDescription(sb.ToString().CharLimit())
				.WithTitle("Command List")
				.AddInlineField("Command Count", count);

			await ReplyAsync(string.Empty, false, embed.Build());
		}

		[Command("help"), Summary("Gets detailed info about a specific command")]
		public async Task HelpAsync([Remainder]string commandName)
		{
			var commandIndex = Regex.Match(commandName, "\\d+$").Value.SmartParse();
			var commands = Global.Commands.Commands;

			if (commandIndex > 0)
				commandName = commandName.RegexRemove("\\d+$").TrimEnd();

			var command = commands.Convert(x => new Tuple<CommandInfo, int>(x, GetHelpScore(x, commandName))).Where(x => x.Item2 < 4).OrderBy(x => x.Item2).FirstOrDefault();

			if (command == null)
				await ReplyAsync($"There doesn't seem to be a command named **{commandName}** :cry: \n\nList all commands by not adding any parameters to your `-help` command.");
			else
			{
				if (commandIndex > 0)
					commands = new CommandInfo[] { commands.Where(x => x.Module.Name == command.Item1.Module.Name).ElementAt(commandIndex - 1) };

				foreach (var cmd in commands.Where(x => x.Module.Name == command.Item1.Module.Name))
				{
					var embed = new EmbedBuilder();

					embed
						.WithColor(Color.Gold)
						.WithFooter($"Requested by {Context.User.Username}")
						.WithCurrentTimestamp()
						.WithDescription(cmd.Summary ?? "No Description")
						.WithTitle($"{cmd.Module.Name.FormatWords(true)} Command")
						.AddField(":capital_abcd: Aliases", cmd.Aliases.ListStrings(", "))
						.AddInlineField(":symbols: Parameters", cmd.Parameters.Count)
						.AddInlineField(":exclamation: Remarks", cmd.Remarks ?? "None");

					foreach (var par in cmd.Parameters)
						embed.AddField($"{(cmd.Parameters.ToList().IndexOf(par) + 1).ToEmoji()} **{par.Name.FormatWords(true)}** Parameter",
							par.Summary ?? $"\n" +
							$"`Value Type{" ".Copy(13 - 10)}:` {(par.IsRemainder ? "Text" : TypeName(par.Type))}\n" +
							$"`Is Optional{" ".Copy(13 - 11)}:` {par.IsOptional.YesNo()}\n" +
							(par.IsOptional ? $"`Default{" ".Copy(13 - 7)}:` {par.DefaultValue}\n" : "") +
							$"`Is Multiple{" ".Copy(13 - 11)}:` {par.IsMultiple.YesNo()}");

					await ReplyAsync(string.Empty, false, embed.Build());
				}
			}
		}

		public static int GetHelpScore(CommandInfo cmd, string commandName)
		{
			var min = cmd.Module.Name.SpellCheck(commandName, false);

			if (cmd.Module.Name.Contains(commandName))
				return 0;

			min = Math.Min(min, cmd.Aliases.Min(x => x.Contains(commandName) ? 0 : x.SpellCheck(commandName, false)));

			return min;
		}

		private void BuildCommandList(IEnumerable<CommandInfo> commands, ref StringBuilder sb, out int count)
		{
			var maxL = commands.Max(x => x.Name.Length);
			var commandsListed = new List<string>();

			foreach (var cmd in commands.OrderBy(x => x.Name))
			{
				if (!commandsListed.Contains(cmd.Name))
				{
					commandsListed.Add(cmd.Name);
					sb.AppendFormat("• `{0}", cmd.Name);
					sb.Append("\0".Copy(maxL - cmd.Name.Length));
					sb.Append("` ");
					sb.AppendLine(cmd.Summary ?? "No Description");
				}
			}

			count = commandsListed.Count;
		}

		public static object TypeName(Type type)
		{
			if (type == typeof(string))
				return "Word";

			if (type == typeof(int))
				return "Integer";

			if (type.AnyOf(typeof(double), typeof(float), typeof(decimal)))
				return "Decimal";

			if (type == typeof(bool))
				return "Boolean (Yes / No)";

			if (type == typeof(SocketUser))
				return "User";

			if (type == typeof(SocketChannel))
				return "Channel";

			return type.Name;
		}
	}
}
