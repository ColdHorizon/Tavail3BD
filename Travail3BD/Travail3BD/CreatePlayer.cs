using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                SqlDataAdapter Classes = new SqlDataAdapter("select className from classes", connexion);
                DataTable dt = new DataTable();
                Classes.Fill(dt);

                comboBoxClass.DataSource = dt;
                comboBoxClass.DisplayMember = "className";
            }




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                DataTable dt = new DataTable();
                string classname = comboBoxClass.Text;
                SqlDataAdapter Class = new SqlDataAdapter("select atkBase,defBase,hpBase from classes where className = @class", connexion);
                Class.SelectCommand.Parameters.AddWithValue("@class", classname);
                Class.Fill(dt);
                dataGridView1.DataSource = dt.DefaultView;
                
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string chaine_connexion = @"Server=localhost;Database=FurryRPG;Trusted_Connection=True;TrustServerCertificate=True";
            using (SqlConnection connexion = new SqlConnection(chaine_connexion))
            {
                connexion.Open();
                SqlDataAdapter NewPlayer = new SqlDataAdapter("insert into players(playerId,playerName,hp,atk,def,classID) values(1,@playerName", connexion);
                string playerName = textBoxName.Text;
                NewPlayer.SelectCommand.Parameters.AddWithValue("@playerName", playerName);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
