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
    public partial class EditAiForm : Form
    {
        private AI ai;

        public EditAiForm()
        {
            InitializeComponent();
        }

        public EditAiForm(AI ai)
        {
            InitializeComponent();

            this.ai = ai;

            foreach (Tuple<int, int> wp in ai.getWaypoints())
            {
                waypointBox.Items.Add(wp);
            }
        }

        private void deleteWaypointButton_Click(object sender, EventArgs e)
        {
            ai.deleteWaypoint((Tuple<int, int>)waypointBox.SelectedItem);
            this.Dispose();
        }

        private void editWaypointButton_Click(object sender, EventArgs e)
        {
            Tuple<int, int> waypoint = (Tuple<int, int>)waypointBox.SelectedItem;
            EditWaypointForm waypointForm = new EditWaypointForm(ai, (Tuple<int, int>)waypointBox.SelectedItem);
            waypointForm.Show();
            this.Dispose();
        }

        private void addWaypointButton_Click(object sender, EventArgs e)
        {
            NewWaypointForm waypointForm = new NewWaypointForm(ai);
            waypointForm.Show();
            this.Dispose();
        }
    }
}
