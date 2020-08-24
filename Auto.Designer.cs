namespace autohana
{
   partial class Auto
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auto));
            this.delayNewfeed = new System.Windows.Forms.Label();
            this.delayFrom = new System.Windows.Forms.NumericUpDown();
            this.delayTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.JobMaxOfDay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.socapdagiai = new System.Windows.Forms.Label();
            this.socapgiaikhongthanh = new System.Windows.Forms.Label();
            this.sotiennhan = new System.Windows.Forms.Label();
            this.soLanKhongGiaiDuoctien = new System.Windows.Forms.Label();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cookie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.golike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passgolike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passhana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Done = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runhana = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rungolike = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.An = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Stop = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DKhana = new System.Windows.Forms.DataGridViewButtonColumn();
            this.checkLoadImage = new System.Windows.Forms.CheckBox();
            this.ConvertData = new System.Windows.Forms.Button();
            this.uidAddHana = new System.Windows.Forms.TextBox();
            this.passAddHana = new System.Windows.Forms.TextBox();
            this.listAccounts = new System.Windows.Forms.Button();
            this.clean = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.delayFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobMaxOfDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // delayNewfeed
            // 
            this.delayNewfeed.AutoSize = true;
            this.delayNewfeed.Location = new System.Drawing.Point(12, 36);
            this.delayNewfeed.Name = "delayNewfeed";
            this.delayNewfeed.Size = new System.Drawing.Size(114, 13);
            this.delayNewfeed.TabIndex = 0;
            this.delayNewfeed.Text = "Thao tác với tài khoản";
            // 
            // delayFrom
            // 
            this.delayFrom.Location = new System.Drawing.Point(195, 29);
            this.delayFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delayFrom.Name = "delayFrom";
            this.delayFrom.Size = new System.Drawing.Size(40, 20);
            this.delayFrom.TabIndex = 2;
            this.delayFrom.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // delayTo
            // 
            this.delayTo.Location = new System.Drawing.Point(253, 29);
            this.delayTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delayTo.Name = "delayTo";
            this.delayTo.Size = new System.Drawing.Size(43, 20);
            this.delayTo.TabIndex = 3;
            this.delayTo.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Kết thúc khi số job thành công";
            // 
            // JobMaxOfDay
            // 
            this.JobMaxOfDay.Location = new System.Drawing.Point(253, 60);
            this.JobMaxOfDay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.JobMaxOfDay.Name = "JobMaxOfDay";
            this.JobMaxOfDay.Size = new System.Drawing.Size(43, 20);
            this.JobMaxOfDay.TabIndex = 5;
            this.JobMaxOfDay.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Kết thúc auto khi có số tìa khoản chekpoint";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(253, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "Chạy";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // socapdagiai
            // 
            this.socapdagiai.AutoSize = true;
            this.socapdagiai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.socapdagiai.Location = new System.Drawing.Point(399, 35);
            this.socapdagiai.Name = "socapdagiai";
            this.socapdagiai.Size = new System.Drawing.Size(41, 13);
            this.socapdagiai.TabIndex = 25;
            this.socapdagiai.Text = "label10";
            // 
            // socapgiaikhongthanh
            // 
            this.socapgiaikhongthanh.AutoSize = true;
            this.socapgiaikhongthanh.ForeColor = System.Drawing.Color.Red;
            this.socapgiaikhongthanh.Location = new System.Drawing.Point(479, 35);
            this.socapgiaikhongthanh.Name = "socapgiaikhongthanh";
            this.socapgiaikhongthanh.Size = new System.Drawing.Size(41, 13);
            this.socapgiaikhongthanh.TabIndex = 26;
            this.socapgiaikhongthanh.Text = "label11";
            // 
            // sotiennhan
            // 
            this.sotiennhan.AutoSize = true;
            this.sotiennhan.ForeColor = System.Drawing.Color.Lime;
            this.sotiennhan.Location = new System.Drawing.Point(563, 35);
            this.sotiennhan.Name = "sotiennhan";
            this.sotiennhan.Size = new System.Drawing.Size(41, 13);
            this.sotiennhan.TabIndex = 27;
            this.sotiennhan.Text = "label12";
            // 
            // soLanKhongGiaiDuoctien
            // 
            this.soLanKhongGiaiDuoctien.AutoSize = true;
            this.soLanKhongGiaiDuoctien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.soLanKhongGiaiDuoctien.Location = new System.Drawing.Point(655, 35);
            this.soLanKhongGiaiDuoctien.Name = "soLanKhongGiaiDuoctien";
            this.soLanKhongGiaiDuoctien.Size = new System.Drawing.Size(41, 13);
            this.soLanKhongGiaiDuoctien.TabIndex = 28;
            this.soLanKhongGiaiDuoctien.Text = "label13";
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stt,
            this.Id,
            this.Pass,
            this.Fa,
            this.cookie,
            this.name,
            this.golike,
            this.passgolike,
            this.hana,
            this.passhana,
            this.total,
            this.Done,
            this.Error,
            this.ReWork,
            this.runhana,
            this.rungolike,
            this.An,
            this.Stop,
            this.status,
            this.Action,
            this.DKhana});
            this.dgvAccounts.Location = new System.Drawing.Point(3, 139);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.Size = new System.Drawing.Size(1301, 543);
            this.dgvAccounts.TabIndex = 29;
            this.dgvAccounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // stt
            // 
            this.stt.DataPropertyName = "Stt";
            this.stt.HeaderText = "#";
            this.stt.Name = "stt";
            this.stt.Width = 30;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Id.DefaultCellStyle = dataGridViewCellStyle1;
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            // 
            // Pass
            // 
            this.Pass.DataPropertyName = "Pass";
            this.Pass.HeaderText = "Pass";
            this.Pass.Name = "Pass";
            // 
            // Fa
            // 
            this.Fa.DataPropertyName = "Fa";
            this.Fa.HeaderText = "2Fa";
            this.Fa.Name = "Fa";
            this.Fa.Width = 60;
            // 
            // cookie
            // 
            this.cookie.DataPropertyName = "cookie";
            this.cookie.HeaderText = "cookie";
            this.cookie.Name = "cookie";
            this.cookie.Width = 60;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.HeaderText = "name";
            this.name.Name = "name";
            this.name.Width = 70;
            // 
            // golike
            // 
            this.golike.DataPropertyName = "golike";
            this.golike.HeaderText = "golike";
            this.golike.Name = "golike";
            this.golike.Width = 60;
            // 
            // passgolike
            // 
            this.passgolike.DataPropertyName = "passgolike";
            this.passgolike.HeaderText = "passgl";
            this.passgolike.Name = "passgolike";
            this.passgolike.Width = 50;
            // 
            // hana
            // 
            this.hana.DataPropertyName = "hana";
            this.hana.HeaderText = "hana";
            this.hana.Name = "hana";
            this.hana.Width = 60;
            // 
            // passhana
            // 
            this.passhana.DataPropertyName = "passhana";
            this.passhana.HeaderText = "passhn";
            this.passhana.Name = "passhana";
            this.passhana.Width = 50;
            // 
            // total
            // 
            this.total.DataPropertyName = "Total";
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Lime;
            this.total.DefaultCellStyle = dataGridViewCellStyle2;
            this.total.HeaderText = "Total";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.Width = 40;
            // 
            // Done
            // 
            this.Done.DataPropertyName = "Done";
            this.Done.HeaderText = "Done";
            this.Done.Name = "Done";
            this.Done.Width = 40;
            // 
            // Error
            // 
            this.Error.DataPropertyName = "Error";
            this.Error.HeaderText = "Error";
            this.Error.Name = "Error";
            this.Error.Width = 40;
            // 
            // ReWork
            // 
            this.ReWork.DataPropertyName = "ReWork";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            this.ReWork.DefaultCellStyle = dataGridViewCellStyle3;
            this.ReWork.HeaderText = "reW";
            this.ReWork.Name = "ReWork";
            this.ReWork.Width = 40;
            // 
            // runhana
            // 
            this.runhana.DataPropertyName = "runhana";
            this.runhana.HeaderText = "Rha";
            this.runhana.Name = "runhana";
            this.runhana.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.runhana.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.runhana.Width = 40;
            // 
            // rungolike
            // 
            this.rungolike.DataPropertyName = "RungoLike";
            this.rungolike.HeaderText = "Rgo";
            this.rungolike.Name = "rungolike";
            this.rungolike.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rungolike.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.rungolike.Width = 40;
            // 
            // An
            // 
            this.An.DataPropertyName = "An";
            this.An.HeaderText = "An";
            this.An.Name = "An";
            this.An.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.An.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.An.Width = 40;
            // 
            // Stop
            // 
            this.Stop.DataPropertyName = "stop";
            this.Stop.HeaderText = "stop";
            this.Stop.Name = "Stop";
            this.Stop.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Stop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Stop.Width = 40;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "Trạng thái";
            this.status.Name = "status";
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Action
            // 
            this.Action.DataPropertyName = "Action";
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Action.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Action.Width = 60;
            // 
            // DKhana
            // 
            this.DKhana.DataPropertyName = "DKhana";
            this.DKhana.HeaderText = "DKhana";
            this.DKhana.Name = "DKhana";
            this.DKhana.Width = 50;
            // 
            // checkLoadImage
            // 
            this.checkLoadImage.AutoSize = true;
            this.checkLoadImage.Location = new System.Drawing.Point(402, 67);
            this.checkLoadImage.Name = "checkLoadImage";
            this.checkLoadImage.Size = new System.Drawing.Size(91, 17);
            this.checkLoadImage.TabIndex = 30;
            this.checkLoadImage.Text = "không tải ảnh";
            this.checkLoadImage.UseVisualStyleBackColor = true;
            // 
            // ConvertData
            // 
            this.ConvertData.Location = new System.Drawing.Point(402, 91);
            this.ConvertData.Name = "ConvertData";
            this.ConvertData.Size = new System.Drawing.Size(155, 23);
            this.ConvertData.TabIndex = 31;
            this.ConvertData.Text = "Convert Data";
            this.ConvertData.UseVisualStyleBackColor = true;
            this.ConvertData.Click += new System.EventHandler(this.ConvertData_Click);
            // 
            // uidAddHana
            // 
            this.uidAddHana.Location = new System.Drawing.Point(566, 93);
            this.uidAddHana.Name = "uidAddHana";
            this.uidAddHana.Size = new System.Drawing.Size(197, 20);
            this.uidAddHana.TabIndex = 32;
            // 
            // passAddHana
            // 
            this.passAddHana.Location = new System.Drawing.Point(769, 93);
            this.passAddHana.Name = "passAddHana";
            this.passAddHana.Size = new System.Drawing.Size(187, 20);
            this.passAddHana.TabIndex = 33;
            // 
            // listAccounts
            // 
            this.listAccounts.Location = new System.Drawing.Point(976, 93);
            this.listAccounts.Name = "listAccounts";
            this.listAccounts.Size = new System.Drawing.Size(95, 23);
            this.listAccounts.TabIndex = 34;
            this.listAccounts.Text = "DS tài khoản";
            this.listAccounts.UseVisualStyleBackColor = true;
            this.listAccounts.Click += new System.EventHandler(this.listAccounts_Click);
            // 
            // clean
            // 
            this.clean.Location = new System.Drawing.Point(1093, 90);
            this.clean.Name = "clean";
            this.clean.Size = new System.Drawing.Size(75, 23);
            this.clean.TabIndex = 35;
            this.clean.Text = "Clean";
            this.clean.UseVisualStyleBackColor = true;
            this.clean.Click += new System.EventHandler(this.clean_Click);
            // 
            // Auto
            // 
            this.ClientSize = new System.Drawing.Size(1338, 688);
            this.Controls.Add(this.clean);
            this.Controls.Add(this.listAccounts);
            this.Controls.Add(this.passAddHana);
            this.Controls.Add(this.uidAddHana);
            this.Controls.Add(this.ConvertData);
            this.Controls.Add(this.checkLoadImage);
            this.Controls.Add(this.dgvAccounts);
            this.Controls.Add(this.soLanKhongGiaiDuoctien);
            this.Controls.Add(this.sotiennhan);
            this.Controls.Add(this.socapgiaikhongthanh);
            this.Controls.Add(this.socapdagiai);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.JobMaxOfDay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.delayTo);
            this.Controls.Add(this.delayFrom);
            this.Controls.Add(this.delayNewfeed);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Auto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoHana And Golike By Tinh";
            ((System.ComponentModel.ISupportInitialize)(this.delayFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobMaxOfDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label delayNewfeed;
        private System.Windows.Forms.NumericUpDown delayFrom;
        private System.Windows.Forms.NumericUpDown delayTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown JobMaxOfDay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label socapdagiai;
        private System.Windows.Forms.Label socapgiaikhongthanh;
        private System.Windows.Forms.Label sotiennhan;
        private System.Windows.Forms.Label soLanKhongGiaiDuoctien;
        public System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.CheckBox checkLoadImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fa;
        private System.Windows.Forms.DataGridViewTextBoxColumn cookie;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn golike;
        private System.Windows.Forms.DataGridViewTextBoxColumn passgolike;
        private System.Windows.Forms.DataGridViewTextBoxColumn hana;
        private System.Windows.Forms.DataGridViewTextBoxColumn passhana;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Done;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReWork;
        private System.Windows.Forms.DataGridViewCheckBoxColumn runhana;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rungolike;
        private System.Windows.Forms.DataGridViewCheckBoxColumn An;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Stop;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewButtonColumn Action;
        private System.Windows.Forms.DataGridViewButtonColumn DKhana;
        private System.Windows.Forms.Button ConvertData;
        private System.Windows.Forms.TextBox uidAddHana;
        private System.Windows.Forms.TextBox passAddHana;
        private System.Windows.Forms.Button listAccounts;
        private System.Windows.Forms.Button clean;
    }
}

