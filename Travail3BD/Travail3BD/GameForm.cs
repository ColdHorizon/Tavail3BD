using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Media;
using System.Windows.Forms;

namespace Travail3BD
{
    public partial class GameForm : Form
    {
        SoundPlayer combat = new SoundPlayer();
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
        int enemyMaxHp;
        int enemyAtk;
        int enemyDef;

        int enemyAbilityPower;
        string enemyAbilityType;

        int projectionCharges = 0;
        bool upgradePending = false;

        int focusBonus = 0;

        bool enemySkipTurn = false;
        bool enemyBackstabbed = false;

        Random rng = new Random();
        DataTable abilityTable = new DataTable();

        public GameForm(int id)
        {
            InitializeComponent();

            try
            {
                combat.SoundLocation = @"4-01. TOTSUGEKI.wav";
                combat.PlayLooping();
            }
            catch
            {
            }

            playerId = id;
            this.Load += GameForm_Load;

            textBoxUpgrades.Enabled = false;
            buttonUpg.Enabled = false;

            textBoxLog.ReadOnly = true;
            textBoxLog.TabStop = false;
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
                SqlCommand cmd = new SqlCommand(
                    "SELECT classes.hpBase, classes.atkBase, classes.defBase " +
                    "FROM players " +
                    "JOIN classes ON players.classId = classes.classId " +
                    "WHERE players.playerId = @id", c);

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
            abilityTable = new DataTable();

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

            DataRow basic = abilityTable.NewRow();
            basic["abilityName"] = "Basic Attack";
            basic["abilityPower"] = 0;
            basic["abilityType"] = "basic";
            abilityTable.Rows.InsertAt(basic, 0);

            DataRow focus = abilityTable.NewRow();
            focus["abilityName"] = "Focus";
            focus["abilityPower"] = 0;
            focus["abilityType"] = "focus";
            abilityTable.Rows.InsertAt(focus, 1);
        }

        private void LoadEnemy()
        {
            enemySkipTurn = false;
            enemyBackstabbed = false;
            projectionCharges = 0;

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

                enemyMaxHp = enemyHp;

                SqlDataAdapter da2 = new SqlDataAdapter(
                    "SELECT a.abilityPower, a.abilityType " +
                    "FROM enemy_abilities ea " +
                    "JOIN abilities a ON ea.abilityId = a.abilityId " +
                    "WHERE ea.enemyId = @id", c);
                da2.SelectCommand.Parameters.AddWithValue("@id", enemyId);
                DataTable ab = new DataTable();
                da2.Fill(ab);

                if (ab.Rows.Count > 0)
                {
                    enemyAbilityPower = (int)ab.Rows[0]["abilityPower"];
                    enemyAbilityType = ab.Rows[0]["abilityType"].ToString();
                }
                else
                {
                    enemyAbilityPower = 0;
                    enemyAbilityType = "basic";
                }
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
            if (abilityTable.Rows.Count > 1)
                buttonAtk2.Text = abilityTable.Rows[1]["abilityName"].ToString();
            if (abilityTable.Rows.Count > 2)
                buttonAtk3.Text = abilityTable.Rows[2]["abilityName"].ToString();
        }

        private void PlayerAttack(int power, string abilityName, string abilityType)
        {
            if (abilityType == "focus")
            {
                int restore = 1 + focusBonus;
                playerEnergy += restore;
                if (playerEnergy > 3) playerEnergy = 3;

                textBoxLog.AppendText("Tu te concentres et récupères " + restore + " énergie.\r\n");
                EnemyTurn();
                UpdateUI();
                return;
            }

            int cost = 0;

            if (abilityType == "basic")
                cost = 0;
            else if (abilityType == "barrage" || abilityType == "backstab" || abilityType == "rewind")
                cost = 2;
            else
                cost = 1;

            if (playerEnergy < cost)
            {
                textBoxLog.AppendText("Pas assez d'énergie.\r\n");
                return;
            }

            playerEnergy -= cost;

            if (abilityType == "basic")
            {
                power = playerAtk * 2;
            }

            if (abilityType == "rewind")
            {
                int heal = (int)(playerMaxHp * 0.25);
                playerHp += heal;
                if (playerHp > playerMaxHp) playerHp = playerMaxHp;
                textBoxLog.AppendText("Tu utilises Rewind et récupères " + heal + " HP.\r\n");
                EnemyTurn();
                UpdateUI();
                return;
            }

            if (abilityType == "projection")
            {
                projectionCharges++;
                textBoxLog.AppendText("Projection charge (" + projectionCharges + "/3).\r\n");

                if (projectionCharges >= 3)
                {
                    power *= 3;
                    playerHp += 1;
                    if (playerHp > playerMaxHp) playerHp = playerMaxHp;

                    textBoxLog.AppendText("Projection burst! Triple dégâts +1 HP.\r\n");
                    projectionCharges = 0;
                }
            }

            if (abilityType == "barrage")
            {
                int hits = 0;
                int chance = 100;
                int totalDmg = 0;

                while (true)
                {
                    int roll = rng.Next(100);
                    if (roll >= chance)
                        break;

                    hits++;
                    int hitDmg = power + playerAtk - enemyDef;
                    if (hitDmg < 0) hitDmg = 0;
                    totalDmg += hitDmg;
                    chance -= 5;
                }

                enemyHp -= totalDmg;
                textBoxLog.AppendText("Tu utilises Barrage et touches " + hits + " fois pour " + totalDmg + " dégâts.\r\n");

                if (enemyHp <= 0)
                {
                    EnemyDefeated();
                    Upgrade();
                    return;
                }

                EnemyTurn();
                UpdateUI();
                return;
            }

            if (abilityType == "double-or-nothing")
            {
                int baseDmg = power + playerAtk - enemyDef;
                if (baseDmg < 0) baseDmg = 0;

                int roll = rng.Next(100);
                int dmg = 0;

                if (roll < 50)
                {
                    dmg = baseDmg * 2;
                    textBoxLog.AppendText("Double or Nothing réussit !\r\n");
                }
                else
                {
                    dmg = 0;
                    textBoxLog.AppendText("Double or Nothing échoue... 0 dégâts.\r\n");
                }

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
                return;
            }

            if (abilityType.StartsWith("chance-hit"))
            {
                int roll = rng.Next(100);
                if (roll < 10)
                    power *= 3;
                else if (roll < 25)
                    power *= 2;
            }

            int finalDmg = power + playerAtk - enemyDef;
            if (finalDmg < 0) finalDmg = 0;

            enemyHp -= finalDmg;
            textBoxLog.AppendText("Tu utilises " + abilityName + " et infliges " + finalDmg + " dégâts.\r\n");

            if (abilityType == "quick-jab")
            {
                int roll = rng.Next(100);
                if (roll < 40)
                {
                    enemySkipTurn = true;
                    textBoxLog.AppendText("Quick Jab réussit ! L'ennemi va sauter son prochain tour.\r\n");
                }
            }

            if (abilityType == "backstab")
            {
                enemyBackstabbed = true;
                textBoxLog.AppendText("Backstab appliqué ! L'ennemi perdra 25% de ses HP max à chaque attaque.\r\n");
            }

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
            if (upgradePending)
                return;

            upgradePending = true;

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT upgradeId, upgradeName, upgradeValue FROM upgrades", c);
                DataTable table = new DataTable();
                da.Fill(table);

                textBoxLog.AppendText("Choose an upgrade:\r\n");

                textBoxUpgrades.Enabled = true;
                buttonAtk1.Enabled = false;
                buttonAtk2.Enabled = false;
                buttonAtk3.Enabled = false;
                buttonUpg.Enabled = true;

                foreach (DataRow row in table.Rows)
                {
                    string id = row["upgradeId"].ToString();
                    string name = row["upgradeName"].ToString();
                    string value = row["upgradeValue"].ToString();

                    textBoxLog.AppendText(id + ": " + name + " (" + value + ")\r\n");
                }
            }
        }

        private void EnemyTurn()
        {
            if (enemyHp <= 0)
                return;

            if (enemySkipTurn)
            {
                textBoxLog.AppendText(enemyName + " est étourdi et saute son tour.\r\n");
                enemySkipTurn = false;
                return;
            }

            if (enemyBackstabbed)
            {
                int dot = (int)(enemyMaxHp * 0.25);
                enemyHp -= dot;
                textBoxLog.AppendText(enemyName + " subit " + dot + " dégâts de Backstab.\r\n");

                if (enemyHp <= 0)
                {
                    EnemyDefeated();
                    Upgrade();
                    return;
                }
            }

            if (enemyAbilityType == "miss25")
            {
                int roll = rng.Next(100);
                if (roll < 25)
                {
                    textBoxLog.AppendText(enemyName + " rate son attaque !\r\n");
                    return;
                }
            }

            int dmg;

            if (enemyAbilityType == "basic")
            {
                dmg = enemyAtk + enemyAbilityPower - playerDef;
                if (dmg < 1) dmg = 1;
            }
            else
            {
                float reduction = playerDef / 100f;
                if (reduction > 0.90f) reduction = 0.90f;

                dmg = (int)Math.Floor(enemyAtk * (1 - reduction));
                if (dmg < 1) dmg = 1;
            }

            playerHp -= dmg;

            textBoxLog.AppendText(enemyName + " attaque et inflige " + dmg + " dégâts.\r\n");

            if (playerHp <= 0)
                GameOver();
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
            combat.Stop();
            MessageBox.Show("Game Over ! Score final : " + score);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = abilityTable.Rows[0]["abilityName"].ToString();
            int power = (int)abilityTable.Rows[0]["abilityPower"];
            string type = abilityTable.Rows[0]["abilityType"].ToString();
            PlayerAttack(power, name, type);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = abilityTable.Rows[1]["abilityName"].ToString();
            int power = (int)abilityTable.Rows[1]["abilityPower"];
            string type = abilityTable.Rows[1]["abilityType"].ToString();
            PlayerAttack(power, name, type);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = abilityTable.Rows[2]["abilityName"].ToString();
            int power = (int)abilityTable.Rows[2]["abilityPower"];
            string type = abilityTable.Rows[2]["abilityType"].ToString();
            PlayerAttack(power, name, type);
        }

        private void buttonUpg_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxUpgrades.Text, out int choice))
                return;

            if (choice == 1)
            {
                playerMaxHp += 2;
                playerHp += 2;
                textBoxLog.AppendText("HP increased.\r\n");
            }
            else if (choice == 2)
            {
                playerAtk += 2;
                textBoxLog.AppendText("ATK increased.\r\n");
            }
            else if (choice == 3)
            {
                playerDef += 2;
                textBoxLog.AppendText("DEF increased.\r\n");
            }
            else if (choice == 4)
            {
                focusBonus += 1;
                textBoxLog.AppendText("Focus restores +1 more energy.\r\n");
            }
            else
                return;

            upgradePending = false;

            textBoxUpgrades.Enabled = false;
            buttonAtk1.Enabled = true;
            buttonAtk2.Enabled = true;
            buttonAtk3.Enabled = true;
            buttonUpg.Enabled = false;

            textBoxUpgrades.Text = "";

            LoadEnemy();
            UpdateUI();
        }
    }
}
