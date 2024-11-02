using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper2
{
    public partial class Form3 : Form
    {
        private Scoreboard scoreboard;
        public Form3()
        {
            InitializeComponent();
            this.scoreboard = new Scoreboard();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Panel panel = this.scoreboard.getForRender();
            panel.Left = 20;
            panel.Top = 100;
            this.Controls.Add(panel);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.github.com/29apo29/minesweeper",
                UseShellExecute = true
            });
        }
    }
}
