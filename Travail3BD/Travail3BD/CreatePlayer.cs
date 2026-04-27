using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Travail3BD
{
    public partial class CreatePlayer : Form
    {
        public CreatePlayer()
        {
            InitializeComponent();
        }

        private void CreatePlayer_Load(object sender, EventArgs e)
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT className FROM classes", c);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBoxClass.DataSource = dt;
                comboBoxClass.DisplayMember = "className";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                string className = comboBoxClass.Text;

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT atkBase, defBase, hpBase FROM classes WHERE className = @c",
                    c
                );

                da.SelectCommand.Parameters.AddWithValue("@c", className);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string className = comboBoxClass.Text;

            if (name == "")
            {
                MessageBox.Show("Entre un nom.");
                return;
            }

            string conn = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection c = new SqlConnection(conn))
            {
                c.Open();

                // Get class stats
                SqlCommand getClass = new SqlCommand(
                    "SELECT hpBase, atkBase, defBase, classId FROM classes WHERE className = @c",
                    c
                );
                getClass.Parameters.AddWithValue("@c", className);

                SqlDataReader r = getClass.ExecuteReader();
                r.Read();

                int hp = r.GetInt32(0);
                int atk = r.GetInt32(1);
                int def = r.GetInt32(2);
                int classId = r.GetInt32(3);

                r.Close();

                Random rng = new Random();
                int newId = rng.Next(1, 9999);

                SqlCommand insert = new SqlCommand(
                    "INSERT INTO players (playerId, playerName, hp, atk, def, classId) VALUES (@id, @name, @hp, @atk, @def, @classId)",
                    c
                );

                insert.Parameters.AddWithValue("@id", newId);
                insert.Parameters.AddWithValue("@name", name);
                insert.Parameters.AddWithValue("@hp", hp);
                insert.Parameters.AddWithValue("@atk", atk);
                insert.Parameters.AddWithValue("@def", def);
                insert.Parameters.AddWithValue("@classId", classId);

                insert.ExecuteNonQuery();
            }

            MessageBox.Show("Personnage créé !");
            this.Close();
        }
    }
}
