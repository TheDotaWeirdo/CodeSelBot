using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Extensions;
using Newtonsoft.Json;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord.Classes
{
	public class Data
	{
		private const string USER_JSON = "./Data/Users.json";
		private const string GENERAL_JSON = "./Data/General.json";

		private const string USER_BACKUP_JSON = "./Data/Users_Backup.json";
		private const string GENERAL_BACKUP_JSON = "./Data/General_Backup.json";

		public ulong Uptime = 0;

		public List<UserData> UserData = new List<UserData>();
		public List<string> NotifiedItems = new List<string>();

		public void SaveUsers(IEnumerable<IGuildUser> users = null)
		{
			var userDats = UserData.Where(x => users == null || users.Any(y => y.Id == x.ID));

			foreach (var item in userDats)
				SQLHandler.UpdateUserTracking(item);
		}

		public void SaveUser(SocketUser author)
			=> SQLHandler.UpdateUserTracking(UserData.FirstThat(x => author.Id == x.ID));

		public void SaveGeneral()
			=> SQLHandler.UpdateBotData(this);
	}
}
