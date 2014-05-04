namespace DracosDescendentsLevelEditor
{
    partial class EditAiForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.waypointBox = new System.Windows.Forms.ComboBox();
            this.editWaypointButton = new System.Windows.Forms.Button();
            this.deleteWaypointButton = new System.Windows.Forms.Button();
            this.addWaypointButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 20;
            this.label1.Text = "Edit AI";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Select Waypoint:";
            // 
            // waypointBox
            // 
            this.waypointBox.FormattingEnabled = true;
            this.waypointBox.Location = new System.Drawing.Point(43, 144);
            this.waypointBox.Name = "waypointBox";
            this.waypointBox.Size = new System.Drawing.Size(187, 21);
            this.waypointBox.TabIndex = 22;
            // 
            // editWaypointButton
            // 
            this.editWaypointButton.Location = new System.Drawing.Point(53, 171);
            this.editWaypointButton.Name = "editWaypointButton";
            this.editWaypointButton.Size = new System.Drawing.Size(75, 23);
            this.editWaypointButton.TabIndex = 23;
            this.editWaypointButton.Text = "Edit";
            this.editWaypointButton.UseVisualStyleBackColor = true;
            this.editWaypointButton.Click += new System.EventHandler(this.editWaypointButton_Click);
            // 
            // deleteWaypointButton
            // 
            this.deleteWaypointButton.Location = new System.Drawing.Point(143, 171);
            this.deleteWaypointButton.Name = "deleteWaypointButton";
            this.deleteWaypointButton.Size = new System.Drawing.Size(75, 23);
            this.deleteWaypointButton.TabIndex = 24;
            this.deleteWaypointButton.Text = "Delete";
            this.deleteWaypointButton.UseVisualStyleBackColor = true;
            this.deleteWaypointButton.Click += new System.EventHandler(this.deleteWaypointButton_Click);
            // 
            // addWaypointButton
            // 
            this.addWaypointButton.Location = new System.Drawing.Point(74, 71);
            this.addWaypointButton.Name = "addWaypointButton";
            this.addWaypointButton.Size = new System.Drawing.Size(127, 23);
            this.addWaypointButton.TabIndex = 25;
            this.addWaypointButton.Text = "Add New Waypoint";
            this.addWaypointButton.UseVisualStyleBackColor = true;
            this.addWaypointButton.Click += new System.EventHandler(this.addWaypointButton_Click);
            // 
            // EditAiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.addWaypointButton);
            this.Controls.Add(this.deleteWaypointButton);
            this.Controls.Add(this.editWaypointButton);
            this.Controls.Add(this.waypointBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "EditAiForm";
            this.Text = "EditAiForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox waypointBox;
        private System.Windows.Forms.Button editWaypointButton;
        private System.Windows.Forms.Button deleteWaypointButton;
        private System.Windows.Forms.Button addWaypointButton;
    }
}