using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TickIT.Forms
{
    public partial class ChangePasswordForm : Form
    {
        private string connectionString = "Data Source=TickIT.db;Version=3;";

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            string email = textBox_Email.Text.Trim();
            string currentPassword = textBox_CurrentPassword.Text;
            string newPassword = textBox_NewPassword.Text;
            string repeatPassword = textBox_RepeatPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(currentPassword) ||
                string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(repeatPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != repeatPassword)
            {
                MessageBox.Show("Nowe hasło i jego powtórzenie nie są zgodne.", "Error ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT PasswordHash FROM Users WHERE Email = @Email";
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Email", email);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("User with provided email address does not exist in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string storedPassword = dt.Rows[0]["PasswordHash"].ToString();

                        if (currentPassword != storedPassword)
                        {
                            MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string updateQuery = "UPDATE Users SET PasswordHash = @NewPassword, IsPasswordChangeRequired = 0 WHERE Email = @Email";
                    using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password has been changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            LoginForm loginForm = new LoginForm();
                            loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Password change failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
