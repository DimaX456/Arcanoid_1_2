using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid_1_2
{
    public partial class Form1 : Form
    {

        bool goLeft;
        bool goRight;
        bool isGameOver;

        int score;
        int ballx;
        int bally;
        int playerSpeed;

        Random rnd = new Random();

        PictureBox[] blockArray;

        public Form1()
        {
            InitializeComponent();

            PlaceBlocks();
        }

        private void setupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            txtScore.Text = "Очки: " + score;

            ball.Left = 391;
            ball.Top = 347;

            player.Left = 357;

            gameTimer.Start();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        private void gameOver (string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Очки: " + score + " " + message;
        }

        private void PlaceBlocks()
        {
            blockArray = new PictureBox[15];
            int a = 0;
            int top = 50;
            int left = 100;

            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 32;
                blockArray[i].Width = 100;
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;

                if (a==5)
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }
                if (a<5)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 130;
                }
            }

            setupGame();
        }

        private void removeBlocks()
        {
            foreach(PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }

        private void mainGameTimeEvent(object sender, EventArgs e)
        {

            txtScore.Text = "Очки: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (goRight == true && player.Left < 677)
            {
                player.Left += playerSpeed;
            }

            ball.Left += ballx;
            ball.Top += bally;

            if (ball.Left < 0 || ball.Left > 737)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }

            if (ball.Bounds.IntersectsWith(player.Bounds))
            {

                //ball.Top = 347;

                bally = rnd.Next(5, 12) * -1;

                if (ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }
                else
                {
                    ballx = rnd.Next(5, 12);
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        bally = -bally;
                        this.Controls.Remove(x);
                    }
                }
            }

            if (score == 15)
            {
                gameOver("||Вы выиграли! Нажмите на Enter, чтобы начать сначала.");
            }

            if (ball.Top > 805)
            {
                gameOver("||Вы проиграли! Нажмите на Enter, чтобы начать сначала.");
            }
        }

        private void keysdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        private void keysup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                PlaceBlocks();
            }
        }
    }
}
