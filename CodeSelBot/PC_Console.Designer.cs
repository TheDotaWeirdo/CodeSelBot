namespace CodeSelBot
{
	partial class PC_Console
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ConsoleBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// ConsoleBox
			// 
			this.ConsoleBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
			this.ConsoleBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ConsoleBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ConsoleBox.Font = new System.Drawing.Font("Consolas", 8.5F);
			this.ConsoleBox.Location = new System.Drawing.Point(5, 30);
			this.ConsoleBox.Margin = new System.Windows.Forms.Padding(7);
			this.ConsoleBox.MaxLength = 999999;
			this.ConsoleBox.Multiline = true;
			this.ConsoleBox.Name = "ConsoleBox";
			this.ConsoleBox.ReadOnly = true;
			this.ConsoleBox.Size = new System.Drawing.Size(773, 403);
			this.ConsoleBox.TabIndex = 13;
			this.ConsoleBox.WordWrap = false;
			// 
			// PC_Console
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.ConsoleBox);
			this.Name = "PC_Console";
			this.Text = "Console";
			this.Controls.SetChildIndex(this.ConsoleBox, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox ConsoleBox;
	}
}
