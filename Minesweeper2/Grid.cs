using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper2
{
    internal class Grid
    {
        private MineButton[,] buttons;
        private Panel panel;
        private Form2 form;

        public Grid(MineButtons buttons, Panel panel, Form2 form)
        {
            this.buttons = buttons.getMineButtons();
            this.panel = panel;
            this.form = form;
        }

        public void renderButtons()
        {
            for (int i = 0; i < this.buttons.GetLength(0); i++) 
            { 
                for(int j = 0; j < this.buttons.GetLength(1); j++)
                {
                    this.panel.Controls.Add(this.buttons[i, j]);
                }
            }
        }
        public void UpdatePanelSize(int totalButtonWidth)
        {
            this.panel.Width = totalButtonWidth;
            this.panel.Height = totalButtonWidth;
            this.panel.Left = (form.ClientSize.Width - this.panel.Width) / 2;
        }
        public void clearPanel()
        {
            this.panel.Controls.Clear();
        }
    }
}
