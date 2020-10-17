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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auto));
            this.delayNewfeed = new System.Windows.Forms.Label();
            this.delayFrom = new System.Windows.Forms.NumericUpDown();
            this.delayTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.JobMaxOfDay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.checkLoadImage = new System.Windows.Forms.CheckBox();
            this.ConvertData = new System.Windows.Forms.Button();
            this.uidAddHana = new System.Windows.Forms.TextBox();
            this.passAddHana = new System.Windows.Forms.TextBox();
            this.listAccounts = new System.Windows.Forms.Button();
            this.clean = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.isCheckBackUpFriendNew = new System.Windows.Forms.CheckBox();
            this.locthanhvien = new System.Windows.Forms.Button();
            this.nuoinick = new System.Windows.Forms.CheckBox();
            this.numberAction = new System.Windows.Forms.TextBox();
            this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cookie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameTDS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passTDS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.golike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passgolike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passhana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XuHT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XuThem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Done = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunTDS = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.runhana = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rungolike = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.An = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Stop = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            this.UserAgent = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            // dgvAccounts
            // 
            this.dgvAccounts.AllowUserToAddRows = false;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stt,
            this.Id,
            this.Pass,
            this.Fa,
            this.cookie,
            this.name,
            this.nameTDS,
            this.passTDS,
            this.golike,
            this.passgolike,
            this.hana,
            this.passhana,
            this.XuHT,
            this.XuThem,
            this.total,
            this.Done,
            this.Error,
            this.ReWork,
            this.RunTDS,
            this.runhana,
            this.rungolike,
            this.An,
            this.Stop,
            this.status,
            this.Action,
            this.UserAgent});
            this.dgvAccounts.Location = new System.Drawing.Point(12, 119);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.Size = new System.Drawing.Size(1182, 343);
            this.dgvAccounts.TabIndex = 29;
            this.dgvAccounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dgvAccounts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvAccounts_MouseClick);
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
            this.checkLoadImage.CheckedChanged += new System.EventHandler(this.checkLoadImage_CheckedChanged);
            // 
            // ConvertData
            // 
            this.ConvertData.Location = new System.Drawing.Point(402, 91);
            this.ConvertData.Name = "ConvertData";
            this.ConvertData.Size = new System.Drawing.Size(155, 23);
            this.ConvertData.TabIndex = 31;
            this.ConvertData.Text = "Convert Data";
            this.ConvertData.UseVisualStyleBackColor = true;
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
            this.clean.Location = new System.Drawing.Point(1077, 93);
            this.clean.Name = "clean";
            this.clean.Size = new System.Drawing.Size(75, 23);
            this.clean.TabIndex = 35;
            this.clean.Text = "Clean";
            this.clean.UseVisualStyleBackColor = true;
            this.clean.Click += new System.EventHandler(this.clean_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(253, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 36;
            this.button1.Tag = " ";
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // isCheckBackUpFriendNew
            // 
            this.isCheckBackUpFriendNew.AutoSize = true;
            this.isCheckBackUpFriendNew.Checked = true;
            this.isCheckBackUpFriendNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isCheckBackUpFriendNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.isCheckBackUpFriendNew.Location = new System.Drawing.Point(499, 66);
            this.isCheckBackUpFriendNew.Name = "isCheckBackUpFriendNew";
            this.isCheckBackUpFriendNew.Size = new System.Drawing.Size(164, 17);
            this.isCheckBackUpFriendNew.TabIndex = 37;
            this.isCheckBackUpFriendNew.Text = "Chỉ BackUp những người mới";
            this.isCheckBackUpFriendNew.UseVisualStyleBackColor = true;
            this.isCheckBackUpFriendNew.CheckedChanged += new System.EventHandler(this.isCheckBackUpFriendNew_CheckedChanged);
            // 
            // locthanhvien
            // 
            this.locthanhvien.Location = new System.Drawing.Point(669, 61);
            this.locthanhvien.Name = "locthanhvien";
            this.locthanhvien.Size = new System.Drawing.Size(121, 23);
            this.locthanhvien.TabIndex = 38;
            this.locthanhvien.Text = "Lọc người dùng";
            this.locthanhvien.UseVisualStyleBackColor = true;
            this.locthanhvien.Click += new System.EventHandler(this.locthanhvien_Click);
            // 
            // nuoinick
            // 
            this.nuoinick.AutoSize = true;
            this.nuoinick.Checked = true;
            this.nuoinick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nuoinick.Location = new System.Drawing.Point(824, 62);
            this.nuoinick.Name = "nuoinick";
            this.nuoinick.Size = new System.Drawing.Size(71, 17);
            this.nuoinick.TabIndex = 39;
            this.nuoinick.Text = "Nuôi nick";
            this.nuoinick.UseVisualStyleBackColor = true;
            this.nuoinick.CheckedChanged += new System.EventHandler(this.nuoinick_CheckedChanged);
            // 
            // numberAction
            // 
            this.numberAction.Location = new System.Drawing.Point(913, 60);
            this.numberAction.Name = "numberAction";
            this.numberAction.Size = new System.Drawing.Size(79, 20);
            this.numberAction.TabIndex = 40;
            this.numberAction.Text = "30";
            // 
            // stt
            // 
            this.stt.DataPropertyName = "Stt";
            this.stt.Frozen = true;
            this.stt.HeaderText = "#";
            this.stt.Name = "stt";
            this.stt.Width = 30;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Id.DefaultCellStyle = dataGridViewCellStyle1;
            this.Id.Frozen = true;
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            // 
            // Pass
            // 
            this.Pass.DataPropertyName = "Pass";
            this.Pass.Frozen = true;
            this.Pass.HeaderText = "Pass";
            this.Pass.Name = "Pass";
            // 
            // Fa
            // 
            this.Fa.DataPropertyName = "Fa";
            this.Fa.Frozen = true;
            this.Fa.HeaderText = "2Fa";
            this.Fa.Name = "Fa";
            this.Fa.Width = 60;
            // 
            // cookie
            // 
            this.cookie.DataPropertyName = "cookie";
            this.cookie.Frozen = true;
            this.cookie.HeaderText = "cookie";
            this.cookie.Name = "cookie";
            this.cookie.Width = 60;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.Frozen = true;
            this.name.HeaderText = "name";
            this.name.Name = "name";
            this.name.Width = 70;
            // 
            // nameTDS
            // 
            this.nameTDS.DataPropertyName = "nameTDS";
            this.nameTDS.Frozen = true;
            this.nameTDS.HeaderText = "nameTDS";
            this.nameTDS.Name = "nameTDS";
            this.nameTDS.Width = 80;
            // 
            // passTDS
            // 
            this.passTDS.DataPropertyName = "passTDS";
            this.passTDS.Frozen = true;
            this.passTDS.HeaderText = "passTDS";
            this.passTDS.Name = "passTDS";
            this.passTDS.Width = 60;
            // 
            // golike
            // 
            this.golike.DataPropertyName = "golike";
            this.golike.Frozen = true;
            this.golike.HeaderText = "golike";
            this.golike.Name = "golike";
            this.golike.Visible = false;
            this.golike.Width = 60;
            // 
            // passgolike
            // 
            this.passgolike.DataPropertyName = "passgolike";
            this.passgolike.Frozen = true;
            this.passgolike.HeaderText = "passgl";
            this.passgolike.Name = "passgolike";
            this.passgolike.Visible = false;
            this.passgolike.Width = 50;
            // 
            // hana
            // 
            this.hana.DataPropertyName = "hana";
            this.hana.Frozen = true;
            this.hana.HeaderText = "hana";
            this.hana.Name = "hana";
            this.hana.Visible = false;
            this.hana.Width = 60;
            // 
            // passhana
            // 
            this.passhana.DataPropertyName = "passhana";
            this.passhana.Frozen = true;
            this.passhana.HeaderText = "passhn";
            this.passhana.Name = "passhana";
            this.passhana.Visible = false;
            this.passhana.Width = 50;
            // 
            // XuHT
            // 
            this.XuHT.DataPropertyName = "XuHT";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.XuHT.DefaultCellStyle = dataGridViewCellStyle2;
            this.XuHT.Frozen = true;
            this.XuHT.HeaderText = "XuHT";
            this.XuHT.Name = "XuHT";
            this.XuHT.Width = 80;
            // 
            // XuThem
            // 
            this.XuThem.DataPropertyName = "XuThem";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Lime;
            this.XuThem.DefaultCellStyle = dataGridViewCellStyle3;
            this.XuThem.Frozen = true;
            this.XuThem.HeaderText = "XuThem";
            this.XuThem.Name = "XuThem";
            this.XuThem.Width = 80;
            // 
            // total
            // 
            this.total.DataPropertyName = "Total";
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Lime;
            this.total.DefaultCellStyle = dataGridViewCellStyle4;
            this.total.Frozen = true;
            this.total.HeaderText = "Total";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.total.Visible = false;
            this.total.Width = 40;
            // 
            // Done
            // 
            this.Done.DataPropertyName = "Done";
            this.Done.Frozen = true;
            this.Done.HeaderText = "Done";
            this.Done.Name = "Done";
            this.Done.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Done.Visible = false;
            this.Done.Width = 40;
            // 
            // Error
            // 
            this.Error.DataPropertyName = "Error";
            this.Error.Frozen = true;
            this.Error.HeaderText = "Error";
            this.Error.Name = "Error";
            this.Error.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Error.Visible = false;
            this.Error.Width = 40;
            // 
            // ReWork
            // 
            this.ReWork.DataPropertyName = "ReWork";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Red;
            this.ReWork.DefaultCellStyle = dataGridViewCellStyle5;
            this.ReWork.Frozen = true;
            this.ReWork.HeaderText = "reW";
            this.ReWork.Name = "ReWork";
            this.ReWork.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReWork.Visible = false;
            this.ReWork.Width = 40;
            // 
            // RunTDS
            // 
            this.RunTDS.DataPropertyName = "RunTDS";
            this.RunTDS.Frozen = true;
            this.RunTDS.HeaderText = "Rtds";
            this.RunTDS.Name = "RunTDS";
            this.RunTDS.Width = 40;
            // 
            // runhana
            // 
            this.runhana.DataPropertyName = "runhana";
            this.runhana.Frozen = true;
            this.runhana.HeaderText = "Rha";
            this.runhana.Name = "runhana";
            this.runhana.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.runhana.Visible = false;
            this.runhana.Width = 40;
            // 
            // rungolike
            // 
            this.rungolike.DataPropertyName = "RungoLike";
            this.rungolike.Frozen = true;
            this.rungolike.HeaderText = "Rgo";
            this.rungolike.Name = "rungolike";
            this.rungolike.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rungolike.Visible = false;
            this.rungolike.Width = 40;
            // 
            // An
            // 
            this.An.DataPropertyName = "An";
            this.An.Frozen = true;
            this.An.HeaderText = "An";
            this.An.Name = "An";
            this.An.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.An.Width = 40;
            // 
            // Stop
            // 
            this.Stop.DataPropertyName = "stop";
            this.Stop.Frozen = true;
            this.Stop.HeaderText = "stop";
            this.Stop.Name = "Stop";
            this.Stop.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Stop.Width = 40;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.status.DataPropertyName = "status";
            this.status.Frozen = true;
            this.status.HeaderText = "Trạng thái";
            this.status.Name = "status";
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.status.Width = 238;
            // 
            // Action
            // 
            this.Action.DataPropertyName = "Action";
            this.Action.Frozen = true;
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Action.Width = 60;
            // 
            // UserAgent
            // 
            this.UserAgent.DataPropertyName = "UserAgent";
            this.UserAgent.Frozen = true;
            this.UserAgent.HeaderText = "UserAgent";
            this.UserAgent.Name = "UserAgent";
            this.UserAgent.ReadOnly = true;
            this.UserAgent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UserAgent.Visible = false;
            // 
            // Auto
            // 
            this.ClientSize = new System.Drawing.Size(1199, 688);
            this.Controls.Add(this.numberAction);
            this.Controls.Add(this.nuoinick);
            this.Controls.Add(this.locthanhvien);
            this.Controls.Add(this.isCheckBackUpFriendNew);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clean);
            this.Controls.Add(this.listAccounts);
            this.Controls.Add(this.passAddHana);
            this.Controls.Add(this.uidAddHana);
            this.Controls.Add(this.ConvertData);
            this.Controls.Add(this.checkLoadImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.JobMaxOfDay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.delayTo);
            this.Controls.Add(this.delayFrom);
            this.Controls.Add(this.delayNewfeed);
            this.Controls.Add(this.dgvAccounts);
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
        public System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.CheckBox checkLoadImage;
        private System.Windows.Forms.Button ConvertData;
        private System.Windows.Forms.TextBox uidAddHana;
        private System.Windows.Forms.TextBox passAddHana;
        private System.Windows.Forms.Button listAccounts;
        private System.Windows.Forms.Button clean;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox isCheckBackUpFriendNew;
        private System.Windows.Forms.Button locthanhvien;
        private System.Windows.Forms.CheckBox nuoinick;
        private System.Windows.Forms.TextBox numberAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fa;
        private System.Windows.Forms.DataGridViewTextBoxColumn cookie;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameTDS;
        private System.Windows.Forms.DataGridViewTextBoxColumn passTDS;
        private System.Windows.Forms.DataGridViewTextBoxColumn golike;
        private System.Windows.Forms.DataGridViewTextBoxColumn passgolike;
        private System.Windows.Forms.DataGridViewTextBoxColumn hana;
        private System.Windows.Forms.DataGridViewTextBoxColumn passhana;
        private System.Windows.Forms.DataGridViewTextBoxColumn XuHT;
        private System.Windows.Forms.DataGridViewTextBoxColumn XuThem;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Done;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReWork;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RunTDS;
        private System.Windows.Forms.DataGridViewCheckBoxColumn runhana;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rungolike;
        private System.Windows.Forms.DataGridViewCheckBoxColumn An;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Stop;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewButtonColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserAgent;
    }
}

