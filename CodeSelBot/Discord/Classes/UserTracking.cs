using System;
using System.Collections.Generic;
using Discord;

namespace CodeSelBot.Discord
{
	public class UserTracking
	{
		public int MessagesSent = 0;
		public int CommandsRequested = 0;
		public int CharactersSent = 0;
		public int TimesMoved = 0;
		public ulong AFKMoveChanId = 0;
		public double ExileTime = 0;

		public DateTime LastOnline;
		public DateTime OnlineSince;

		public List<Game> Games = new List<Game>();
	}
}
