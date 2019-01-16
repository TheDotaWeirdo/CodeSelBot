using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlickControls.Panels;
using Extensions;

namespace CodeSelBot
{
	public partial class PC_Console : PanelContent
	{
		public PC_Console()
		{
			InitializeComponent();
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			ConsoleBox.BackColor = design.BackColor;
			ConsoleBox.ForeColor = design.ForeColor;
		}
	}
}
