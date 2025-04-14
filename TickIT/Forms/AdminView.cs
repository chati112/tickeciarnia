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

                    string query = "SELECT UserID, FirstName, LastName, Email, Phone, Role, IsInactive FROM Users"; // Pobranie użytkowników

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView_Users.DataSource = dt;
                        FormatDataGridView(); // Formatowanie kolumn

                        // szare tło dla nieaktywnych userow
                        foreach (DataGridViewRow row in dataGridView_Users.Rows)
                        {
                            if (row.Cells["IsInactive"].Value != null && Convert.ToInt32(row.Cells["IsInactive"].Value) == 1)
                            {
                                row.DefaultCellStyle.BackColor = Color.LightGray; // Szare tło dla nieaktywnych
                                row.DefaultCellStyle.ForeColor = Color.DarkGray;  // Przyciemnienie tekstu dla czytelności
                            }
                        }
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
            dataGridView_Users.ReadOnly = true;
            dataGridView_Users.Columns["IsInactive"].Visible = false;
            dataGridView_Users.AllowUserToAddRows = false;
            dataGridView_Users.AllowUserToDeleteRows = false;
            dataGridView_Users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Users.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView_Users.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView_Users.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_Users.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_Users.RowTemplate.Height = 30;
            dataGridView_Users.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView_Users.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView_Users.GridColor = Color.Black;
        }



        private void btn_DeactivateUser_Click(object sender, EventArgs e)
        {

            if (dataGridView_Users.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznaczyć użytkownika do dezaktywacji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridView_Users.SelectedRows[0].Cells["UserID"].Value);

            DialogResult result = MessageBox.Show("Czy na pewno chcesz dezaktywować tego użytkownika?",
                "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE Users SET IsInactive = 1 WHERE UserID = @UserID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Użytkownik został dezaktywowany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się dezaktywować użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Błąd bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            // Dane z formularza
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();
            string password = "1234"; // domyślne hasło – możesz tu dodać generator

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Wypełnij wszystkie wymagane pola.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Sprawdzenie, czy użytkownik o takim emailu już istnieje
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        long count = (long)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Użytkownik o podanym adresie email już istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Dodanie nowego użytkownika
                    string insertQuery = @"INSERT INTO Users (FirstName, LastName, Email, Phone, Role, PasswordHash, IsInactive, IsPasswordChangeRequired) 
                                   VALUES (@FirstName, @LastName, @Email, @Phone, @Role, @PasswordHash, 0, 1)";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                    {
                        adapter.InsertCommand = new SQLiteCommand(insertQuery, conn);
                        adapter.InsertCommand.Parameters.AddWithValue("@FirstName", firstName);
                        adapter.InsertCommand.Parameters.AddWithValue("@LastName", lastName);
                        adapter.InsertCommand.Parameters.AddWithValue("@Email", email);
                        adapter.InsertCommand.Parameters.AddWithValue("@Phone", phone);
                        adapter.InsertCommand.Parameters.AddWithValue("@Role", role);
                        adapter.InsertCommand.Parameters.AddWithValue("@PasswordHash", password); // możesz hashować

                        int rows = adapter.InsertCommand.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Użytkownik został dodany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView(); // odświeżenie grida
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się dodać użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Błąd SQLite: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message);
            }
        }

        private void dataGridView_Users_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_Users.Rows[e.RowIndex];

                numericUpDown3.Value = Convert.ToInt32(row.Cells["UserID"].Value);
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                cmbRole.Text = row.Cells["Role"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int userId = (int)numericUpDown3.Value;
            if (userId == 0)
            {
                MessageBox.Show("Najpierw wybierz użytkownika z listy.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.Text;

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE Users SET 
                             FirstName = @FirstName, 
                             LastName = @LastName, 
                             Email = @Email, 
                             Phone = @Phone, 
                             Role = @Role 
                             WHERE UserID = @UserID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dane użytkownika zostały zaktualizowane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Aktualizacja nie powiodła się.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Błąd bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
