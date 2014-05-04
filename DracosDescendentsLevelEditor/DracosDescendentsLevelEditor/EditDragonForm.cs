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
    public partial class EditDragonForm : Form
    {
        private Dragon dragon;

        public EditDragonForm()
        {
            InitializeComponent();
        }

        public EditDragonForm(Dragon dragon)
        {
            InitializeComponent();
            this.dragon = dragon;
            xBox.SelectedText = Convert.ToString(dragon.x);
            yBox.SelectedText = Convert.ToString(dragon.y);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                dragon.x = Convert.ToInt32(xBox.Text);
                dragon.y = Convert.ToInt32(yBox.Text);
            }
            catch
            {

            }
            this.Dispose();
        }
    }
}
