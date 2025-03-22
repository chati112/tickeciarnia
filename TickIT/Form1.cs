using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TickIT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection();
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                // Ustawienie connection string dla SQLite
                conn.ConnectionString = "Data Source=TickIT.db;Version=3;";

                cmd.CommandText = "SELECT * FROM Users";
                cmd.Connection = conn;

                conn.Open();

                SQLiteDataReader dr = cmd.ExecuteReader();

                // Odczyt danych i wyświetlanie ich na etykiecie
                while (dr.Read())
                {
                    lbl_users.Text += $"{dr.GetValue(0).ToString()} {dr.GetString(1).ToString()} {dr.GetValue(2).ToString()} {dr.GetValue(3).ToString()}\n";
                }
            }
            catch (SQLiteException ex2)
            {
                MessageBox.Show("SQLite Error: " + ex2.Message);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Error: " + ex1.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
