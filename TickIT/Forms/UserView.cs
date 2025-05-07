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
using TickIT.Forms;
using TickIT.Forms.user;


namespace TickIT
{
    public partial class UserView : Form
    {
        private int currentUserId;
        private BindingSource ticketsBindingSource = new BindingSource();

        public UserView(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
        }

        private void UserView_Load(object sender, EventArgs e)
        {
            LoadUserTickets(currentUserId);

            bindingNavigatorTickets.BindingSource = ticketsBindingSource;


            ticketsBindingSource.PositionChanged += TicketsBindingSource_PositionChanged;
        }

        private void LoadUserTickets(int userId)
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
                    S.StatusName AS Status  
                    FROM Tickets T
                    INNER JOIN Statuses S ON T.StatusID = S.StatusID
                    WHERE T.UserID = @UserID";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@UserID", userId);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        ticketsBindingSource.DataSource = dt;
                        dataGridViewTickets.DataSource = ticketsBindingSource;

                        // Formatowanie
                        dataGridViewTickets.RowHeadersVisible = false;
                        dataGridViewTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        if (dt.Columns.Contains("TicketID"))
                        {
                            dataGridViewTickets.Columns["TicketID"].HeaderText = "ID";
                            dataGridViewTickets.Columns["TicketID"].Width = 30;
                        }
                        if (dt.Columns.Contains("Title"))
                        {
                            dataGridViewTickets.Columns["Title"].HeaderText = "Summary";
                            dataGridViewTickets.Columns["Title"].Width = 150;
                        }
                        if (dt.Columns.Contains("Status"))
                        {
                            dataGridViewTickets.Columns["Status"].HeaderText = "Status";
                            dataGridViewTickets.Columns["Status"].Width = 60;
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

        private void TicketsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (ticketsBindingSource.Current != null)
            {
                DataRowView currentRow = (DataRowView)ticketsBindingSource.Current;
                int ticketId = Convert.ToInt32(currentRow["TicketID"]);
                LoadTicketDetailsAndJournal(ticketId);
            }
        }

        private void LoadTicketDetailsAndJournal(int ticketId)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Ładowanie szczegółów ticketa
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

                    using (SQLiteDataAdapter adapterDetails = new SQLiteDataAdapter(queryDetails, conn))
                    {
                        adapterDetails.SelectCommand.Parameters.AddWithValue("@TicketID", ticketId);

                        DataTable dtDetails = new DataTable();
                        adapterDetails.Fill(dtDetails);

                        dataGridViewDetails.DataSource = dtDetails;
                        dataGridViewDetails.RowHeadersVisible = false;
                        dataGridViewDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridViewDetails.Columns["ID"].Width = 30;
                        dataGridViewDetails.Columns["Status"].Width = 50;
                        dataGridViewDetails.Columns["Priority"].Width = 50;
                    }

                    // Ładowanie komentarzy
                    string queryJournal = @"
                    SELECT 
                    C.CommentText AS Comment, 
                    C.CreatedDate AS Date,
                    U.FirstName || ' ' || U.LastName AS Author
                    FROM Comments C
                    INNER JOIN Users U ON C.UserID = U.UserID
                    WHERE C.TicketID = @TicketID
                    ORDER BY C.CreatedDate ASC";

                    using (SQLiteDataAdapter adapterJournal = new SQLiteDataAdapter(queryJournal, conn))
                    {
                        adapterJournal.SelectCommand.Parameters.AddWithValue("@TicketID", ticketId);

                        DataTable dtJournal = new DataTable();
                        adapterJournal.Fill(dtJournal);

                        dataGridViewJournal.DataSource = dtJournal;
                        dataGridViewJournal.RowHeadersVisible = false;
                        dataGridViewJournal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridViewJournal.Columns["Comment"].Width = 475;
                        dataGridViewJournal.Columns["Date"].Width = 110;
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

        private void button2_Click(object sender, EventArgs e)
        {
            NewTicketForm newTicketForm = new NewTicketForm();
            newTicketForm.ShowDialog();
        }

        private void bindingNavigatorAddNewItem1_Click(object sender, EventArgs e)
        {
            NewTicketForm newTicketForm = new NewTicketForm();
            newTicketForm.ShowDialog();
        }

        private void AddComment(int ticketId, int userId, string commentText)
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

                    // Tworzymy DataSet
                    DataSet dataSet = new DataSet();

                    // Ładujemy strukturę tabeli Comments do DataSetu (ale bez danych)
                    string selectQuery = "SELECT * FROM Comments WHERE 1=0"; // Trik: 0 wierszy
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, conn))
                    {
                        adapter.Fill(dataSet, "Comments");

                        // Dodajemy nowy wiersz
                        DataRow newRow = dataSet.Tables["Comments"].NewRow();
                        newRow["TicketID"] = ticketId;
                        newRow["UserID"] = userId;
                        newRow["CommentText"] = commentText;
                        newRow["CreatedDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        dataSet.Tables["Comments"].Rows.Add(newRow);

                        // Tworzymy CommandBuilder do wygenerowania INSERT automatycznie
                        SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter);

                        // Zapisujemy zmiany do bazy
                        adapter.Update(dataSet, "Comments");

                        MessageBox.Show("Komentarz został dodany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Odświeżenie widoku komentarzy
                LoadTicketDetailsAndJournal(ticketId);
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


        private void buttonAddComment_Click(object sender, EventArgs e)
        {
            if (dataGridViewTickets.CurrentRow != null)
            {
                int ticketId = Convert.ToInt32(dataGridViewTickets.CurrentRow.Cells["TicketID"].Value);
                AddComment(ticketId, currentUserId, textBoxActivity.Text.Trim());
                textBoxActivity.Clear(); // Czyścimy textbox po dodaniu komentarza
            }
            else
            {
                MessageBox.Show("Proszę najpierw wybrać zgłoszenie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChangePriority_Click(object sender, EventArgs e)
        {
            if (dataGridViewTickets.CurrentRow == null)
            {
                MessageBox.Show("Wybierz zgłoszenie.", "Brak wyboru", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ticketId = Convert.ToInt32(dataGridViewTickets.CurrentRow.Cells["TicketID"].Value);
            string selectedPriority = comboBox_Priority.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedPriority))
            {
                MessageBox.Show("Wybierz priorytet.", "Brak priorytetu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string queryCreatedDate = "SELECT CreatedDate FROM Tickets WHERE TicketID = @TicketID";
                    DataTable dtCreatedDate = new DataTable();

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryCreatedDate, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@TicketID", ticketId);
                        adapter.Fill(dtCreatedDate);
                    }

                    if (dtCreatedDate.Rows.Count == 0)
                    {
                        MessageBox.Show("Nie znaleziono zgłoszenia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DateTime createdDate = DateTime.Parse(dtCreatedDate.Rows[0]["CreatedDate"].ToString());
                    TimeSpan timeSinceCreation = DateTime.Now - createdDate;

                    // Blokada na Critical jeśli zgłoszenie młodsze niż 24h
                    if (timeSinceCreation.TotalHours < 24 && selectedPriority == "Critical")
                    {
                        MessageBox.Show("Nie możesz ustawić priorytetu 'Critical' dla zgłoszenia utworzonego mniej niż 24 godziny temu. Najwyższy obecnie dostępny priorytet to High", "Ograniczenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string queryPriorityID = "SELECT PriorityID FROM Priorities WHERE PriorityName = @PriorityName";
                    DataTable dtPriority = new DataTable();

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryPriorityID, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@PriorityName", selectedPriority);
                        adapter.Fill(dtPriority);
                    }

                    if (dtPriority.Rows.Count == 0)
                    {
                        MessageBox.Show("Nie znaleziono priorytetu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int priorityId = Convert.ToInt32(dtPriority.Rows[0]["PriorityID"]);

                    string updateQuery = "UPDATE Tickets SET PriorityID = @PriorityID WHERE TicketID = @TicketID";

                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, conn))
                    {
                        updateCommand.Parameters.AddWithValue("@PriorityID", priorityId);
                        updateCommand.Parameters.AddWithValue("@TicketID", ticketId);
                        updateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Priorytet zgłoszenia został zmieniony.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTicketDetailsAndJournal(ticketId);
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

        private void btn_reopen_Click(object sender, EventArgs e)
        {
            ReopenForm reopenForm = new ReopenForm(currentUserId);
            reopenForm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadUserTickets(currentUserId);
        }
    }

}
