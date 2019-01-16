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
	public class Echo : ModuleBase<SocketCommandContext>
	{
		[Command("echo"), Alias("say"), Summary("Echoes back whatever text you input after the command")]
		public async Task EchoAsync([Remainder]string echo)
		{
			await ReplyAsync(echo);
		}
	}
}
