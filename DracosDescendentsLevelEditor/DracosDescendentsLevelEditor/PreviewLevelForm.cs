using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DracosDescendentsLevelEditor
{
    public partial class PreviewLevelForm : Form
    {
        //Everything is drawn in pixels so we want to be able to scale it up...
        public float SCALE_FACTOR;

        List<Gate> gateList;
        List<Planet> planetList;
        int levelWidth;
        int levelHeight;

        public PreviewLevelForm()
        {
            InitializeComponent();
        }

        public PreviewLevelForm(List<Planet> planetList, List<Gate> gateList, int width, int height)
        {
            InitializeComponent();
            this.planetList = planetList;
            this.gateList = gateList;
            levelWidth = width;
            levelHeight = height;

            SCALE_FACTOR = 1750.0f / levelWidth;
        }

        public void Draw()
        {
            //Setup the stuff we draw on
            SolidBrush myBrush = new SolidBrush(Color.Green);
            Graphics formGraphics = this.CreateGraphics();

            //Setup the pen to draw gates and grid
            Pen myPen = new Pen(Color.Black);
            myPen.Width = SCALE_FACTOR;

            //Setting up drawing of planet IDs
            int i = 0;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 6);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();

            //Draw the x,y grid
            for (int j = 0; j < levelWidth; j += 50)
            {
                formGraphics.DrawLine(myPen, j * SCALE_FACTOR, 0, j * SCALE_FACTOR, levelHeight * SCALE_FACTOR);
                formGraphics.DrawString("" + j, drawFont, myBrush, j * SCALE_FACTOR, 0, drawFormat);
            }
            for (int k = 0; k < levelHeight; k += 50)
            {
                formGraphics.DrawLine(myPen, 0, k * SCALE_FACTOR, levelWidth * SCALE_FACTOR, k * SCALE_FACTOR);
                formGraphics.DrawString("" + k, drawFont, myBrush, 0, k * SCALE_FACTOR, drawFormat);
            }

            drawFont = new System.Drawing.Font("Arial", 16);

            //Draw the planets
            foreach (Planet planet in planetList)
            {
                int radius = (int) (planet.radius * SCALE_FACTOR);
                int xCoord = (int) (planet.x * SCALE_FACTOR);
                int yCoord = (int) (planet.y * SCALE_FACTOR);

                if (planet.type == "regular")
                {
                    myBrush.Color = Color.RosyBrown;
                }
                else if (planet.type == "gaseous")
                {
                    myBrush.Color = Color.GreenYellow;
                }
                else if (planet.type == "lava")
                {
                    myBrush.Color = Color.Firebrick;
                }

                formGraphics.FillEllipse(myBrush, new Rectangle(xCoord - radius, yCoord - radius, radius * 2, radius * 2));

                myBrush.Color = Color.Black;
                formGraphics.DrawString("" + i, drawFont, myBrush, xCoord, yCoord, drawFormat);
                i++;
            }

            drawFont.Dispose();
            myBrush.Dispose();

            //Draw the gates
            myPen.Color = Color.Black;
            foreach (Gate gate in gateList)
            {
                float x1 = gate.planet1.x * SCALE_FACTOR;
                float y1 = gate.planet1.y * SCALE_FACTOR;
                float x2 = gate.planet2.x * SCALE_FACTOR;
                float y2 = gate.planet2.y * SCALE_FACTOR;

                formGraphics.DrawLine(myPen, x1, y1, x2, y2);
            }

            myPen.Dispose();
            formGraphics.Dispose();
        }
    }
}
