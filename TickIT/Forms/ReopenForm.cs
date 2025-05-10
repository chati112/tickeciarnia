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

namespace TickIT.Forms.user
{
    public partial class ReopenForm : Form
    {
        private int currentUserId;

        public ReopenForm(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ReopenForm_Load(object sender, EventArgs e)
        {
            LoadReopenReasons();
        }

        private void LoadReopenReasons()
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT ReasonText FROM ReopenReasons";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "Reasons");

                        comboBoxReason.DataSource = ds.Tables["Reasons"];
                        comboBoxReason.DisplayMember = "ReasonText";
                        comboBoxReason.SelectedIndex = -1; // Brak domyślnego wyboru
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Błąd bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ticketId = (int)numericUpDownReopenID.Value;

            if (comboBoxReason.SelectedIndex == -1)
            {
                MessageBox.Show("Wybierz powód reopenowania incydentu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDescription.Text))
            {
                MessageBox.Show("Uzupełnij opis powodu reopenowania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reason = comboBoxReason.Text.Trim();
            string description = textBoxDescription.Text.Trim();
            string commentText = $"REOPEN: {reason}\nDESCRIPTION: {description}";

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string checkTicketQuery = @"
                        SELECT T.TicketID 
                        FROM Tickets T
                        INNER JOIN Statuses S ON T.StatusID = S.StatusID
                        WHERE T.TicketID = @TicketID AND T.UserID = @UserID AND S.StatusName = 'Resolved'";

                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkTicketQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TicketID", ticketId);
                        checkCmd.Parameters.AddWithValue("@UserID", currentUserId);

                        object result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Nie można ponownie otworzyć tego ticketa. Upewnij się, że podałeś poprawne ID i ma zgłoszenie status 'Resolved'.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        string getStatusIdQuery = "SELECT StatusID FROM Statuses WHERE StatusName = 'Open'";
                        int statusId;

                        using (SQLiteCommand getStatusCmd = new SQLiteCommand(getStatusIdQuery, conn))
                        {
                            object result = getStatusCmd.ExecuteScalar();
                            if (result == null)
                                throw new Exception("Nie znaleziono statusu 'Open' w tabeli Statuses.");
                            statusId = Convert.ToInt32(result);
                        }

                        string updateQuery = "UPDATE Tickets SET StatusID = @StatusID WHERE TicketID = @TicketID";

                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@StatusID", statusId);
                            updateCmd.Parameters.AddWithValue("@TicketID", ticketId);
                            updateCmd.ExecuteNonQuery();
                        }

                        string insertCommentQuery = @"
                    INSERT INTO Comments (TicketID, UserID, CommentText, CreatedDate)
                    VALUES (@TicketID, @UserID, @CommentText, CURRENT_TIMESTAMP)";

                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertCommentQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@TicketID", ticketId);
                            insertCmd.Parameters.AddWithValue("@UserID", currentUserId);
                            insertCmd.Parameters.AddWithValue("@CommentText", commentText);
                            insertCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }

                    MessageBox.Show("Incydent został ponownie otwarty.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
