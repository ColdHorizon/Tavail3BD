using Microsoft.Data.SqlClient;
using System;
using System.Collections;
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

        DataTable abilityTable = new DataTable();



        public GameForm(int id)
        {
            InitializeComponent();
            playerId = id;
            this.Load += GameForm_Load;
            textBoxUpgrades.Enabled = false;
            buttonUpg.Enabled = false;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            LoadPlayer();
            LoadPlayerAbilities();
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

        private void LoadPlayerAbilities()
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT abilities.abilityName, abilities.abilityPower, abilities.abilityType " +
                    "FROM abilities " +
                    "JOIN class_abilities ON abilities.abilityId = class_abilities.abilityId " +
                    "JOIN players ON players.classId = class_abilities.classId " +
                    "WHERE players.playerId = @id",
                    c
                );

                da.SelectCommand.Parameters.AddWithValue("@id", playerId);

                da.Fill(abilityTable);
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

            if (abilityTable.Rows.Count > 0)
                buttonAtk1.Text = abilityTable.Rows[0]["abilityName"].ToString();
            else
                buttonAtk1.Text = "-";

            if (abilityTable.Rows.Count > 1)
                buttonAtk2.Text = abilityTable.Rows[1]["abilityName"].ToString();
            else
                buttonAtk2.Text = "-";

            if (abilityTable.Rows.Count > 2)
                buttonAtk3.Text = abilityTable.Rows[2]["abilityName"].ToString();
            else
                buttonAtk3.Text = "-";
        }

        private void PlayerAttack(int power, int cost, string abilityName, string abilityType)
        {
            if (playerEnergy < cost)
            {
                MessageBox.Show("Pas assez d'énergie");
                return;
            }

            playerEnergy -= cost;

            if (abilityType.StartsWith("chance-hit"))
            {
                int roll = rng.Next(100);
                if (roll < 10)
                    power *= 3;
                else if (roll < 25)
                    power *= 2;
            }

            int dmg = power + playerAtk - enemyDef;
            if (dmg < 0) dmg = 0;

            enemyHp -= dmg;

            textBoxLog.AppendText("Tu utilises " + abilityName + " et infliges " + dmg + " dégâts.\r\n");

            if (enemyHp <= 0)
            {
                EnemyDefeated();
                Upgrade();
                return;
            }

            EnemyTurn();
            UpdateUI();
        }
        private void Upgrade()
        {
            

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            string info = "select upgradeName,upgradeValue,upgradetype from upgrades where upgradeid = @id";



            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM upgrades", c);
                DataTable table = new DataTable();
                da.Fill(table);
                int count = table.Rows.Count;
                textBoxLog.AppendText("Choose an upgrade ");
                textBoxUpgrades.Enabled = true;
                buttonAtk1.Enabled = false;
                buttonAtk2.Enabled = false;
                buttonAtk3.Enabled = false;
                buttonUpg.Enabled = true;
                for (int i = 1; i <= count; i++)
                {

                    SqlCommand infoUpgrades = new SqlCommand(info, c);
                    infoUpgrades.Parameters.AddWithValue("@id", i);
                    using (SqlDataReader reader = infoUpgrades.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Name = reader["upgradeName"].ToString();
                            string Value = reader["upgradeValue"].ToString();
                            string Type = reader["upgradetype"].ToString();
                            textBoxLog.AppendText("" + Name + " Value  " + Value + " upgradetype " + Type + ". ");
                        }
                    }
                }
            }

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
            enemyHp = 0;
            textBoxLog.AppendText(enemyName + " est vaincu !\r\n");

            score++;
            playerEnergy++;
            if (playerEnergy > 3) playerEnergy = 3;

            int heal = (int)(playerMaxHp * 0.20);
            playerHp += heal;
            if (playerHp > playerMaxHp) playerHp = playerMaxHp;

            
            UpdateUI();
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over ! Score final : " + score);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (abilityTable.Rows.Count > 0)
            {
                string name = abilityTable.Rows[0]["abilityName"].ToString();
                int power = (int)abilityTable.Rows[0]["abilityPower"];
                string type = abilityTable.Rows[0]["abilityType"].ToString();
                PlayerAttack(power, 0, name, type);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (abilityTable.Rows.Count > 1)
            {
                string name = abilityTable.Rows[1]["abilityName"].ToString();
                int power = (int)abilityTable.Rows[1]["abilityPower"];
                string type = abilityTable.Rows[1]["abilityType"].ToString();
                PlayerAttack(power, 1, name, type);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (abilityTable.Rows.Count > 2)
            {
                string name = abilityTable.Rows[2]["abilityName"].ToString();
                int power = (int)abilityTable.Rows[2]["abilityPower"];
                string type = abilityTable.Rows[2]["abilityType"].ToString();
                PlayerAttack(power, 0, name, type);
            }
        }

        private void GameForm_Load_1(object sender, EventArgs e)
        {

        }

        private void buttonUpg_Click(object sender, EventArgs e)
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            textBoxUpgrades.Enabled = false;
            buttonAtk1.Enabled = true;
            buttonAtk2.Enabled = true;
            buttonAtk3.Enabled = true;
            buttonUpg.Enabled = false;
            LoadEnemy();
            UpdateUI();
        }
    }
}
