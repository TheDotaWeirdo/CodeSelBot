using CodeSelBot.Discord.Classes;
using System.Collections.Generic;

namespace CodeSelBot.Discord
{
	public class UserPreferences
	{
		public bool AutoMove = true;
		public bool AFKMove = true;
		public bool Track = true;

		public UserSubs Subscriptions = new UserSubs();
        public List<string> Acolytes = new List<string>();
	}
}
