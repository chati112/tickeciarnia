using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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

                    string query = "SELECT UserID, FirstName, LastName, Email, Phone, Role, IsInactive FROM Users";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView_Users.DataSource = dt;
                        FormatDataGridView(); 

                        foreach (DataGridViewRow row in dataGridView_Users.Rows)
                        {
                            if (row.Cells["IsInactive"].Value != null && Convert.ToInt32(row.Cells["IsInactive"].Value) == 1)
                            {
                                row.DefaultCellStyle.BackColor = Color.LightGray; 
                                row.DefaultCellStyle.ForeColor = Color.DarkGray;  
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
                MessageBox.Show("Proszę zaznaczyć użytkownika do aktywacji/dezaktywacji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridView_Users.SelectedRows[0].Cells["UserID"].Value);
            int isInactive = Convert.ToInt32(dataGridView_Users.SelectedRows[0].Cells["IsInactive"].Value);

            string action = isInactive == 1 ? "aktywować" : "dezaktywować";
            int newStatus = isInactive == 1 ? 0 : 1;

            DialogResult result = MessageBox.Show($"Czy na pewno chcesz {action} tego użytkownika?",
                "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            string connectionString = "Data Source=TickIT.db;Version=3;";
            string selectQuery = "SELECT * FROM Users";
            string updateQuery = "UPDATE Users SET IsInactive = @IsInactive WHERE UserID = @UserID";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    var adapter = new SQLiteDataAdapter(selectQuery, conn);
                    var commandBuilder = new SQLiteCommandBuilder(adapter); // choć niekonieczny tutaj

                    var dsUsers = new DataSet();
                    adapter.Fill(dsUsers, "Users");

                    DataRow[] rows = dsUsers.Tables["Users"].Select("UserID = " + userId);

                    if (rows.Length == 0)
                    {
                        MessageBox.Show("Nie znaleziono użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DataRow row = rows[0];
                    row["IsInactive"] = newStatus;

                    // Przypisz własny UpdateCommand
                    adapter.UpdateCommand = new SQLiteCommand(updateQuery, conn);
                    adapter.UpdateCommand.Parameters.Add("@IsInactive", DbType.Int32, 0, "IsInactive");
                    adapter.UpdateCommand.Parameters.Add("@UserID", DbType.Int32, 0, "UserID");

                    // W razie potrzeby: AcceptChangesDuringUpdate = false
                    adapter.Update(dsUsers, "Users");

                    string msg = newStatus == 1 ? "Użytkownik został dezaktywowany." : "Użytkownik został aktywowany.";
                    MessageBox.Show(msg, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView();
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

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();
            string password = "1234"; 

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
                        adapter.InsertCommand.Parameters.AddWithValue("@PasswordHash", password); 

                        int rows = adapter.InsertCommand.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Użytkownik został dodany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView(); 
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

                numericUpDownID_edit.Value = Convert.ToInt32(row.Cells["UserID"].Value);
                textBoxFirstname_edit.Text = row.Cells["FirstName"].Value.ToString();
                textBoxLastname_edit.Text = row.Cells["LastName"].Value.ToString();
                textBoxEmail_edit.Text = row.Cells["Email"].Value.ToString();
                textBoxPhone_edit.Text = row.Cells["Phone"].Value.ToString();
                comboBoxRole_edit.Text = row.Cells["Role"].Value.ToString();
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            int userId = (int)numericUpDownID_edit.Value;
            if (userId == 0)
            {
                MessageBox.Show("Najpierw wybierz użytkownika z listy.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string firstName = textBoxFirstname_edit.Text.Trim();
            string lastName = textBoxLastname_edit.Text.Trim();
            string email = textBoxEmail_edit.Text.Trim();
            string phone = textBoxPhone_edit.Text.Trim();
            string role = comboBoxRole_edit.SelectedItem?.ToString().Trim();

            if (role != "User" && role != "Technician" && role != "Admin")
            {
                MessageBox.Show("Wybierz poprawną rolę: User, Technician lub Admin.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT FirstName, LastName, Email, Phone, Role FROM Users WHERE UserID = @UserID";
                    DataTable dbData = new DataTable();

                    using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dbData);
                        }
                    }

                    if (dbData.Rows.Count == 0)
                    {
                        MessageBox.Show("Nie znaleziono użytkownika w bazie danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DataRow dbRow = dbData.Rows[0];

                    if (
                        dbRow["FirstName"].ToString() == firstName &&
                        dbRow["LastName"].ToString() == lastName &&
                        dbRow["Email"].ToString() == email &&
                        dbRow["Phone"].ToString() == phone &&
                        dbRow["Role"].ToString() == role
                    )
                    {
                        MessageBox.Show("Nie wprowadzono żadnych zmian.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string updateQuery = @"UPDATE Users SET 
                      FirstName = @FirstName, 
                      LastName = @LastName, 
                      Email = @Email, 
                      Phone = @Phone, 
                      Role = @Role 
                      WHERE UserID = @UserID";

                    using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@FirstName", firstName);
                        updateCmd.Parameters.AddWithValue("@LastName", lastName);
                        updateCmd.Parameters.AddWithValue("@Email", email);
                        updateCmd.Parameters.AddWithValue("@Phone", phone);
                        updateCmd.Parameters.AddWithValue("@Role", role);
                        updateCmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

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
