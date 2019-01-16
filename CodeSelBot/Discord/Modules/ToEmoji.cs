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
	public class ToEmoji : ModuleBase<SocketCommandContext>
	{
		[Command("toemoji"), Alias("emoji", "!e"), Summary("Converts a given text to Emoji characters")]
		public async Task ToEmojiAsync([Remainder]string textToConvert)
		{
			await ReplyAsync(textToConvert.ToEmoji());
		}
	}
}
