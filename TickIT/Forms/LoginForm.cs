using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickIT.Forms;

namespace TickIT
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = button1;

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

                    string query = "SELECT UserID, Email, PasswordHash, Role, IsInactive, IsPasswordChangeRequired FROM Users WHERE Email = @Email";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", enteredUsername);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int isInactive = Convert.ToInt32(reader["IsInactive"]);
                                if (isInactive == 1)
                                {
                                    MessageBox.Show("Twoje konto jest nieaktywne.\nSkontaktuj się z administratorem w celu aktywacji.", "Konto zablokowane", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                string storedPassword = reader["PasswordHash"].ToString();
                                string role = reader["Role"].ToString();
                                int userID = Convert.ToInt32(reader["UserID"]);
                                int isPasswordChangeRequired = Convert.ToInt32(reader["IsPasswordChangeRequired"]);

                                if (enteredPassword == storedPassword) 
                                {
                                    if (isPasswordChangeRequired == 1)
                                    {
                                        MessageBox.Show("Zostałeś zalogowany, ale musisz zmienić hasło.", "Wymagana zmiana hasła", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        ChangePasswordForm changePasswordForm = new ChangePasswordForm();
                                        changePasswordForm.Show();
                                        this.Hide(); 
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Witaj {enteredUsername}!", "Logowanie udane", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        OpenOperatingPanel(role, userID);
                                    }
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


        private void OpenOperatingPanel(string role, int userID)
        {
            this.Hide(); 

            Form userForm;

            switch (role)
            {
                case "User":
                    userForm = new UserView(userID);
                    break;
                case "Technician":
                    userForm = new TechnicianView(userID);
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

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_resetpwd_Click(object sender, EventArgs e)
        {
            RecoverPasswordForm recoverPasswordForm = new RecoverPasswordForm();
            recoverPasswordForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
  
        }
    }
}
