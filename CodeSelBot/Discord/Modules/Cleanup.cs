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
	public class Cleanup : ModuleBase<SocketCommandContext>
	{
		[Command("clean"), Alias("clear", "delete"), Summary("Clears the bot's previous messages")]
		public async Task ClearAsync()
		{
			var msgs = await Context.Channel.GetMessagesAsync().Flatten();

			var msgsToDelete = msgs.Where(x => x.Timestamp > DateTimeOffset.Now.AddDays(-17) &&
				(x.Author.Id == Global.Client.CurrentUser.Id 
				|| x.Content.StartsWith("-") 
				|| x.MentionedUserIds.Contains(Global.Client.CurrentUser.Id)));

			await Context.Channel.DeleteMessagesAsync(msgsToDelete);

			await ReplyAsync(":put_litter_in_its_place: " + msgsToDelete.Count().ToEmoji()).SelfDestruct();
		}

		[Command("clean"), Alias("clear", "delete"), Summary("Clears an amount of previously sent messages")]
		public async Task ClearAsync(int messageCount)
		{
			var msgs = await Context.Channel.GetMessagesAsync(messageCount).Flatten();

			var msgsToDelete = msgs.Where(x => x.Timestamp > DateTimeOffset.Now.AddDays(-17));

			await Context.Channel.DeleteMessagesAsync(msgsToDelete);

			await ReplyAsync(":put_litter_in_its_place: " + msgsToDelete.Count().ToEmoji()).SelfDestruct();
		}

		[Command("clean"), Alias("clear", "delete"), Summary("Clears previous messages sent by a specific user")]
		public async Task ClearAsync(SocketUser user, int maxMessageCount = 10)
		{
			var msgs = await Context.Channel.GetMessagesAsync().Flatten();

			var msgsToDelete = msgs.Where(x => x.Timestamp > DateTimeOffset.Now.AddDays(-17) && x.Author == user).Take(maxMessageCount);

			await Context.Channel.DeleteMessagesAsync(msgsToDelete);

			try { await Context.Message.DeleteAsync(); }
			catch { }

			await ReplyAsync(":put_litter_in_its_place: " + msgsToDelete.Count().ToEmoji()).SelfDestruct();
		}

		[Command("clean"), Alias("clear", "delete"), Summary("Clears previous messages sent since a specific timespan"), Remarks("Add mentions or message count at the end\nto enhance the query")]
		public async Task ClearAsync([Remainder]string context)
		{
			var msgs = await Context.Channel.GetMessagesAsync().Flatten();
			var count = Regex.Match(context, "\\d+$").If(x => x.Success, x => x.Value.SmartParse(), 100);
			var users = Context.Message.MentionedUsers;

			context = context.RegexRemove("\\d+$").RegexRemove("<[#@][^>]+>");

			var hours = Regex.Match(context, "(\\d+) ?hours?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
			var mins = Regex.Match(context, "(\\d+) ?mins?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);
			var secs = Regex.Match(context, "(\\d+) ?sec(?:ond)?s?").If(x => x.Success, x => x.Groups[1].Value.SmartParse(), 0);

			var time = DateTimeOffset.Now.AddTicks(new TimeSpan(-hours, -mins, -secs).Ticks);

			if (context.Contains("today"))
				time = new DateTimeOffset(DateTime.Today);

			var msgsToDelete = msgs.Where(x => x.Timestamp > time && (users.Count == 0 || users.Any(u => u.Id == x.Author.Id))).Take(count);

			await Context.Channel.DeleteMessagesAsync(msgsToDelete);
			
			try { await Context.Message.DeleteAsync(); }
			catch { }

			await ReplyAsync(":put_litter_in_its_place: " + msgsToDelete.Count().ToEmoji()).SelfDestruct();
		}
	}
}
