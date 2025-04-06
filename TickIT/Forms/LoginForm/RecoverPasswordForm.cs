using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TickIT.Helpers;
using TickIT.Services;

namespace TickIT.Forms
{
    public partial class RecoverPasswordForm : Form
    {
        public RecoverPasswordForm()
        {
            InitializeComponent();
        }

        private void btn_SendRequest_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;

            bool result = UserService.ResetUserPassword(email);
            if (result)
            {
                MessageBox.Show("Temporary password has been sent to you e-mail address.");
                this.Close();
            }
            else
            {
                MessageBox.Show("User with provided e-mail does not exist in the system", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
