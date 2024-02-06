namespace HVN_System.View.PlantKPI
{
    partial class frmKPIHRLaborResouces
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPIHRLaborResouces));
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckTurnoverRate = new DevExpress.XtraCharts.ChartControl();
            this.btnAbsentInfo = new DevExpress.XtraEditors.SimpleButton();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ckAbsenteeism = new DevExpress.XtraCharts.ChartControl();
            this.label2 = new System.Windows.Forms.Label();
            this.ckHeadcount = new DevExpress.XtraCharts.ChartControl();
            this.label4 = new System.Windows.Forms.Label();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.txtLaborAndOT = new System.Windows.Forms.TextBox();
            this.txtSOP = new System.Windows.Forms.TextBox();
            this.txtLabor = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.panel28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckTurnoverRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAbsenteeism)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckHeadcount)).BeginInit();
            this.pnMTR.SuspendLayout();
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
            this.panel28.Paint += new System.Windows.Forms.PaintEventHandler(this.panel28_Paint);
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
            this.panel1.Controls.Add(this.ckTurnoverRate);
            this.panel1.Controls.Add(this.btnAbsentInfo);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ckAbsenteeism);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ckHeadcount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(222, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1651, 850);
            this.panel1.TabIndex = 11;
            // 
            // ckTurnoverRate
            // 
            this.ckTurnoverRate.Legend.Name = "Default Legend";
            this.ckTurnoverRate.Location = new System.Drawing.Point(28, 55);
            this.ckTurnoverRate.Name = "ckTurnoverRate";
            this.ckTurnoverRate.PaletteName = "Equity";
            this.ckTurnoverRate.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckTurnoverRate.Size = new System.Drawing.Size(1583, 234);
            this.ckTurnoverRate.TabIndex = 11;
            // 
            // btnAbsentInfo
            // 
            this.btnAbsentInfo.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAbsentInfo.ImageOptions.Image")));
            this.btnAbsentInfo.Location = new System.Drawing.Point(884, 576);
            this.btnAbsentInfo.Name = "btnAbsentInfo";
            this.btnAbsentInfo.Size = new System.Drawing.Size(47, 37);
            this.btnAbsentInfo.TabIndex = 10;
            this.btnAbsentInfo.Click += new System.EventHandler(this.btnAbsentInfo_Click);
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
            this.cboYear.Location = new System.Drawing.Point(228, 0);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(113, 53);
            this.cboYear.TabIndex = 9;
            this.cboYear.Text = "2021";
            this.cboYear.SelectedValueChanged += new System.EventHandler(this.cboYear_SelectedValueChanged);
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Location = new System.Drawing.Point(26, 0);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(195, 53);
            this.cboMonth.TabIndex = 8;
            this.cboMonth.SelectionChangeCommitted += new System.EventHandler(this.cboMonth_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(742, 571);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 45);
            this.label3.TabIndex = 5;
            this.label3.Text = "Absence";
            // 
            // ckAbsenteeism
            // 
            this.ckAbsenteeism.Legend.Name = "Default Legend";
            this.ckAbsenteeism.Location = new System.Drawing.Point(28, 619);
            this.ckAbsenteeism.Name = "ckAbsenteeism";
            this.ckAbsenteeism.PaletteName = "Equity";
            this.ckAbsenteeism.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckAbsenteeism.Size = new System.Drawing.Size(1583, 221);
            this.ckAbsenteeism.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(719, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 45);
            this.label2.TabIndex = 3;
            this.label2.Text = "Headcount";
            // 
            // ckHeadcount
            // 
            this.ckHeadcount.Legend.Name = "Default Legend";
            this.ckHeadcount.Location = new System.Drawing.Point(28, 335);
            this.ckHeadcount.Name = "ckHeadcount";
            this.ckHeadcount.PaletteName = "Equity";
            this.ckHeadcount.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckHeadcount.Size = new System.Drawing.Size(1583, 234);
            this.ckHeadcount.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(700, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 45);
            this.label4.TabIndex = 1;
            this.label4.Text = "Turnover Rate";
            // 
            // pnMTR
            // 
            this.pnMTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.pnMTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMTR.Controls.Add(this.txtLaborAndOT);
            this.pnMTR.Controls.Add(this.txtSOP);
            this.pnMTR.Controls.Add(this.txtLabor);
            this.pnMTR.Controls.Add(this.label7);
            this.pnMTR.Controls.Add(this.label8);
            this.pnMTR.Controls.Add(this.label9);
            this.pnMTR.Location = new System.Drawing.Point(16, 863);
            this.pnMTR.Name = "pnMTR";
            this.pnMTR.Size = new System.Drawing.Size(168, 148);
            this.pnMTR.TabIndex = 12;
            // 
            // txtLaborAndOT
            // 
            this.txtLaborAndOT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtLaborAndOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLaborAndOT.Location = new System.Drawing.Point(4, 78);
            this.txtLaborAndOT.Name = "txtLaborAndOT";
            this.txtLaborAndOT.ReadOnly = true;
            this.txtLaborAndOT.Size = new System.Drawing.Size(72, 35);
            this.txtLaborAndOT.TabIndex = 0;
            this.txtLaborAndOT.Text = "120";
            this.txtLaborAndOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSOP
            // 
            this.txtSOP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtSOP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSOP.Location = new System.Drawing.Point(91, 78);
            this.txtSOP.Name = "txtSOP";
            this.txtSOP.ReadOnly = true;
            this.txtSOP.Size = new System.Drawing.Size(72, 35);
            this.txtSOP.TabIndex = 0;
            this.txtSOP.Text = "125";
            this.txtSOP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLabor
            // 
            this.txtLabor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.txtLabor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLabor.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabor.Location = new System.Drawing.Point(32, 40);
            this.txtLabor.Name = "txtLabor";
            this.txtLabor.ReadOnly = true;
            this.txtLabor.Size = new System.Drawing.Size(100, 28);
            this.txtLabor.TabIndex = 0;
            this.txtLabor.Text = "100";
            this.txtLabor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(86, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "Target SOP";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(2, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 21);
            this.label8.TabIndex = 1;
            this.label8.Text = "Labor+OT";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(52, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 28);
            this.label9.TabIndex = 1;
            this.label9.Text = "Labor";
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
            // frmKPIHRLaborResouces
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
            this.Name = "frmKPIHRLaborResouces";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckTurnoverRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAbsenteeism)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckHeadcount)).EndInit();
            this.pnMTR.ResumeLayout(false);
            this.pnMTR.PerformLayout();
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
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.TextBox txtLabor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSOP;
        private System.Windows.Forms.TextBox txtLaborAndOT;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraCharts.ChartControl ckHeadcount;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraCharts.ChartControl ckAbsenteeism;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
        private DevExpress.XtraEditors.SimpleButton btnAbsentInfo;
        private DevExpress.XtraCharts.ChartControl ckTurnoverRate;
        private System.Windows.Forms.Label label4;
    }
}