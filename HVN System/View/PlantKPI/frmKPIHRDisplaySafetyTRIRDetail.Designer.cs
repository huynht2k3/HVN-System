namespace HVN_System.View.PlantKPI
{
    partial class frmKPIHRDisplaySafetyTRIRDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPIHRDisplaySafetyTRIRDetail));
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtNoDayWithoutAccident = new System.Windows.Forms.TextBox();
            this.textBox32 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNumberDS = new System.Windows.Forms.TextBox();
            this.txtNumberDSNotDone = new System.Windows.Forms.TextBox();
            this.txtNumberDSDone = new System.Windows.Forms.TextBox();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDateTRIR = new System.Windows.Forms.DateTimePicker();
            this.lbDDeath = new System.Windows.Forms.Label();
            this.lbMDeath = new System.Windows.Forms.Label();
            this.lbDLost = new System.Windows.Forms.Label();
            this.lbDNoLost = new System.Windows.Forms.Label();
            this.lbMNoLost = new System.Windows.Forms.Label();
            this.lbMLost = new System.Windows.Forms.Label();
            this.lbDFirst = new System.Windows.Forms.Label();
            this.lbDNear = new System.Windows.Forms.Label();
            this.lbDRisky = new System.Windows.Forms.Label();
            this.lbMFirst = new System.Windows.Forms.Label();
            this.lbMNear = new System.Windows.Forms.Label();
            this.lbMRisky = new System.Windows.Forms.Label();
            this.txtDateTRIR = new System.Windows.Forms.Label();
            this.txtYearTRIR = new System.Windows.Forms.Label();
            this.panel28.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnMTR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Segoe UI", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(539, 24);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(823, 89);
            this.label33.TabIndex = 7;
            this.label33.Text = "KPI PLANT MANAGEMENT";
            // 
            // lbCurrentDateTime
            // 
            this.lbCurrentDateTime.AutoSize = true;
            this.lbCurrentDateTime.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurrentDateTime.Location = new System.Drawing.Point(1504, 47);
            this.lbCurrentDateTime.Name = "lbCurrentDateTime";
            this.lbCurrentDateTime.Size = new System.Drawing.Size(357, 54);
            this.lbCurrentDateTime.TabIndex = 1;
            this.lbCurrentDateTime.Text = "06/02/2021 13:02";
            // 
            // panel28
            // 
            this.panel28.BackColor = System.Drawing.Color.White;
            this.panel28.Controls.Add(this.pictureBox1);
            this.panel28.Controls.Add(this.label33);
            this.panel28.Controls.Add(this.lbCurrentDateTime);
            this.panel28.Location = new System.Drawing.Point(12, 12);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(1861, 128);
            this.panel28.TabIndex = 10;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Status";
            this.gridColumn13.FieldName = "IsAction";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 5;
            this.gridColumn13.Width = 87;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "Status";
            this.gridColumn14.FieldName = "IsAction";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            this.gridColumn14.Width = 87;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.panel1.Controls.Add(this.lbMRisky);
            this.panel1.Controls.Add(this.lbDRisky);
            this.panel1.Controls.Add(this.lbMNear);
            this.panel1.Controls.Add(this.lbDNear);
            this.panel1.Controls.Add(this.lbMFirst);
            this.panel1.Controls.Add(this.lbDFirst);
            this.panel1.Controls.Add(this.lbMLost);
            this.panel1.Controls.Add(this.lbMNoLost);
            this.panel1.Controls.Add(this.lbDNoLost);
            this.panel1.Controls.Add(this.lbDLost);
            this.panel1.Controls.Add(this.lbMDeath);
            this.panel1.Controls.Add(this.txtYearTRIR);
            this.panel1.Controls.Add(this.txtDateTRIR);
            this.panel1.Controls.Add(this.lbDDeath);
            this.panel1.Controls.Add(this.dtpDateTRIR);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Location = new System.Drawing.Point(222, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1651, 850);
            this.panel1.TabIndex = 11;
            // 
            // txtNoDayWithoutAccident
            // 
            this.txtNoDayWithoutAccident.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNoDayWithoutAccident.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtNoDayWithoutAccident.Location = new System.Drawing.Point(21, 315);
            this.txtNoDayWithoutAccident.Multiline = true;
            this.txtNoDayWithoutAccident.Name = "txtNoDayWithoutAccident";
            this.txtNoDayWithoutAccident.ReadOnly = true;
            this.txtNoDayWithoutAccident.Size = new System.Drawing.Size(174, 50);
            this.txtNoDayWithoutAccident.TabIndex = 14;
            this.txtNoDayWithoutAccident.Text = "450 days without accident";
            this.txtNoDayWithoutAccident.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox32
            // 
            this.textBox32.BackColor = System.Drawing.Color.Green;
            this.textBox32.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox32.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox32.ForeColor = System.Drawing.Color.White;
            this.textBox32.Location = new System.Drawing.Point(21, 161);
            this.textBox32.Name = "textBox32";
            this.textBox32.ReadOnly = true;
            this.textBox32.Size = new System.Drawing.Size(174, 27);
            this.textBox32.TabIndex = 15;
            this.textBox32.Text = "Safety";
            this.textBox32.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(-2, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(206, 28);
            this.label9.TabIndex = 1;
            this.label9.Text = "Dangerous Situation";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(26, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 21);
            this.label8.TabIndex = 1;
            this.label8.Text = "Done";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(112, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "Not done";
            // 
            // txtNumberDS
            // 
            this.txtNumberDS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.txtNumberDS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNumberDS.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberDS.Location = new System.Drawing.Point(51, 34);
            this.txtNumberDS.Name = "txtNumberDS";
            this.txtNumberDS.ReadOnly = true;
            this.txtNumberDS.Size = new System.Drawing.Size(100, 46);
            this.txtNumberDS.TabIndex = 0;
            this.txtNumberDS.Text = "0";
            this.txtNumberDS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtNumberDSNotDone
            // 
            this.txtNumberDSNotDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNumberDSNotDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNumberDSNotDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberDSNotDone.Location = new System.Drawing.Point(115, 77);
            this.txtNumberDSNotDone.Name = "txtNumberDSNotDone";
            this.txtNumberDSNotDone.ReadOnly = true;
            this.txtNumberDSNotDone.Size = new System.Drawing.Size(72, 47);
            this.txtNumberDSNotDone.TabIndex = 0;
            this.txtNumberDSNotDone.Text = "5";
            this.txtNumberDSNotDone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtNumberDSDone
            // 
            this.txtNumberDSDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNumberDSDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNumberDSDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberDSDone.Location = new System.Drawing.Point(14, 77);
            this.txtNumberDSDone.Name = "txtNumberDSDone";
            this.txtNumberDSDone.ReadOnly = true;
            this.txtNumberDSDone.Size = new System.Drawing.Size(72, 47);
            this.txtNumberDSDone.TabIndex = 0;
            this.txtNumberDSDone.Text = "3";
            this.txtNumberDSDone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnMTR
            // 
            this.pnMTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.pnMTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMTR.Controls.Add(this.txtNumberDSDone);
            this.pnMTR.Controls.Add(this.txtNumberDSNotDone);
            this.pnMTR.Controls.Add(this.txtNumberDS);
            this.pnMTR.Controls.Add(this.label7);
            this.pnMTR.Controls.Add(this.label8);
            this.pnMTR.Controls.Add(this.label9);
            this.pnMTR.Location = new System.Drawing.Point(10, 863);
            this.pnMTR.Name = "pnMTR";
            this.pnMTR.Size = new System.Drawing.Size(204, 148);
            this.pnMTR.TabIndex = 12;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(21, 194);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(174, 115);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // btnHome
            // 
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(25, 568);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(72, 49);
            this.btnHome.TabIndex = 13;
            this.btnHome.Text = "Back";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HVN_System.Properties.Resources.Hutchinson;
            this.pictureBox1.Location = new System.Drawing.Point(9, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(514, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::HVN_System.Properties.Resources.TRIR;
            this.pictureBox3.Location = new System.Drawing.Point(130, 154);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1410, 610);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 17;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(622, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 45);
            this.label1.TabIndex = 18;
            this.label1.Text = "TRIR on";
            // 
            // dtpDateTRIR
            // 
            this.dtpDateTRIR.CustomFormat = "dd MMM,yyyy";
            this.dtpDateTRIR.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.dtpDateTRIR.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTRIR.Location = new System.Drawing.Point(757, 46);
            this.dtpDateTRIR.Name = "dtpDateTRIR";
            this.dtpDateTRIR.Size = new System.Drawing.Size(230, 50);
            this.dtpDateTRIR.TabIndex = 19;
            this.dtpDateTRIR.ValueChanged += new System.EventHandler(this.dtpDateTRIR_ValueChanged);
            // 
            // lbDDeath
            // 
            this.lbDDeath.AutoSize = true;
            this.lbDDeath.BackColor = System.Drawing.Color.Red;
            this.lbDDeath.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lbDDeath.Location = new System.Drawing.Point(564, 376);
            this.lbDDeath.Name = "lbDDeath";
            this.lbDDeath.Size = new System.Drawing.Size(23, 28);
            this.lbDDeath.TabIndex = 20;
            this.lbDDeath.Text = "0";
            // 
            // lbMDeath
            // 
            this.lbMDeath.AutoSize = true;
            this.lbMDeath.BackColor = System.Drawing.Color.Red;
            this.lbMDeath.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lbMDeath.Location = new System.Drawing.Point(1096, 376);
            this.lbMDeath.Name = "lbMDeath";
            this.lbMDeath.Size = new System.Drawing.Size(23, 28);
            this.lbMDeath.TabIndex = 21;
            this.lbMDeath.Text = "0";
            // 
            // lbDLost
            // 
            this.lbDLost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(1)))));
            this.lbDLost.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDLost.Location = new System.Drawing.Point(564, 426);
            this.lbDLost.Name = "lbDLost";
            this.lbDLost.Size = new System.Drawing.Size(24, 30);
            this.lbDLost.TabIndex = 22;
            this.lbDLost.Text = "0";
            // 
            // lbDNoLost
            // 
            this.lbDNoLost.AutoSize = true;
            this.lbDNoLost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(113)))), ((int)(((byte)(193)))));
            this.lbDNoLost.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDNoLost.Location = new System.Drawing.Point(564, 490);
            this.lbDNoLost.Name = "lbDNoLost";
            this.lbDNoLost.Size = new System.Drawing.Size(24, 30);
            this.lbDNoLost.TabIndex = 23;
            this.lbDNoLost.Text = "0";
            // 
            // lbMNoLost
            // 
            this.lbMNoLost.AutoSize = true;
            this.lbMNoLost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(113)))), ((int)(((byte)(193)))));
            this.lbMNoLost.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMNoLost.Location = new System.Drawing.Point(1096, 490);
            this.lbMNoLost.Name = "lbMNoLost";
            this.lbMNoLost.Size = new System.Drawing.Size(24, 30);
            this.lbMNoLost.TabIndex = 24;
            this.lbMNoLost.Text = "0";
            // 
            // lbMLost
            // 
            this.lbMLost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(1)))));
            this.lbMLost.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMLost.Location = new System.Drawing.Point(1096, 426);
            this.lbMLost.Name = "lbMLost";
            this.lbMLost.Size = new System.Drawing.Size(24, 30);
            this.lbMLost.TabIndex = 25;
            this.lbMLost.Text = "0";
            // 
            // lbDFirst
            // 
            this.lbDFirst.AutoSize = true;
            this.lbDFirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(1)))));
            this.lbDFirst.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDFirst.Location = new System.Drawing.Point(564, 559);
            this.lbDFirst.Name = "lbDFirst";
            this.lbDFirst.Size = new System.Drawing.Size(24, 30);
            this.lbDFirst.TabIndex = 26;
            this.lbDFirst.Text = "0";
            // 
            // lbDNear
            // 
            this.lbDNear.AutoSize = true;
            this.lbDNear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(204)))), ((int)(((byte)(170)))));
            this.lbDNear.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDNear.Location = new System.Drawing.Point(564, 627);
            this.lbDNear.Name = "lbDNear";
            this.lbDNear.Size = new System.Drawing.Size(24, 30);
            this.lbDNear.TabIndex = 27;
            this.lbDNear.Text = "0";
            // 
            // lbDRisky
            // 
            this.lbDRisky.AutoSize = true;
            this.lbDRisky.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.lbDRisky.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDRisky.Location = new System.Drawing.Point(564, 702);
            this.lbDRisky.Name = "lbDRisky";
            this.lbDRisky.Size = new System.Drawing.Size(24, 30);
            this.lbDRisky.TabIndex = 28;
            this.lbDRisky.Text = "0";
            // 
            // lbMFirst
            // 
            this.lbMFirst.AutoSize = true;
            this.lbMFirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(1)))));
            this.lbMFirst.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMFirst.Location = new System.Drawing.Point(1096, 559);
            this.lbMFirst.Name = "lbMFirst";
            this.lbMFirst.Size = new System.Drawing.Size(24, 30);
            this.lbMFirst.TabIndex = 26;
            this.lbMFirst.Text = "0";
            // 
            // lbMNear
            // 
            this.lbMNear.AutoSize = true;
            this.lbMNear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(204)))), ((int)(((byte)(170)))));
            this.lbMNear.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMNear.Location = new System.Drawing.Point(1096, 627);
            this.lbMNear.Name = "lbMNear";
            this.lbMNear.Size = new System.Drawing.Size(24, 30);
            this.lbMNear.TabIndex = 27;
            this.lbMNear.Text = "0";
            // 
            // lbMRisky
            // 
            this.lbMRisky.AutoSize = true;
            this.lbMRisky.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(255)))), ((int)(((byte)(153)))));
            this.lbMRisky.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMRisky.Location = new System.Drawing.Point(1096, 702);
            this.lbMRisky.Name = "lbMRisky";
            this.lbMRisky.Size = new System.Drawing.Size(24, 30);
            this.lbMRisky.TabIndex = 28;
            this.lbMRisky.Text = "0";
            // 
            // txtDateTRIR
            // 
            this.txtDateTRIR.AutoSize = true;
            this.txtDateTRIR.BackColor = System.Drawing.Color.White;
            this.txtDateTRIR.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateTRIR.Location = new System.Drawing.Point(508, 308);
            this.txtDateTRIR.Name = "txtDateTRIR";
            this.txtDateTRIR.Size = new System.Drawing.Size(131, 30);
            this.txtDateTRIR.TabIndex = 20;
            this.txtDateTRIR.Text = "05 Apr,2022";
            // 
            // txtYearTRIR
            // 
            this.txtYearTRIR.AutoSize = true;
            this.txtYearTRIR.BackColor = System.Drawing.Color.White;
            this.txtYearTRIR.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYearTRIR.Location = new System.Drawing.Point(1080, 308);
            this.txtYearTRIR.Name = "txtYearTRIR";
            this.txtYearTRIR.Size = new System.Drawing.Size(61, 30);
            this.txtYearTRIR.TabIndex = 20;
            this.txtYearTRIR.Text = "2022";
            // 
            // frmKPIHRDisplaySafetyTRIRDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.txtNoDayWithoutAccident);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.textBox32);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pnMTR);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel28);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKPIHRDisplaySafetyTRIRDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnMTR.ResumeLayout(false);
            this.pnMTR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label lbCurrentDateTime;
        private System.Windows.Forms.Panel panel28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.TextBox txtNoDayWithoutAccident;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox textBox32;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNumberDS;
        private System.Windows.Forms.TextBox txtNumberDSNotDone;
        private System.Windows.Forms.TextBox txtNumberDSDone;
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.DateTimePicker dtpDateTRIR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbMRisky;
        private System.Windows.Forms.Label lbDRisky;
        private System.Windows.Forms.Label lbMNear;
        private System.Windows.Forms.Label lbDNear;
        private System.Windows.Forms.Label lbMFirst;
        private System.Windows.Forms.Label lbDFirst;
        private System.Windows.Forms.Label lbMLost;
        private System.Windows.Forms.Label lbMNoLost;
        private System.Windows.Forms.Label lbDNoLost;
        private System.Windows.Forms.Label lbDLost;
        private System.Windows.Forms.Label lbMDeath;
        private System.Windows.Forms.Label txtYearTRIR;
        private System.Windows.Forms.Label txtDateTRIR;
        private System.Windows.Forms.Label lbDDeath;
    }
}