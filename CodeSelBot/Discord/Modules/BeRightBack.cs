using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Extensions;

namespace CodeSelBot.Discord.Modules
{
	public class BeRightBack : ModuleBase<SocketCommandContext>
	{
		[Command("brb"), Alias("afk"), Summary("Temporarily mutes you for a small duration"), Remarks("Duration is marked as `x hours y mins z secs` with each being optional")]
		public async Task BeRightBackAsync([Remainder]string duration = "3 mins")
		{
			var hours = Regex.Match(duration, "(\\d+) ?hours?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
			var mins = Regex.Match(duration, "(\\d+) ?mins?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
			var secs = Regex.Match(duration, "(\\d+) ?sec(?:ond)?s?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);

			if (0.AllOf(hours, mins, secs))
				await ReplyAsync(InvalidResponces.Random());
			else
			{
				var time = new TimeSpan(hours, mins, secs);

				await ReplyAsync(Responces.Random().Replace("#", time.ToReadableString() + "\n*Use `-back` if you're back early*")).SelfDestruct();
				await ((IGuildUser)Context.User).ModifyAsync(x => { x.Deaf = true; x.Mute = true; });
				await Context.Message.DeleteAsync();

				var timer = new Timer(time.TotalMilliseconds)
				{ AutoReset = false, Enabled = true };

				timer.Elapsed += (s, e) =>
				{
					((IGuildUser)Context.User).ModifyAsync(x => { x.Deaf = false; x.Mute = false; });
				};
			}
		}

		[Command("back"), Summary("Lets the bot know that you're back early")]
		public async Task BackAsync()
		{
			await ((IGuildUser)Context.User).ModifyAsync(x => { x.Deaf = false; x.Mute = false; });
			await Context.Message.DeleteAsync();
		}

		private string[] Responces = new string[]
		{
			"Got it! I'll un-mute you in #",
			"# and counting!",
			"Too loud ey? Should be better in #..",
			"Some silence at last.. Or at least for #"
		};

		private string[] InvalidResponces = new string[]
		{
			"Well I can't work with that time..",
			"Maybe some context on how long you wanna go?",
			"Throw a `-hel brb` my way so I show you how it's done.",
			":rolling_eyes: it's `-brb X hours X mins X secs` times are all optional"
		};
	}
}
