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
	public class CreateInvite : ModuleBase<SocketCommandContext>
	{
		[Command("createinvite"), Alias("invite"), Summary("Creates a new invite to the Server")]
		public async Task CreateInviteAsync(int expireTimeInMinutes = 10, int maxUses = 2)
		{
			var typing = Context.Channel.EnterTypingState();
			var inv = await((IGuildChannel)Context.Channel).CreateInviteAsync(expireTimeInMinutes.Between(1, 60) * 60, maxUses.Between(1, 5), true);
			var embed = new EmbedBuilder();

			embed
				.WithColor(Color.Blue)
				.WithTitle("Server Invite")
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithDescription(Responces.Random().Replace("#", Context.Guild.Name).Replace("@", Context.User.Mention))
				.AddField("URL", inv.Url)
				.AddInlineField("Expires", $"{DateTime.Now.AddMilliseconds((int)inv.MaxAge).ToLongTimeString()} ({DateTimeOffset.Now.Offset.Hours.If(x => x >= 0, x => "+" + x, x => x.ToString())}:00)")
				.AddInlineField("Channel", Context.Channel.Mention())
				.AddInlineField("Max Uses", inv.MaxUses);

			await ReplyAsync(string.Empty, false, embed.Build());

			typing.Dispose();
		}

		private static string[] Responces =
		{
			"Here's an invite to #",
			"Look what I made for #!",
			"# now has one more invite link!",
			"Your invite link, as requested, @",
			"One invite, coming up!",
			"Here's the key for more discord friends!",
			"More friends for Code.Sel Bot!!",
			"More people to conspire against! :slight_smile:"
		};
	}
}
