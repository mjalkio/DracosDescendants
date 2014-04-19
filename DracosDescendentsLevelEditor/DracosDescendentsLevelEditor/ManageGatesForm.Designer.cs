namespace DracosDescendentsLevelEditor
{
    partial class ManageGatesForm
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
            this.editGateButton = new System.Windows.Forms.Button();
            this.gateSelectBox = new System.Windows.Forms.ComboBox();
            this.deleteGateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(81, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select a Gate";
            // 
            // editGateButton
            // 
            this.editGateButton.Location = new System.Drawing.Point(149, 146);
            this.editGateButton.Name = "editGateButton";
            this.editGateButton.Size = new System.Drawing.Size(75, 23);
            this.editGateButton.TabIndex = 9;
            this.editGateButton.Text = "Edit";
            this.editGateButton.UseVisualStyleBackColor = true;
            this.editGateButton.Click += new System.EventHandler(this.editGateButton_Click);
            // 
            // gateSelectBox
            // 
            this.gateSelectBox.FormattingEnabled = true;
            this.gateSelectBox.Location = new System.Drawing.Point(12, 94);
            this.gateSelectBox.Name = "gateSelectBox";
            this.gateSelectBox.Size = new System.Drawing.Size(260, 21);
            this.gateSelectBox.TabIndex = 8;
            // 
            // deleteGateButton
            // 
            this.deleteGateButton.Location = new System.Drawing.Point(68, 146);
            this.deleteGateButton.Name = "deleteGateButton";
            this.deleteGateButton.Size = new System.Drawing.Size(75, 23);
            this.deleteGateButton.TabIndex = 10;
            this.deleteGateButton.Text = "Delete";
            this.deleteGateButton.UseVisualStyleBackColor = true;
            this.deleteGateButton.Click += new System.EventHandler(this.deleteGateButton_Click);
            // 
            // ManageGatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.deleteGateButton);
            this.Controls.Add(this.editGateButton);
            this.Controls.Add(this.gateSelectBox);
            this.Controls.Add(this.label1);
            this.Name = "ManageGatesForm";
            this.Text = "ManageGatesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editGateButton;
        private System.Windows.Forms.ComboBox gateSelectBox;
        private System.Windows.Forms.Button deleteGateButton;
    }
}