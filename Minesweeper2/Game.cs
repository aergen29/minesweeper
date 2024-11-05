using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Minesweeper2
{
    internal class Game
    {
        private string username;
        private int clickCount;
        private int size;
        private int mineCount;
        private int buttonWidth;
        private int buttonHeight;
        private Panel gridPanel;
        private Label timeLabel;
        private MineButtons buttons;
        private int maxClicked;
        private int score;
        private Form2 form;
        private int openedFieldCount;
        private bool isGameFinished;
        private System.Threading.Timer timer;
        private int second;
        private Grid grid;
        private Label clickLabel;
        private Scoreboard scoreboard;

        public Game(string username, int size, int mineCount, Panel gridPanel, Label timeLabel, Label clickLabel, Form2 form)
        {
            this.username = username;
            this.size = size;
            this.mineCount = mineCount;
            this.gridPanel = gridPanel;
            this.timeLabel = timeLabel;
            this.clickLabel = clickLabel;
            this.form = form;
            this.controlWidthHeight();
            this.clickCount = 0;
            this.buttons = new MineButtons(this.size, this.mineCount, this.buttonWidth, this.buttonHeight, this);
            this.maxClicked = size * size - mineCount;
            this.prepareButtons();
            this.renderGame();
            this.openedFieldCount = 0;
            this.isGameFinished = false;
            this.second = 0;
            timer = new System.Threading.Timer(this.updateTimer, null, 0, 1000);
        }

        public void quickClose()
        {
            this.StopTimer();
            this.grid.clearPanel();
        }

        private void prepareButtons()
        {
            this.buttons.prepare();
        }

        private void updateTimer(object state)
        {
            this.second++;
            updateTimeLabel();
        }
        private void updateTimeLabel()
        {
            this.timeLabel.Invoke(new Action(() => this.timeLabel.Text = getTimeForWrite()));
        }

        private string getTimeForWrite()
        {
            int hour, minute, second;
            minute = (this.second / 60) % (60 * 60);
            hour = this.second / (60 * 60);
            second = this.second % 60;
            string hourString = hour != 0 ? (hour < 10 ? "0" + hour + ":" : hour + ":") : "";
            string minuteString = (minute != 0 || hour != 0) ? (minute < 10 ? "0" + minute + ":" : minute + ":") : "";
            string secondString = second < 10 ? "0" + second : second + "";
            return hourString + minuteString + secondString;
        }

        public void renderGame()
        {
            this.grid = new Grid(this.buttons, this.gridPanel, this.form);
            grid.renderButtons();
            updatePanelandFormSizes();
        }

        public void FieldClicked(bool count)
        {
            if (count)
            {
                this.clickCount++;
                this.clickLabel.Invoke(new Action(() => this.clickLabel.Text = this.clickCount.ToString()));
            }
            this.openedFieldCount++;
            this.gameFinishedControl();
        }
        public void mineClicked()
        {
            gameFinished(false);
        }

        private void gameFinishedControl()
        {
            if (this.openedFieldCount == this.maxClicked)
            {
                gameFinished(true);
            }
        }
        private void gameFinished(bool which)
        {
            string stat = "";
            this.StopTimer();
            if (!which)
            {
                int flaggedCount = this.buttons.mineButtonsFlagCounter();
                this.buttons.openMines();
                this.score = (int)((flaggedCount / (double)this.second) * 1000);
                stat = "Kaybettiniz!";
            }
            else
            {
                this.buttons.addFlagForEnd();
                this.score = (int)((this.mineCount / (double)this.second) * 1000);
                stat = "Tebrikler!";
            }
            this.isGameFinished = true;
            this.scoreboard = new Scoreboard(this.username, this.score);
            this.scoreboard.add();
            MessageBox.Show(stat+"\n"+"Kullanıcı adı: " + this.username + "  Skor: " + this.score);
        }
        private void controlWidthHeight()
        {
            int w = 0;
            if (this.size <= 15) w = 45;
            else if (this.size <= 20) w = 40;
            else if (this.size <= 25) w = 35;
            else w = 30;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height - 180;
            if (w * this.size > screenHeight)
            {
                w = screenHeight / this.size;
            }
            this.buttonWidth = w;
            this.buttonHeight = w;
        }

        private void updatePanelandFormSizes()
        {
            int width = (this.buttonWidth + 1) * this.size;
            form.updateFormMinimumSize(width);
            this.grid.UpdatePanelSize(width);
        }
        public bool getIsGameFinished() { return this.isGameFinished; }

        private void StopTimer()
        {
            this.timer.Dispose();
        }
        public int getMinedCount() { return this.mineCount; }

    }
}
