namespace DracosDescendentsLevelEditor
{
    partial class ManageAiForm
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
            this.aiSelectBox = new System.Windows.Forms.ComboBox();
            this.newAiButton = new System.Windows.Forms.Button();
            this.editAiButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select an AI";
            // 
            // aiSelectBox
            // 
            this.aiSelectBox.FormattingEnabled = true;
            this.aiSelectBox.Location = new System.Drawing.Point(12, 121);
            this.aiSelectBox.Name = "aiSelectBox";
            this.aiSelectBox.Size = new System.Drawing.Size(260, 21);
            this.aiSelectBox.TabIndex = 9;
            // 
            // newAiButton
            // 
            this.newAiButton.Location = new System.Drawing.Point(109, 207);
            this.newAiButton.Name = "newAiButton";
            this.newAiButton.Size = new System.Drawing.Size(75, 23);
            this.newAiButton.TabIndex = 11;
            this.newAiButton.Text = "Overwrite";
            this.newAiButton.UseVisualStyleBackColor = true;
            this.newAiButton.Click += new System.EventHandler(this.newAiButton_Click);
            // 
            // editAiButton
            // 
            this.editAiButton.Location = new System.Drawing.Point(109, 169);
            this.editAiButton.Name = "editAiButton";
            this.editAiButton.Size = new System.Drawing.Size(75, 23);
            this.editAiButton.TabIndex = 12;
            this.editAiButton.Text = "Edit";
            this.editAiButton.UseVisualStyleBackColor = true;
            this.editAiButton.Click += new System.EventHandler(this.editAiButton_Click);
            // 
            // ManageAiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.editAiButton);
            this.Controls.Add(this.newAiButton);
            this.Controls.Add(this.aiSelectBox);
            this.Controls.Add(this.label1);
            this.Name = "ManageAiForm";
            this.Text = "ManageAiForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox aiSelectBox;
        private System.Windows.Forms.Button newAiButton;
        private System.Windows.Forms.Button editAiButton;
    }
}