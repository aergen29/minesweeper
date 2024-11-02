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
    public partial class Form2 : Form
    {
        private int minMine = 0;
        private int maxMine = 0;
        private int size = 0;
        private int mineCount = 0;
        private string username;
        Game game;
        Form1 form1;

        public Form2(string username, Form1 form1)
        {
            InitializeComponent();
            updateFormMinimumSize(0);
            this.username = username;
            label4.Text = this.username;
            this.form1 = form1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.game != null) this.game.quickClose();
            this.game = null;
            this.form1.setIsForm2Opened(false, sender, e);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool sizeC = sizeControl(textBox1.Text);
            if (sizeC)
            {
                this.textBox2_TextChanged(sender, e);
                mineMinMaxControl();
                textBox2.Text = textBox2.Text == "" ? this.minMine.ToString() : textBox2.Text;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            mineSizeControl(textBox2.Text);
        }
        private void writeWarning(string text)
        {
            label3.Text = text;
        }

        private void setButtonEnabled(bool status)
        {
            button1.Enabled = status;
        }

        private bool sizeControl(string text)
        {
            int a;
            bool success = int.TryParse(text, out a);
            if (!success)
            {
                writeWarning("Büyüklük bir sayı olmalıdır!");
                setButtonEnabled(false);
                return false;
            }
            else
            {
                if (a < 10 || a > 30)
                {
                    writeWarning("Büyüklük 10 ile 30 arasında bir değer almalıdır!");
                    setButtonEnabled(false);
                    return false;
                }
                else
                {
                    this.size = a;
                    writeWarning("");
                    setButtonEnabled(true);
                    mineMinMaxControl();
                    return true;
                }
            }
        }

        private void mineSizeControl(string text)
        {
            int a;
            bool success = int.TryParse(text, out a);
            if (!success)
            {
                writeWarning("Mayın sayısı bir sayı olmalıdır!");
                setButtonEnabled(false);
            }
            else
            {
                if (a < this.minMine || a > this.maxMine)
                {
                    writeWarning("Mayın Sayısı " + this.minMine + " ile " + this.maxMine + " arasında olmalıdır");
                    setButtonEnabled(false);
                }
                else
                {
                    writeWarning("");
                    setButtonEnabled(true);
                    this.mineCount = a;
                }
            }
        }
        private void mineMinMaxControl()
        {
            if (this.size == 0)
            {
                this.minMine = 0;
                this.maxMine = 0;
            }
            else
            {
                if (this.size < 10 || this.size > 30)
                {
                    writeWarning("Büyüklük 10 ile 30 arasında bir değer almalıdır!");
                }
                else
                {
                    //if (this.size <= 15)
                    //{
                    //    this.minMine = 10;
                    //    this.maxMine = 30;
                    //}
                    //else if (this.size <= 20)
                    //{
                    //    this.minMine = 30;
                    //    this.maxMine = 60;
                    //}
                    //else if (this.size <= 25)
                    //{
                    //    this.minMine = 60;
                    //    this.maxMine = 100;
                    //}
                    //else if (this.size <= 30)
                    //{
                    //    this.minMine = 100;
                    //    this.maxMine = 140;
                    //}
                    this.minMine = this.size * this.size / 10;
                    this.maxMine = this.minMine * 3;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.game != null)
            {
                this.game.quickClose();
                this.game = null;
            }

            this.game = new Game(this.username, this.size, this.mineCount, panel2, label5, label6, this);
        }

        public void updateFormMinimumSize(int totalWidth)
        {
            if (totalWidth == 0)
            {
                this.MinimumSize = new Size(700, 400);
                return;
            }
            this.MinimumSize = new Size(totalWidth + 30 < 700 ? 700 : totalWidth + 30, totalWidth + 120 < 400 ? 400 : totalWidth + 120);
        }
    }
}
