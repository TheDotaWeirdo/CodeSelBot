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
	public class CoinFlip : ModuleBase<SocketCommandContext>
	{
		[Command("flip a coin"), Alias("flip coin", "flip", "toss coin", "toss"), Summary("Flips a random coin")]
		public async Task CoinFlipAsync()
		{
			var typing = Context.Channel.EnterTypingState();
			await ReplyAsync(Responces.Random().Replace("#", Context.User.Mention)).WaitForIt();

			System.Threading.Thread.Sleep(500);

			await ReplyAsync(new string[] { $"<:Tails:322829225537306634>  { "tails".ToEmoji()}", $"<:Heads:322829220172529676>  {"heads".ToEmoji()}" }.Random());

			typing.Dispose();
		}

		private string[] Responces = new string[]
		{
			"Flipping a coin for #",
			"Let's see what the future holds for you, #",
			"I'll just put my heads in the tails..",
			"And a one and a two and a flip!",
			"Digital tossing is weird..",
			"Going for a Toss for Mr. #"
		};
	}
}
