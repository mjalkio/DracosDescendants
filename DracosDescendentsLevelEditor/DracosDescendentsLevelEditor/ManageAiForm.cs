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
    public partial class ManageAiForm : Form
    {
        private List<AI> aiList;

        public ManageAiForm()
        {
            InitializeComponent();
        }

        public ManageAiForm(List<AI> aiList)
        {
            InitializeComponent();
            this.aiList = aiList;

            foreach (AI a in aiList)
            {
                aiSelectBox.Items.Add(a);
            }

            aiSelectBox.SelectedItem = aiList[0];
            aiSelectBox.DisplayMember = "Display";
        }

        private void newAiButton_Click(object sender, EventArgs e)
        {
            // Initializes the variables to pass to the MessageBox.Show method. 

            string message = "This will overwrite the current AI.  Are you sure you want to do this?";
            string caption = "Oh Dayum Popups";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                // Closes the parent form. 
                NewAiForm aiForm = new NewAiForm((AI)aiSelectBox.SelectedItem);
                aiForm.Show();
                this.Dispose();
            }
        }

        private void editAiButton_Click(object sender, EventArgs e)
        {
            EditAiForm aiForm = new EditAiForm((AI)aiSelectBox.SelectedItem);
            aiForm.Show();
            this.Dispose();
        }
    }
}
