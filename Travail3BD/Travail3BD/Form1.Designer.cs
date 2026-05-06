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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label2 = new Label();
            buttonNew = new Button();
            comboBoxPly = new ComboBox();
            buttonUsePly = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ControlLightLight;
            label2.Location = new Point(71, 140);
            label2.Name = "label2";
            label2.Size = new Size(162, 31);
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
            comboBoxPly.FlatStyle = FlatStyle.Popup;
            comboBoxPly.FormattingEnabled = true;
            comboBoxPly.Location = new Point(71, 226);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.HighlightText;
            label1.Location = new Point(334, 124);
            label1.Name = "label1";
            label1.Size = new Size(406, 47);
            label1.TabIndex = 7;
            label1.Text = "Welcome to Redimere";
            label1.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(553, 200);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(224, 226);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 192);
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(buttonUsePly);
            Controls.Add(comboBoxPly);
            Controls.Add(buttonNew);
            Controls.Add(label2);
            Name = "Form1";
            Text = "Redimere";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Button buttonNew;
        private ComboBox comboBoxPly;
        private Button buttonUsePly;
        private Label label1;
        private PictureBox pictureBox1;
    }
}
