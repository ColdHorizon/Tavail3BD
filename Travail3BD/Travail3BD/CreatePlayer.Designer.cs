namespace Travail3BD
{
    partial class CreatePlayer
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
            textBoxName = new TextBox();
            comboBoxClass = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label9 = new Label();
            buttonCrt = new Button();
            dataGridView1 = new DataGridView();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(28, 159);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(125, 27);
            textBoxName.TabIndex = 0;
            textBoxName.TextChanged += textBoxName_TextChanged;
            // 
            // comboBoxClass
            // 
            comboBoxClass.FormattingEnabled = true;
            comboBoxClass.Location = new Point(455, 158);
            comboBoxClass.Name = "comboBoxClass";
            comboBoxClass.Size = new Size(151, 28);
            comboBoxClass.TabIndex = 1;
            comboBoxClass.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(344, 27);
            label1.Name = "label1";
            label1.Size = new Size(119, 20);
            label1.TabIndex = 2;
            label1.Text = "Create Character";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(59, 77);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 3;
            label2.Text = "name";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(508, 93);
            label9.Name = "label9";
            label9.Size = new Size(42, 20);
            label9.TabIndex = 10;
            label9.Text = "Class";
            // 
            // buttonCrt
            // 
            buttonCrt.Location = new Point(12, 376);
            buttonCrt.Name = "buttonCrt";
            buttonCrt.Size = new Size(166, 29);
            buttonCrt.TabIndex = 11;
            buttonCrt.Text = "Create Character";
            buttonCrt.UseVisualStyleBackColor = true;
            buttonCrt.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(233, 250);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(555, 188);
            dataGridView1.TabIndex = 12;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(500, 213);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 13;
            label3.Text = "label3";
            // 
            // CreatePlayer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(dataGridView1);
            Controls.Add(buttonCrt);
            Controls.Add(label9);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBoxClass);
            Controls.Add(textBoxName);
            Name = "CreatePlayer";
            Text = "CreatePlayer";
            Load += CreatePlayer_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxName;
        private ComboBox comboBoxClass;
        private Label label1;
        private Label label2;
        private Label label9;
        private Button buttonCrt;
        private DataGridView dataGridView1;
        private Label label3;
    }
}