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

namespace TickIT
{
    public partial class TechnicianView : Form
    {
        public TechnicianView()
        {
            InitializeComponent();
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


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int ticketID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                LoadComments(ticketID);
                LoadUserData(ticketID);
            }
        }
    }
}
