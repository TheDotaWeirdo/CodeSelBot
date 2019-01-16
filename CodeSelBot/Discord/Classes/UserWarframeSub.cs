using Discord;

namespace CodeSelBot.Discord
{
	public class UserWarframeSub
	{
		public ulong UserId;

		public string Type;

		public string Param;

		public UserWarframeSub(ulong userId, string type, string param)
		{
			UserId = userId;
			Type = type;
			Param = param;
		}

		public UserWarframeSub() { }
	}
}
