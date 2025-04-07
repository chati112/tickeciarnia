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
                    t.TicketID,
                    t.Title,
                    t.Description,
                    t.CreatedDate,
                    p.PriorityName,
                    s.StatusName,
                    t.ResolvedDate
                FROM Tickets t
                JOIN Priorities p ON t.PriorityID = p.PriorityID
                JOIN Statuses s ON t.StatusID = s.StatusID
            ";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                        FormatTicketsGridView();
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

        private void FormatTicketsGridView()
        {
            var dgv = dataGridView1;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;   

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 8);

            dgv.AllowUserToAddRows = false;      
            dgv.RowHeadersVisible = false;       
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 

            dgv.MultiSelect = false;             
            dgv.ReadOnly = true;                 

            dgv.Columns["TicketID"].HeaderText = "ID";
            dgv.Columns["Title"].HeaderText = "Tytuł";
            dgv.Columns["Description"].HeaderText = "Opis";
            dgv.Columns["CreatedDate"].HeaderText = "Data utworzenia";
            dgv.Columns["PriorityName"].HeaderText = "Priorytet";
            dgv.Columns["StatusName"].HeaderText = "Status";
            dgv.Columns["ResolvedDate"].HeaderText = "Data rozwiązania";
        }
    }
}
