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
    public partial class ManageDragonsForm : Form
    {
        public ManageDragonsForm()
        {
            InitializeComponent();
        }

        public ManageDragonsForm(List<Dragon> dragonList)
        {
            InitializeComponent();

            foreach (Dragon d in dragonList)
            {
                dragonSelectBox.Items.Add(d);
            }

            dragonSelectBox.DisplayMember = "Display";
        }

         private void editDragonButton_Click(object sender, EventArgs e)
         {
             EditDragonForm dragonForm = new EditDragonForm((Dragon)dragonSelectBox.SelectedItem);
             dragonForm.Show();
             this.Dispose();
         }
    }
}
