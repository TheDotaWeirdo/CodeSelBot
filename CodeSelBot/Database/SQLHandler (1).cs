using CodeSelBot.Discord;
using CodeSelBot.Discord.Classes;
using Discord;
using Extensions;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystemManager.Database
{
	public static class SQLHandler
	{
		public const string ConnectionString = @"Data Source=DESKTOP-IRDFTEA\SQL2012;Initial Catalog=CodeSelData;User ID=sa;Password=8520";

		public static void AddUser(IUser user)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.AddUser", user.Id, user.Username, user.Discriminator);

		public static Data GetBotData()
			=> SqlHelper.ExecuteReader(ConnectionString, "dbo.GetBotData").Select(DatabaseConvertions.BotData).First();

		public static IEnumerable<string> GetBotNotifications()
			=> SqlHelper.ExecuteReader(ConnectionString, "dbo.GetBotNotifications").Select(DatabaseConvertions.BotNotify);

		public static void UpdateBotData(Data data)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.SetBotData", data.Uptime);

        public static IEnumerable<Tuple<ulong, string>> GetUsersAcolytes()
            => SqlHelper.ExecuteReader(ConnectionString, "dbo.GetUsersAcolytes").Select(DatabaseConvertions.UserAcolytes);

        public static IEnumerable<UserData> GetUserData()
			=> SqlHelper.ExecuteReader(ConnectionString, "dbo.GetUserInfo").Select(DatabaseConvertions.UserData);

		public static IEnumerable<Tuple<ulong, Game>> GetUserGames()
			=> SqlHelper.ExecuteReader(ConnectionString, "dbo.GetUserGames").Select(DatabaseConvertions.UserGames);

		public static void AddUserGame(ulong id, Game game)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.AddUserGame", id, game.Name, game.StreamUrl, game.StreamType);

		public static void UpdateUserPreferences(UserData data)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.UpdateUserPreferences",
				data.ID,
				data.Preferences.Track,
				data.Preferences.AFKMove,
				data.Preferences.AutoMove);

		public static void UpdateUserSubs(UserData data)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.UpdateUserSubs",
				data.ID,
				data.Preferences.Subscriptions.W_Sortie,
				data.Preferences.Subscriptions.W_Alerts,
				data.Preferences.Subscriptions.W_Invasions,
                data.Preferences.Subscriptions.W_Baro,
                data.Preferences.Subscriptions.W_Acolytes);

		public static void UpdateUserTracking(UserData data)
			=> SqlHelper.ExecuteScalar(ConnectionString, "dbo.UpdateUserTracking",
				data.ID,
				data.Tracking.MessagesSent,
				data.Tracking.CharactersSent,
				data.Tracking.CommandsRequested,
				data.Tracking.ExileTime,
				data.Tracking.TimesMoved,
				data.Tracking.LastOnline.If(x => x.Year <= 1753, new DateTime(1753, 1, 1)),
				data.Tracking.OnlineSince.If(x => x.Year <= 1753, new DateTime(1753, 1, 1)));

		#region Other

		public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
		{
			while (reader.Read())
				yield return projection(reader);
		}

		public static DataTable ToDataTable<T>(this IEnumerable<T> list)
		{
			var data = list.ToList();

			PropertyDescriptorCollection props =
				 TypeDescriptor.GetProperties(typeof(T));
			DataTable table = new DataTable();
			for (int i = 0; i < props.Count; i++)
			{
				PropertyDescriptor prop = props[i];
				table.Columns.Add(prop.Name, prop.PropertyType);
			}
			object[] values = new object[props.Count];
			foreach (T item in data)
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = props[i].GetValue(item);
				}
				table.Rows.Add(values);
			}
			return table;
		}

		public static void AddNotification(string id)
		{
			Global.Data.NotifiedItems.Add(id);
			SqlHelper.ExecuteScalar(ConnectionString, "dbo.AddNotify", id, DateTime.Now);
		}

		public static void RemoveNotification(string id)
		{
			Global.Data.NotifiedItems.Remove(id);
			SqlHelper.ExecuteScalar(ConnectionString, "dbo.RemoveNotify", id);
        }

        public static void AddUserAcolyte(ulong id1, string id2)
            => SqlHelper.ExecuteScalar(ConnectionString, "dbo.AddUserAcolyte", id1, id2);

        public static void RemoveUserAcolyte(ulong id1, string id2)
            => SqlHelper.ExecuteScalar(ConnectionString, "dbo.RemoveUserAcolyte", id1, id2);

        #endregion
    }
}
