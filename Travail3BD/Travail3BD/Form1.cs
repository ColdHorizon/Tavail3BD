using System.Data;
using Microsoft.Data.SqlClient;

namespace Travail3BD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Charge la liste des joueurs au démarrage
            LoadPlayers();
        }

        // Charge tous les joueurs depuis la base de données
        private void LoadPlayers()
        {
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            DataTable dt = new DataTable();

            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                connexion.Open();

                // Récupère tous les joueurs
                SqlDataAdapter adapter_programmes = new SqlDataAdapter("SELECT * FROM players", connexion);
                adapter_programmes.Fill(dt);
            }

            // Affiche les noms dans la ComboBox
            comboBoxPly.DataSource = dt;
            comboBoxPly.DisplayMember = "playerName";
        }

        // Bouton : créer un nouveau joueur
        private void buttonNew_Click(object sender, EventArgs e)
        {
            CreatePlayer p = new CreatePlayer();
            p.ShowDialog();

            // Recharge la liste après création
            LoadPlayers();
        }

        private void comboBoxPly_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Rien ici (événement non utilisé)
        }

        // Bouton : utiliser le joueur sélectionné
        private void buttonUsePly_Click(object sender, EventArgs e)
        {
            if (comboBoxPly.SelectedItem == null)
            {
                MessageBox.Show("Sélectionne un joueur.");
                return;
            }

            // Récupère l'ID du joueur sélectionné
            DataRowView row = comboBoxPly.SelectedItem as DataRowView;
            int playerId = (int)row["playerId"];

            // Ouvre la fenêtre de jeu avec ce joueur
            GameForm game = new GameForm(playerId);
            game.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Non utilisé
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Non utilisé
        }
    }
}
