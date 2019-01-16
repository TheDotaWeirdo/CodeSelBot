using CodeSelBot.Discord.Classes;
using Discord;
using Discord.Commands;
using Extensions;
using Newtonsoft.Json;
using RestSharp;
using SchoolSystemManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSelBot.Discord.Modules
{
	public class Warframe : ModuleBase<SocketCommandContext>
	{
		[Command("warframe"), Alias("wf"), Summary("Executes Warframe related commands like getting current alerts and fissures.")]
		public async Task WarframeAsync([Remainder]string context)
		{
			await Context.Channel.TriggerTypingAsync();
			var func = new List<Func<WarframeInfo, string, Task>>();
			var ex = (Exception)null;
			var errPart = (ErrorPart?)null;
			context = context.ToLower();

			if (context.StartsWith("sub ") || context.StartsWith("unsub "))
				func.Add(WarframeSubscribe);
			else
			{
				if (context.Contains("alert") || context.ContainsWord("a"))
					func.Add(WarframeAlerts);
				if (context.Contains("cetus") || context.ContainsWord("c"))
					func.Add(WarframeCetus);
				if (context.Contains("invasion") || context.ContainsWord("i"))
					func.Add(WarframeInvasions);
				if (context.Contains("fissure") || context.ContainsWord("f"))
					func.Add(WarframeFissures);
				if (context.Contains("sortie") || context.ContainsWord("s"))
					func.Add(WarframeSortie);
                if (context.Contains("baro") || context.ContainsWord("b"))
                    func.Add(WarframeBaro);
                if (context.Contains("acolyte") || context.ContainsWord("ac"))
                    func.Add(WarframeAcolytes);
                if (context.Contains("ostron") || context.ContainsWord("o"))
                    func.Add(WarframeOstron);
            }

			if (func.Count == 0)
			{ ex = new Exception("Context is invalid"); errPart = ErrorPart.User; goto err; }

			try
			{
				var responce = new RestClient("https://api.warframestat.us/PC").Execute(new RestRequest(Method.GET));
				var warInf = JsonConvert.DeserializeObject<WarframeInfo>(responce.Content);

				foreach (var item in func)
					await item(warInf, context);
			}
			catch (Exception exc) { ex = exc; errPart = ErrorPart.Bot; goto err; }
			Context.Message.SelfDestruct();

			return;
			err:
			await ReplyAsync(string.Empty, false, DiscordExtensions.GetErrorEmbed("Error", ex.Message, Context, commandName: "Warframe", errorPart: errPart)).SelfDestruct(120000);
		}

		private async Task WarframeAlerts(WarframeInfo inf, string context)
		{
			var embed = new EmbedBuilder();
			var @params = context.Split(' ');

			embed
				.WithColor(Color.Blue)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithThumbnailUrl("https://i.imgur.com/udgwKOs.png")
				.WithTitle("Current");

			if (@params.Contains("noc"))
				embed.Title += " No Credit";

			if (@params.Contains("noe"))
				embed.Title += " No Endo";

			if (@params.Contains("good"))
				embed.Title += " Good";

			embed.Title += " Alerts";

			foreach (var alert in inf.alerts)
			{
				if (@params.Contains("noc") && alert.rewardTypes.All(x => x == "credits"))
					continue;
				if (@params.Contains("noe") && alert.rewardTypes.All(x => x == "endo"))
					continue;
				if (@params.Contains("good") && !alert.rewardTypes.Any("nitain", "vandal", "wraith", "mutalist", "neuralSensors", "orokinCell", "oxium", "argonCrystal", "tellurium", "reactor", "catalyst", "forma", "synthula", "exilus", "riven", "kavatGene"))
					continue;

				embed.AddField($"{WarframeExtensions.GetFactionIcon(alert.mission.faction)} {alert.mission.node} • Expires in {alert.eta}",
					$"**Type:** {alert.mission.archwingRequired.If("Archwing ", "")}{alert.mission.nightmare.If("Nightmare ", "")}" +
					$"{alert.mission.type}\n**Rewards:** {alert.mission.reward.itemString.If(string.IsNullOrEmpty, x => "", x => x + " & ")}{alert.mission.reward.credits} <:WarframeCredits:470867750101975051> Credits\n\u200b");
			}
			
			if (embed.Fields.Count == 0)
				await ReplyAsync("No Alerts match your querry").SelfDestruct(30000);
			else
				await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
        }

        private async Task WarframeOstron(WarframeInfo inf, string context)
        {
            var ostron = inf.syndicateMissions.FirstThat(x => x.syndicate == "Ostrons");
            var embed = new EmbedBuilder();
            var @params = context.Split(' ');

            embed
                .WithColor(Color.Blue)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp()
                .WithThumbnailUrl("https://vignette.wikia.nocookie.net/warframe/images/8/80/OstronSigil.png/revision/latest/scale-to-width-down/350?cb=20171017011442")
                .WithTitle("Current Ostron Bounties")
                .WithDescription($"Bounties will expire in {ostron.eta}\n\u200b");

            foreach (var job in ostron.jobs)
            {
					embed.AddField($"{job.type} ({job.enemyLevels[0]} - {job.enemyLevels[1]})",
						$"Rewards {job.standingStages.Sum()} Standing over {job.standingStages.Length} missions\n\n" +
						job.rewardPool.ListStrings(x => $"  • {x}\n") +
						$"\u200b");
            }

            await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
        }

      private async Task WarframeCetus(WarframeInfo inf, string context)
		{
			var embed = new EmbedBuilder();

			embed
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithTitle(( inf.cetusCycle.isDay ? ":sunny:" : ":full_moon:" ) + " Cetus Cycle");

			if (inf.cetusCycle.isDay)
				embed.WithColor(249, 229, 82);
			else
				embed.WithColor(82, 90, 249);

			embed.WithDescription($"Currently {( inf.cetusCycle.isDay ? "Day" : "Night" )}-Time, {inf.cetusCycle.timeLeft} till {( inf.cetusCycle.isDay ? "Night" : "Day" )}\n\nGet notified when this cycle ends with `-warframe sub cetus`");

			await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
		}

		private async Task WarframeFissures(WarframeInfo inf, string context)
		{
			var embed = new EmbedBuilder();
			var @params = context.Split(' ');

			embed
				.WithColor(255, 249, 211)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithThumbnailUrl("https://i.imgur.com/h3M80eE.png")
				.WithTitle("Current");

			if (@params.Contains("good"))
				embed.Title += " Good";

			if (@params.Any("t1", "t2", "t3", "t4"))
				embed.Title += " " + @params.Where(x => x.AnyOf("t1", "t2", "t3", "t4")).ListStrings(x => x.ToUpper() + ", ", false);

			embed.Title += " Fissures";

			foreach (var fissure in inf.fissures.Where(x => !x.expired).OrderBy(x => x.tierNum))
			{
				if (@params.Contains("good") && !fissure.missionType.AnyOf("Defense", "Excavation", "Survival", "Capture", "Exterminate"))
					continue;

				if (@params.Any("t1", "t2", "t3", "t4") && !@params.Any(x => x == $"t{fissure.tierNum}"))
						continue;

				embed.AddField($"{WarframeExtensions.GetFactionIcon(fissure.enemy)} {fissure.node} • {fissure.tier} • Expires in {fissure.eta}",
					$"{fissure.missionType} Tier {fissure.tierNum} Fissure\n\u200b");
			}
			
			if (embed.Fields.Count == 0)
				await ReplyAsync("No Fissures match your querry").SelfDestruct(30000);
			else
				await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
		}

		private async Task WarframeInvasions(WarframeInfo inf, string context)
		{
			var embed = new EmbedBuilder();
			var @params = context.Split(' ');

			embed
				.WithColor(Color.Orange)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithThumbnailUrl("https://i.imgur.com/mumtkEX.png")
				.WithTitle("Current");

			if (@params.Contains("good"))
				embed.Title += " Good";

			embed.Title += " Invasions";

			foreach (var invasion in inf.invasions.Where(x => !x.completed))
			{
				if (@params.Contains("good") && !invasion.rewardTypes.Any("vandal", "wraith", "mutalist", "reactor", "catalyst", "forma", "exilus", "riven"))
					continue;

				var desc = string.Empty;

				if (invasion.vsInfestation)
				{
					desc = $"{WarframeExtensions.GetFactionIcon(invasion.defendingFaction)} {invasion.defendingFaction} Reward:  {invasion.defenderReward.asString}";
				}
				else
				{
					desc = $"{WarframeExtensions.GetFactionIcon(invasion.attackingFaction)} {invasion.attackingFaction} Reward:  {invasion.attackerReward.asString}\n\n" +
							 $"{WarframeExtensions.GetFactionIcon(invasion.defendingFaction)} {invasion.defendingFaction} Reward:  {invasion.defenderReward.asString}";
				}

				desc += "\n\u200b";

				embed.AddField($"{invasion.node} • {WarframeExtensions.GetFactionIcon(invasion.attackingFaction)} vs {WarframeExtensions.GetFactionIcon(invasion.defendingFaction)} • {Math.Round(invasion.completion, 2)}%", desc);
			}

			if (embed.Fields.Count == 0)
				await ReplyAsync("No Invasions match your querry").SelfDestruct(30000);
			else
				await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
		}

		private async Task WarframeSortie(WarframeInfo inf, string context)
		{
			var embed = new EmbedBuilder();

			embed
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithTitle("Sortie")
				.WithFactionColor(inf.sortie.faction)
				.WithThumbnailUrl(WarframeExtensions.GetBossIcon(inf.sortie.boss))
				.WithDescription($"{WarframeExtensions.GetFactionIcon(inf.sortie.faction)} {inf.sortie.boss} • Expires in {inf.sortie.eta}\n\n\u200b");

			foreach (var mission in inf.sortie.variants)
			{
				embed.AddField($":white_small_square: {mission.missionType} on {mission.node}", $"{mission.modifier}\n\n{mission.modifierDescription}\n\u200b");
			}

			await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
        }

        private async Task WarframeBaro(WarframeInfo inf, string context)
        {
            var embed = new EmbedBuilder();

            embed
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp()
                .WithTitle("Baro Ki'Teer")
                .WithColor(72, 148, 162)
                .WithThumbnailUrl("https://i.imgur.com/y9qT4VK.png");

            if (inf.voidTrader.active)
            {
                embed.WithDescription($"Baro Ki'Teer is currently in the {inf.voidTrader.location}\n\n\u200b");

                foreach (var item in inf.voidTrader.inventory)
                {
                    embed.AddInlineField(item.item, $"{item.ducats} <:ducats:472659552383533068>  •  {item.credits} <:WarframeCredits:470867750101975051>\n\u200b");
                }
            }
            else
                embed.WithDescription($"Baro Ki'Teer is due in {inf.voidTrader.startString}");

            await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
        }

        private async Task WarframeAcolytes(WarframeInfo inf, string context)
        {
            foreach (var acolyte in inf.persistentEnemies)
            {
                var embed = new EmbedBuilder();

                embed
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp()
                    .WithTitle($"{acolyte.agentType} Info")
                    .WithColor(Color.DarkRed)
                    .WithAcolyte(acolyte.agentType)
                    .AddInlineField("Health", $"{Math.Round(100 * acolyte.healthPercent, 2)} %")
                    .AddInlineField("Rank", acolyte.rank)
                    .AddInlineField("Flee Damage", acolyte.fleeDamage);

                if (acolyte.isDiscovered)
                    embed.WithDescription($"**{acolyte.agentType}** is currently on **{acolyte.lastDiscoveredAt}**\n\u200b");
                else
                    embed.WithDescription($"**{acolyte.agentType}** is not currently discovered\n\u200b");

                await ReplyAsync("", embed: embed.Build()).SelfDestruct(600000);
            }
        }

        private async Task WarframeSubscribe(WarframeInfo inf, string context)
		{
			var sub = !context.StartsWith("un");
			var @params = context.Split(' ');
			var dat = Context.User.GetData();
			var valid = false;

			if(context.Contains(',') && Regex.IsMatch(context, @"(\w+),(.+)"))
			{
				var match = Regex.Match(context, @"(\w+),(.+)");
				var subInf = new UserWarframeSub(Context.User.Id, match.Groups[1].Value, match.Groups[2].Value);

				await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.WarframeData.WarframeSubs.Contains(subInf), sub, "Custom Warframe Subscription")).SelfDestruct(30000);

				if (sub)
				{
					dat.WarframeData.WarframeSubs.AddIfNotExist(subInf);
					SQLHandler.AddUserWarframeSub(subInf);
				}
				else
				{
					dat.WarframeData.WarframeSubs.Remove(subInf);
					SQLHandler.DeleteUserWarframeSub(subInf);
				}

				return;
			}

			if (@params.Contains("cetus") || @params.Any("c"))
			{
				valid = true;

				await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.WarframeData.CetusReminders.ContainsKey(inf.cetusCycle.id), sub, "Cetus Timer")).SelfDestruct(30000);

				if (dat.WarframeData.CetusReminders.ContainsKey(inf.cetusCycle.id) != sub)
				{
					if (sub)
						dat.WarframeData.CetusReminders.Add(inf.cetusCycle.id, inf.cetusCycle.expiry.ToLocalTime());
					else
						dat.WarframeData.CetusReminders.Remove(inf.cetusCycle.id);
				}
			}

			if (@params.Contains("sortie") || @params.Any("s"))
			{
				valid = true;

				await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Subscriptions.W_Sortie, sub, "Daily Sortie Reminder")).SelfDestruct(30000);

				dat.Preferences.Subscriptions.W_Sortie = sub;
            }

            if (@params.Contains("alert") || @params.Any("a"))
            {
                valid = true;

                await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Subscriptions.W_Alerts, sub, "Important Alert Notifications")).SelfDestruct(30000);

                dat.Preferences.Subscriptions.W_Alerts = sub;
            }

            if (@params.Contains("acolyte") || @params.Contains("acolytes") || @params.Any("ac"))
            {
                valid = true;
                var found = false;
                foreach (var acolyte in inf.persistentEnemies)
                {
                    if (@params.Contains(acolyte.agentType.ToLower()))
                    {
                        await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Acolytes.Any(acolyte.id), sub, acolyte.agentType + " Notifications")).SelfDestruct(30000);

								if (dat.Preferences.Acolytes.Any(acolyte.id) != sub)
								{
									if (sub)
									{
										dat.Preferences.Acolytes.Remove(acolyte.id);
										SQLHandler.RemoveUserAcolyte(Context.User.Id, acolyte.id);
									}
									else
									{
										dat.Preferences.Acolytes.Add(acolyte.id);
										SQLHandler.AddUserAcolyte(Context.User.Id, acolyte.id);
									}
								}
                        found = true;
                    }
                }
                if (!found)
                {
                    await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Subscriptions.W_Acolytes, sub, "Acolyte Notifications")).SelfDestruct(30000);
                    dat.Preferences.Subscriptions.W_Acolytes = sub;
                }
            }

            if (@params.Contains("invasion") || @params.Any("i"))
			{
				valid = true;

				await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Subscriptions.W_Invasions, sub, "Important Invasions Notifications")).SelfDestruct(30000);

				dat.Preferences.Subscriptions.W_Invasions = sub;
			}

			if (@params.Contains("baro") || @params.Any("b"))
			{
				valid = true;

				await ReplyAsync(string.Empty, false, DiscordExtensions.GetSubscriptionEmbed(Context, dat.Preferences.Subscriptions.W_Baro, sub, "Baro Ki'Teer Announcement")).SelfDestruct(30000);

				dat.Preferences.Subscriptions.W_Baro = sub;
			}

			if (!valid)
				await ReplyAsync(string.Empty, false, DiscordExtensions.GetErrorEmbed("Error", "Subscription Context is missing", Context, commandName: "Warframe Sub", errorPart: ErrorPart.User)).SelfDestruct(120000);
			else
				SQLHandler.UpdateUserSubs(dat);
		}
	}
}