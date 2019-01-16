using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Extensions;

namespace CodeSelBot.Discord.Modules
{
	public class Info : ModuleBase<SocketCommandContext>
	{
		[Command("info"), Summary("Gets info about various things in the server")]
		public async Task InfoAsync([Remainder]string context)
		{
			if (context == "rythm")
				await InfoRythm();
		}

		private async Task InfoRythm()
		{
			var embed = new EmbedBuilder();

			embed
				.WithColor(255, 25, 25)
				.WithFooter($"Requested by {Context.User.Username}")
				.WithCurrentTimestamp()
				.WithTitle("Rythm Info")
				.AddField("play", "Plays a song with the given name or url.")
				.AddField("disconnect", "Disconnect the bot from the voice channel it is in.")
				.AddField("np", "Shows what song the bot is currently playing.")
				.AddField("skip", "Skips the currently playing song.")
				.AddField("seek", "Seeks to a certain point in the current track.")
				.AddField("remove", "Removes a certain entry from the queue.")
				.AddField("loopqueue", "Loops the whole queue.")
				.AddField("loop", "Loop the currently playing song.")
				.AddField("join", "Summons the bot to your voice channel.")
				.AddField("lyrics", "Gets the lyrics of the current playing song")
				.AddField("resume", "Resume paused music.")
				.AddField("settings", "Change Rythm's settings.")
				.AddField("move", "Moves a certain song to the first position in the queue or to a chosen position")
				.AddField("forward", "Forwards by a certain amount in the current track.")
				.AddField("skipto", "Skips to a certain position in the queue.")
				.AddField("clear", "Clears the queue.")
				.AddField("replay", "Reset the progress of the current song")
				.AddField("pause", "Pauses the currently playing track.")
				.AddField("removedupes", "Removes duplicate songs from the queue.")
				.AddField("rewind", "Rewinds by a certain amount in the current track.")
				.AddField("playtop", "Like the play command, but queues from the top.")
				.AddField("playskip", "Adds a song to the top of the queue then skips to it.")
				.AddField("shuffle", "Shuffles the queue.")
				.AddField("queue", "View the queue. To view different pages, type the command with the specified page number after it (queue 2).")
				.AddField("leavecleanup", "Removes absent user's songs from the Queue.");

			await ReplyAsync(string.Empty, false, embed.Build());
		}
	}
}
