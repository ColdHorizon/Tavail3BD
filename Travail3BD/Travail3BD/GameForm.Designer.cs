namespace Travail3BD
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelPlayerHP = new Label();
            labelPlayerEnergy = new Label();
            labelEnemyName = new Label();
            labelEnemyHP = new Label();
            labelScore = new Label();
            buttonAtk1 = new Button();
            buttonAtk2 = new Button();
            buttonAtk3 = new Button();
            textBoxLog = new TextBox();
            SuspendLayout();
            // 
            // labelPlayerHP
            // 
            labelPlayerHP.AutoSize = true;
            labelPlayerHP.Location = new Point(30, 30);
            labelPlayerHP.Name = "labelPlayerHP";
            labelPlayerHP.Size = new Size(100, 20);
            labelPlayerHP.TabIndex = 0;
            labelPlayerHP.Text = "Player HP";
            // 
            // labelPlayerEnergy
            // 
            labelPlayerEnergy.AutoSize = true;
            labelPlayerEnergy.Location = new Point(30, 60);
            labelPlayerEnergy.Name = "labelPlayerEnergy";
            labelPlayerEnergy.Size = new Size(110, 20);
            labelPlayerEnergy.TabIndex = 1;
            labelPlayerEnergy.Text = "Player Energy";
            // 
            // labelEnemyName
            // 
            labelEnemyName.AutoSize = true;
            labelEnemyName.Location = new Point(400, 30);
            labelEnemyName.Name = "labelEnemyName";
            labelEnemyName.Size = new Size(105, 20);
            labelEnemyName.TabIndex = 2;
            labelEnemyName.Text = "Enemy Name";
            // 
            // labelEnemyHP
            // 
            labelEnemyHP.AutoSize = true;
            labelEnemyHP.Location = new Point(400, 60);
            labelEnemyHP.Name = "labelEnemyHP";
            labelEnemyHP.Size = new Size(85, 20);
            labelEnemyHP.TabIndex = 3;
            labelEnemyHP.Text = "Enemy HP";
            // 
            // labelScore
            // 
            labelScore.AutoSize = true;
            labelScore.Location = new Point(30, 90);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(49, 20);
            labelScore.TabIndex = 4;
            labelScore.Text = "Score";
            // 
            // buttonAtk1
            // 
            buttonAtk1.Location = new Point(30, 150);
            buttonAtk1.Name = "buttonAtk1";
            buttonAtk1.Size = new Size(150, 40);
            buttonAtk1.TabIndex = 5;
            buttonAtk1.Text = "Attack 1";
            buttonAtk1.UseVisualStyleBackColor = true;
            buttonAtk1.Click += button1_Click;
            // 
            // buttonAtk2
            // 
            buttonAtk2.Location = new Point(30, 200);
            buttonAtk2.Name = "buttonAtk2";
            buttonAtk2.Size = new Size(150, 40);
            buttonAtk2.TabIndex = 6;
            buttonAtk2.Text = "Attack 2";
            buttonAtk2.UseVisualStyleBackColor = true;
            buttonAtk2.Click += button2_Click;
            // 
            // buttonAtk3
            // 
            buttonAtk3.Location = new Point(30, 250);
            buttonAtk3.Name = "buttonAtk3";
            buttonAtk3.Size = new Size(150, 40);
            buttonAtk3.TabIndex = 7;
            buttonAtk3.Text = "Attack 3";
            buttonAtk3.UseVisualStyleBackColor = true;
            buttonAtk3.Click += button3_Click;
            // 
            // textBoxLog
            // 
            textBoxLog.Location = new Point(220, 150);
            textBoxLog.Multiline = true;
            textBoxLog.ScrollBars = ScrollBars.Vertical;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.Size = new Size(350, 250);
            textBoxLog.TabIndex = 8;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 450);
            Controls.Add(textBoxLog);
            Controls.Add(buttonAtk3);
            Controls.Add(buttonAtk2);
            Controls.Add(buttonAtk1);
            Controls.Add(labelScore);
            Controls.Add(labelEnemyHP);
            Controls.Add(labelEnemyName);
            Controls.Add(labelPlayerEnergy);
            Controls.Add(labelPlayerHP);
            Name = "GameForm";
            Text = "GameForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPlayerHP;
        private Label labelPlayerEnergy;
        private Label labelEnemyName;
        private Label labelEnemyHP;
        private Label labelScore;
        private Button buttonAtk1;
        private Button buttonAtk2;
        private Button buttonAtk3;
        private TextBox textBoxLog;
    }
}
