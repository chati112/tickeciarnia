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

        public TechnicianView(int userID)
        {
            InitializeComponent();
            loggedUserID = userID;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int ticketID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                LoadComments(ticketID);
                LoadUserData(ticketID);
            }
        }

        private void TechnicianView_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    t.TicketID as ID,
                    t.Title,
                    t.Description,
                    t.CreatedDate as 'Created Date',
                    p.PriorityName as Priority,
                    s.StatusName as Status,
                    t.ResolvedDate as 'Resolved Date'
                FROM Tickets t
                JOIN Priorities p ON t.PriorityID = p.PriorityID
                JOIN Statuses s ON t.StatusID = s.StatusID
            ";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                        FormatGridView(dataGridView1);
                    }
                }

                LoadStatusOptions();

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }



        }

        private void LoadComments(int ticketID)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  

                    string query = @"
                    SELECT 
            c.TicketID as ID, 
            c.CommentText as Comment, 
            u.FirstName || ' ' || u.LastName AS Technician,
            c.CreatedDate as 'Created Date'
        FROM Comments c
        INNER JOIN Tickets t ON c.TicketID = t.TicketID
        INNER JOIN Users u ON t.TechnicianID = u.UserID
        WHERE t.TicketID = ?
                ";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {                           
                        adapter.SelectCommand.Parameters.AddWithValue("@TicketID", ticketID);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);  

                        dataGridView3.DataSource = dt;
                        FormatGridView(dataGridView3);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Error: " + ex.Message);  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);  
            }
        }

        private void LoadUserData(int ticketID)
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    u.FirstName || ' ' || u.LastName AS 'User',
                    u.Phone, 
                    u.Email
                FROM Users u
                INNER JOIN Tickets t ON u.UserID = t.UserID
                WHERE t.TicketID = ? AND u.Role = 'User';
            ";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("?", ticketID);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView2.DataSource = dt;
                        FormatGridView(dataGridView2);
                    }
                }

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FormatGridView(DataGridView dataGridView)
        {
            var dgv = dataGridView;
          

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;   

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 8);

            dgv.AllowUserToAddRows = false;      
            dgv.RowHeadersVisible = false;       
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 

            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
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

        private void RefreshTickets()
        {
            string connectionString = "Data Source=TickIT.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    t.TicketID as ID,
                    t.Title,
                    t.Description,
                    t.CreatedDate as 'Created Date',
                    p.PriorityName as Priority,
                    s.StatusName as Status,
                    t.ResolvedDate as 'Resolved Date'
                FROM Tickets t
                JOIN Priorities p ON t.PriorityID = p.PriorityID
                JOIN Statuses s ON t.StatusID = s.StatusID
            ";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        FormatGridView(dataGridView1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odświeżania: " + ex.Message);
            }
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

            RefreshTickets();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
