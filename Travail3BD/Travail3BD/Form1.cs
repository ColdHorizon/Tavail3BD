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
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            DataTable dt = new DataTable();
            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                
                connexion.Open();
                SqlDataAdapter adapter_programmes = new SqlDataAdapter("Select * FROM players", connexion);
                adapter_programmes.Fill(dt);

                comboBoxPly.DataSource = dt;
                comboBoxPly.DisplayMember = "playerName";


            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            CreatePlayer player = new CreatePlayer();
            player.ShowDialog();
        }

        private void comboBoxPly_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
