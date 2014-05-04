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
    public partial class NewAiForm : Form
    {
        private AI ai;

        public NewAiForm()
        {
            InitializeComponent();
        }

        public NewAiForm(AI ai)
        {
            InitializeComponent();
            ai.clearAI();
            this.ai = ai;
        }

        private void addWaypointButton_Click(object sender, EventArgs e)
        {
            try
            {
                ai.addWaypoint(Convert.ToInt32(xBox.Text), Convert.ToInt32(yBox.Text));
            }
            catch{}
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
