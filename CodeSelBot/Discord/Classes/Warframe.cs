using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Extensions;
using Newtonsoft.Json;

namespace CodeSelBot.Discord.Classes
{
    public static class WarframeExtensions
    {
        public static string GetBossIcon(string boss)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CodeSelBot.WarframeBossIcons.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var bosses = JsonConvert.DeserializeObject<dynamic[]>(reader.ReadToEnd());

                return bosses.FirstThat(x => x.Boss == boss).Icon;
            }
        }

        public static EmbedBuilder WithFactionColor(this EmbedBuilder embed, string faction)
        {
            switch (faction)
            {
                case "Grineer":
                    embed.WithColor(Color.DarkOrange);
                    break;

                case "Corpus":
                    embed.WithColor(Color.DarkBlue);
                    break;

                case "Infested":
                case "Infestation":
                    embed.WithColor(Color.DarkGreen);
                    break;

                case "Corrupted":
                case "Orokin":
                    embed.WithColor(255, 249, 211);
                    break;

                default:
                    break;
            }

            return embed;
        }

        public static EmbedBuilder WithAcolyte(this EmbedBuilder embed, string acolyte)
        {
            switch (acolyte)
            {
                case "Angst":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/RnoDbl1.png");

                case "Malice":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/PTQZGm0.png");

                case "Torment":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/L3WKbLT.png");

                case "Violence":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/I1I4Ntt.png");

                case "Misery":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/ayaryFq.png");

                case "Mania":
                    return embed.WithThumbnailUrl(@"https://i.imgur.com/z9M8LVk.png");

                default:
                    return embed;
            }
        }

        public static string GetFactionIcon(string factionName)
        {
            switch (factionName)
            {
                case "Grineer":
                    return "<:grineer:470877872362356757>";

                case "Corpus":
                    return "<:corpus:471057169228496931>";

                case "Infested":
                case "Infestation":
                    return "<:infested:470877872379265034>";

                case "Corrupted":
                case "Orokin":
                    return "<:corrupted:471057169413046277>";

                default:
                    return "";
            }
        }
    }

    #region Classes
#pragma warning disable IDE1006 // Naming Styles

    public class WarframeInfo
    {
        public DateTime timestamp { get; set; }
        public News[] news { get; set; }
        public Event[] events { get; set; }
        public Alert[] alerts { get; set; }
        public Sortie sortie { get; set; }
        public Syndicatemission[] syndicateMissions { get; set; }
        public Fissure[] fissures { get; set; }
        public object[] globalUpgrades { get; set; }
        public Flashsale[] flashSales { get; set; }
        public Invasion[] invasions { get; set; }
        public Darksector[] darkSectors { get; set; }
        public Voidtrader voidTrader { get; set; }
        public Dailydeal[] dailyDeals { get; set; }
        public Simaris simaris { get; set; }
        public Conclavechallenge[] conclaveChallenges { get; set; }
        public PersistentEnemy[] persistentEnemies { get; set; }
        public Earthcycle earthCycle { get; set; }
        public Cetuscycle cetusCycle { get; set; }
        public Constructionprogress constructionProgress { get; set; }
    }

    public class PersistentEnemy
    {
        public string id { get; set; }
        public string agentType { get; set; }
        public string locationTag { get; set; }
        public int rank { get; set; }
        public float healthPercent { get; set; }
        public int fleeDamage { get; set; }
        public DateTime lastDiscoveredTime { get; set; }
        public string lastDiscoveredAt { get; set; }
        public bool isDiscovered { get; set; }
        public bool isUsingTicketing { get; set; }
        public string pid { get; set; }
    }

    public class Sortie
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public string rewardPool { get; set; }
        public Variant[] variants { get; set; }
        public string boss { get; set; }
        public string faction { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class Variant
    {
        public string boss { get; set; }
        public string planet { get; set; }
        public string missionType { get; set; }
        public string modifier { get; set; }
        public string modifierDescription { get; set; }
        public string node { get; set; }
    }

    public class Voidtrader
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public string character { get; set; }
        public string location { get; set; }
        public Inventory[] inventory { get; set; }
        public string psId { get; set; }
        public bool active { get; set; }
        public string startString { get; set; }
        public string endString { get; set; }
    }

    public class Inventory
    {
        public string item { get; set; }
        public int ducats { get; set; }
        public int credits { get; set; }
    }

    public class Simaris
    {
        public string target { get; set; }
        public bool isTargetActive { get; set; }
        public string asString { get; set; }
    }

    public class Earthcycle
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public bool isDay { get; set; }
        public string timeLeft { get; set; }
    }

    public class Cetuscycle
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public bool isDay { get; set; }
        public string timeLeft { get; set; }
        public bool isCetus { get; set; }
        public string shortString { get; set; }
    }

    public class Constructionprogress
    {
        public string id { get; set; }
        public string fomorianProgress { get; set; }
        public string razorbackProgress { get; set; }
        public string unknownProgress { get; set; }
    }

    public class News
    {
        public string id { get; set; }
        public string message { get; set; }
        public string link { get; set; }
        public string imageLink { get; set; }
        public bool priority { get; set; }
        public DateTime date { get; set; }
        public string eta { get; set; }
        public bool update { get; set; }
        public bool primeAccess { get; set; }
        public bool stream { get; set; }
        public Translations translations { get; set; }
        public string asString { get; set; }
    }

    public class Translations
    {
        public string en { get; set; }
        public string fr { get; set; }
        public string de { get; set; }
        public string es { get; set; }
        public string ru { get; set; }
        public string tr { get; set; }
        public string ja { get; set; }
        public string zh { get; set; }
        public string ko { get; set; }
        public string tc { get; set; }
        public string it { get; set; }
        public string pt { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public int maximumScore { get; set; }
        public int smallInterval { get; set; }
        public int largeInterval { get; set; }
        public string faction { get; set; }
        public string description { get; set; }
        public string node { get; set; }
        public string[] concurrentNodes { get; set; }
        public Reward[] rewards { get; set; }
        public bool expired { get; set; }
        public string asString { get; set; }
    }

    public class Reward
    {
        public string[] items { get; set; }
        public object[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Alert
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public Mission mission { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
        public string[] rewardTypes { get; set; }
    }

    public class Mission
    {
        public string node { get; set; }
        public string type { get; set; }
        public string faction { get; set; }
        public Reward1 reward { get; set; }
        public int minEnemyLevel { get; set; }
        public int maxEnemyLevel { get; set; }
        public bool nightmare { get; set; }
        public bool archwingRequired { get; set; }
        public int maxWaveNum { get; set; }
    }

    public class Reward1
    {
        public string[] items { get; set; }
        public Counteditem[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Counteditem
    {
        public int count { get; set; }
        public string type { get; set; }
    }

    public class Syndicatemission
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public string syndicate { get; set; }
        public string[] nodes { get; set; }
        public Job[] jobs { get; set; }
        public string eta { get; set; }
    }

    public class Job
    {
        public string id { get; set; }
        public string type { get; set; }
        public int[] enemyLevels { get; set; }
        public int[] standingStages { get; set; }
        public string[] rewardPool { get; set; }
    }

    public class Fissure
    {
        public string id { get; set; }
        public string node { get; set; }
        public string missionType { get; set; }
        public string enemy { get; set; }
        public string tier { get; set; }
        public int tierNum { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class Flashsale
    {
        public string item { get; set; }
        public DateTime expiry { get; set; }
        public int discount { get; set; }
        public int premiumOverride { get; set; }
        public bool isFeatured { get; set; }
        public bool isPopular { get; set; }
        public string id { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class Invasion
    {
        public string id { get; set; }
        public string node { get; set; }
        public string desc { get; set; }
        public Attackerreward attackerReward { get; set; }
        public string attackingFaction { get; set; }
        public Defenderreward defenderReward { get; set; }
        public string defendingFaction { get; set; }
        public bool vsInfestation { get; set; }
        public DateTime activation { get; set; }
        public int count { get; set; }
        public int requiredRuns { get; set; }
        public float completion { get; set; }
        public bool completed { get; set; }
        public string eta { get; set; }
        public string[] rewardTypes { get; set; }
    }

    public class Attackerreward
    {
        public object[] items { get; set; }
        public Counteditem1[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Counteditem1
    {
        public int count { get; set; }
        public string type { get; set; }
    }

    public class Defenderreward
    {
        public object[] items { get; set; }
        public Counteditem2[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Counteditem2
    {
        public int count { get; set; }
        public string type { get; set; }
    }

    public class Darksector
    {
        public string id { get; set; }
        public bool isAlliance { get; set; }
        public string defenderName { get; set; }
        public int defenderDeployemntActivation { get; set; }
        public string defenderMOTD { get; set; }
        public string deployerName { get; set; }
        public string deployerClan { get; set; }
        public object[] history { get; set; }
    }

    public class Dailydeal
    {
        public string item { get; set; }
        public DateTime expiry { get; set; }
        public int originalPrice { get; set; }
        public int salePrice { get; set; }
        public int total { get; set; }
        public int sold { get; set; }
        public string id { get; set; }
        public string eta { get; set; }
        public int discount { get; set; }
    }

    public class Conclavechallenge
    {
        public string id { get; set; }
        public string description { get; set; }
        public DateTime expiry { get; set; }
        public int amount { get; set; }
        public string mode { get; set; }
        public string category { get; set; }
        public string eta { get; set; }
        public bool expired { get; set; }
        public bool daily { get; set; }
        public bool rootChallenge { get; set; }
        public string endString { get; set; }
        public string asString { get; set; }
    }

#pragma warning restore IDE1006 // Naming Styles
    #endregion
}
