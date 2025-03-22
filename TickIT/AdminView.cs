using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TickIT
{
    public partial class AdminView : Form
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT UserID, FullName, Email, Phone, Role FROM Users"; // Pobranie wszystkich użytkowników

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView_Users.DataSource = dt;
                        FormatDataGridView(); // Ponowne formatowanie
                    }
                }
            }
            catch (SQLiteException ex2)
            {
                MessageBox.Show("SQLite Error: " + ex2.Message);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Error: " + ex1.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormatDataGridView()
        {
            // Zablokowanie edycji
            dataGridView_Users.ReadOnly = true;

            // Ukrycie pustego wiersza na końcu
            dataGridView_Users.AllowUserToAddRows = false;
            dataGridView_Users.AllowUserToDeleteRows = false;

            // Auto-dopasowanie szerokości kolumn do zawartości
            dataGridView_Users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Us   tawienie nagłówków na pogrubione
            dataGridView_Users.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView_Users.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView_Users.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Wyrównanie tekstu w komórkach do środka
            dataGridView_Users.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Ustawienie wysokości wierszy
            dataGridView_Users.RowTemplate.Height = 30;

            // Zmiana koloru naprzemiennych wierszy dla lepszej czytelności
            dataGridView_Users.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;

            // Ustawienie obramowania komórek
            dataGridView_Users.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView_Users.GridColor = Color.Black;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
