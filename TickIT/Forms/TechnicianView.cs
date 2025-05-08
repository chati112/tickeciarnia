using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Windows.Forms.VisualStyles;
using System.Security.Cryptography.X509Certificates;
using TickIT.Services;


namespace TickIT
{
    public partial class TechnicianView : Form
    {
        private int loggedUserID;
        private BindingSource commentsBindingSource = new BindingSource();

        public TechnicianView(int userID)
        {
            InitializeComponent();
            loggedUserID = userID;
        }


        private void TechnicianView_Load(object sender, EventArgs e)
        {
            bindingNavigatorComments.BindingSource = commentsBindingSource;
            commentsBindingSource.PositionChanged += CommentsBindingSource_PositionChanged;
            LoadAllTicketsForTechnician();
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView3.CellClick += dataGridView3_CellClick;
            bindingNavigatorComments.AddNewItem = null;
        }

        private void LoadComments(int ticketId)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            DataTable dtJournal = new DataTable();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    C.CommentID AS ID,
                    C.CommentText AS Comment,
                    C.CreatedDate AS 'Date Added',
                    U.FirstName || ' ' || U.LastName AS Author
                FROM Comments C
                INNER JOIN Users U ON C.UserID = U.UserID
                WHERE C.TicketID = @TicketID
                ORDER BY C.CreatedDate ASC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TicketID", ticketId);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dtJournal);
                        }
                    }
                }

                
                commentsBindingSource.DataSource = dtJournal;
                dataGridView3.DataSource = commentsBindingSource;
                bindingNavigatorComments.BindingSource = commentsBindingSource;
                dataGridView3.RowHeadersVisible = false;

                commentsBindingSource.PositionChanged += CommentsBindingSource_PositionChanged;

                if (dataGridView3.Columns.Contains("Comment"))
                    dataGridView3.Columns["Comment"].Width = 330;
                if (dataGridView3.Columns.Contains("ID"))
                    dataGridView3.Columns["ID"].Width = 35;
                if (dataGridView3.Columns["CommentID"] != null)
                {
                    dataGridView3.Columns["CommentID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania komentarzy: " + ex.Message);
            }
        }


        private void LoadTicketDetailsToGrid(int ticketId)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string queryDetails = @"
                SELECT 
                    T.TicketID AS ID, 
                    T.CreatedDate AS 'Reported date', 
                    T.ResolvedDate AS 'Resolved date', 
                    U1.FirstName || ' ' || U1.LastName AS Customer,
                    U2.FirstName || ' ' || U2.LastName AS Asignee,
                    S.StatusName AS Status,
                    P.PriorityName AS Priority
                FROM Tickets T
                INNER JOIN Users U1 ON T.UserID = U1.UserID
                LEFT JOIN Users U2 ON T.TechnicianID = U2.UserID
                INNER JOIN Statuses S ON T.StatusID = S.StatusID
                INNER JOIN Priorities P ON T.PriorityID = P.PriorityID
                WHERE T.TicketID = @TicketID";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryDetails, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@TicketID", ticketId);

                        DataTable dtDetails = new DataTable();
                        adapter.Fill(dtDetails);

                        dataGridView2.DataSource = dtDetails;
                        dataGridView2.RowHeadersVisible = false;
                        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        if (dtDetails.Columns.Contains("ID"))
                            dataGridView2.Columns["ID"].Width = 30;

                        if (dtDetails.Columns.Contains("Status"))
                            dataGridView2.Columns["Status"].Width = 70;

                        if (dtDetails.Columns.Contains("Priority"))
                            dataGridView2.Columns["Priority"].Width = 50;
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


        private void LoadAllTicketsForTechnician()
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                     SELECT 
                            T.TicketID,
                            T.Title,
                            T.CreatedDate,
                            P.PriorityName,
                            S.StatusName,
                            T.ResolvedDate
                        FROM Tickets T
                        INNER JOIN Priorities P ON T.PriorityID = P.PriorityID
                        INNER JOIN Statuses S ON T.StatusID = S.StatusID
                        ORDER BY 
                            CASE WHEN S.StatusName = 'Resolved' THEN 1 ELSE 0 END,
                            T.CreatedDate ASC";


                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                        dataGridView1.RowHeadersVisible = false;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        if (dt.Columns.Contains("TicketID"))
                        {
                            dataGridView1.Columns["TicketID"].HeaderText = "ID";
                            dataGridView1.Columns["TicketID"].Width = 30;
                        }
                        if (dt.Columns.Contains("Title"))
                        {
                            dataGridView1.Columns["Title"].HeaderText = "Title";
                            dataGridView1.Columns["Title"].Width = 150;
                        }
                        if (dt.Columns.Contains("CreatedDate"))
                        {
                            dataGridView1.Columns["CreatedDate"].HeaderText = "Created";
                            dataGridView1.Columns["CreatedDate"].Width = 100;
                        }
                        if (dt.Columns.Contains("PriorityName"))
                        {
                            dataGridView1.Columns["PriorityName"].HeaderText = "Priority";
                            dataGridView1.Columns["PriorityName"].Width = 70;
                        }
                        if (dt.Columns.Contains("StatusName"))
                        {
                            dataGridView1.Columns["StatusName"].HeaderText = "Status";
                            dataGridView1.Columns["StatusName"].Width = 70;
                        }
                        if (dt.Columns.Contains("ResolvedDate"))
                        {
                            dataGridView1.Columns["ResolvedDate"].HeaderText = "Resolved";
                            dataGridView1.Columns["ResolvedDate"].Width = 100;
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





        private void buttonAssign_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Nie wybrano żadnego zgłoszenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ticketId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["TicketID"].Value);
            int technicianId = loggedUserID;

            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    int inProgressStatusId;
                    using (SQLiteCommand statusCmd = new SQLiteCommand("SELECT StatusID FROM Statuses WHERE StatusName = 'In progress'", conn))
                    {
                        object result = statusCmd.ExecuteScalar();
                        inProgressStatusId = Convert.ToInt32(result);
                    }

                    string updateQuery = @"
                UPDATE Tickets 
                SET TechnicianID = @TechnicianID,
                    StatusID = @StatusID
                WHERE TicketID = @TicketID";

                    using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianID", technicianId);
                        cmd.Parameters.AddWithValue("@StatusID", inProgressStatusId);
                        cmd.Parameters.AddWithValue("@TicketID", ticketId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Zgłoszenie zostało przypisane'.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAllTicketsForTechnician(); // Odśwież listę
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się przypisać zgłoszenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas przypisywania zgłoszenia: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells["TicketID"].Value != null)
            {
                int ticketId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["TicketID"].Value);
                LoadTicketDetailsToGrid(ticketId);
                LoadComments(ticketId);
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView3.Rows[e.RowIndex].Cells["Comment"].Value != null)
            {
                string commentText = dataGridView3.Rows[e.RowIndex].Cells["Comment"].Value.ToString();
                textBox1.Text = commentText;
            }
        }


        private void CommentsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (commentsBindingSource.Current is DataRowView currentRow)
            {
                textBox1.Text = currentRow["Comment"].ToString();
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (commentsBindingSource.Current == null)
            {
                MessageBox.Show("Nie można usunąć jedynego komentarza w zgłoszeniu !", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView currentRowView = commentsBindingSource.Current as DataRowView;
            if (currentRowView == null || currentRowView["ID"] == DBNull.Value)
            {
                MessageBox.Show("Nie można odczytać ID komentarza.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int commentId = Convert.ToInt32(currentRowView["ID"]);

            DialogResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten komentarz?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM Comments WHERE CommentID = @CommentID";

                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@CommentID", commentId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Komentarz został usunięty.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Usuń z DataTable bez przeładowania z bazy, lub przeładuj jeśli wolisz świeże dane:
                            int ticketId = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ID"].Value);
                            LoadComments(ticketId);
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się usunąć komentarza.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania komentarza: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddComment(int ticketId, int userId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                MessageBox.Show("Komentarz nie może być pusty.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    DataSet dataSet = new DataSet();


                    string selectQuery = "SELECT * FROM Comments WHERE 1=0"; // Trik: 0 wierszy
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, conn))
                    {
                        adapter.Fill(dataSet, "Comments");

                        DataRow newRow = dataSet.Tables["Comments"].NewRow();
                        newRow["TicketID"] = ticketId;
                        newRow["UserID"] = userId;
                        newRow["CommentText"] = commentText;
                        newRow["CreatedDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        dataSet.Tables["Comments"].Rows.Add(newRow);


                        SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter);

                        adapter.Update(dataSet, "Comments");

                        MessageBox.Show("Komentarz został dodany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LoadComments(ticketId);
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

        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow != null)
            {
                int ticketId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["TicketID"].Value);
                AddComment(ticketId, loggedUserID, textBox1.Text.Trim());
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Proszę najpierw wybrać zgłoszenie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Nie wybrano żadnego zgłoszenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ticketId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["TicketID"].Value);
            string ticketTitle = dataGridView1.CurrentRow.Cells["Title"].Value.ToString();
            string connectionString = "Data Source=TickIT.db;Version=3;";
            string recipientEmail = "";
            string userName = "";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Sprawdzenie, czy zgłoszenie ma przypisanego technika i czy jest przypisane do aktualnego
                    string checkAssignmentQuery = "SELECT TechnicianID FROM Tickets WHERE TicketID = @TicketID";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkAssignmentQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TicketID", ticketId);
                        object assignedTechId = checkCmd.ExecuteScalar();

                        if (assignedTechId == DBNull.Value)
                        {
                            MessageBox.Show("Zgłoszenie nie ma przypisanego technika i nie może zostać rozwiązane.", "Odmowa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        int technicianId = Convert.ToInt32(assignedTechId);
                        if (technicianId != loggedUserID)
                        {
                            MessageBox.Show("Zgłoszenie nie jest przypisane do Ciebie i nie może zostać rozwiązane.", "Odmowa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Pobierz ID statusu "Resolved"
                    string getStatusIdQuery = "SELECT StatusID FROM Statuses WHERE StatusName = 'Resolved'";
                    int resolvedStatusId = Convert.ToInt32(new SQLiteCommand(getStatusIdQuery, conn).ExecuteScalar());

                    // Pobierz dane użytkownika
                    string userQuery = @"
            SELECT U.Email, U.FirstName || ' ' || U.LastName AS FullName
            FROM Tickets T
            INNER JOIN Users U ON T.UserID = U.UserID
            WHERE T.TicketID = @TicketID";

                    using (SQLiteCommand cmd = new SQLiteCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TicketID", ticketId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                recipientEmail = reader["Email"].ToString();
                                userName = reader["FullName"].ToString();
                            }
                        }
                    }

                    // Aktualizacja statusu i daty rozwiązania
                    string updateQuery = @"
            UPDATE Tickets 
            SET StatusID = @StatusID, ResolvedDate = @ResolvedDate 
            WHERE TicketID = @TicketID";

                    using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@StatusID", resolvedStatusId);
                        updateCmd.Parameters.AddWithValue("@ResolvedDate", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@TicketID", ticketId);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                // Wysyłka e-maila
                EmailService.SendTicketResolvedEmail(recipientEmail, userName, ticketTitle);

                MessageBox.Show("Zgłoszenie zostało oznaczone jako rozwiązane. Wysłano e-mail do użytkownika.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAllTicketsForTechnician();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

