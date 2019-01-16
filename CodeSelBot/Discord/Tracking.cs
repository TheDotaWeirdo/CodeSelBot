using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Timers;

namespace CodeSelBot.Discord
{
	public static class Tracking
	{
		internal static void OnUserCommand(SocketUserMessage message)
		{
			message.Author.GetData().Tracking.CommandsRequested++;
			Global.Data.SaveUser(message.Author);
		}

		internal static void OnUserMessage(SocketUserMessage message)
		{
			message.Author.GetData().Tracking.MessagesSent++;
			message.Author.GetData().Tracking.CharactersSent += message.Content.Length;
			Global.Data.SaveUser(message.Author);
		}
	}
}
