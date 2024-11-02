using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper2
{
    public class User
    {
        public string Username { get; set; }
        public int Score { get; set; }
    }
    internal class Scoreboard
    {
        private User[] users;
        private User user;
        private string username;
        private int score;
        private string csv;
        private const string FILE_NAME = "./scoreboard.csv";

        public Scoreboard()
        {
            this.getFromStorage();
        }
        public Scoreboard(string username, int score)
        {
            this.username = username;
            this.score = score;
            this.getFromStorage();
        }

        public void update()
        {
            this.getFromStorage();
        }

        private void getFromStorage()
        {
            try
            {
                this.csv = File.ReadAllText(FILE_NAME, System.Text.Encoding.UTF8);
            }
            catch (Exception ex) 
            {
                this.csv = "null";
            }
            this.ConvertCSVToArray();
        }
        private void ConvertCSVToArray()
        {
            if (this.csv == "null")
            {
                this.users = new User[0];
                return; 
            }
            string[] lines = ArrayUtils.removeLastItem(this.csv.Split(';'));
            User[] users = new User[0];
            foreach (string line in lines) 
            {
                string[] splited = line.Split(',');
                string username = splited[0];
                int score = int.Parse(splited[1]);
                users = ArrayUtils.add(users, new User { Username = username, Score = score });
            }
            this.users =  users;
        }

        public void add()
        {
            int location = this.findLocation();
            if (location == -1) return;
            this.user = new User { Username = this.username, Score = this.score };
            this.users = ArrayUtils.addWithLocation(this.users,user,location);
            this.updateStorage();
        }

        private int findLocation()
        {
            if (this.users.Length  == 0) return 0;
            int i = 0;
            foreach (User user in this.users)
            {
                if(user.Score <= this.score)
                {
                    return i;
                }
                i++;
            }
            if (this.users.Length < 10) return this.users.Length;
            return -1;
        }
        private void updateStorage()
        {
            string csv = "";
            foreach(User user in this.users)
            {
                csv += user.Username + "," + user.Score + ";";
            }
            File.WriteAllText(FILE_NAME,csv);
        }
        public Panel getForRender()
        {
            Panel panel = new Panel();
            if (this.users.Length == 0)
            {
                Label label = new Label();
                label.Text = "Henüz oynanmış oyun yok.";
                panel.Controls.Add(label);
            }
            else
            {
                Panel[] panels = this.getPanelsForRender();
                int top = 0;
                foreach(Panel p in panels)
                {
                    panel.Controls.Add(p);
                    //Debugger.Break();
                }
                //Debugger.Break();
            }
            panel.Height = 1500;
            return panel;
        }
        private Panel[] getPanelsForRender()
        {
            Panel[] panels = new Panel[0];
            int i = 0;
            foreach (User user in this.users)
            {
                Label label1 = new Label();
                label1.Text = user.Username;
                Label label2 = new Label();
                label2.Text = user.Score.ToString();
                label2.Left = 100;
                Panel panel = new Panel();
                panel.Controls.Add(label1);
                panel.Controls.Add(label2);
                panel.Top = i * 20;
                panel.Height = 20;
                panels = ArrayUtils.add(panels, panel);
                i++;
            }
            return panels;
        }

    }
}
