using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Testing__PixelEngine_
{
    public partial class PhysicsWIn : Form
    {
        public Element[,] ScreenArray;
        public Bitmap screen = new Bitmap(5000, 5000);
        public List<Point> Points = new List<Point>();
        public int mouseX;
        public int mouseY;
        public int floor;

        public PhysicsWIn()
        {
            InitializeComponent();
            ScreenArray = new Element[ClientSize.Width, ClientSize.Height];
            this.DoubleBuffered = true;
            floor = Height -90;
            Start();
            MouseDown += PhysicsWIn_MouseDown;
            Paint += PhysicsWIn_Paint;
        }
        void Start() // set initial values
        {

        }
        private void PhysicsWIn_Paint(object sender, PaintEventArgs pea)
        {;
            pea.Graphics.DrawImage(screen, 0, 0);
        }
        private void PhysicsWIn_MouseDown(object sender, MouseEventArgs mea)
        {
            // adding pixels with a click
            mouseX = mea.X;
            mouseY = mea.Y;
            if (mea.Button == MouseButtons.Left)
            {
                //add elements to list ---------------------------------------------------------------------------------------
                screen.SetPixel(mouseX, mouseY, Color.Black);
                Points.Add(mea.Location);
            }
            else
            {
                for (int i = -20; i < 20; i++)
                {
                    for (int v = -20; v < 20; v++)
                    {
                        screen.SetPixel(mouseX+i, mouseY + v, Color.Black);
                        Points.Add(new Point(mouseX + i, mouseY + v));
                    }
                }
            }
            Refresh(); // invalidate() & update()
        }
        
        private int AllowedToMove(Point p)
        {
            if (p.Y == floor - 1)
            {
                return 0; // not allowed to move anymore
            }
            else if (screen.GetPixel(p.X, p.Y + 1).ToArgb() == 0) // if pixel below is empty
            {
                return 1; // allowed to move
            }
            else // pixel is not empty 
            {
                if (screen.GetPixel(p.X - 1, p.Y + 1).ToArgb() == 0 || screen.GetPixel(p.X + 1, p.Y + 1).ToArgb() == 0) // check diagonals
                {
                    return 2; 
                }
                return 3;
            }
        }
        private Point GetLowest(Point p)
        {
            Point New = p;
            if (screen.GetPixel(p.X, p.Y + 1).ToArgb() != 0) // if pixel is not empty
            {
               New = new Point(New.X, New.Y +1);
               New = GetLowest(New);
            }
            return New;
        }
        private void Gravity(List<Point> pointList)
        {
            int dir = 1; // direction for first check with diagonals
            bool notDone = false;
            List<Point> newPoints = new List<Point>();
            foreach (Point pt in pointList)
            {
                int value = AllowedToMove(pt);
                if (value == 1)
                {
                    notDone = true;
                }
                

                if (pt.Y == floor-1) // if pixel hits bottom
                {
                    newPoints.Add(new Point(pt.X, pt.Y));
                    screen.SetPixel(pt.X, pt.Y, Color.Red);
                }
                else if (value ==1) // if the pixel below is white (empty)
                {
                    newPoints.Add(new Point(pt.X, pt.Y + 1));
                    screen.SetPixel(pt.X, pt.Y + 1, Color.Black);
                    screen.SetPixel(pt.X, pt.Y, Color.Empty);
                }
                else if (value == 2) // if one of the diagonals is 
                {
                    if (screen.GetPixel(pt.X - dir, pt.Y + 1).ToArgb() == 0)
                    {
                        newPoints.Add(new Point(pt.X - dir, pt.Y + 1));
                        screen.SetPixel(pt.X - dir, pt.Y + 1, Color.Black);
                        screen.SetPixel(pt.X , pt.Y, Color.Empty);
                        dir = -1; // switch first direction checked
                    }
                    else if (screen.GetPixel(pt.X + dir, pt.Y + 1).ToArgb() == 0)
                    {
                        newPoints.Add(new Point(pt.X + dir, pt.Y + 1));
                        screen.SetPixel(pt.X + dir, pt.Y + 1, Color.Red);
                        screen.SetPixel(pt.X, pt.Y, Color.Empty);
                        dir = 1; // switch first direction checked
                    }
                }
                else // if pixel is not empty
                {
                    newPoints.Add(new Point(pt.X, pt.Y));
                    screen.SetPixel(pt.X, pt.Y, Color.Red);
                }
            }
            // reset the lists and redraw the screen
            pointList.Clear();
            foreach (Point pt in newPoints)
            {
                pointList.Add(pt);
            }
            newPoints.Clear();
            Refresh();
            //Thread.Sleep(0); // 1000/9.81 (gravity) -- need to add speedup 9.82m/s^2
            if (notDone)
            {
                Gravity(pointList);
            }
            else
            {
                pointList.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // do the operations -----------------------------------------------------------------------------------
            Gravity(Points);
        }
    }
}
