using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PongClient
{
    public partial class Form1 : Form
    {
        public Bitmap buffer;
        private Timer gameTimer;
        private Game game;
        private bool firstRun;

        public Form1()
        {
            InitializeComponent();

            //Set firstRun
            this.firstRun = false;

            //Set the winform style ready for painting
            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

            //Start gametick
            this.gameTimer = new Timer() { Interval = 10 };
            this.gameTimer.Tick += new EventHandler(gameTick);
            this.gameTimer.Start();

            //Events for painting to the form
            this.ResizeEnd += new EventHandler(createBuffer);
            this.Load += new EventHandler(createBuffer);
            this.Paint += new PaintEventHandler(paint);
            this.KeyDown += Form1_KeyDown;
        }

        private void gameTick(object sender, EventArgs e)
        {
            if (this.buffer != null)
            {
                //Creates game object with a reference to the forms buffer
                if (!this.firstRun)
                {
                    this.game = new Game(ref this.buffer);
                    this.firstRun = true;
                }

                //Game controller with states
                switch (this.game.CurrentState)
                {
                    case Game.State.Connecting:
                        this.game.Connect(this.ClientSize.Width, this.ClientSize.Height);
                        break;
                }


                //Invalidates the controls on each tick
                this.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void createBuffer(object sender, EventArgs e)
        {
            if (this.buffer != null) //Disposes of buffer at the start
                this.buffer.Dispose();

            //Initalizes buffer
            this.buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

 
        private void paint(object sender, PaintEventArgs e)
        {
            if (this.buffer != null)
            {
                //Draws buffer to the form
                e.Graphics.DrawImageUnscaled(this.buffer, Point.Empty);
            }
        }
    }
}
