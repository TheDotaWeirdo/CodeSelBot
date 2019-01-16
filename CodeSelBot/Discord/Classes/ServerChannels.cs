using Discord;

namespace CodeSelBot.Discord.Classes
{
	public class ServerChannels
	{
		public ulong ID { get; set; }
		public ChannelType Type { get; set; }
		public bool IsGaming { get; set; }
		public ulong? Role { get; set; }
	}
}
