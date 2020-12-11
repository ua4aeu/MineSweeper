using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace CetMineSweper
{
    public class Mine:Button
    {
        public static readonly int WIDTH = 25;
        public static readonly int HEIGHT = 25;
        private Color[] colors = new Color[]{Color.White, Color.Black, Color.Brown, Color.Blue, Color.Red, Color.Red, Color.Red, Color.Red, Color.Red};
        public bool IsBomb { get; set; }
        int neighbourBomb;
        public bool isOpen { get; set; }
        public bool isFlagged { get; set; }
        public int NeighbourBomb {
            get { return neighbourBomb; }
            set { neighbourBomb=value;
            this.ForeColor = colors[neighbourBomb];
                    }
        }
        public int Row { get; set; }
        public int Column { get; set; }
        public Mine()
        {
            NeighbourBomb = 0;
            Width = WIDTH;
            Height = HEIGHT;
            isOpen = false;
            IsBomb = false;
            Text = "";
            Font = new Font("Arial", 10, FontStyle.Bold);
        }
    }
}
