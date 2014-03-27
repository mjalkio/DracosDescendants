using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DracosDescendentsLevelEditor
{
    public partial class PreviewLevelForm : Form
    {
        //Everything is drawn in pixels so we want to be able to scale it up...
        public readonly int SCALE_FACTOR = 4;

        List<Gate> gateList;
        List<Planet> planetList;

        public PreviewLevelForm()
        {
            InitializeComponent();
        }

        public PreviewLevelForm(List<Planet> planetList, List<Gate> gateList)
        {
            InitializeComponent();
            this.planetList = planetList;
            this.gateList = gateList;
        }

        public void Draw()
        {
            SolidBrush myBrush = new SolidBrush(Color.RosyBrown);
            Graphics formGraphics = this.CreateGraphics();

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
            }

            myBrush.Dispose();

            Pen myPen = new Pen(Color.Black);
            myPen.Width = SCALE_FACTOR;

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
