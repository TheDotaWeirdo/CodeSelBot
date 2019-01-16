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
	public class Log : ModuleBase<SocketCommandContext>
	{
		[Command("log"), Alias("get log", "show log"), Summary("Echoes back the current Log window's content")]
		public async Task LogAsync(int lines = 10)
		{
			await Context.Channel.TriggerTypingAsync();
			await ReplyAsync("```" + Global.ConsoleLines().TakeLast(lines.Between(1, 50)).ListStrings("\n").CharLimit(1994) + "```");
		}
	}
}
