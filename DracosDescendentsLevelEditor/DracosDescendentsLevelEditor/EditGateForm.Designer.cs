namespace DracosDescendentsLevelEditor
{
    partial class EditGateForm
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
            this.gateBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.planet2Box = new System.Windows.Forms.ComboBox();
            this.planet1Box = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gateBox
            // 
            this.gateBox.FormattingEnabled = true;
            this.gateBox.Location = new System.Drawing.Point(49, 162);
            this.gateBox.Name = "gateBox";
            this.gateBox.Size = new System.Drawing.Size(187, 21);
            this.gateBox.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Select the gate to place this gate after:";
            // 
            // planet2Box
            // 
            this.planet2Box.FormattingEnabled = true;
            this.planet2Box.Location = new System.Drawing.Point(119, 96);
            this.planet2Box.Name = "planet2Box";
            this.planet2Box.Size = new System.Drawing.Size(121, 21);
            this.planet2Box.TabIndex = 16;
            // 
            // planet1Box
            // 
            this.planet1Box.FormattingEnabled = true;
            this.planet1Box.Location = new System.Drawing.Point(119, 67);
            this.planet1Box.Name = "planet1Box";
            this.planet1Box.Size = new System.Drawing.Size(121, 21);
            this.planet1Box.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Planet 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Planet 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(93, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 19;
            this.label1.Text = "Edit Gate";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(104, 208);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // EditGateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gateBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.planet2Box);
            this.Controls.Add(this.planet1Box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "EditGateForm";
            this.Text = "EditGateForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox gateBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox planet2Box;
        private System.Windows.Forms.ComboBox planet1Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
    }
}