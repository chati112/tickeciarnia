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
    public partial class NewTicketForm : Form
    {
        private int _currentUserId;

        public NewTicketForm(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text.Trim();
            string description = textBoxDescription.Text.Trim();
            string selectedPriority = comboBoxPriority.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(selectedPriority))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string getPriorityQuery = "SELECT PriorityID FROM Priorities WHERE PriorityName = @PriorityName";
                    int priorityId;
                    using (SQLiteCommand cmd = new SQLiteCommand(getPriorityQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PriorityName", selectedPriority);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Nie znaleziono wybranego priorytetu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        priorityId = Convert.ToInt32(result);
                    }

                    string getStatusQuery = "SELECT StatusID FROM Statuses WHERE StatusName = 'New'";
                    int statusId = Convert.ToInt32(new SQLiteCommand(getStatusQuery, conn).ExecuteScalar());

                    int ticketId;
                    string insertTicketQuery = @"
                INSERT INTO Tickets (Title, Description, UserID, StatusID, PriorityID, CreatedDate)
                VALUES (@Title, @Description, @UserID, @StatusID, @PriorityID, @CreatedDate);
                SELECT last_insert_rowid();";

                    using (SQLiteCommand insertCmd = new SQLiteCommand(insertTicketQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Title", title);
                        insertCmd.Parameters.AddWithValue("@Description", description);
                        insertCmd.Parameters.AddWithValue("@UserID", _currentUserId);
                        insertCmd.Parameters.AddWithValue("@StatusID", statusId);
                        insertCmd.Parameters.AddWithValue("@PriorityID", priorityId);
                        insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        ticketId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }

                    string insertCommentQuery = @"
                INSERT INTO Comments (TicketID, UserID, CommentText, CreatedDate)
                VALUES (@TicketID, @UserID, @CommentText, @CreatedDate)";

                    using (SQLiteCommand commentCmd = new SQLiteCommand(insertCommentQuery, conn))
                    {
                        commentCmd.Parameters.AddWithValue("@TicketID", ticketId);
                        commentCmd.Parameters.AddWithValue("@UserID", _currentUserId);
                        commentCmd.Parameters.AddWithValue("@CommentText", description);
                        commentCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        commentCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Zgłoszenie zostało utworzone.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxTitle.Clear();
                textBoxDescription.Clear();
                comboBoxPriority.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
