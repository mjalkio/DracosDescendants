namespace DracosDescendentsLevelEditor
{
    partial class ManageDragonsForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.dragonSelectBox = new System.Windows.Forms.ComboBox();
            this.editDragonButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "How to Train Yo\' Dragon";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select a dragon:";
            // 
            // dragonSelectBox
            // 
            this.dragonSelectBox.FormattingEnabled = true;
            this.dragonSelectBox.Location = new System.Drawing.Point(51, 107);
            this.dragonSelectBox.Name = "dragonSelectBox";
            this.dragonSelectBox.Size = new System.Drawing.Size(177, 21);
            this.dragonSelectBox.TabIndex = 6;
            // 
            // editDragonButton
            // 
            this.editDragonButton.Location = new System.Drawing.Point(101, 159);
            this.editDragonButton.Name = "editDragonButton";
            this.editDragonButton.Size = new System.Drawing.Size(75, 23);
            this.editDragonButton.TabIndex = 7;
            this.editDragonButton.Text = "Edit";
            this.editDragonButton.UseVisualStyleBackColor = true;
            this.editDragonButton.Click += new System.EventHandler(this.editDragonButton_Click);
            // 
            // ManageDragonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.editDragonButton);
            this.Controls.Add(this.dragonSelectBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ManageDragonsForm";
            this.Text = "ManageDragonsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox dragonSelectBox;
        private System.Windows.Forms.Button editDragonButton;
    }
}