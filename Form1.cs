using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColorsDB {
    public partial class Form1 : Form {
        string connectionString;
        SqlConnection connection;
        public Form1() {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ColorsDB.Properties.Settings.ColorsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e) {
            PopulateColorsTable();
        }

        private void PopulateColorsTable() {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PrimaryColor", connection))
            {
                DataTable colorsTable = new DataTable();
                adapter.Fill(colorsTable);

                listPrimaryColors.DisplayMember = "PrimaryColorName";
                listPrimaryColors.ValueMember = "Id";
                listPrimaryColors.DataSource = colorsTable;
            }
        }

        private void listPrimaryColors_SelectedIndexChanged(object sender, EventArgs e) {
            PopulateShades();
        }

        private void PopulateShades() {
            string query = "SELECT Shades.ColorName FROM PrimaryColor INNER JOIN Shades ON Shades.PrimaryColorId = PrimaryColor.Id WHERE PrimaryColor.Id = @PrimaryColorId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@PrimaryColorId", listPrimaryColors.SelectedValue);
                DataTable shadesTable = new DataTable();
                adapter.Fill(shadesTable);

                listShades.DisplayMember = "ColorName";
                listShades.ValueMember = "Id";
                listShades.DataSource = shadesTable;
            }
        }
    }
}
