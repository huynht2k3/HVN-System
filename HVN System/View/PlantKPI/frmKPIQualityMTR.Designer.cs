namespace HVN_System.View.PlantKPI
{
    partial class frmKPIQualityMTR
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPIQualityMTR));
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.txtPPM = new System.Windows.Forms.TextBox();
            this.panel28 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckMTRYearly = new DevExpress.XtraCharts.ChartControl();
            this.label2 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckMTR = new DevExpress.XtraCharts.ChartControl();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.txtMTRLastMonth = new System.Windows.Forms.TextBox();
            this.txtMTRThisMonth = new System.Windows.Forms.TextBox();
            this.txtMTRLastday = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tmRefreshData = new System.Windows.Forms.Timer(this.components);
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnMTRYearly = new DevExpress.XtraEditors.SimpleButton();
            this.panel28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckMTRYearly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckMTR)).BeginInit();
            this.pnMTR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox33
            // 
            this.textBox33.BackColor = System.Drawing.Color.Green;
            this.textBox33.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox33.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox33.ForeColor = System.Drawing.Color.White;
            this.textBox33.Location = new System.Drawing.Point(12, 161);
            this.textBox33.Name = "textBox33";
            this.textBox33.ReadOnly = true;
            this.textBox33.Size = new System.Drawing.Size(172, 27);
            this.textBox33.TabIndex = 0;
            this.textBox33.Text = "Quality";
            this.textBox33.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // txtPPM
            // 
            this.txtPPM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtPPM.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtPPM.Location = new System.Drawing.Point(16, 329);
            this.txtPPM.Name = "txtPPM";
            this.txtPPM.ReadOnly = true;
            this.txtPPM.Size = new System.Drawing.Size(168, 29);
            this.txtPPM.TabIndex = 0;
            this.txtPPM.Text = "PPM: 540";
            this.txtPPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.panel1.Controls.Add(this.ckMTRYearly);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ckMTR);
            this.panel1.Location = new System.Drawing.Point(222, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1651, 850);
            this.panel1.TabIndex = 11;
            // 
            // ckMTRYearly
            // 
            this.ckMTRYearly.Legend.Name = "Default Legend";
            this.ckMTRYearly.Location = new System.Drawing.Point(35, 484);
            this.ckMTRYearly.Name = "ckMTRYearly";
            this.ckMTRYearly.PaletteName = "Equity";
            this.ckMTRYearly.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckMTRYearly.Size = new System.Drawing.Size(1583, 332);
            this.ckMTRYearly.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(673, 427);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(328, 45);
            this.label2.TabIndex = 6;
            this.label2.Text = "MTR Last 12 Months";
            // 
            // cboYear
            // 
            this.cboYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.cboYear.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Items.AddRange(new object[] {
            "2021",
            "2022",
            "2023",
            "2024",
            "2025"});
            this.cboYear.Location = new System.Drawing.Point(932, 15);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(113, 53);
            this.cboYear.TabIndex = 5;
            this.cboYear.Text = "2021";
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            this.cboYear.SelectionChangeCommitted += new System.EventHandler(this.cboYear_SelectionChangeCommitted);
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Location = new System.Drawing.Point(731, 15);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(195, 53);
            this.cboMonth.TabIndex = 4;
            this.cboMonth.SelectionChangeCommitted += new System.EventHandler(this.cboMonth_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(601, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 45);
            this.label1.TabIndex = 2;
            this.label1.Text = "MTR in";
            // 
            // ckMTR
            // 
            this.ckMTR.Legend.Name = "Default Legend";
            this.ckMTR.Location = new System.Drawing.Point(35, 83);
            this.ckMTR.Name = "ckMTR";
            this.ckMTR.PaletteName = "Equity";
            this.ckMTR.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckMTR.Size = new System.Drawing.Size(1583, 332);
            this.ckMTR.TabIndex = 0;
            // 
            // pnMTR
            // 
            this.pnMTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.pnMTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMTR.Controls.Add(this.txtMTRLastMonth);
            this.pnMTR.Controls.Add(this.txtMTRThisMonth);
            this.pnMTR.Controls.Add(this.txtMTRLastday);
            this.pnMTR.Controls.Add(this.label7);
            this.pnMTR.Controls.Add(this.label8);
            this.pnMTR.Controls.Add(this.label9);
            this.pnMTR.Location = new System.Drawing.Point(16, 863);
            this.pnMTR.Name = "pnMTR";
            this.pnMTR.Size = new System.Drawing.Size(168, 148);
            this.pnMTR.TabIndex = 12;
            // 
            // txtMTRLastMonth
            // 
            this.txtMTRLastMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtMTRLastMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMTRLastMonth.Location = new System.Drawing.Point(4, 78);
            this.txtMTRLastMonth.Name = "txtMTRLastMonth";
            this.txtMTRLastMonth.ReadOnly = true;
            this.txtMTRLastMonth.Size = new System.Drawing.Size(72, 35);
            this.txtMTRLastMonth.TabIndex = 0;
            this.txtMTRLastMonth.Text = "78%";
            this.txtMTRLastMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMTRThisMonth
            // 
            this.txtMTRThisMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtMTRThisMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMTRThisMonth.Location = new System.Drawing.Point(91, 78);
            this.txtMTRThisMonth.Name = "txtMTRThisMonth";
            this.txtMTRThisMonth.ReadOnly = true;
            this.txtMTRThisMonth.Size = new System.Drawing.Size(72, 35);
            this.txtMTRThisMonth.TabIndex = 0;
            this.txtMTRThisMonth.Text = "59%";
            this.txtMTRThisMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMTRLastday
            // 
            this.txtMTRLastday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.txtMTRLastday.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMTRLastday.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMTRLastday.Location = new System.Drawing.Point(39, 42);
            this.txtMTRLastday.Name = "txtMTRLastday";
            this.txtMTRLastday.ReadOnly = true;
            this.txtMTRLastday.Size = new System.Drawing.Size(100, 28);
            this.txtMTRLastday.TabIndex = 0;
            this.txtMTRLastday.Text = "59%";
            this.txtMTRLastday.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(97, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "MTR m";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 21);
            this.label8.TabIndex = 1;
            this.label8.Text = "MTR m-1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(33, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 28);
            this.label9.TabIndex = 1;
            this.label9.Text = "MTR Daily";
            // 
            // tmRefreshData
            // 
            this.tmRefreshData.Enabled = true;
            this.tmRefreshData.Interval = 160000;
            this.tmRefreshData.Tick += new System.EventHandler(this.tmRefreshData_Tick);
            // 
            // btnHome
            // 
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(16, 573);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(72, 49);
            this.btnHome.TabIndex = 13;
            this.btnHome.Text = "Home";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(16, 198);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(168, 125);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // btnMTRYearly
            // 
            this.btnMTRYearly.Appearance.BackColor = System.Drawing.Color.Green;
            this.btnMTRYearly.Appearance.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnMTRYearly.Appearance.Options.UseBackColor = true;
            this.btnMTRYearly.Appearance.Options.UseFont = true;
            this.btnMTRYearly.Location = new System.Drawing.Point(16, 378);
            this.btnMTRYearly.Name = "btnMTRYearly";
            this.btnMTRYearly.Size = new System.Drawing.Size(168, 36);
            this.btnMTRYearly.TabIndex = 16;
            this.btnMTRYearly.Text = "MTR Yearly";
            this.btnMTRYearly.Click += new System.EventHandler(this.btnMTRYearly_Click);
            // 
            // frmKPIQualityMTR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnMTRYearly);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pnMTR);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel28);
            this.Controls.Add(this.txtPPM);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.textBox33);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKPIQualityMTR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckMTRYearly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckMTR)).EndInit();
            this.pnMTR.ResumeLayout(false);
            this.pnMTR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox33;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label lbCurrentDateTime;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox txtPPM;
        private System.Windows.Forms.Panel panel28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraCharts.ChartControl ckMTR;
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.TextBox txtMTRLastday;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMTRThisMonth;
        private System.Windows.Forms.TextBox txtMTRLastMonth;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmRefreshData;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
        private DevExpress.XtraCharts.ChartControl ckMTRYearly;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnMTRYearly;
    }
}