using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Media;
using System.Windows.Forms;

namespace Travail3BD
{
    public partial class GameForm : Form
    {
        // --- Variables du joueur ---
        SoundPlayer combat = new SoundPlayer();
        int playerId = 1;
        int playerMaxHp, playerHp, playerAtk, playerDef;
        int playerEnergy = 3;
        int score = 0;

        // --- Variables de l'ennemi ---
        int enemyId;
        string enemyName;
        int enemyHp, enemyMaxHp, enemyAtk, enemyDef;
        int enemyAbilityPower;
        string enemyAbilityType;

        // --- États spéciaux ---
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
            playerId = id;

            // Musique de combat
            try { combat.SoundLocation = @"4-01. TOTSUGEKI.wav"; combat.PlayLooping(); } catch { }

            // Empêche l'édition du log
            textBoxLog.ReadOnly = true;

            this.Load += GameForm_Load;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            LoadPlayer();          // Charge les stats du joueur
            LoadPlayerAbilities(); // Charge les abilities selon la classe
            LoadEnemy();           // Génère un ennemi
            UpdateUI();            // Met à jour l'affichage
        }

        // --- Charge les stats du joueur depuis SQL ---
        private void LoadPlayer()
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT classes.hpBase, classes.atkBase, classes.defBase " +
                    "FROM players JOIN classes ON players.classId = classes.classId " +
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
            }
        }

        // --- Charge les abilities du joueur ---
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
                    "WHERE players.playerId = @id", c);

                da.SelectCommand.Parameters.AddWithValue("@id", playerId);
                da.Fill(abilityTable);
            }

            // Ajoute Basic Attack
            DataRow basic = abilityTable.NewRow();
            basic["abilityName"] = "Basic Attack";
            basic["abilityPower"] = 0;
            basic["abilityType"] = "basic";
            abilityTable.Rows.InsertAt(basic, 0);

            // Ajoute Focus
            DataRow focus = abilityTable.NewRow();
            focus["abilityName"] = "Focus";
            focus["abilityPower"] = 0;
            focus["abilityType"] = "focus";
            abilityTable.Rows.InsertAt(focus, 1);
        }

        // --- Génère un ennemi selon le score ---
        private void LoadEnemy()
        {
            enemySkipTurn = false;
            enemyBackstabbed = false;
            projectionCharges = 0;

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                // Ennemis débloqués selon le score
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM enemies WHERE " +
                    "(enemyId = 1) OR " +
                    "(enemyId = 2 AND @s >= 3) OR " +
                    "(enemyId = 3 AND @s >= 5) OR " +
                    "(enemyId = 4 AND @s >= 10)", c);

                da.SelectCommand.Parameters.AddWithValue("@s", score);

                DataTable dt = new DataTable();
                da.Fill(dt);

                // Choix aléatoire
                DataRow row = dt.Rows[rng.Next(dt.Rows.Count)];

                enemyId = (int)row["enemyId"];
                enemyName = row["enemyName"].ToString();
                enemyHp = (int)row["hp"];
                enemyAtk = (int)row["atk"];
                enemyDef = (int)row["def"];

                // Scaling selon le score
                double scale = 1 + (score * 0.10);
                enemyHp = (int)(enemyHp * scale);
                enemyAtk = (int)(enemyAtk * scale);
                enemyDef = (int)(enemyDef * scale);

                enemyMaxHp = enemyHp;

                // Ability ennemie
                SqlDataAdapter da2 = new SqlDataAdapter(
                    "SELECT a.abilityPower, a.abilityType FROM enemy_abilities ea " +
                    "JOIN abilities a ON ea.abilityId = a.abilityId WHERE ea.enemyId = @id", c);

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

        // --- Met à jour l'affichage ---
        private void UpdateUI()
        {
            labelPlayerHP.Text = $"HP Joueur : {playerHp}/{playerMaxHp}";
            labelPlayerEnergy.Text = $"Énergie : {playerEnergy}";
            labelEnemyName.Text = enemyName;
            labelEnemyHP.Text = $"HP Ennemi : {enemyHp}";
            labelScore.Text = $"Score : {score}";

            if (abilityTable.Rows.Count > 0) buttonAtk1.Text = abilityTable.Rows[0]["abilityName"].ToString();
            if (abilityTable.Rows.Count > 1) buttonAtk2.Text = abilityTable.Rows[1]["abilityName"].ToString();
            if (abilityTable.Rows.Count > 2) buttonAtk3.Text = abilityTable.Rows[2]["abilityName"].ToString();
            if (abilityTable.Rows.Count > 3) buttonAtk4.Text = abilityTable.Rows[3]["abilityName"].ToString();
        }

        // --- Gestion des attaques du joueur ---
        private void PlayerAttack(int power, string name, string type)
        {
            // Focus = récupère énergie
            if (type == "focus")
            {
                int restore = 1 + focusBonus;
                playerEnergy = Math.Min(3, playerEnergy + restore);
                textBoxLog.AppendText($"Tu récupères {restore} énergie.\r\n");
                EnemyTurn();
                UpdateUI();
                return;
            }

            // Coût d'énergie
            int cost = (type == "basic") ? 0 :
                       (type == "barrage" || type == "backstab" || type == "rewind") ? 2 : 1;

            if (playerEnergy < cost)
            {
                textBoxLog.AppendText("Pas assez d'énergie.\r\n");
                return;
            }

            playerEnergy -= cost;

            // Basic Attack
            if (type == "basic")
                power = playerAtk * 2;

            // Rewind = soin
            if (type == "rewind")
            {
                int heal = (int)(playerMaxHp * 0.25);
                playerHp = Math.Min(playerMaxHp, playerHp + heal);
                textBoxLog.AppendText($"Rewind : +{heal} HP.\r\n");
                EnemyTurn();
                UpdateUI();
                return;
            }

            // Projection
            if (type == "projection")
            {
                projectionCharges++;
                textBoxLog.AppendText($"Projection ({projectionCharges}/3)\r\n");

                if (projectionCharges >= 3)
                {
                    power *= 3;
                    playerHp = Math.Min(playerMaxHp, playerHp + 1);
                    textBoxLog.AppendText("Projection Burst !\r\n");
                    projectionCharges = 0;
                }
            }

            // Barrage multi-hit
            if (type == "barrage")
            {
                int hits = 0, total = 0, chance = 100;

                while (rng.Next(100) < chance)
                {
                    hits++;
                    int dmg = Math.Max(0, power + playerAtk - enemyDef);
                    total += dmg;
                    chance -= 5;
                }

                enemyHp -= total;
                textBoxLog.AppendText($"Barrage : {hits} coups, {total} dégâts.\r\n");

                if (enemyHp <= 0) { EnemyDefeated(); Upgrade(); return; }

                EnemyTurn();
                UpdateUI();
                return;
            }

            // Double or Nothing
            if (type == "double-or-nothing")
            {
                int baseDmg = Math.Max(0, power + playerAtk - enemyDef);
                int dmg = (rng.Next(100) < 50) ? baseDmg * 2 : 0;

                textBoxLog.AppendText(dmg > 0 ? "Double réussi !\r\n" : "Échec...\r\n");

                enemyHp -= dmg;
                textBoxLog.AppendText($"{name} inflige {dmg} dégâts.\r\n");

                if (enemyHp <= 0) { EnemyDefeated(); Upgrade(); return; }

                EnemyTurn();
                UpdateUI();
                return;
            }

            // Chance-hit
            if (type.StartsWith("chance-hit"))
            {
                int roll = rng.Next(100);
                if (roll < 10) power *= 3;
                else if (roll < 25) power *= 2;
            }

            // Dégâts normaux
            int final = Math.Max(0, power + playerAtk - enemyDef);
            enemyHp -= final;
            textBoxLog.AppendText($"{name} inflige {final} dégâts.\r\n");

            // Quick Jab = étourdit
            if (type == "quick-jab" && rng.Next(100) < 40)
            {
                enemySkipTurn = true;
                textBoxLog.AppendText("L'ennemi est étourdi.\r\n");
            }

            // Backstab = DOT
            if (type == "backstab")
            {
                enemyBackstabbed = true;
                textBoxLog.AppendText("Backstab appliqué.\r\n");
            }

            if (enemyHp <= 0) { EnemyDefeated(); Upgrade(); return; }

            EnemyTurn();
            UpdateUI();
        }

        // --- Affiche les upgrades ---
        private void Upgrade()
        {
            if (upgradePending)
                return;

            upgradePending = true;

            // Affiche le message d'upgrade
            textBoxLog.AppendText("Choisissez un upgrade :\r\n");

            // Active la zone de saisie + bouton
            textBoxUpgrades.Enabled = true;
            buttonUpg.Enabled = true;

            // Désactive les attaques pendant le choix
            buttonAtk1.Enabled = false;
            buttonAtk2.Enabled = false;
            buttonAtk3.Enabled = false;
            buttonAtk4.Enabled = false;

            // Focus automatique dans la zone d'upgrade ---
            textBoxUpgrades.Focus();
            textBoxUpgrades.Select();
            textBoxUpgrades.SelectionStart = 0;

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM upgrades", c);
                DataTable t = new DataTable();
                da.Fill(t);

                // Affiche les options d'upgrade
                foreach (DataRow r in t.Rows)
                    textBoxLog.AppendText($"{r["upgradeId"]}: {r["upgradeName"]}\r\n");
            }
        }

        // --- Tour de l'ennemi ---
        private void EnemyTurn()
        {
            if (enemyHp <= 0) return;

            // Étourdi
            if (enemySkipTurn)
            {
                textBoxLog.AppendText("L'ennemi saute son tour.\r\n");
                enemySkipTurn = false;
                return;
            }

            // Backstab DOT
            if (enemyBackstabbed)
            {
                int dot = (int)(enemyMaxHp * 0.25);
                enemyHp -= dot;
                textBoxLog.AppendText($"Backstab : {dot} dégâts.\r\n");

                if (enemyHp <= 0) { EnemyDefeated(); Upgrade(); return; }
            }

            // Miss25
            if (enemyAbilityType == "miss25" && rng.Next(100) < 25)
            {
                textBoxLog.AppendText("L'ennemi rate son attaque.\r\n");
                return;
            }

            // Dégâts
            int dmg = Math.Max(1, enemyAtk + enemyAbilityPower - playerDef);
            playerHp -= dmg;

            textBoxLog.AppendText($"L'ennemi inflige {dmg} dégâts.\r\n");

            if (playerHp <= 0) GameOver();
        }

        // --- Ennemi vaincu ---
        private void EnemyDefeated()
        {
            textBoxLog.AppendText($"{enemyName} est vaincu !\r\n");

            score++;
            playerEnergy = Math.Min(3, playerEnergy + 1);

            int heal = (int)(playerMaxHp * 0.20);
            playerHp = Math.Min(playerMaxHp, playerHp + heal);

            UpdateUI();
        }

        // --- Fin de partie + sauvegarde du meilleur score ---
        private void GameOver()
        {
            combat.Stop();

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                SqlCommand read = new SqlCommand("SELECT bestScore FROM players WHERE playerId = @id", c);
                read.Parameters.AddWithValue("@id", playerId);
                int best = (int)read.ExecuteScalar();

                if (score > best)
                {
                    SqlCommand update = new SqlCommand(
                        "UPDATE players SET bestScore = @s WHERE playerId = @id", c);
                    update.Parameters.AddWithValue("@s", score);
                    update.Parameters.AddWithValue("@id", playerId);
                    update.ExecuteNonQuery();

                    MessageBox.Show("Nouveau record ! Score : " + score);
                }
                else
                {
                    MessageBox.Show("Game Over ! Score : " + score);
                }
            }

            this.Close();
        }

        // --- Boutons d'attaque ---
        private void button1_Click(object sender, EventArgs e)
        {
            string n = abilityTable.Rows[0]["abilityName"].ToString();
            int p = (int)abilityTable.Rows[0]["abilityPower"];
            string t = abilityTable.Rows[0]["abilityType"].ToString();
            PlayerAttack(p, n, t);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string n = abilityTable.Rows[1]["abilityName"].ToString();
            int p = (int)abilityTable.Rows[1]["abilityPower"];
            string t = abilityTable.Rows[1]["abilityType"].ToString();
            PlayerAttack(p, n, t);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string n = abilityTable.Rows[2]["abilityName"].ToString();
            int p = (int)abilityTable.Rows[2]["abilityPower"];
            string t = abilityTable.Rows[2]["abilityType"].ToString();
            PlayerAttack(p, n, t);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (abilityTable.Rows.Count <= 3) return;

            string n = abilityTable.Rows[3]["abilityName"].ToString();
            int p = (int)abilityTable.Rows[3]["abilityPower"];
            string t = abilityTable.Rows[3]["abilityType"].ToString();
            PlayerAttack(p, n, t);
        }

        // --- Bouton Upgrade ---
        private void buttonUpg_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxUpgrades.Text, out int choice))
                return;

            if (choice == 1) { playerMaxHp += 2; playerHp += 2; }
            else if (choice == 2) { playerAtk += 2; }
            else if (choice == 3) { playerDef += 2; }
            else if (choice == 4) { focusBonus += 1; }
            else return;

            upgradePending = false;

            textBoxUpgrades.Enabled = false;
            buttonUpg.Enabled = false;

            buttonAtk1.Enabled = true;
            buttonAtk2.Enabled = true;
            buttonAtk3.Enabled = true;
            buttonAtk4.Enabled = true;

            textBoxUpgrades.Text = "";

            // ⭐ IMPORTANT : recharge un nouvel ennemi
            LoadEnemy();
            UpdateUI();
        }
    }
}
