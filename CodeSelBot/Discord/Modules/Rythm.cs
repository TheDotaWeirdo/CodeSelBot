using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;
using Extensions;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord.Modules
{
	public class Rythm : ModuleBase<SocketCommandContext>
	{
		[Command("rythm"), Summary("Echoes back a querried list of songs played on rythm")]
		public async Task RythmHistory([Remainder]string conext)
		{
			conext = conext?.ToLower() ?? string.Empty;
			var top = Regex.Match(conext, "^top (\\d+)").If(x => x.Success, x => x.Groups[1].Value.SmartParse(50), 50);
			var date = (DateTime?)null;

			conext = conext.RegexRemove("^top (\\d+)");

			if (Regex.IsMatch(conext, "past:.{2,20}$"))
			{
				var match = Regex.Match(conext, "past:(.{2,20})$").Groups[1].Value;

				var hours = Regex.Match(match, "(\\d+) ?hours?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
				var mins = Regex.Match(match, "(\\d+) ?mins?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
				var secs = Regex.Match(match, "(\\d+) ?sec(?:ond)?s?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);

				date = DateTime.Now.AddTicks(-new TimeSpan(hours, mins, secs).Ticks);
				conext = conext.RegexRemove("past:.{2,20}$");
			}
			else if (conext.EndsWith("today"))
			{
				date = DateTime.Today;
				conext = conext.RegexRemove("today$");
			}
			else if (conext.EndsWith("yesterday"))
			{
				date = DateTime.Today.AddDays(-1);
				conext = conext.RegexRemove("yesterday$");
			}

			var results = SQLHandler.Rythm_GetHistory(date, top);

			if (!string.IsNullOrWhiteSpace(conext))
				results = results.Where(x => x.SearchCheck(conext));

			if (results.Any())
				await ReplyAsync(results.ListStrings(x => $"• {x}\n").CharLimit());
			else
				await ReplyAsync("No results found :confused:");
		}

		[Command("rythm"), Summary("Echoes back a querried list of songs played on rythm")]
		public async Task RythmHistory() => await RythmHistory(string.Empty);
	}
}
