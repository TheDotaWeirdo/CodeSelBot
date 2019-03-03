using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Extensions;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord
{
	public enum ErrorPart { User, Bot, Network }

	public static class DiscordExtensions
	{
		public static string CharLimit(this string str, int charLimit = 2000)
		{
			if (str.Length > charLimit)
				return str.Substring(0, charLimit - 3) + "...";
			return str;
		}

		public static string CenterText(this string title, int width = 100, bool padright = false)
		{
			var sb = new StringBuilder();
			foreach (var item in title.Split('\n','\r'))
			{
				for (int i = 0; i < (width - item.RegexReplace("<.+?>", "......").RegexReplace(":.+?:", "....").Length) / 2; i++)
				{
					sb.Append("\u200b\u2000");
				}
				sb.Append(item);
				if(padright)
				{
					for (int i = 0; i < ((width - item.RegexReplace("<.+?>", "......").RegexReplace(":.+?:", "....").Length) / 2); i++)
					{
						sb.Append("\u200b\u2000");
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}

		public static string JustifyContent<T>(this string text, IEnumerable<T> list)
		{
			var sb = new StringBuilder();
			var width = 46 / list.Count().Between(1, 2);
			var i = 0;

			foreach (var c in text)
			{
				sb.Append(c);
				i++;

				if (i == width)
					sb.AppendLine();
			}

			return sb.ToString();
		}

		public static UserData GetData(this IUser user)
		{
			if (Global.Data.UserData.Any(x => x.ID == user.Id))
				return Global.Data.UserData.FirstThat(x => x.ID == user.Id);

			var newData = new UserData(user.Id);
			Global.Data.UserData.Add(newData);
			SQLHandler.AddUser(user);

			return newData;
		}

		public static ulong GetGameChannelId(this IUser user)
		{
			if (user.Game.HasValue)
			{
				var game = user.Game.Value.Name.RegexRemove("[^\\w]").ToLower();
				var channel = Global.ServerDictionary.Channels.Any(x => ChannelMatch(x.Key, game) <= 1) 
					? Global.ServerDictionary.Channels.FirstThat(x => ChannelMatch(x.Key, game) <= 1).Value : null;

				if (channel != null && channel.IsGaming)
					return channel.ID;
			}
			return 0;
		}

		public static async Task<IUserMessage> WaitForIt(this Task<IUserMessage> message)
		{
			var msg = await message;

			while (msg.Timestamp == DateTimeOffset.MinValue)
				await Task.Delay(100);

			return await Task.FromResult(msg);
		}

		public static async Task<IGuildChannel> FindChannel(this IGuild guild, string channelName, ChannelType? channelType = null)
		{
			var channels = (await guild.GetChannelsAsync()).Where(x => channelType == null || x.GetChannelType() == channelType);

			channels = channels.OrderBy(x => ChannelMatch(x.Name, channelName));

			return await Task.FromResult(channels.Where(x => ChannelMatch(x.Name, channelName) < 3).FirstOrDefault());
		}

		private static int ChannelMatch(string chan, string compare)
		{
			chan = chan.ToLower();
			compare = compare.ToLower();

			if (chan.GetWords().Any(compare.GetWords()))
				return 0;

			chan = chan.Where(char.IsLetterOrDigit);
			compare = compare.Where(char.IsLetterOrDigit);

			return chan.SpellCheck(compare, false);
		}

		public static Embed GetSubscriptionEmbed(SocketCommandContext context, bool oldSub, bool newSub, string title)
		{
			var embed = new EmbedBuilder();

			embed
				.WithFooter($"Requested by {context.User.Username}")
				.WithCurrentTimestamp()
				.WithDescription("\u200b")
				.WithTitle("Subscription Change")
				.AddInlineField("Subscription", title);

			if (oldSub != newSub)
			{
				embed
					.AddInlineField("Change Type", newSub ? "Subscribed" : "UnSubscribed")
					.AddField("\u200b", $"{context.User.Username}, you successfully {( newSub ? "Subscribed to" : "UnSubscribed from" )} {title}");

				if (newSub)
					embed.WithColor(80, 237, 174);
				else
					embed.WithColor(237, 205, 80);
			}
			else
			{
				embed
					.WithColor(237, 64, 64)
					.AddInlineField("Change Type", "Unchanged")
					.AddField("\u200b", $"{context.User.Username}, you are already {( newSub ? "Subscribed to" : "UnSubscribed from" )} {title}");
			}

			return embed.Build();
		}

		public static ChannelType GetChannelType(this IChannel chan)
		{
			if (chan is ITextChannel || chan is ISocketMessageChannel)
				return ChannelType.Text;

			if (chan is IAudioChannel || chan is ISocketAudioChannel)
				return ChannelType.Voice;

			if (chan is IDMChannel || chan is IPrivateChannel || chan is ISocketPrivateChannel)
				return ChannelType.DM;

			return ChannelType.Group;
		}

		public static async Task<IGuildChannel> GetUsersChannel(this IGuild guild, SocketUser user, ChannelType? channelType = null)
			=> (await guild.GetChannelsAsync()).FirstThat(x => channelType == null || x.GetChannelType() == channelType && x.GetUsersAsync().Flatten().GetAwaiter().GetResult().Any(u => u.Id == user.Id));

		internal static Embed GetErrorEmbed(string title, string message, SocketCommandContext context, string commandName = null, ErrorPart? errorPart = null)
		{
			var embed = new EmbedBuilder();
			
			embed
				.WithColor(Color.Red)
				.WithFooter($"Requested by {context.User.Username}")
				.WithCurrentTimestamp()
				.WithDescription(message)
				.WithTitle(title);

			if (commandName != null)
				embed.AddInlineField("Command", commandName);

			if (errorPart != null)
				embed.AddInlineField("Fault", errorPart.ToString());

			return embed.Build();
		}

		public static string Mention(this IChannel channel)
			=> $"<#{channel.Id}>";

		public static void SelfDestruct(this IUserMessage msg, int delay = 15000)
			=> new Action(async () => { try { await msg.DeleteAsync(); } catch { } }).RunInBackground(delay);

		public static void SelfDestruct(this IMessage msg, int delay = 15000)
			=> new Action(async () => { try { await msg.DeleteAsync(); } catch { } }).RunInBackground(delay);

		public static async Task<IUserMessage> SelfDestruct(this Task<IUserMessage> msg, int delay = 15000)
		{
			var message = await msg;

			new Action(async () => { try { await message.DeleteAsync(); } catch { } }).RunInBackground(delay);

			return await Task.FromResult(message);
		}

		public static async Task<IMessage> SelfDestruct(this Task<IMessage> msg, int delay = 15000)
		{
			var message = await msg;

			new Action(async () => { try { await message.DeleteAsync(); } catch { } }).RunInBackground(delay);

			return await Task.FromResult(message);
		}

		public static async Task<IMessage> SelfDestruct(this Task<RestUserMessage> msg, int delay = 15000)
		{
			var message = await msg;

			new Action(async () => { try { await message.DeleteAsync(); } catch { } }).RunInBackground(delay);

			return await Task.FromResult(message);
		}

		public static string ToEmoji(this object obj)
			=> obj.ToString().ToLower().Convert(c => Emojies.ContainsKey(c) ? Emojies[c] : c.ToString()).ListStrings();

		public static Dictionary<char, string> Emojies = new Dictionary<char, string>
		{
			{ ' ', "  "                       },
			{ '1', ":one:"                    },
			{ '2', ":two:"                    },
			{ '3', ":three:"                  },
			{ '4', ":four:"                   },
			{ '5', ":five:"                   },
			{ '6', ":six:"                    },
			{ '7', ":seven:"                  },
			{ '8', ":eight:"                  },
			{ '9', ":nine:"                   },
			{ '0', ":zero:"                   },
			{ '#', ":hash:"                   },
			{ '*', ":asterisk:"               },
			{ '.', "<:Dot:321286041569067008>"},
			{ 'a', "<:H_A:285782726274056192>"},
			{ 'b', "<:H_B:285782726706069504>"},
			{ 'c', "<:H_C:285782726983024641>"},
			{ 'd', "<:H_D:285782729734488065>"},
			{ 'e', "<:H_E:285782730141204482>"},
			{ 'f', "<:H_F:285782730573086720>"},
			{ 'g', "<:H_G:285782731021877248>"},
			{ 'h', "<:H_H:285782730699046913>"},
			{ 'i', "<:H_I:285782730992779264>"},
			{ 'j', "<:H_J:285782731298832393>"},
			{ 'k', "<:H_K:285782731508678657>"},
			{ 'l', "<:H_L:285782731919589376>"},
			{ 'm', "<:H_M:285783607711367168>"},
			{ 'n', "<:H_N:285783607707172865>"},
			{ 'o', "<:H_O:285783610542522368>"},
			{ 'p', "<:H_P:285783610362167296>"},
			{ 'q', "<:H_Q:285783610949369856>"},
			{ 'r', "<:H_R:285783610995507201>"},
			{ 's', "<:H_S:285783612983476226>"},
			{ 't', "<:H_T:285783613004316673>"},
			{ 'u', "<:H_U:285783613126213634>"},
			{ 'v', "<:H_V:285783613818011649>"},
			{ 'w', "<:H_W:285783615013519360>"},
			{ 'x', "<:H_X:285783617949401089>"},
			{ 'y', "<:H_Y:285783618285076480>"},
			{ 'z', "<:H_Z:285783618792456192>"},
		};
	}
}
