using Discord;

namespace CodeSelBot.Discord
{
	public class UserData
	{
		public ulong ID;

		public IUser User => Global.Client.GetUser(ID);

		public UserTracking Tracking;

		public UserPreferences Preferences;

		public UserVolatile Session = new UserVolatile();

		public UserWarframe WarframeData = new UserWarframe();

		public UserData(ulong iD)
		{
			ID = iD;
			Tracking = new UserTracking();
			Preferences = new UserPreferences();
		}

		public UserData() { }
	}
}
