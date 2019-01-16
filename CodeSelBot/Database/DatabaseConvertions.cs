using System;
using System.Data;
using CodeSelBot.Discord;
using CodeSelBot.Discord.Classes;
using Discord;
using Extensions;

namespace SchoolSystemManager.Database
{
	public static class DatabaseConvertions
	{
		public static Tuple<ulong, Game> UserGames(IDataReader arg) => new Tuple<ulong, Game>
		(
			ulong.Parse(arg["UserId"].ToString()),
			new Game(arg["Name"].ToString(), arg["StreamUrl"].ToString(), (StreamType)Enum.Parse(typeof(StreamType), arg["StreamType"].ToString()))
		);

		public static UserData UserData(IDataReader arg) => new UserData()
		{
			ID = ulong.Parse(arg["UserId"].ToString()),
			Preferences = new UserPreferences()
			{
				AFKMove = (bool)arg["AFKMove"],
				AutoMove = (bool)arg["AutoMove"],
				Track = (bool)arg["Track"],
				Subscriptions = new UserSubs()
				{
					W_Sortie = (bool)arg["W_Sortie"],
					W_Alerts = (bool)arg["W_Alerts"],
					W_Invasions = (bool)arg["W_Invasions"],
                    W_Baro = (bool)arg["W_Baro"],
                    W_Acolytes = (bool)arg["W_Acolytes"],
                }
			},
			Tracking = new UserTracking()
			{
				MessagesSent = arg["MessagesSent"].ToString().SmartParse(),
				CharactersSent = arg["CharsSent"].ToString().SmartParse(),
				CommandsRequested = arg["CommandsRequested"].ToString().SmartParse(),
				TimesMoved = arg["TimesMoved"].ToString().SmartParse(),
				ExileTime = arg["ExileTime"].ToString().SmartParse(),
				LastOnline = DateTime.TryParse(arg["LastOnline"].ToString(), out var lsR) ? lsR : DateTime.MinValue,
				OnlineSince = DateTime.TryParse(arg["OnlineSince"].ToString(), out var osR) ? osR : DateTime.MinValue
			}
		};

      public static Tuple<ulong, string> UserAcolytes(IDataReader arg)
         => new Tuple<ulong, string>(ulong.Parse((string)arg["UserId"]), (string)arg["Acolyte"]);

      public static string BotNotify(IDataReader arg) => arg["Id"].ToString();

		public static Data BotData(IDataReader arg) => new Data()
		{
			Uptime = ulong.Parse(arg["Uptime"].ToString())
		};

		public static UserWarframeSub UserWarframeSub(IDataReader arg) => new UserWarframeSub
		(
			ulong.Parse((string)arg["UserId"]),
			(string)arg["Type"],
			(string)arg["Param"]
		);
	}
}