using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CodeSelBot.Discord.Classes;
using Discord;
using Discord.WebSocket;
using Extensions;
using Newtonsoft.Json;
using RestSharp;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord
{
	public static class Periodic
	{
		private static Dictionary<string, ServerChannels> Channels => Global.ServerDictionary.Channels;
		private static Timer Timer = new Timer(5000);
		private static Timer WarframeTimer = new Timer(60000);

		internal static void Start()
		{
			Timer.Elapsed += Timer_Elapsed;
			Timer.Start();
			WarframeTimer.Elapsed += WarframeTimer_Elapsed;
			WarframeTimer.Start();
		}

		internal static void Resume()
		{
			Timer.Start();
			WarframeTimer.Start();
		}

		internal static void Stop()
		{
			Timer.Stop();
			WarframeTimer.Start();
		}

		private static void WarframeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var responce = new RestClient("https://api.warframestat.us/PC").Execute(new RestRequest(Method.GET));
			var warInf = JsonConvert.DeserializeObject<WarframeInfo>(responce.Content);

			#region Cetus
			if (warInf.cetusCycle.expiry.AddMinutes(-2) <= DateTime.UtcNow)
				if (Global.Data.UserData.Any(x => x.WarframeData.CetusReminders.ContainsKey(warInf.cetusCycle.id)))
				{
					foreach (var user in Global.Data.UserData.Where(x => x.WarframeData.CetusReminders.ContainsKey(warInf.cetusCycle.id)))
					{
						user.WarframeData.CetusReminders.Remove(warInf.cetusCycle.id);
						var embed = new EmbedBuilder();

						embed
							.WithColor(220, 220, 220)
							.WithCurrentTimestamp()
							.WithDescription("\u200b")
							.WithTitle(":bell: Cetus Cycle Reminder")
							.AddField("\u200b", $"The Cetus Cycle is almost done! {warInf.cetusCycle.timeLeft} till {( warInf.cetusCycle.isDay ? "Night" : "Day" )}!");

						user.User.SendMessageAsync("", embed: embed.Build());
					}
				}
			#endregion

			#region Sortie
			if (!Global.Data.NotifiedItems.Any(warInf.sortie.id))
			{
				var embed = new EmbedBuilder();

				embed
					.WithFooter($"Daily Sortie Announcement")
					.WithCurrentTimestamp()
					.WithTitle($"{WarframeExtensions.GetFactionIcon(warInf.sortie.faction)} {warInf.sortie.boss} Sortie")
					.WithFactionColor(warInf.sortie.faction)
					.WithThumbnailUrl(WarframeExtensions.GetBossIcon(warInf.sortie.boss))
					.WithDescription("\n\u200b");

				foreach (var mission in warInf.sortie.variants)
				{
					embed.AddField($":white_small_square: {mission.missionType} on {mission.node}", $"{mission.modifier}\n\n{mission.modifierDescription}\n\u200b");
				}

				Global.Client.Guilds.First().GetTextChannel(Global.ServerDictionary.Channels["WarframeText"].ID).SendMessageAsync(Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Sortie).ListStrings(x => $"<@{x.ID}> • ").TrimEnd(3), embed: embed.Build());
				SQLHandler.AddNotification(warInf.sortie.id);
			}
			#endregion

			#region Baro
			if (warInf.voidTrader.active && !Global.Data.NotifiedItems.Any(warInf.voidTrader.id))
			{
				var embed = new EmbedBuilder();

				embed
					.WithFooter($"Baro Ki'Teer Announcement")
					.WithCurrentTimestamp()
					.WithTitle($"Baro Ki'Teer Has Come from the Void")
					.WithColor(72, 148, 162)
					.WithThumbnailUrl("https://i.imgur.com/y9qT4VK.png")
					.WithDescription($"Baro Ki'Teer is currently in the {warInf.voidTrader.location}\n\n\u200b");

				foreach (var item in warInf.voidTrader.inventory)
				{
					embed.AddInlineField(item.item, $"{item.ducats} <:ducats:472659552383533068>  •  {item.credits} <:WarframeCredits:470867750101975051>\n\u200b");
				}

				Global.Client.Guilds.First().GetTextChannel(Global.ServerDictionary.Channels["WarframeText"].ID).SendMessageAsync(Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Baro).ListStrings(x => $"<@{x.ID}> • ").TrimEnd(3), embed: embed.Build());

				SQLHandler.AddNotification(warInf.voidTrader.id);
			}
			#endregion

			#region Alerts
			if (Global.Data.UserData.Any(x => x.Preferences.Subscriptions.W_Alerts))
			{
				var alerts = warInf.alerts.Where(alert =>
				{
					if (alert.expired)
						return false;

					if (Global.Data.NotifiedItems.Any(alert.id))
						return false;

					return alert.rewardTypes.Any("nitain", "vandal", "wraith", "mutalist", "neuralSensors", "orokinCell", "oxium", "argonCrystal", "tellurium", "reactor", "catalyst", "forma", "synthula", "exilus", "riven", "kavatGene");
				});

				foreach (var alert in alerts)
				{
					var embed = GenerateEmbed(alert);

					foreach (var user in Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Alerts))
						user.User.SendMessageAsync("", embed: embed.Build());

					SQLHandler.AddNotification(alert.id);
				}
			}
			#endregion

			#region Invasions
			if (Global.Data.UserData.Any(x => x.Preferences.Subscriptions.W_Invasions))
			{
				var invasions = warInf.invasions.Where(invasion =>
				{
					if (invasion.completed)
						return false;

					if (Global.Data.NotifiedItems.Any(invasion.id))
						return false;

					return invasion.rewardTypes.Any("vandal", "wraith", "reactor", "catalyst", "forma", "exilus", "riven");
				});

				foreach (var invasion in invasions)
				{
					var embed = GenerateEmbed(invasion);

					foreach (var user in Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Invasions))
						user.User.SendMessageAsync("", embed: embed.Build());

					SQLHandler.AddNotification(invasion.id);
				}
			}
			#endregion

			#region Acolytes

			if(warInf.persistentEnemies.Any())
			{
				foreach (var acolyte in warInf.persistentEnemies)
				{
					if (!Global.Data.NotifiedItems.Any(acolyte.id))
					{
						var embed = new EmbedBuilder();

						embed
							.WithFooter($"Acolyte Appearance")
							.WithCurrentTimestamp()
							.WithTitle($"{acolyte.agentType} Appeared!")
							.WithColor(Color.DarkRed)
                            .WithAcolyte(acolyte.agentType)
							.WithDescription($"One of the Stalker's Acolytes, **{acolyte.agentType}**, just appeared.\nRally yourself, Tenno!\n\u200b")
							.AddInlineField("Rank", acolyte.rank)
							.AddInlineField("Flee Damage", acolyte.fleeDamage);

						Global.Client.Guilds.First().GetTextChannel(Global.ServerDictionary.Channels["WarframeText"].ID).SendMessageAsync(Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Sortie).ListStrings(x => $"<@{x.ID}> • ").TrimEnd(3), embed: embed.Build());

						SQLHandler.AddNotification(acolyte.id);
					}

					if (acolyte.isDiscovered)
					{
						if (!Global.Data.NotifiedItems.Any($"_{acolyte.id}"))
						{
							var embed = new EmbedBuilder();

							embed
								.WithFooter($"Acolyte Location Found")
								.WithCurrentTimestamp()
								.WithTitle($"{acolyte.agentType} has been Discovered!")
								.WithColor(Color.DarkRed)
                                .WithAcolyte(acolyte.agentType)
                                .WithDescription($"**{acolyte.lastDiscoveredAt}** was found to be hiding the Acolyte, **{acolyte.agentType}**\n\u200b")
								.AddInlineField("Health", $"{Math.Round(100 * acolyte.healthPercent, 2)} %")
								.AddInlineField("Rank", acolyte.rank)
								.AddInlineField("Flee Damage", acolyte.fleeDamage);

                        foreach (var user in Global.Data.UserData.Where(x => x.Preferences.Subscriptions.W_Acolytes && !x.Preferences.Acolytes.Any(acolyte.id)))
                           user.User.SendMessageAsync("", embed: embed.Build());

                        SQLHandler.AddNotification($"_{acolyte.id}");
						}
					}
					else if (Global.Data.NotifiedItems.Any($"_{acolyte.id}"))
						SQLHandler.RemoveNotification($"_{acolyte.id}");
				}
			}

			#endregion

			#region Custom Subs

			foreach (var user in Global.Data.UserData.Where(x => x.WarframeData.WarframeSubs.Count > 0))
			{
				foreach (var subInf in user.WarframeData.WarframeSubs)
				{
					switch (subInf.Type)
					{
						case "i":
						case "invasion":
							foreach (var inv in warInf.invasions)
							{
								if (!Global.Data.NotifiedItems.Contains(user.ID + inv.id) &&
									(inv.attackerReward.asString.SearchCheck(subInf.Param) ||
									inv.defenderReward.asString.SearchCheck(subInf.Param) ||
									inv.rewardTypes.Any(x => x.SearchCheck(subInf.Param))
									))
								{
									user.User.SendMessageAsync("", embed: GenerateEmbed(inv).Build());
									SQLHandler.AddNotification(user.ID + inv.id);
								}
							}
							break;
						case "a":
						case "alert":
							foreach (var alert in warInf.alerts)
							{
								if (!Global.Data.NotifiedItems.Contains(user.ID + alert.id) && alert.mission.reward.itemString.SearchCheck(subInf.Param))
								{
									user.User.SendMessageAsync("", embed: GenerateEmbed(alert).Build());
									SQLHandler.AddNotification(user.ID + alert.id);
								}
							}
							break;
						default:
							break;
					}
				}
			}

			#endregion
		}

		private static EmbedBuilder GenerateEmbed(Alert alert)
		{
			var embed = new EmbedBuilder();

			embed
				.WithFooter($"Warframe Alert of Interest")
				.WithCurrentTimestamp()
				.WithTitle($"{WarframeExtensions.GetFactionIcon(alert.mission.faction)} {alert.mission.reward.itemString} Alert")
				.WithFactionColor(alert.mission.faction)
				.WithThumbnailUrl(alert.mission.reward.thumbnail)
				.WithDescription("\n\u200b")
				.AddInlineField("Reward", $"{alert.mission.reward.itemString.If(string.IsNullOrEmpty, x => "", x => x + " & ")}{alert.mission.reward.credits} <:WarframeCredits:470867750101975051>")
				.AddInlineField("Duration", alert.eta)
				.AddInlineField("Mission", $"{alert.mission.archwingRequired.If("Archwing ", "")}{alert.mission.nightmare.If("Nightmare ", "")} {alert.mission.type}")
				.AddInlineField("Node", alert.mission.node);
			return embed;
		}

		private static EmbedBuilder GenerateEmbed(Invasion invasion)
		{
			var embed = new EmbedBuilder();

			embed
				.WithFooter($"Warframe Invasion of Interest")
				.WithCurrentTimestamp()
				.WithThumbnailUrl("https://i.imgur.com/mumtkEX.png")
				.WithTitle($"{invasion.desc} Invasion")
				.WithColor(Color.Orange)
				.WithDescription($"A new Invasion has started on {invasion.node}\n\u200b")
				.AddField($"{WarframeExtensions.GetFactionIcon(invasion.defendingFaction)} Defense Reward", $"{invasion.defenderReward.asString}\n\u200b");

			if (!invasion.vsInfestation)
				embed.AddField($"{WarframeExtensions.GetFactionIcon(invasion.attackingFaction)} Attackers Reward", $"{invasion.attackerReward.asString}\n\u200b");

			return embed;
		}

		private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Global.Data.Uptime += (ulong)Timer.Interval;

			if (Global.Client.Guilds.Any())
			{
				var users = Global.Client.Guilds.First().Users;

				foreach (var user in users)
				{
					var userDat = user.GetData();

					if (userDat.Preferences.Track)
					{
						UpdateOnlineTracking(user, userDat);
						if (user.Game?.Name != null)
						{
							if (!userDat.Tracking.Games.Any(user.Game.Value))
							{
								userDat.Tracking.Games.Add(user.Game.Value);
								SQLHandler.AddUserGame(user.Id, user.Game.Value);
							}
						}
					}
				}

				var channels = Global.Client.Guilds.First().Channels.Where(x => x.GetChannelType() == ChannelType.Voice);

				AutoMove(users.Where(x => x.VoiceChannel != null).ToArray(), channels);

				GiveRoles(users.Where(x => x.Game.HasValue));

				Global.Data.SaveUsers();
			}

			Global.Data.SaveGeneral();
		}

		private static void GiveRoles(IEnumerable<SocketGuildUser> users)
		{
			foreach (var user in users)
			{
				var gameId = user.GetGameChannelId();

				if (gameId != 0)
				{
					var roleId = Channels.FirstThat(x => x.Value.ID == gameId).Value.Role;

					if (roleId != null && !user.Roles.Any(x => x.Id == roleId))
					{
						var role = Global.Client.Guilds.First().GetRole((ulong)roleId);
						user.AddRoleAsync(role);
						Global.Bot.Log(ActionType.Add, LogSeverity.Verbose, "Roles", $"Added the {role.Name} role to {user.Username}#{user.Discriminator}");
					}
				}
			}
		}

		private static void AutoMove(IReadOnlyCollection<SocketGuildUser> users, IEnumerable<SocketGuildChannel> channels)
		{
			if (users.Count == 0 || !Global.AutoMove)
				return;

			var usersToMove = new List<Tuple<SocketGuildUser, ulong, ulong>>();

			#region AFK Channel
			foreach (var user in users)
			{
				if (user.VoiceChannel.Users.Count == 1)
				{
					var udata = user.GetData();
					if (udata.Preferences.AFKMove)
					{
						udata.Session.AFKTimer += Timer.Interval;
						if (udata.Session.AFKTimer > 150000)
						{
							usersToMove.Add(new Tuple<SocketGuildUser, ulong, ulong>((user as SocketGuildUser), user.VoiceChannel.Id, Channels["AFK"].ID));
							udata.Session.AFKMoveChanId = user.VoiceChannel.Id;
							udata.Session.AFKTimer = 0;
						}
					}
				}
			}

			foreach (var user in users.Where(x => x.VoiceChannel.Id == Channels["AFK"].ID))
			{
				var userDat = user.GetData();
				var userGameChan = user.GetGameChannelId();

				userDat.Session.AFKTimer = 0;
				
				// AFKMove Back To Channel
				if (userDat.Session.AFKMoveChanId != 0 && channels.FirstThat(x => x.Id == userDat.Session.AFKMoveChanId).Users.Count > 0)
				{
					if (!usersToMove.Any(x => x.Item1 == user))
					{
						usersToMove.Add(new Tuple<SocketGuildUser, ulong, ulong>(user, Channels["AFK"].ID, userDat.Session.AFKMoveChanId));
						userDat.Session.AFKMoveChanId = 0;
						userDat.Tracking.TimesMoved++;
					}
				}

				if(userDat.Preferences.AFKMove && userGameChan != 0 && userGameChan != userDat.Session.CurrentGame)
				{
					usersToMove.Add(new Tuple<SocketGuildUser, ulong, ulong>(user, Channels["AFK"].ID, userGameChan));
					userDat.Session.AFKMoveChanId = 0;
					userDat.Session.CurrentGame = userGameChan;
					userDat.Tracking.TimesMoved++;
				}
				else if (userGameChan != userDat.Session.CurrentGame)
					userDat.Session.CurrentGame = 0;
			}

			foreach (var user in users.Where(x => !usersToMove.Any(y => y.Item1.Id == x.Id) && x.VoiceChannel.Id != Channels["AFK"].ID))
				user.GetData().Session.AFKMoveChanId = 0;
			#endregion

			#region Main Channel
			foreach (var user in users.Where(x => x.VoiceChannel.Id == Channels["Main"].ID))
			{
				var userDat = user.GetData();

				if (user.GetGameChannelId() != userDat.Session.CurrentGame)
					userDat.Session.CurrentGame = 0;
			}
			#endregion

			#region Gaming AutoMove
			foreach (var chan in Channels.Where(x => x.Value.Type == ChannelType.Voice && x.Value.ID != Channels["AFK"].ID).Convert(x => x.Value))
			{
				var channel = channels.FirstThat(x => x.Id == chan.ID);

				if (channel.Users.Count > 0 && channel.Users.All(u => !usersToMove.Any(x => x.Item1.Id == u.Id)))
				{
					var targetChans = new List<ulong>();

					foreach (var user in channel.Users)
					{
						var userGameChan = user.GetGameChannelId();

						if (userGameChan != 0)
							targetChans.Add(userGameChan);
					}

					if (targetChans.Count == 0)
						continue;

					var dic = targetChans.ConvertDictionary(x => new KeyValuePair<ulong, int>(x, targetChans.Count(y => y == x)));

					if(dic.Max(x => x.Value) >= Math.Floor(channel.Users.Count / 2d) && dic.FirstThat(x => x.Value == dic.Max(y => y.Value)).Key != chan.ID)
						foreach (var user in channel.Users)
						{
							var userDat = user.GetData();
							var userGameChan = dic.FirstThat(x => x.Value == dic.Max(y => y.Value)).Key;

							usersToMove.Add(new Tuple<SocketGuildUser, ulong, ulong>((user as SocketGuildUser), chan.ID, userGameChan));
							userDat.Session.CurrentGame = userGameChan;
							userDat.Tracking.TimesMoved++;
						}
				}
			}
			#endregion

			#region Gaming Channels
			foreach (var chan in Channels.Where(x => x.Value.Type == ChannelType.Voice && x.Value.IsGaming).Convert(x => x.Value))
			{
				var channel = channels.FirstThat(x => x.Id == chan.ID);

				if (channel.Users.All(x => (chan.Role == null && x.Game == null) || (chan.Role != null && x.GetGameChannelId() != channel.Id)))
					foreach (var user in channel.Users)
					{
						var userDat = user.GetData();
						var userGameChan = user.GetGameChannelId();
						var targetChan = channel.Users.All(x => x.Game.HasValue) ? "Gaming" : "Main";

						if (!usersToMove.Any(x => x.Item1 == user))
						{
							usersToMove.Add(new Tuple<SocketGuildUser, ulong, ulong>((user as SocketGuildUser), chan.ID, Channels[targetChan].ID));
							userDat.Session.CurrentGame = 0;
							userDat.Tracking.TimesMoved++;
						}
					}
			}
			#endregion

			if (usersToMove.Count(x => x.Item2 != x.Item3) > 0)
			{
				var embed = new EmbedBuilder();

				embed
					.WithColor(Color.Teal)
					.WithFooter($"Automatic Reply for Auto-Move Functionality")
					.WithCurrentTimestamp()
					.WithDescription("The following move actions have been taken")
					.WithTitle("Move Actions");

				foreach (var item in usersToMove.Where(x => x.Item2 != x.Item3))
				{
					item.Item1.ModifyAsync(x => x.ChannelId = item.Item3);

					Global.Bot.Log(ActionType.Add, LogSeverity.Info, "Movement", $"Moved {item.Item1.Username}#{item.Item1.Discriminator} from {channels.FirstThat(x => x.Id == item.Item2).Name} to {channels.FirstThat(x => x.Id == item.Item3).Name}");
					
					if (item.Item3 != Channels["AFK"].ID)
						embed
							.AddInlineField("User", item.Item1.Mention)
							.AddInlineField("From", channels.FirstThat(x => x.Id == item.Item2).Name)
							.AddInlineField("To", channels.FirstThat(x => x.Id == item.Item3).Name);
				}

				#pragma warning disable CS4014
				if (false && usersToMove.Count(x => x.Item2 != x.Item3 && x.Item3 != Channels["AFK"].ID && (x.Item2 != Channels["AFK"].ID || x.Item1.GetData().Session.AFKMoveChanId != 0)) > 0)
					Global.Client.Guilds.First().DefaultChannel.SendMessageAsync("", false, embed.Build()).SelfDestruct();
				#pragma warning restore CS4014
			}
		}

		private static void UpdateOnlineTracking(SocketGuildUser user, UserData userDat)
		{
			if (user.Status == UserStatus.Offline)
				userDat.Tracking.OnlineSince = DateTime.MinValue;
			else
			{
				userDat.Tracking.LastOnline = DateTime.Now;

				if (userDat.Tracking.OnlineSince == DateTime.MinValue)
					userDat.Tracking.OnlineSince = DateTime.Now; ;
			}
		}
	}
}
