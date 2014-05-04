﻿namespace DracosDescendentsLevelEditor
{
    partial class EditWaypointForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.xBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.yBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 24);
            this.label1.TabIndex = 20;
            this.label1.Text = "Edit Waypoint";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(232, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Select the waypoint to place this waypoint after:";
            // 
            // waypointBox
            // 
            this.waypointBox.FormattingEnabled = true;
            this.waypointBox.Location = new System.Drawing.Point(53, 111);
            this.waypointBox.Name = "waypointBox";
            this.waypointBox.Size = new System.Drawing.Size(187, 21);
            this.waypointBox.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "X";
            // 
            // xBox
            // 
            this.xBox.Location = new System.Drawing.Point(98, 148);
            this.xBox.Name = "xBox";
            this.xBox.Size = new System.Drawing.Size(100, 20);
            this.xBox.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Y";
            // 
            // yBox
            // 
            this.yBox.Location = new System.Drawing.Point(98, 179);
            this.yBox.Name = "yBox";
            this.yBox.Size = new System.Drawing.Size(100, 20);
            this.yBox.TabIndex = 27;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(107, 214);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 28;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // EditWaypointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.yBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.xBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.waypointBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "EditWaypointForm";
            this.Text = "EditWaypointForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox waypointBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox xBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox yBox;
        private System.Windows.Forms.Button saveButton;
    }
}