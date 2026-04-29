using System.Drawing;
using System.Windows.Forms;

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
            textBoxUpgrades = new TextBox();
            buttonUpg = new Button();
            SuspendLayout();

            labelPlayerHP.AutoSize = true;
            labelPlayerHP.Location = new Point(30, 30);
            labelPlayerHP.Name = "labelPlayerHP";
            labelPlayerHP.Size = new Size(72, 20);
            labelPlayerHP.TabIndex = 0;
            labelPlayerHP.Text = "Player HP";

            labelPlayerEnergy.AutoSize = true;
            labelPlayerEnergy.Location = new Point(30, 60);
            labelPlayerEnergy.Name = "labelPlayerEnergy";
            labelPlayerEnergy.Size = new Size(98, 20);
            labelPlayerEnergy.TabIndex = 1;
            labelPlayerEnergy.Text = "Player Energy";

            labelEnemyName.AutoSize = true;
            labelEnemyName.Location = new Point(400, 30);
            labelEnemyName.Name = "labelEnemyName";
            labelEnemyName.Size = new Size(97, 20);
            labelEnemyName.TabIndex = 2;
            labelEnemyName.Text = "Enemy Name";

            labelEnemyHP.AutoSize = true;
            labelEnemyHP.Location = new Point(400, 60);
            labelEnemyHP.Name = "labelEnemyHP";
            labelEnemyHP.Size = new Size(76, 20);
            labelEnemyHP.TabIndex = 3;
            labelEnemyHP.Text = "Enemy HP";

            labelScore.AutoSize = true;
            labelScore.Location = new Point(30, 90);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(46, 20);
            labelScore.TabIndex = 4;
            labelScore.Text = "Score";

            buttonAtk1.Location = new Point(30, 154);
            buttonAtk1.Name = "buttonAtk1";
            buttonAtk1.Size = new Size(150, 40);
            buttonAtk1.TabIndex = 5;
            buttonAtk1.Text = "Attack 1";
            buttonAtk1.UseVisualStyleBackColor = true;
            buttonAtk1.Click += button1_Click;

            buttonAtk2.Location = new Point(30, 200);
            buttonAtk2.Name = "buttonAtk2";
            buttonAtk2.Size = new Size(150, 40);
            buttonAtk2.TabIndex = 6;
            buttonAtk2.Text = "Attack 2";
            buttonAtk2.UseVisualStyleBackColor = true;
            buttonAtk2.Click += button2_Click;

            buttonAtk3.Location = new Point(30, 250);
            buttonAtk3.Name = "buttonAtk3";
            buttonAtk3.Size = new Size(150, 40);
            buttonAtk3.TabIndex = 7;
            buttonAtk3.Text = "Attack 3";
            buttonAtk3.UseVisualStyleBackColor = true;
            buttonAtk3.Click += button3_Click;

            textBoxLog.Location = new Point(220, 150);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ScrollBars = ScrollBars.Vertical;
            textBoxLog.Size = new Size(350, 250);
            textBoxLog.TabIndex = 8;

            textBoxUpgrades.Location = new Point(41, 341);
            textBoxUpgrades.Name = "textBoxUpgrades";
            textBoxUpgrades.Size = new Size(125, 27);
            textBoxUpgrades.TabIndex = 9;

            buttonUpg.Location = new Point(30, 374);
            buttonUpg.Name = "buttonUpg";
            buttonUpg.Size = new Size(150, 40);
            buttonUpg.TabIndex = 10;
            buttonUpg.Text = "Upgrade";
            buttonUpg.UseVisualStyleBackColor = true;
            buttonUpg.Click += buttonUpg_Click;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 450);
            Controls.Add(buttonUpg);
            Controls.Add(textBoxUpgrades);
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

        private Label labelPlayerHP;
        private Label labelPlayerEnergy;
        private Label labelEnemyName;
        private Label labelEnemyHP;
        private Label labelScore;
        private Button buttonAtk1;
        private Button buttonAtk2;
        private Button buttonAtk3;
        private TextBox textBoxLog;
        private TextBox textBoxUpgrades;
        private Button buttonUpg;
    }
}
