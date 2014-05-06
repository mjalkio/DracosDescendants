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
    public partial class ManageTextForm : Form
    {
        private List<Text> textList;

        public ManageTextForm()
        {
            InitializeComponent();
        }

        public ManageTextForm(List<Text> textList)
        {
            InitializeComponent();

            this.textList = textList;

            foreach(Text text in textList)
            {
                oldTextBox.Items.Add(text);
            }

            if (textList.Count != 0)
            {
                oldTextBox.SelectedItem = textList[0];
            }

            oldTextBox.DisplayMember = "Display";
        }

        private void deleteTextButton_Click(object sender, EventArgs e)
        {
            textList.Remove((Text)oldTextBox.SelectedItem);
            this.Dispose();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int startX = Convert.ToInt32(startBox.Text);
                int endX = Convert.ToInt32(endBox.Text);
                string text = textBox.Text;

                textList.Add(new Text(startX, endX, text));

                this.Dispose();
            }
            catch { }
        }
    }
}
