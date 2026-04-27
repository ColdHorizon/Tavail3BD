namespace Travail3BD
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
            label2 = new Label();
            buttonNew = new Button();
            comboBoxPly = new ComboBox();
            buttonUsePly = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(89, 73);
            label2.Name = "label2";
            label2.Size = new Size(106, 20);
            label2.TabIndex = 2;
            label2.Text = "Select a player";
            label2.Click += label2_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(31, 349);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(94, 29);
            buttonNew.TabIndex = 4;
            buttonNew.Text = "New Player";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // comboBoxPly
            // 
            comboBoxPly.FormattingEnabled = true;
            comboBoxPly.Location = new Point(71, 210);
            comboBoxPly.Name = "comboBoxPly";
            comboBoxPly.Size = new Size(151, 28);
            comboBoxPly.TabIndex = 5;
            comboBoxPly.SelectedIndexChanged += comboBoxPly_SelectedIndexChanged;
            // 
            // buttonUsePly
            // 
            buttonUsePly.Location = new Point(166, 349);
            buttonUsePly.Name = "buttonUsePly";
            buttonUsePly.Size = new Size(94, 29);
            buttonUsePly.TabIndex = 6;
            buttonUsePly.Text = "Use Player";
            buttonUsePly.UseVisualStyleBackColor = true;
            buttonUsePly.Click += buttonUsePly_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonUsePly);
            Controls.Add(comboBoxPly);
            Controls.Add(buttonNew);
            Controls.Add(label2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Button buttonNew;
        private ComboBox comboBoxPly;
        private Button buttonUsePly;
    }
}
