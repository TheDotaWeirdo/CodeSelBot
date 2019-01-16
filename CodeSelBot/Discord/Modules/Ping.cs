using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Extensions;

namespace CodeSelBot.Discord.Modules
{
	public class Ping : ModuleBase<SocketCommandContext>
	{
		[Command("ping"), Summary("Check the Bot's connection and latency")]
		public async Task PingAsync()
		{
			var embed = new EmbedBuilder();

			embed
				.WithColor(255, 255, 255)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.AddInlineField("Ping", Responces.Random())
				.AddInlineField("Latency", $"{Context.Client.Latency} ms"); var embed2 = new EmbedBuilder();

			embed2
				.WithColor(255, 255, 255)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithDescription(embed.Build().ToString())
				.AddInlineField("Ping", Responces.Random())
				.AddInlineField("Latency", $"{Context.Client.Latency} ms");

			await ReplyAsync(string.Empty, false, embed2.Build());

			await Context.Message.DeleteAsync();
		}

		private static string[] Responces = 
		{
			"I'm Alive!",
			"I'm Here!!",
			"Status: ONLINE",
			"Bip Boop?",
			"Lifeline Check"
		};
	}
}
