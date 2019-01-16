using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Extensions;
using System.Text.RegularExpressions;

namespace CodeSelBot.Discord.Modules
{
	public class ChooseBetween : ModuleBase<SocketCommandContext>
	{
		[Command("choose between"), Alias("choose", "random"), Summary("Echoes back a random item from given options"), Remarks("Options are separated by spaces,\nor set within quotations (\" \")")]
		public async Task ChooseBetweenAsync([Remainder]string options)
		{
			options = options.RegexRemove("^ ?between ");

			var optionList = new List<string>(Regex.Matches(options, "\"(.+?)\"").Convert(x => x.Groups[1].Value));
			options = options.RegexRemove("\"(.+?)\"");

			optionList.AddRange(Regex.Matches(options, @"<[#@][^>]+>").Convert(x => x.Value));
			options = options.RegexRemove(@"<[#@][^>]+>");

			optionList.AddRange(Regex.Matches(options, @"\b([^ ]+?)\b").Convert(x => x.Groups[1].Value));

			optionList.RemoveAll(string.IsNullOrWhiteSpace);

			if (optionList.Count == 0)
				await ReplyAsync("I could not find any options to choose from :cry:");
			else
				await ReplyAsync(Responces.Random().Replace("#", optionList.Random()));
		}

		private static string[] Responces =
		{
			"How about **#**?",
			"**#** sounds good",
			":thinking: **#** :thinking:",
			"**#**, Final Offer.",
			"That **#** lookin good over there"
		};
	}
}
