using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickIT.Helpers;

namespace TickIT.Services
{
    internal class UserService
    {
        public static bool ResetUserPassword(string email)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            string selectQuery = "SELECT * FROM Users WHERE Email = @Email";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();


                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                    {
                        adapter.SelectCommand = new SQLiteCommand(selectQuery, conn);
                        adapter.SelectCommand.Parameters.AddWithValue("@Email", email);

                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "Users");

                        if (ds.Tables["Users"].Rows.Count == 1)
                        {

                            string newPassword = PasswordHelper.GenerateNewPassword();

                            DataRow userRow = ds.Tables["Users"].Rows[0];
                            userRow["PasswordHash"] = newPassword; 

                            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
                            adapter.UpdateCommand = builder.GetUpdateCommand();

                            adapter.Update(ds, "Users");

                            EmailService.SendNewPasswordEmail(email, newPassword);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd przy resetowaniu hasła: " + ex.Message);
                }
            }

            return false;
        }

    }
}
