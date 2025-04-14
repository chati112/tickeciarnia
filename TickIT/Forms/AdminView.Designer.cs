namespace TickIT
{
    partial class AdminView
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView_Users = new System.Windows.Forms.DataGridView();
            this.lbl_FullName = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_AddUser = new System.Windows.Forms.Button();
            this.btn_DeactivateUser = new System.Windows.Forms.Button();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownID = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Users)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownID)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_Users
            // 
            this.dataGridView_Users.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Users.Location = new System.Drawing.Point(12, 12);
            this.dataGridView_Users.Name = "dataGridView_Users";
            this.dataGridView_Users.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Users.Size = new System.Drawing.Size(436, 481);
            this.dataGridView_Users.TabIndex = 0;
            this.dataGridView_Users.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Users_CellClick);
            // 
            // lbl_FullName
            // 
            this.lbl_FullName.AutoSize = true;
            this.lbl_FullName.Location = new System.Drawing.Point(488, 25);
            this.lbl_FullName.Name = "lbl_FullName";
            this.lbl_FullName.Size = new System.Drawing.Size(55, 13);
            this.lbl_FullName.TabIndex = 1;
            this.lbl_FullName.Text = "Firstname:";
            this.lbl_FullName.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "User",
            "Technician",
            "Admin"});
            this.cmbRole.Location = new System.Drawing.Point(491, 127);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(247, 21);
            this.cmbRole.TabIndex = 2;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(491, 41);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 20);
            this.txtFirstName.TabIndex = 3;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(491, 78);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 20);
            this.txtPhone.TabIndex = 4;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(617, 80);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 20);
            this.txtEmail.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(488, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Phone:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(614, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Email adress:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(488, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Role:";
            // 
            // btn_AddUser
            // 
            this.btn_AddUser.Location = new System.Drawing.Point(491, 162);
            this.btn_AddUser.Name = "btn_AddUser";
            this.btn_AddUser.Size = new System.Drawing.Size(247, 35);
            this.btn_AddUser.TabIndex = 11;
            this.btn_AddUser.Text = "Add user";
            this.btn_AddUser.UseVisualStyleBackColor = true;
            this.btn_AddUser.Click += new System.EventHandler(this.btn_AddUser_Click);
            // 
            // btn_DeactivateUser
            // 
            this.btn_DeactivateUser.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_DeactivateUser.Location = new System.Drawing.Point(135, 15);
            this.btn_DeactivateUser.Name = "btn_DeactivateUser";
            this.btn_DeactivateUser.Size = new System.Drawing.Size(117, 35);
            this.btn_DeactivateUser.TabIndex = 13;
            this.btn_DeactivateUser.Text = "Deactivate user";
            this.btn_DeactivateUser.UseVisualStyleBackColor = false;
            this.btn_DeactivateUser.Click += new System.EventHandler(this.btn_DeactivateUser_Click);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(617, 41);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(100, 20);
            this.txtLastName.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(614, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Lastname:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(617, 249);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(614, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Lastname:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(491, 370);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 35);
            this.button1.TabIndex = 24;
            this.button1.Text = "Edit user";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(131, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Role:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(614, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Email adress:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Phone:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(617, 288);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 20;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(18, 104);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 19;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(18, 59);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 20);
            this.textBox8.TabIndex = 18;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "User",
            "Technician",
            "Admin"});
            this.comboBox2.Location = new System.Drawing.Point(134, 103);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Firstname:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Moccasin;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBox8);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.textBox7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(483, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 185);
            this.panel1.TabIndex = 27;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Moccasin;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.numericUpDown3);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.textBox9);
            this.panel2.Controls.Add(this.comboBox3);
            this.panel2.Controls.Add(this.textBox10);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 185);
            this.panel2.TabIndex = 30;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(57, 18);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.ReadOnly = true;
            this.numericUpDown3.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown3.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "ID:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(131, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Role:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "Firstname:";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(18, 59);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 20);
            this.textBox9.TabIndex = 18;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "User",
            "Technician",
            "Admin"});
            this.comboBox3.Location = new System.Drawing.Point(134, 103);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 17;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(18, 104);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(100, 20);
            this.textBox10.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "Phone:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(57, 18);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "ID:";
            // 
            // numericUpDownID
            // 
            this.numericUpDownID.Location = new System.Drawing.Point(58, 24);
            this.numericUpDownID.Name = "numericUpDownID";
            this.numericUpDownID.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownID.TabIndex = 31;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "ID:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(483, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(263, 196);
            this.panel3.TabIndex = 32;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel4.Controls.Add(this.btn_DeactivateUser);
            this.panel4.Controls.Add(this.numericUpDownID);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Location = new System.Drawing.Point(483, 432);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(263, 61);
            this.panel4.TabIndex = 33;
            // 
            // AdminView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 509);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_AddUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.lbl_FullName);
            this.Controls.Add(this.dataGridView_Users);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Name = "AdminView";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Users)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownID)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Users;
        private System.Windows.Forms.Label lbl_FullName;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_AddUser;
        private System.Windows.Forms.Button btn_DeactivateUser;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}

