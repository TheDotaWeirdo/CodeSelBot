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
	public class MoveTo : ModuleBase<SocketCommandContext>
	{
		[RequireUserPermission(GuildPermission.MoveMembers), Command("move"), Summary("Moves users from one channel to the other")]
		public async Task MoveToAsync([Remainder]string context)
		{
			ErrorPart errPart = ErrorPart.Network;
			Exception ex;
			try
			{
				IGuildChannel fromChannel, toChannel;
				if (context.Contains(" to "))
				{
					var from = Regex.Match(context, "(.+) to").Groups[1].Value.Trim();

					if (from == string.Empty)
					{ ex = new Exception("**From** Channel is empty"); errPart = ErrorPart.User; goto err; }

					fromChannel = await Context.Guild.FindChannel(from, ChannelType.Voice);

					if (fromChannel == null)
					{ ex = new Exception("**From** Channel could not be found"); errPart = ErrorPart.User; goto err; }
				}
				else
				{
					fromChannel = await Context.Guild.GetUsersChannel(Context.User, ChannelType.Voice);

					if (fromChannel == null)
					{ ex = new Exception("**From** Channel could not be found"); errPart = ErrorPart.Bot; goto err; }
				}

				var to = Regex.Match(context, "to (.+)").Groups[1].Value.Trim();
				
				if (to == string.Empty)
				{ ex = new Exception("**To** Channel is empty"); errPart = ErrorPart.User; goto err; }
					
				toChannel = await Context.Guild.FindChannel(to, ChannelType.Voice);
				
				if (toChannel == null)
				{ ex = new Exception("**To** Channel could not be found"); errPart = ErrorPart.User; goto err; }

				var users = await fromChannel.GetUsersAsync().Flatten();

				users.Foreach(user =>
				{
					user.ModifyAsync(x => x.ChannelId = toChannel.Id);
					user.GetData().Tracking.TimesMoved++;
					Global.Bot.Log(ActionType.Add, LogSeverity.Info, "Movement", $"Moved {user.Username}#{user.Discriminator} from {fromChannel.Name} to {toChannel.Name}");
				});

				await Context.Message.DeleteAsync();
				await ReplyAsync(string.Empty, false, MoveEmbed(fromChannel, toChannel, users.ListStrings(x => x.Mention + ", ", false)));

				Global.Data.SaveUsers(users);
			}
			catch (Exception exc) { ex = exc; errPart = ErrorPart.Bot; goto err; }

			return;
			err: await ReplyAsync(string.Empty, false, DiscordExtensions.GetErrorEmbed("Error", ex.Message, Context, commandName: "Move To", errorPart: errPart)).SelfDestruct(120000);
		}

		private Embed MoveEmbed(IGuildChannel fromChannel, IGuildChannel toChannel, string userNames)
		{
			var embed = new EmbedBuilder();

			embed
				.WithColor(Color.Teal)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.AddInlineField("From", fromChannel.Name)
				.AddInlineField("\u200b", ":arrow_right:")
				.AddInlineField("To", toChannel.Name)
				.WithDescription($"Moved {userNames}")
				.WithTitle("Move Actions");

			return embed.Build();
		}
	}
}
