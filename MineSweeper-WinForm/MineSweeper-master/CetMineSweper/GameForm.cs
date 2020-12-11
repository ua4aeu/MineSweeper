using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CetMineSweper
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }
        int WIDTH = 25;
        int HEIGHT = 25;
        int BOMB_COUNT = 80;
        Mine[,] Mines;
        private void GameForm_Load(object sender, EventArgs e)
        {
           
        }

        private void CreateField()
        {
            int x = 0;
            int y = 0;
            Mines=new Mine[WIDTH,HEIGHT];
            Field.Enabled = true;
            Field.Controls.Clear();
            Field.Location = new Point(0, 70);
            Field.Width = WIDTH * Mine.WIDTH;
            Field.Height = HEIGHT * Mine.HEIGHT;
            this.Width = Field.Width+20;
            this.Height = Field.Height + Field.Location.Y+50;
            
            for (int r  = 0; r < WIDTH; r++)
            {
                for (int c = 0; c < HEIGHT; c++)
                {
                   
                    
                    Mine m = new Mine();                     
                    x = c * Mine.WIDTH;
                    y = r * Mine.HEIGHT;
                    m.Location = new Point(x, y);
                    Mines[r, c] = m;
                    m.Row = r;
                    m.Column = c;
                    Field.Controls.Add(m);
                    m.Click += MineClick;
                    m.MouseUp += mine_MouseUp;
                    m.MouseDown += mineMouseDown;
                   
                }                  
            }

            //for (int i = 0; i < WIDTH*HEIGHT; i++)
            //{
                
            //}
        }
        int oldgame = 0;
        int num_of_left;

        

        private void mine_MouseUp(object sender, MouseEventArgs e)
        {
            btn_smile.BackgroundImage = Properties.Resources.smile;
        }

        

        void mineMouseDown(object sender, MouseEventArgs e)
        {
            btn_smile.BackgroundImage = Properties.Resources.sad;
            Mine mine = sender as Mine;

            
            if (e.Button == MouseButtons.Right)
            {
                if (mine.isFlagged)
                {
                    mine.isFlagged = false;
                    mine.BackgroundImage = null;
                    BOMB_COUNT++;
                    
                }
                else { 
                mine.isFlagged = true;
                mine.BackgroundImage = Properties.Resources.FlagIcon;
                BOMB_COUNT--;
                
                }
                lbl_count.Text = BOMB_COUNT.ToString();
            }
        }
        List<Mine> bombs = new List<Mine>();

        private void PlaceBombs()
        {
            Random r = new Random();
            for (int i = 0; i < BOMB_COUNT; i++)
            {
                Mine m = Mines[r.Next(WIDTH), r.Next(HEIGHT)];
                if (m.IsBomb)
                {
                    i--;
                    continue;
                }
                else { 
                    m.IsBomb=true;
                    //m.Text = "X";
                    bombs.Add(m);
                    foreach (var mine in GetNeighbours(m))
                    {
                        mine.NeighbourBomb++;
                        
                        if (!mine.IsBomb)
                        {
                           // mine.Text = mine.NeighbourBomb.ToString();
                        }
                    }

                }
                
            }

        }

        void MineClick(object sender, EventArgs e)
        {
            Mine mine = sender as Mine;
            if (!mine.isFlagged)
            {
                OpenMine(mine);
            }
           
        }

        private void OpenMine(Mine mine)
        {
            if (mine.isOpen || mine.isFlagged==true)
            {
                return;
            }

            mine.isOpen = true;

            if (mine.IsBomb)
            {
                Field.Enabled = false;
                foreach (var item in Mines)
                {
                    if (item.IsBomb == false && item.isFlagged == true)
                    {
                        item.BackgroundImage = Properties.Resources.wrong; 
                    }
                    else if (item.IsBomb == true && item.isFlagged == false)
                    {
                        item.BackgroundImage = Properties.Resources.mine;
                    }
                }

                mine.BackgroundImage = Properties.Resources.mine3;
                tmrSecond.Stop();
                MessageBox.Show("Вы погибли!");
            }
            else if (mine.NeighbourBomb > 0)
            {
                mine.Text = mine.NeighbourBomb.ToString();
                mine.Enabled = false;

            }
            else
            {
                mine.Visible = false;
            
                foreach (var m in GetNeighbours(mine))
                {
                    OpenMine(m);
                }

            }



           
            if (mine.IsBomb==false)
            {
                num_of_left--;
                label1.Text = num_of_left.ToString();
                if (num_of_left == 0)
                {
                    tmrSecond.Stop();
                    Field.Enabled = false;
                    foreach (var item in bombs)
                    {
                        item.BackgroundImage = Properties.Resources.FlagIcon;
                    }
                    MessageBox.Show("Вы выиграли!");
                }

            }
            
        }

        List<Mine> GetNeighbours(Mine m) {
            List<Mine> neighbours = new List<Mine>();
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if ((r == 0 && c == 0)|| (m.Row + r<0) || (m.Column + c<0)||
                        (m.Row + r>=WIDTH) || (m.Column + c>=HEIGHT))
                    {
                        continue;
                    }
                    Mine neighbour = Mines[m.Row + r, m.Column + c];
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
            
         }
        int second = 0;
        DateTime StartTime;
        private void tmrSecond_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - StartTime;
            
            lblSeconds.Text =Math.Round(ts.TotalSeconds).ToString();
        }

        private void startAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (oldgame)
            {
                case 0:
                    break;
                case 1 :
                    OpenGame(5, 5, 2);
                    break;
                case 2:
                    OpenGame(16, 16, 40);
                    break;
                case 3:
                    OpenGame(23, 23, 100);
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGame(5, 5, 2);
            oldgame = 1;
        }

        private void OpenGame(int w, int h, int bc)
        {
            WIDTH = w;
            HEIGHT = h;
            BOMB_COUNT = bc;
            btn_smile.Visible = true;
            CreateField();
            PlaceBombs();
            lblSeconds.Location = new Point( this.Width - 50,30);
            btn_smile.Location = new Point(this.Width / 2 - 23, 30);
            lbl_count.Location = new Point(15 , 30);
            lbl_count.Text = BOMB_COUNT.ToString();
            StartTime = DateTime.Now;
            tmrSecond.Start();
            num_of_left = w * h - bc;
            
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGame(16, 16, 40);
            oldgame = 2;
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGame(23, 23, 100);
            oldgame = 3;
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Vasilenko I.K. \n2020 ©");
        }

        private void btn_smile_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn  = sender as Button;
            btn.BackgroundImage = Properties.Resources.sad;

        }

        private void btn_smile_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = Properties.Resources.smile;
        }

        private void btn_smile_Click(object sender, EventArgs e)
        {
            switch (oldgame)
            {
                case 0:
                    break;
                case 1:
                    OpenGame(5, 5, 2);
                    break;
                case 2:
                    OpenGame(16, 16, 40);
                    break;
                case 3:
                    OpenGame(23, 23, 100);
                    break;
            }

        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
