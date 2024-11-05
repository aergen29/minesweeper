using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.PropertyGridInternal;


namespace Minesweeper2
{
    internal class MineButtons
    {
        private MineButton[,] buttons;
        private MineButton[] hasMineButtons;
        private int size;
        private int mineCount;
        private int buttonWidth;
        private int buttonHeight;
        private Game game;


        public MineButtons(int size, int mineCount,int buttonWidth,int buttonHeight,Game game)
        {
            this.buttonWidth = buttonWidth;
            this.buttonHeight = buttonHeight;
            this.size = size;
            this.mineCount = mineCount;
            this.buttons = new MineButton[size, size];
            this.hasMineButtons = new MineButton[mineCount];
            this.game = game;
        }

        public void openMines()
        {
            for (int i = 0; i < this.hasMineButtons.Length; i++)
            {
                this.hasMineButtons[i].openMine();
            }
        }

        public void prepare()
        {
            this.createButtons();
            this.updateNeighboors();
            this.updateMineCounts();
        }

        public void createButtons()
        {
            int[,] mineCoords = this.createMineCoordinates();
            int size = this.buttons.GetLength(0);
            int hasMineCounter = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {                    
                    if(ArrayUtils.contains(mineCoords, new int[] { i, j }))
                    {
                        this.hasMineButtons[hasMineCounter] = new MineButton(i, j,this.buttonWidth,this.buttonHeight,true, this.game);
                        this.buttons[i, j] = this.hasMineButtons[hasMineCounter];
                        hasMineCounter++;
                        continue;
                    }
                    else
                    {
                        this.buttons[i, j] = new MineButton(i, j, this.buttonWidth, this.buttonHeight, false, this.game);
                    }
                }
            }
        }
        private void updateMineCounts()
        {
            for (int i = 0; i < this.hasMineButtons.Length; i++)
            {
                this.hasMineButtons[i].neighboorsMineCountUpdate();
            }
        }

        private void updateNeighboors()
        {
            for(int i = 0;  i < this.buttons.GetLength(0); i++)
            {
                for(int j = 0; j < this.buttons.GetLength(1); j++)
                {
                    this.updateNeighboor(i, j);
                }
            }
        }
        private void updateNeighboor(int x, int y)
        {
            int[,] arr = { { x - 1, y - 1 }, { x - 1, y }, { x - 1, y + 1 },
                           { x, y - 1 }, { x, y + 1 },
                           { x + 1, y - 1 }, { x + 1, y }, { x + 1, y + 1 },};
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                if (arr[i,0] < 0 || arr[i, 1] < 0 || arr[i, 0] >= this.size || arr[i, 1] >= this.size) continue;
                int tempX = arr[i,0];
                int tempY = arr[i,1];
                this.buttons[x, y].addNeighboor(this.buttons[tempX, tempY]);
            }
        }

        private int[,] createMineCoordinates()
        {
            int count = this.hasMineButtons.Length;
            int size = this.buttons.GetLength(0);
            int[,] result = new int[count, 2];
            for (int i = 0; i < count; i++) 
            {
                Random random = new Random();
                int x = random.Next(size);
                int y = random.Next(size);  
                while(ArrayUtils.contains(result,new int[] { x, y }))
                {
                    x = random.Next(size);
                    y = random.Next(size);
                }
                result[i,0] = x; 
                result[i,1] = y;
            }
            return result;
        }

        public int mineButtonsFlagCounter()
        {
            int counter = 0;
            for (int i = 0; i < this.hasMineButtons.Length; i++)
            {
                counter = this.hasMineButtons[i].getStatus() == 1 ? counter + 1 : counter;
            }
            return counter;
        }
        public MineButton[,] getMineButtons()
        {
            return this.buttons;
        }
        public void addFlagForEnd()
        {
            foreach(MineButton hasMineButton in this.hasMineButtons) 
            {
                hasMineButton.addFlag();
            }
        } 
    }
    internal class MineButton : Button
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private bool hasMine;
        private int mineCount;
        private int status;
        private MineButton[] neighboors;
        private Game game;
        public Image ButtonImage = null;
        private static int flaggedCount;

        public MineButton(int x, int y, int width, int height, bool hasMine, Game game)
        {
            flaggedCount = 0;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.hasMine = hasMine;
            this.status = 0;
            this.mineCount = 0;
            this.neighboors = new MineButton[0];
            this.game = game;
            this.Click += new EventHandler(FieldClick);
            this.MouseDown += new MouseEventHandler(FieldRightClick);
            this.setLocationAndSize();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (this.ButtonImage != null)
            {
                pevent.Graphics.DrawImage(this.ButtonImage, 0, 0, this.width, this.height);
            }
        }

        public void setLocationAndSize()
        {
            this.Location = new Point(this.x * (this.width + 1), this.y * (this.height + 1));
            this.Size = new Size(this.width, this.height);
        }


        public int getStatus()
        {
            return this.status;
        }

        public void setMineCount()
        {
            this.mineCount++;
        }

        public void addNeighboor(MineButton neighboor)
        {
            this.neighboors = ArrayUtils.add(this.neighboors, neighboor);
        }
        public void neighboorsMineCountUpdate()
        {
            int len = this.neighboors.Length;
            for (int i = 0; i < len; i++) 
            {
                this.neighboors[i].setMineCount();
            }
        }
        public void FieldRightClick(object sender, MouseEventArgs e)
        {
            if (game.getIsGameFinished()) return;
            if (e.Button == MouseButtons.Right)
            {
                if (this.status == 0 && flaggedCount < this.game.getMinedCount())
                {
                    this.addFlag();
                    this.status = 1;
                    flaggedCount++;
                }
                else if(this.status == 1)
                {
                    this.ButtonImage = null;
                    this.Invalidate();
                    this.status = 0;
                    flaggedCount--;
                }
            }
        }

        public void addFlag()
        {
            this.ButtonImage = getImages(true);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.Text = "";
            this.Invalidate();
        }

        public void FieldClick(object sender, EventArgs e)
        {
            if (game.getIsGameFinished()) return;
            if (this.status == 0)
            {
                if (this.hasMine)
                {
                    game.mineClicked();
                    if ((MineButton)sender == this) game.FieldClicked(true);
                    return;
                }
                this.status = 2;
                this.Text = this.mineCount == 0? "":this.mineCount.ToString();
                this.ForeColor = this.mineCount <= 1 ? Color.Green : this.mineCount == 2 ? Color.LightBlue : this.mineCount == 3 ? Color.MediumPurple : Color.Red;
                //this.Enabled = false;
                this.BackColor = Color.RosyBrown;
                this.Font = new Font(this.Font, FontStyle.Bold);
                if ((MineButton)sender == this) game.FieldClicked(true);
                else game.FieldClicked(false);
                if (this.mineCount == 0)
                {
                    clickNeighboors(sender, e);
                }
            }
        }
        private void clickNeighboors(object sender, EventArgs e)
        {
            for (int i = 0; i < this.neighboors.Length; i++)
            {
                this.neighboors[i].FieldClick(sender, e);
            }
        }
        public void openMine()
        {
            //this.Enabled = false;
            this.ButtonImage = getImages(false);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.Text = "";
            this.Invalidate();
        }
        private Bitmap getImages(bool which)
        {
            return which?Properties.Resources.flag.ToBitmap(): Properties.Resources.mine.ToBitmap();
        }
    }
}

//button.ButtonImage = button.getButtonImage() != null ? null : Properties.Resources.flag;
