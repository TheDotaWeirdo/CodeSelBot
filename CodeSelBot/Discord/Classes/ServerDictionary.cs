using System.Collections.Generic;

namespace CodeSelBot.Discord.Classes
{
	public class ServerDictionary
	{
		public ulong ServerID { get; set; }
		public ulong ServerOwnerID { get; set; }
		public ulong BotID { get; set; }
		public Dictionary<string, ServerRole> Roles;
		public Dictionary<string, ServerChannels> Channels;
	}
}
