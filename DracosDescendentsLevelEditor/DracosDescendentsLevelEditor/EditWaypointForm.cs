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
    public partial class EditWaypointForm : Form
    {
        AI ai;
        Tuple<int, int> waypoint;

        public EditWaypointForm()
        {
            InitializeComponent();
        }

        public EditWaypointForm(AI ai, Tuple<int, int> waypoint)
        {
            InitializeComponent();

            this.ai = ai;

            List<Tuple<int, int>> waypoints = ai.getWaypoints();
            this.waypoint = waypoint;

            foreach (Tuple<int, int> wp in waypoints)
            {
                if (wp != waypoint)
                {
                    waypointBox.Items.Add(wp);
                }
            }

            int waypointIndex = waypoints.IndexOf(waypoint);

            if (waypoints.Count > 1)
            {
                waypointBox.SelectedItem = waypoints[waypointIndex - 1];
            }

            xBox.SelectedText = Convert.ToString(waypoint.Item1);
            yBox.SelectedText = Convert.ToString(waypoint.Item2);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(xBox.Text);
                int y = Convert.ToInt32(yBox.Text);

                ai.updateWaypoint(waypoint, (Tuple<int, int>)waypointBox.SelectedItem, x, y);

                this.Dispose();
            }
            catch { }
        }
    }
}
