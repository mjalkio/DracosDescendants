﻿namespace DracosDescendentsLevelEditor
{
    partial class ManagePlanetsForm
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
            this.planetSelectComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.deletePlanetButton = new System.Windows.Forms.Button();
            this.editPlanetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Manage Yo\' Planets";
            // 
            // planetSelectComboBox
            // 
            this.planetSelectComboBox.FormattingEnabled = true;
            this.planetSelectComboBox.Location = new System.Drawing.Point(70, 102);
            this.planetSelectComboBox.Name = "planetSelectComboBox";
            this.planetSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.planetSelectComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select a planet:";
            // 
            // deletePlanetButton
            // 
            this.deletePlanetButton.Location = new System.Drawing.Point(61, 166);
            this.deletePlanetButton.Name = "deletePlanetButton";
            this.deletePlanetButton.Size = new System.Drawing.Size(75, 23);
            this.deletePlanetButton.TabIndex = 5;
            this.deletePlanetButton.Text = "Delete";
            this.deletePlanetButton.UseVisualStyleBackColor = true;
            this.deletePlanetButton.Click += new System.EventHandler(this.deletePlanetButton_Click);
            // 
            // editPlanetButton
            // 
            this.editPlanetButton.Location = new System.Drawing.Point(143, 166);
            this.editPlanetButton.Name = "editPlanetButton";
            this.editPlanetButton.Size = new System.Drawing.Size(75, 23);
            this.editPlanetButton.TabIndex = 6;
            this.editPlanetButton.Text = "Edit";
            this.editPlanetButton.UseVisualStyleBackColor = true;
            this.editPlanetButton.Click += new System.EventHandler(this.editPlanetButton_Click);
            // 
            // ManagePlanetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.editPlanetButton);
            this.Controls.Add(this.deletePlanetButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.planetSelectComboBox);
            this.Controls.Add(this.label1);
            this.Name = "ManagePlanetsForm";
            this.Text = "ManagePlanetsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox planetSelectComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button deletePlanetButton;
        private System.Windows.Forms.Button editPlanetButton;
    }
}