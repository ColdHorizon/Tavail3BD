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
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            DataTable dt = new DataTable();

            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                connexion.Open();
                SqlDataAdapter adapter_programmes = new SqlDataAdapter("SELECT * FROM players", connexion);
                adapter_programmes.Fill(dt);
            }

            comboBoxPly.DataSource = dt;
            comboBoxPly.DisplayMember = "playerName";
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            CreatePlayer p = new CreatePlayer();
            p.ShowDialog();
            LoadPlayers(); // refresh list after creating a character
        }

        private void comboBoxPly_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonUsePly_Click(object sender, EventArgs e)
        {
            if (comboBoxPly.SelectedItem == null)
            {
                MessageBox.Show("Sélectionne un joueur.");
                return;
            }

            DataRowView row = comboBoxPly.SelectedItem as DataRowView;
            int playerId = (int)row["playerId"];

            GameForm game = new GameForm(playerId);
            game.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
