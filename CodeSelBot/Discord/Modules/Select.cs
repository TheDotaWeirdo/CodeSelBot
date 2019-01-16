using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Extensions;
using Microsoft.ApplicationBlocks.Data;
using SchoolSystemManager.Database;

namespace CodeSelBot.Discord.Modules
{
	public class Select : ModuleBase<SocketCommandContext>
	{
		[Command("select"), Alias("exec"), Summary("Executes an SQL query on the database")]
		public async Task EchoAsync([Remainder]string echo)
		{
			echo = echo.RegexRemove(" go ");
			var typing = Context.Channel.EnterTypingState();
			var dt = new DataTable();
			var sb = new StringBuilder("```");

			dt.Load(SqlHelper.ExecuteReader(SQLHandler.ConnectionString, CommandType.Text, Context.Message.Content.TrimStart('-')));

			var maxLengthes = new int[dt.Columns.Count];
			var ind = 0;

			foreach (DataColumn col in dt.Columns)
				maxLengthes[ind++] = col.ColumnName.Length;

			foreach (var row in dt.Select())
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					if (maxLengthes[i] < row[i].ToString().Length)
						maxLengthes[i] = row[i].ToString().Length;
				}

			ind = 0;
			foreach (DataColumn col in dt.Columns)
			{
				sb.Append(col.ColumnName);
				sb.Append(' ', maxLengthes[ind++] - col.ColumnName.Length + 3);
			}
			sb.AppendLine();
			sb.AppendLine();

			foreach (var row in dt.Select())
			{
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					sb.Append(row[i].ToString());
					sb.Append(' ', maxLengthes[i] - row[i].ToString().Length + 3);
				}
				sb.AppendLine();
			}
			
			await ReplyAsync(sb.ToString().CharLimit(1990) + "```");
			typing.Dispose();
		}
	}
}
