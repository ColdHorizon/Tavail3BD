using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Travail3BD
{
    public partial class GameForm : Form
    {
        int playerId = 1;

        int playerMaxHp;
        int playerHp;
        int playerAtk;
        int playerDef;
        int playerEnergy = 3;
        int score = 0;

        int enemyId;
        string enemyName;
        int enemyHp;
        int enemyAtk;
        int enemyDef;

        Random rng = new Random();

        public GameForm()
        {
            InitializeComponent();
        }

        public GameForm(int id)
        {
            InitializeComponent();
            playerId = id;
            this.Load += GameForm_Load;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            LoadPlayer();
            LoadEnemy();
            UpdateUI();
        }

        private void LoadPlayer()
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                SqlCommand cmd = new SqlCommand("SELECT hp, atk, def FROM players WHERE playerId = @id", c);
                cmd.Parameters.AddWithValue("@id", playerId);

                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    playerMaxHp = r.GetInt32(0);
                    playerHp = playerMaxHp;
                    playerAtk = r.GetInt32(1);
                    playerDef = r.GetInt32(2);
                }

                r.Close();
            }
        }

        private void LoadEnemy()
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM enemies", c);
                DataTable dt = new DataTable();
                da.Fill(dt);

                int index = rng.Next(dt.Rows.Count);
                DataRow row = dt.Rows[index];

                enemyId = (int)row["enemyId"];
                enemyName = row["enemyName"].ToString();
                enemyHp = (int)row["hp"];
                enemyAtk = (int)row["atk"];
                enemyDef = (int)row["def"];

                double scale = 1 + (score * 0.10);
                enemyHp = (int)(enemyHp * scale);
                enemyAtk = (int)(enemyAtk * scale);
                enemyDef = (int)(enemyDef * scale);
            }
        }

        private void UpdateUI()
        {
            labelPlayerHP.Text = "HP Joueur : " + playerHp + "/" + playerMaxHp;
            labelPlayerEnergy.Text = "Énergie : " + playerEnergy;
            labelEnemyName.Text = enemyName;
            labelEnemyHP.Text = "HP Ennemi : " + enemyHp;
            labelScore.Text = "Score : " + score;
        }

        private void PlayerAttack(int power, int cost)
        {
            if (playerEnergy < cost)
            {
                MessageBox.Show("Pas assez d'énergie");
                return;
            }

            playerEnergy -= cost;

            int dmg = power + playerAtk - enemyDef;
            if (dmg < 0) dmg = 0;

            enemyHp -= dmg;

            textBoxLog.AppendText("Tu attaques et infliges " + dmg + " dégâts.\r\n");

            if (enemyHp <= 0)
            {
                EnemyDefeated();
                return;
            }

            EnemyTurn();
            UpdateUI();
        }

        private void EnemyTurn()
        {
            int dmg = enemyAtk - playerDef;
            if (dmg < 0) dmg = 0;

            playerHp -= dmg;

            textBoxLog.AppendText(enemyName + " attaque et inflige " + dmg + " dégâts.\r\n");

            if (playerHp <= 0)
            {
                GameOver();
            }
        }

        private void EnemyDefeated()
        {
            textBoxLog.AppendText(enemyName + " est vaincu !\r\n");

            score++;
            playerEnergy++;
            if (playerEnergy > 3) playerEnergy = 3;

            int heal = (int)(playerMaxHp * 0.20);
            playerHp += heal;
            if (playerHp > playerMaxHp) playerHp = playerMaxHp;

            LoadEnemy();
            UpdateUI();
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over ! Score final : " + score);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerAttack(10, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayerAttack(20, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlayerAttack(5, 0);
        }
    }
}
