namespace TickIT.Forms.user
{
    partial class ReopenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxReason = new System.Windows.Forms.ComboBox();
            this.btnReopenConfirm = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownReopenID = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReopenID)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(45, 207);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(250, 139);
            this.textBoxDescription.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(126, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Incident reopen";
            // 
            // comboBoxReason
            // 
            this.comboBoxReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReason.FormattingEnabled = true;
            this.comboBoxReason.Items.AddRange(new object[] {
            "Problem is not solved",
            "Resolution is not clear",
            "Additional support needed",
            "Workaround used, issue not fixed ",
            "Missing information now available"});
            this.comboBoxReason.Location = new System.Drawing.Point(107, 138);
            this.comboBoxReason.Name = "comboBoxReason";
            this.comboBoxReason.Size = new System.Drawing.Size(188, 21);
            this.comboBoxReason.TabIndex = 2;
            this.comboBoxReason.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnReopenConfirm
            // 
            this.btnReopenConfirm.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReopenConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReopenConfirm.Font = new System.Drawing.Font("Sitka Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnReopenConfirm.ForeColor = System.Drawing.Color.White;
            this.btnReopenConfirm.Location = new System.Drawing.Point(190, 381);
            this.btnReopenConfirm.Name = "btnReopenConfirm";
            this.btnReopenConfirm.Size = new System.Drawing.Size(104, 36);
            this.btnReopenConfirm.TabIndex = 3;
            this.btnReopenConfirm.Text = "confirm";
            this.btnReopenConfirm.UseVisualStyleBackColor = false;
            this.btnReopenConfirm.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancel.Font = new System.Drawing.Font("Sitka Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(45, 381);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(104, 36);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Reason:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Description:";
            // 
            // numericUpDownReopenID
            // 
            this.numericUpDownReopenID.Location = new System.Drawing.Point(107, 89);
            this.numericUpDownReopenID.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownReopenID.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownReopenID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownReopenID.Name = "numericUpDownReopenID";
            this.numericUpDownReopenID.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownReopenID.TabIndex = 7;
            this.numericUpDownReopenID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ticket ID:";
            // 
            // ReopenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownReopenID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.btnReopenConfirm);
            this.Controls.Add(this.comboBoxReason);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDescription);
            this.Name = "ReopenForm";
            this.Text = "ReopenForm";
            this.Load += new System.EventHandler(this.ReopenForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReopenID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxReason;
        private System.Windows.Forms.Button btnReopenConfirm;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownReopenID;
        private System.Windows.Forms.Label label4;
    }
}