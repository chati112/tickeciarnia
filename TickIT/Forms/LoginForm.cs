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

namespace TickIT
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            string enteredUsername = textBox_email.Text.Trim();
            string enteredPassword = textBox_password.Text.Trim();

            if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Proszę wprowadzić email i hasło.", "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Email, Password, Role FROM Users WHERE Username = @Username";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", enteredUsername);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                string storedPassword = reader["Password"].ToString();
                                string role = reader["Role"].ToString();

                                if (enteredPassword == storedPassword) // dodać hashowanie
                                {
                                    MessageBox.Show($"Witaj {enteredUsername}!", "Logowanie udane", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Otwiera odpowiedni widok w zlaeznosci od roli
                                    OpenUserPanel(role);
                                }
                                else
                                {
                                    MessageBox.Show("Nieprawidłowe hasło.", "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nie znaleziono użytkownika.", "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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

        private void OpenUserPanel(string role)
        {
            this.Hide(); 

            Form userForm;

            switch (role)
            {
                case "User":
                    userForm = new UserView();
                    break;
                case "Technician":
                    userForm = new TechnicianView();
                    break;
                case "Admin":
                    userForm = new AdminView();
                    break;
                default:
                    MessageBox.Show("Nieznana rola użytkownika!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            userForm.FormClosed += (s, args) => this.Show(); 
            userForm.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
