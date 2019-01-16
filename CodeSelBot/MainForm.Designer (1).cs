namespace CodeSelBot
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.TLP_Console = new System.Windows.Forms.TableLayoutPanel();
			this.ConsoleBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.P_Spacer_1 = new System.Windows.Forms.Panel();
			this.TLP_SideBar = new System.Windows.Forms.TableLayoutPanel();
			this.L_Exit = new SlickControls.Controls.SlickLabel();
			this.L_Message = new SlickControls.Controls.SlickLabel();
			this.L_Disconnect = new SlickControls.Controls.SlickLabel();
			this.L_Reconnect = new SlickControls.Controls.SlickLabel();
			this.L_ClearConsole = new SlickControls.Controls.SlickLabel();
			this.L_AutoMove = new SlickControls.Controls.SlickLabel();
			this.base_P_Content.SuspendLayout();
			this.TLP_Main.SuspendLayout();
			this.TLP_Console.SuspendLayout();
			this.TLP_SideBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.Controls.Add(this.TLP_Main);
			this.base_P_Content.Location = new System.Drawing.Point(1, 73);
			this.base_P_Content.Size = new System.Drawing.Size(781, 359);
			// 
			// base_P_Controls
			// 
			this.base_P_Controls.Size = new System.Drawing.Size(781, 45);
			// 
			// base_P_Top_Spacer
			// 
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(781, 2);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 3;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Controls.Add(this.TLP_Console, 2, 0);
			this.TLP_Main.Controls.Add(this.P_Spacer_1, 1, 0);
			this.TLP_Main.Controls.Add(this.TLP_SideBar, 0, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 1;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Size = new System.Drawing.Size(781, 359);
			this.TLP_Main.TabIndex = 0;
			// 
			// TLP_Console
			// 
			this.TLP_Console.ColumnCount = 1;
			this.TLP_Console.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Console.Controls.Add(this.ConsoleBox, 0, 1);
			this.TLP_Console.Controls.Add(this.label1, 0, 0);
			this.TLP_Console.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Console.Location = new System.Drawing.Point(202, 0);
			this.TLP_Console.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Console.Name = "TLP_Console";
			this.TLP_Console.RowCount = 2;
			this.TLP_Console.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Console.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Console.Size = new System.Drawing.Size(579, 359);
			this.TLP_Console.TabIndex = 3;
			// 
			// ConsoleBox
			// 
			this.ConsoleBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
			this.ConsoleBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ConsoleBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ConsoleBox.Font = new System.Drawing.Font("Consolas", 8.5F);
			this.ConsoleBox.Location = new System.Drawing.Point(7, 28);
			this.ConsoleBox.Margin = new System.Windows.Forms.Padding(7);
			this.ConsoleBox.MaxLength = 999999;
			this.ConsoleBox.Multiline = true;
			this.ConsoleBox.Name = "ConsoleBox";
			this.ConsoleBox.ReadOnly = true;
			this.ConsoleBox.Size = new System.Drawing.Size(565, 324);
			this.ConsoleBox.TabIndex = 0;
			this.ConsoleBox.WordWrap = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 5);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Console";
			// 
			// P_Spacer_1
			// 
			this.P_Spacer_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Spacer_1.Location = new System.Drawing.Point(200, 0);
			this.P_Spacer_1.Margin = new System.Windows.Forms.Padding(0);
			this.P_Spacer_1.Name = "P_Spacer_1";
			this.P_Spacer_1.Size = new System.Drawing.Size(2, 359);
			this.P_Spacer_1.TabIndex = 4;
			// 
			// TLP_SideBar
			// 
			this.TLP_SideBar.ColumnCount = 1;
			this.TLP_SideBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_SideBar.Controls.Add(this.L_Exit, 0, 6);
			this.TLP_SideBar.Controls.Add(this.L_Message, 0, 3);
			this.TLP_SideBar.Controls.Add(this.L_Disconnect, 0, 1);
			this.TLP_SideBar.Controls.Add(this.L_Reconnect, 0, 0);
			this.TLP_SideBar.Controls.Add(this.L_ClearConsole, 0, 4);
			this.TLP_SideBar.Controls.Add(this.L_AutoMove, 0, 2);
			this.TLP_SideBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_SideBar.Location = new System.Drawing.Point(0, 0);
			this.TLP_SideBar.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_SideBar.Name = "TLP_SideBar";
			this.TLP_SideBar.RowCount = 7;
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_SideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_SideBar.Size = new System.Drawing.Size(200, 359);
			this.TLP_SideBar.TabIndex = 5;
			// 
			// L_Exit
			// 
			this.L_Exit.ActiveColor = null;
			this.L_Exit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Exit.AutoSize = true;
			this.L_Exit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_Exit.Center = false;
			this.L_Exit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_Exit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_Exit.HideText = false;
			this.L_Exit.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_Exit.IconSize = 16;
			this.L_Exit.Image = global::CodeSelBot.Properties.Resources.Icon_Logout;
			this.L_Exit.Location = new System.Drawing.Point(15, 327);
			this.L_Exit.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_Exit.Name = "L_Exit";
			this.L_Exit.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_Exit.Size = new System.Drawing.Size(66, 27);
			this.L_Exit.TabIndex = 3;
			this.L_Exit.Text = "Exit";
			// 
			// L_Message
			// 
			this.L_Message.ActiveColor = null;
			this.L_Message.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Message.AutoSize = true;
			this.L_Message.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_Message.Center = false;
			this.L_Message.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_Message.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_Message.HideText = false;
			this.L_Message.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_Message.IconSize = 16;
			this.L_Message.Image = global::CodeSelBot.Properties.Resources.Icon_Message;
			this.L_Message.Location = new System.Drawing.Point(15, 116);
			this.L_Message.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_Message.Name = "L_Message";
			this.L_Message.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_Message.Size = new System.Drawing.Size(102, 27);
			this.L_Message.TabIndex = 2;
			this.L_Message.Text = "Message";
			// 
			// L_Disconnect
			// 
			this.L_Disconnect.ActiveColor = null;
			this.L_Disconnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Disconnect.AutoSize = true;
			this.L_Disconnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_Disconnect.Center = false;
			this.L_Disconnect.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_Disconnect.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_Disconnect.HideText = false;
			this.L_Disconnect.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_Disconnect.IconSize = 16;
			this.L_Disconnect.Image = global::CodeSelBot.Properties.Resources.Icon_Offline;
			this.L_Disconnect.Location = new System.Drawing.Point(15, 42);
			this.L_Disconnect.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_Disconnect.Name = "L_Disconnect";
			this.L_Disconnect.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_Disconnect.Size = new System.Drawing.Size(117, 27);
			this.L_Disconnect.TabIndex = 1;
			this.L_Disconnect.Text = "Disconnect";
			// 
			// L_Reconnect
			// 
			this.L_Reconnect.ActiveColor = null;
			this.L_Reconnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_Reconnect.AutoSize = true;
			this.L_Reconnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_Reconnect.Center = true;
			this.L_Reconnect.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_Reconnect.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_Reconnect.HideText = false;
			this.L_Reconnect.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_Reconnect.IconSize = 16;
			this.L_Reconnect.Image = global::CodeSelBot.Properties.Resources.Icon_Online;
			this.L_Reconnect.Location = new System.Drawing.Point(15, 5);
			this.L_Reconnect.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_Reconnect.Name = "L_Reconnect";
			this.L_Reconnect.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_Reconnect.Size = new System.Drawing.Size(115, 27);
			this.L_Reconnect.TabIndex = 0;
			this.L_Reconnect.Text = "Reconnect";
			// 
			// L_ClearConsole
			// 
			this.L_ClearConsole.ActiveColor = null;
			this.L_ClearConsole.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_ClearConsole.AutoSize = true;
			this.L_ClearConsole.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_ClearConsole.Center = false;
			this.L_ClearConsole.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_ClearConsole.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_ClearConsole.HideText = false;
			this.L_ClearConsole.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_ClearConsole.IconSize = 16;
			this.L_ClearConsole.Image = global::CodeSelBot.Properties.Resources.Icon_Console;
			this.L_ClearConsole.Location = new System.Drawing.Point(15, 153);
			this.L_ClearConsole.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_ClearConsole.Name = "L_ClearConsole";
			this.L_ClearConsole.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_ClearConsole.Size = new System.Drawing.Size(136, 27);
			this.L_ClearConsole.TabIndex = 2;
			this.L_ClearConsole.Text = "Clear Console";
			// 
			// L_AutoMove
			// 
			this.L_AutoMove.ActiveColor = null;
			this.L_AutoMove.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_AutoMove.AutoSize = true;
			this.L_AutoMove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.L_AutoMove.Center = false;
			this.L_AutoMove.Cursor = System.Windows.Forms.Cursors.Hand;
			this.L_AutoMove.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
			this.L_AutoMove.HideText = false;
			this.L_AutoMove.HoverState = SlickControls.Enums.HoverState.Normal;
			this.L_AutoMove.IconSize = 16;
			this.L_AutoMove.Image = global::CodeSelBot.Properties.Resources.Icon_Move;
			this.L_AutoMove.Location = new System.Drawing.Point(15, 79);
			this.L_AutoMove.Margin = new System.Windows.Forms.Padding(15, 5, 3, 5);
			this.L_AutoMove.Name = "L_AutoMove";
			this.L_AutoMove.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.L_AutoMove.Size = new System.Drawing.Size(117, 27);
			this.L_AutoMove.TabIndex = 2;
			this.L_AutoMove.Text = "Auto-Move";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BarIcon = global::CodeSelBot.Properties.Resources.Code_SEL_Reborn_Emoji_2_0;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.FormIcon = global::CodeSelBot.Properties.Resources.Code_SEL_Reborn_Emoji_2_0;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Code.Sel Bot";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.base_P_Content.ResumeLayout(false);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Console.ResumeLayout(false);
			this.TLP_Console.PerformLayout();
			this.TLP_SideBar.ResumeLayout(false);
			this.TLP_SideBar.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		private System.Windows.Forms.TextBox ConsoleBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel TLP_Console;
		private System.Windows.Forms.Panel P_Spacer_1;
        private SlickControls.Controls.SlickLabel L_Reconnect;
        private SlickControls.Controls.SlickLabel L_Message;
        private SlickControls.Controls.SlickLabel L_Disconnect;
        private SlickControls.Controls.SlickLabel L_Exit;
		private SlickControls.Controls.SlickLabel L_ClearConsole;
		private System.Windows.Forms.TableLayoutPanel TLP_SideBar;
		private SlickControls.Controls.SlickLabel L_AutoMove;
	}
}

