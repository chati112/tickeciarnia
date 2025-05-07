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
            commentsBindingSource.PositionChanged +=  CommentsBindingSource_PositionChanged;
            LoadAllTicketsForTechnician();
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView3.CellClick += dataGridView3_CellClick;
            LoadStatusOptions();


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
                INNER JOIN Statuses S ON T.StatusID = S.StatusID";

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
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybrać zgłoszenie do przypisania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ticketID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            string connectionString = "Data Source=TickIT.db;Version=3;";

            var adapter = new SQLiteDataAdapter();
            var connection = new SQLiteConnection(connectionString);

            var command = new SQLiteCommand("UPDATE Tickets SET TechnicianID = @TechnicianID WHERE TicketID = @TicketID");

            command.Parameters.AddWithValue("@TechnicianID", loggedUserID);
            command.Parameters.AddWithValue("@TicketID", ticketID);

            try
            {
                connection.Open();
                adapter.InsertCommand = command;
                adapter.InsertCommand.Connection = connection;
                int rowsAffected = adapter.InsertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Zgłoszenie zostało przypisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Nie udało się przypisać zgłoszenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd przypisania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            TechnicianView_Load(sender, e);
        }



        private void LoadStatusOptions()
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT StatusName FROM Statuses";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        comboBoxStatus.Items.Clear();
                        while (reader.Read())
                        {
                            comboBoxStatus.Items.Add(reader["StatusName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania statusów: " + ex.Message);
            }
        }

        private void buttonChangeStatus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybrać zgłoszenie do zmiany statusu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Proszę wybrać nowy status.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ticketID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            string selectedStatus = comboBoxStatus.SelectedItem.ToString();

            string connectionString = "Data Source=TickIT.db;Version=3;";
            var adapter = new SQLiteDataAdapter();
            var connection = new SQLiteConnection(connectionString);

            try
            {
                connection.Open();

                var checkCommand = new SQLiteCommand("SELECT TechnicianID FROM Tickets WHERE TicketID = @TicketID", connection);
                checkCommand.Parameters.AddWithValue("@TicketID", ticketID);
                object technicianIdObj = checkCommand.ExecuteScalar();

                if (technicianIdObj == null || technicianIdObj == DBNull.Value)
                {
                    MessageBox.Show("Zgłoszenie nie ma przypisanego technika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int assignedTechnicianId = Convert.ToInt32(technicianIdObj);
                if (assignedTechnicianId != loggedUserID)
                {
                    MessageBox.Show("Nie jesteś przypisanym technikiem do tego zgłoszenia.", "Brak dostępu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var statusIdCmd = new SQLiteCommand("SELECT StatusID FROM Statuses WHERE StatusName = @StatusName", connection);
                statusIdCmd.Parameters.AddWithValue("@StatusName", selectedStatus);
                object statusIdObj = statusIdCmd.ExecuteScalar();

                if (statusIdObj == null)
                {
                    MessageBox.Show("Nieprawidłowy status.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int statusID = Convert.ToInt32(statusIdObj);

                string updateQuery = selectedStatus == "Resolved"
                    ? "UPDATE Tickets SET StatusID = @StatusID, ResolvedDate = @ResolvedDate WHERE TicketID = @TicketID"
                    : "UPDATE Tickets SET StatusID = @StatusID WHERE TicketID = @TicketID";

                var updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@StatusID", statusID);
                updateCommand.Parameters.AddWithValue("@TicketID", ticketID);

                if (selectedStatus == "Resolved")
                {
                    updateCommand.Parameters.AddWithValue("@ResolvedDate", DateTime.Now);
                }

                adapter.UpdateCommand = updateCommand;
                int rowsAffected = adapter.UpdateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Status został zmieniony.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Nie udało się zmienić statusu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }


        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

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
    }
}
