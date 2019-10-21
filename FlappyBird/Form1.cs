using FlappyBird.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird.Visual
{
    public partial class Form1 : Form
    {
        private GameManager game;
        public Form1()
        {
            InitializeComponent();
            game = new GameManager();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            doubleBufferedPanel1.Invalidate();
        }

        private void doubleBufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            game.draw(e.Graphics);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            game.cycles = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game = new GameManager();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            GameManager.HEIGHT = doubleBufferedPanel1.Height;
            GameManager.WIDTH = doubleBufferedPanel1.Width;
            game.resetGame();
        }
    }
}
