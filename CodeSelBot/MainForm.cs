using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeSelBot.Discord;
using Discord;
using Extensions;
using SlickControls.Forms;
using SlickControls.Panels;

namespace CodeSelBot
{
	public partial class MainForm : BasePanelForm
	{
		private TextBox ConsoleBox => (CurrentPanel as PC_Console).ConsoleBox;

		public MainForm()
		{
			InitializeComponent();
			FormDesign.Initialize(this, DesignChanged);

			SetPanel<PC_Console>(PanelItem.Empty);

			Global.ConsoleLines = GetConsole;
			Global.Bot = new Discord.CodeSelBot().RunBot();

			PI_Reconnect.OnClick += (s, e) => (Global.Bot = Global.Bot ?? new Discord.CodeSelBot().RunBot()).Reconnect();
			PI_Disconnect.OnClick += (s, e) => Global.Bot.Disconnect();
			PI_Message.OnClick += (s, e) => SendMsg();
			PI_AutoMove.OnClick += (s, e) => Global.AutoMove = !Global.AutoMove;
			PI_Clear.OnClick += (s, e) => ConsoleBox.Text = string.Empty;
			PI_ShutDown.OnClick += (s, e) => { Global.Bot.Disconnect(); Global.Bot = null; };

			Global.Bot.ConsoleChanged += OnConsoleChanged;
			Location = SystemInformation.VirtualScreen.Center(Size);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Global.Client?.LogoutAsync();

			base.OnFormClosing(e);
		}

		private string[] GetConsole()
			=> ConsoleBox.Text.Split('\n');

		private void OnConsoleChanged(Discord.CodeSelBot bot, ActionType action, object args)
		{
			try
			{
				var log = (LogMessage)args;
				if (log.Source == "Gateway" && log.Message.Length > 1000)
					return;

				if (!IsDisposed && !Disposing)
					Invoke(new Action(() =>
					{
						switch (action)
						{
							case ActionType.Add:
								ConsoleBox.Text += args.ToString() + "\r\n";
								ConsoleBox.SelectionStart = ConsoleBox.TextLength;
								ConsoleBox.ScrollToCaret();
								break;
							case ActionType.Remove:
								break;
							case ActionType.Clear:
								ConsoleBox.Text = string.Empty;
								break;
							default:
								break;
						}
					}));
			}
			catch { }
		}

		private void SendMsg()
		{
			var result = MessagePrompt.ShowInput("Enter the message to send", form: this);

			if (result.DialogResult == DialogResult.OK)
				Global.Client.Guilds.FirstOrDefault()?.DefaultChannel.SendMessageAsync(result.Input);
		}
	}
}
