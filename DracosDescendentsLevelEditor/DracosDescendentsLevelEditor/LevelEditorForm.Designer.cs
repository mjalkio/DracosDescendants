namespace DracosDescendentsLevelEditor
{
    partial class LevelEditorForm
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
            this.addPlanetButton = new System.Windows.Forms.Button();
            this.addGateButton = new System.Windows.Forms.Button();
            this.levelHeightUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.levelWidthUpDown = new System.Windows.Forms.NumericUpDown();
            this.previewButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.managePlanetsButton = new System.Windows.Forms.Button();
            this.manageGatesButton = new System.Windows.Forms.Button();
            this.manageDragonsButton = new System.Windows.Forms.Button();
            this.saveXML = new System.Windows.Forms.SaveFileDialog();
            this.openXML = new System.Windows.Forms.OpenFileDialog();
            this.importButton = new System.Windows.Forms.Button();
            this.changeScaleButton = new System.Windows.Forms.Button();
            this.scaleBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.manageAIbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.levelHeightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelWidthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Super Awesome Level Editor";
            // 
            // addPlanetButton
            // 
            this.addPlanetButton.Location = new System.Drawing.Point(113, 97);
            this.addPlanetButton.Name = "addPlanetButton";
            this.addPlanetButton.Size = new System.Drawing.Size(75, 23);
            this.addPlanetButton.TabIndex = 1;
            this.addPlanetButton.Text = "Add Planet";
            this.addPlanetButton.UseVisualStyleBackColor = true;
            this.addPlanetButton.Click += new System.EventHandler(this.addPlanetButton_Click);
            // 
            // addGateButton
            // 
            this.addGateButton.Location = new System.Drawing.Point(113, 126);
            this.addGateButton.Name = "addGateButton";
            this.addGateButton.Size = new System.Drawing.Size(75, 23);
            this.addGateButton.TabIndex = 2;
            this.addGateButton.Text = "Add Gate";
            this.addGateButton.UseVisualStyleBackColor = true;
            this.addGateButton.Click += new System.EventHandler(this.addGateButton_Click);
            // 
            // levelHeightUpDown
            // 
            this.levelHeightUpDown.Location = new System.Drawing.Point(285, 52);
            this.levelHeightUpDown.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.levelHeightUpDown.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.levelHeightUpDown.Name = "levelHeightUpDown";
            this.levelHeightUpDown.Size = new System.Drawing.Size(102, 20);
            this.levelHeightUpDown.TabIndex = 3;
            this.levelHeightUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Level Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Level Width";
            // 
            // levelWidthUpDown
            // 
            this.levelWidthUpDown.Location = new System.Drawing.Point(86, 52);
            this.levelWidthUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.levelWidthUpDown.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.levelWidthUpDown.Name = "levelWidthUpDown";
            this.levelWidthUpDown.Size = new System.Drawing.Size(102, 20);
            this.levelWidthUpDown.TabIndex = 6;
            this.levelWidthUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // previewButton
            // 
            this.previewButton.Location = new System.Drawing.Point(136, 231);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(115, 34);
            this.previewButton.TabIndex = 7;
            this.previewButton.Text = "Preview Level";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(257, 231);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(115, 34);
            this.exportButton.TabIndex = 8;
            this.exportButton.Text = "Export Level";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // managePlanetsButton
            // 
            this.managePlanetsButton.Location = new System.Drawing.Point(218, 97);
            this.managePlanetsButton.Name = "managePlanetsButton";
            this.managePlanetsButton.Size = new System.Drawing.Size(131, 23);
            this.managePlanetsButton.TabIndex = 9;
            this.managePlanetsButton.Text = "Manage Planets";
            this.managePlanetsButton.UseVisualStyleBackColor = true;
            this.managePlanetsButton.Click += new System.EventHandler(this.managePlanetsButton_Click);
            // 
            // manageGatesButton
            // 
            this.manageGatesButton.Location = new System.Drawing.Point(218, 129);
            this.manageGatesButton.Name = "manageGatesButton";
            this.manageGatesButton.Size = new System.Drawing.Size(131, 23);
            this.manageGatesButton.TabIndex = 10;
            this.manageGatesButton.Text = "Manage Gates";
            this.manageGatesButton.UseVisualStyleBackColor = true;
            this.manageGatesButton.Click += new System.EventHandler(this.manageGatesButton_Click);
            // 
            // manageDragonsButton
            // 
            this.manageDragonsButton.Location = new System.Drawing.Point(218, 160);
            this.manageDragonsButton.Name = "manageDragonsButton";
            this.manageDragonsButton.Size = new System.Drawing.Size(131, 23);
            this.manageDragonsButton.TabIndex = 11;
            this.manageDragonsButton.Text = "Manage Dragons";
            this.manageDragonsButton.UseVisualStyleBackColor = true;
            this.manageDragonsButton.Click += new System.EventHandler(this.manageDragonsButton_Click);
            // 
            // saveXML
            // 
            this.saveXML.DefaultExt = "xml";
            this.saveXML.FileOk += new System.ComponentModel.CancelEventHandler(this.saveXML_FileOk);
            // 
            // openXML
            // 
            this.openXML.FileName = "openXML";
            this.openXML.InitialDirectory = "..\\";
            this.openXML.FileOk += new System.ComponentModel.CancelEventHandler(this.openXML_FileOk);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(15, 231);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(115, 34);
            this.importButton.TabIndex = 12;
            this.importButton.Text = "Load Level";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // changeScaleButton
            // 
            this.changeScaleButton.Location = new System.Drawing.Point(113, 166);
            this.changeScaleButton.Name = "changeScaleButton";
            this.changeScaleButton.Size = new System.Drawing.Size(75, 49);
            this.changeScaleButton.TabIndex = 13;
            this.changeScaleButton.Text = "Change Scale";
            this.changeScaleButton.UseVisualStyleBackColor = true;
            this.changeScaleButton.Click += new System.EventHandler(this.changeScaleButton_Click);
            // 
            // scaleBox
            // 
            this.scaleBox.Location = new System.Drawing.Point(7, 195);
            this.scaleBox.Name = "scaleBox";
            this.scaleBox.Size = new System.Drawing.Size(100, 20);
            this.scaleBox.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Multiply Scale By:";
            // 
            // manageAIbutton
            // 
            this.manageAIbutton.Location = new System.Drawing.Point(218, 193);
            this.manageAIbutton.Name = "manageAIbutton";
            this.manageAIbutton.Size = new System.Drawing.Size(131, 23);
            this.manageAIbutton.TabIndex = 16;
            this.manageAIbutton.Text = "Manage AI";
            this.manageAIbutton.UseVisualStyleBackColor = true;
            this.manageAIbutton.Click += new System.EventHandler(this.manageAIbutton_Click);
            // 
            // LevelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 303);
            this.Controls.Add(this.manageAIbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.scaleBox);
            this.Controls.Add(this.changeScaleButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.manageDragonsButton);
            this.Controls.Add(this.manageGatesButton);
            this.Controls.Add(this.managePlanetsButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.levelWidthUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.levelHeightUpDown);
            this.Controls.Add(this.addGateButton);
            this.Controls.Add(this.addPlanetButton);
            this.Controls.Add(this.label1);
            this.Name = "LevelEditorForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.levelHeightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelWidthUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addPlanetButton;
        private System.Windows.Forms.Button addGateButton;
        private System.Windows.Forms.NumericUpDown levelHeightUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown levelWidthUpDown;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button managePlanetsButton;
        private System.Windows.Forms.Button manageGatesButton;
        private System.Windows.Forms.Button manageDragonsButton;
        private System.Windows.Forms.SaveFileDialog saveXML;
        private System.Windows.Forms.OpenFileDialog openXML;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button changeScaleButton;
        private System.Windows.Forms.TextBox scaleBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button manageAIbutton;
    }
}

