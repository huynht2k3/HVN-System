namespace HVN_System.View.PlantKPI
{
    partial class frmKPIHRLaborOTBySection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPIHRLaborOTBySection));
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckOT = new DevExpress.XtraCharts.ChartControl();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.txtCumOT = new System.Windows.Forms.TextBox();
            this.txtCumCredit = new System.Windows.Forms.TextBox();
            this.txtLastday = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel28.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckOT)).BeginInit();
            this.pnMTR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.textBox33.Text = "Labor";
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
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ckOT);
            this.panel1.Location = new System.Drawing.Point(222, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1651, 850);
            this.panel1.TabIndex = 11;
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
            this.cboYear.Location = new System.Drawing.Point(1051, 150);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(113, 53);
            this.cboYear.TabIndex = 7;
            this.cboYear.Text = "2021";
            this.cboYear.SelectedValueChanged += new System.EventHandler(this.cboYear_SelectedValueChanged);
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Location = new System.Drawing.Point(850, 150);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(195, 53);
            this.cboMonth.TabIndex = 6;
            this.cboMonth.SelectionChangeCommitted += new System.EventHandler(this.cboMonth_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(445, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cumulated OT By Section";
            // 
            // ckOT
            // 
            this.ckOT.Legend.Name = "Default Legend";
            this.ckOT.Location = new System.Drawing.Point(35, 241);
            this.ckOT.Name = "ckOT";
            this.ckOT.PaletteName = "Equity";
            this.ckOT.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckOT.Size = new System.Drawing.Size(1583, 476);
            this.ckOT.TabIndex = 0;
            // 
            // pnMTR
            // 
            this.pnMTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.pnMTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMTR.Controls.Add(this.txtCumOT);
            this.pnMTR.Controls.Add(this.txtCumCredit);
            this.pnMTR.Controls.Add(this.txtLastday);
            this.pnMTR.Controls.Add(this.label7);
            this.pnMTR.Controls.Add(this.label8);
            this.pnMTR.Controls.Add(this.label9);
            this.pnMTR.Location = new System.Drawing.Point(16, 863);
            this.pnMTR.Name = "pnMTR";
            this.pnMTR.Size = new System.Drawing.Size(168, 148);
            this.pnMTR.TabIndex = 12;
            // 
            // txtCumOT
            // 
            this.txtCumOT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtCumOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCumOT.Location = new System.Drawing.Point(4, 78);
            this.txtCumOT.Name = "txtCumOT";
            this.txtCumOT.ReadOnly = true;
            this.txtCumOT.Size = new System.Drawing.Size(72, 35);
            this.txtCumOT.TabIndex = 0;
            this.txtCumOT.Text = "78%";
            this.txtCumOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCumCredit
            // 
            this.txtCumCredit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtCumCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCumCredit.Location = new System.Drawing.Point(91, 78);
            this.txtCumCredit.Name = "txtCumCredit";
            this.txtCumCredit.ReadOnly = true;
            this.txtCumCredit.Size = new System.Drawing.Size(72, 35);
            this.txtCumCredit.TabIndex = 0;
            this.txtCumCredit.Text = "59%";
            this.txtCumCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLastday
            // 
            this.txtLastday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.txtLastday.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastday.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastday.Location = new System.Drawing.Point(39, 40);
            this.txtLastday.Name = "txtLastday";
            this.txtLastday.ReadOnly = true;
            this.txtLastday.Size = new System.Drawing.Size(100, 28);
            this.txtLastday.TabIndex = 0;
            this.txtLastday.Text = "59%";
            this.txtLastday.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(79, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "Cum Credit";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(0, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 21);
            this.label8.TabIndex = 1;
            this.label8.Text = "Cumul OT";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(64, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 28);
            this.label9.TabIndex = 1;
            this.label9.Text = "OT";
            // 
            // btnHome
            // 
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.Location = new System.Drawing.Point(16, 573);
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
            // frmKPIHRLaborOTBySection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pnMTR);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel28);
            this.Controls.Add(this.textBox33);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKPIHRLaborOTBySection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckOT)).EndInit();
            this.pnMTR.ResumeLayout(false);
            this.pnMTR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox33;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label lbCurrentDateTime;
        private System.Windows.Forms.Panel panel28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraCharts.ChartControl ckOT;
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.TextBox txtLastday;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCumCredit;
        private System.Windows.Forms.TextBox txtCumOT;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
    }
}