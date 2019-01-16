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
			this.PI_Reconnect = new SlickControls.Panels.PanelItem();
			this.PI_Disconnect = new SlickControls.Panels.PanelItem();
			this.PI_AutoMove = new SlickControls.Panels.PanelItem();
			this.PI_Message = new SlickControls.Panels.PanelItem();
			this.PI_Clear = new SlickControls.Panels.PanelItem();
			this.PI_ShutDown = new SlickControls.Panels.PanelItem();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.base_P_Content.Size = new System.Drawing.Size(781, 431);
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.Location = new System.Drawing.Point(0, 309);
			// 
			// PI_Reconnect
			// 
			this.PI_Reconnect.ForceReopen = false;
			this.PI_Reconnect.Group = "Connection";
			this.PI_Reconnect.Icon = global::CodeSelBot.Properties.Resources.Tiny_Connect;
			this.PI_Reconnect.Selected = false;
			this.PI_Reconnect.Text = "Reconnect";
			// 
			// PI_Disconnect
			// 
			this.PI_Disconnect.ForceReopen = false;
			this.PI_Disconnect.Group = "Connection";
			this.PI_Disconnect.Icon = global::CodeSelBot.Properties.Resources.Tiny_Offline;
			this.PI_Disconnect.Selected = false;
			this.PI_Disconnect.Text = "Disconnect";
			// 
			// PI_AutoMove
			// 
			this.PI_AutoMove.ForceReopen = false;
			this.PI_AutoMove.Group = "Functionality";
			this.PI_AutoMove.Icon = global::CodeSelBot.Properties.Resources.Tiny_Move;
			this.PI_AutoMove.Selected = false;
			this.PI_AutoMove.Text = "Auto-Move";
			// 
			// PI_Message
			// 
			this.PI_Message.ForceReopen = false;
			this.PI_Message.Group = "Functionality";
			this.PI_Message.Icon = global::CodeSelBot.Properties.Resources.Tiny_Message;
			this.PI_Message.Selected = false;
			this.PI_Message.Text = "Message";
			// 
			// PI_Clear
			// 
			this.PI_Clear.ForceReopen = false;
			this.PI_Clear.Group = "Functionality";
			this.PI_Clear.Icon = global::CodeSelBot.Properties.Resources.Tiny_Console;
			this.PI_Clear.Selected = false;
			this.PI_Clear.Text = "Clear Console";
			// 
			// PI_ShutDown
			// 
			this.PI_ShutDown.ForceReopen = false;
			this.PI_ShutDown.Group = "ShutDown";
			this.PI_ShutDown.Icon = global::CodeSelBot.Properties.Resources.Tiny_Logout;
			this.PI_ShutDown.Selected = false;
			this.PI_ShutDown.Text = "Shut Down";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.FormIcon = global::CodeSelBot.Properties.Resources.Code_SEL_Reborn_Emoji_2_0;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.Name = "MainForm";
			this.SidebarItems = new SlickControls.Panels.PanelItem[] {
        this.PI_Reconnect,
        this.PI_Disconnect,
        this.PI_AutoMove,
        this.PI_Message,
        this.PI_Clear,
        this.PI_ShutDown};
			this.Text = "Code.Sel Bot";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.ResumeLayout(false);

		}

		#endregion

		private SlickControls.Panels.PanelItem PI_Reconnect;
		private SlickControls.Panels.PanelItem PI_Disconnect;
		private SlickControls.Panels.PanelItem PI_AutoMove;
		private SlickControls.Panels.PanelItem PI_Message;
		private SlickControls.Panels.PanelItem PI_Clear;
		private SlickControls.Panels.PanelItem PI_ShutDown;
	}
}

