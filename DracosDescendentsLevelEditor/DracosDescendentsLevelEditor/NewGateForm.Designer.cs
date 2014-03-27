namespace DracosDescendentsLevelEditor
{
    partial class NewGateForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.planet1Box = new System.Windows.Forms.ComboBox();
            this.planet2Box = new System.Windows.Forms.ComboBox();
            this.createButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "New Gate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Planet 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Planet 2";
            // 
            // planet1Box
            // 
            this.planet1Box.FormattingEnabled = true;
            this.planet1Box.Location = new System.Drawing.Point(137, 84);
            this.planet1Box.Name = "planet1Box";
            this.planet1Box.Size = new System.Drawing.Size(121, 21);
            this.planet1Box.TabIndex = 4;
            // 
            // planet2Box
            // 
            this.planet2Box.FormattingEnabled = true;
            this.planet2Box.Location = new System.Drawing.Point(137, 125);
            this.planet2Box.Name = "planet2Box";
            this.planet2Box.Size = new System.Drawing.Size(121, 21);
            this.planet2Box.TabIndex = 5;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(106, 179);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 10;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // NewGateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.planet2Box);
            this.Controls.Add(this.planet1Box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "NewGateForm";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox planet1Box;
        private System.Windows.Forms.ComboBox planet2Box;
        private System.Windows.Forms.Button createButton;
    }
}