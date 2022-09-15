namespace BrewingTestUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MoneyWeightBox = new System.Windows.Forms.NumericUpDown();
            this.VPWeightBox = new System.Windows.Forms.NumericUpDown();
            this.RubyWeightBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BagInput = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.BoardInput = new System.Windows.Forms.TextBox();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.StartingTileInput = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MoneyWeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPWeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RubyWeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingTileInput)).BeginInit();
            this.SuspendLayout();
            // 
            // MoneyWeightBox
            // 
            this.MoneyWeightBox.Location = new System.Drawing.Point(12, 12);
            this.MoneyWeightBox.Name = "MoneyWeightBox";
            this.MoneyWeightBox.Size = new System.Drawing.Size(150, 27);
            this.MoneyWeightBox.TabIndex = 0;
            // 
            // VPWeightBox
            // 
            this.VPWeightBox.Location = new System.Drawing.Point(12, 45);
            this.VPWeightBox.Name = "VPWeightBox";
            this.VPWeightBox.Size = new System.Drawing.Size(150, 27);
            this.VPWeightBox.TabIndex = 1;
            // 
            // RubyWeightBox
            // 
            this.RubyWeightBox.Location = new System.Drawing.Point(12, 78);
            this.RubyWeightBox.Name = "RubyWeightBox";
            this.RubyWeightBox.Size = new System.Drawing.Size(150, 27);
            this.RubyWeightBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Money Weight";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "VP Weight";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ruby Weight";
            // 
            // BagInput
            // 
            this.BagInput.Location = new System.Drawing.Point(12, 172);
            this.BagInput.Multiline = true;
            this.BagInput.Name = "BagInput";
            this.BagInput.Size = new System.Drawing.Size(165, 198);
            this.BagInput.TabIndex = 6;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(151, 389);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(94, 29);
            this.SubmitButton.TabIndex = 7;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitClicked);
            // 
            // BoardInput
            // 
            this.BoardInput.Location = new System.Drawing.Point(217, 172);
            this.BoardInput.Multiline = true;
            this.BoardInput.Name = "BoardInput";
            this.BoardInput.Size = new System.Drawing.Size(162, 198);
            this.BoardInput.TabIndex = 8;
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(459, 210);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(215, 20);
            this.OutputLabel.TabIndex = 9;
            this.OutputLabel.Text = "Press Submit to calculate Move";
            // 
            // StartingTileInput
            // 
            this.StartingTileInput.Location = new System.Drawing.Point(12, 126);
            this.StartingTileInput.Name = "StartingTileInput";
            this.StartingTileInput.Size = new System.Drawing.Size(150, 27);
            this.StartingTileInput.TabIndex = 10;
            this.StartingTileInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Starting Tile";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StartingTileInput);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.BoardInput);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.BagInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RubyWeightBox);
            this.Controls.Add(this.VPWeightBox);
            this.Controls.Add(this.MoneyWeightBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.MoneyWeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPWeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RubyWeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingTileInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown MoneyWeightBox;
        private NumericUpDown VPWeightBox;
        private NumericUpDown RubyWeightBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox BagInput;
        private Button SubmitButton;
        private TextBox BoardInput;
        private Label OutputLabel;
        private NumericUpDown StartingTileInput;
        private Label label4;
    }
}