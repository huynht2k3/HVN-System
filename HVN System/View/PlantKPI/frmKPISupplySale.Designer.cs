namespace HVN_System.View.PlantKPI
{
    partial class frmKPISupplySale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPISupplySale));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.ckSaleDetail = new DevExpress.XtraCharts.ChartControl();
            this.label2 = new System.Windows.Forms.Label();
            this.lbSaleCaption = new System.Windows.Forms.Label();
            this.ckSale = new DevExpress.XtraCharts.ChartControl();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.txtForecast = new System.Windows.Forms.TextBox();
            this.txtSaleAchievement = new System.Windows.Forms.TextBox();
            this.txtCumulSale = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            this.tmRefreshData = new System.Windows.Forms.Timer(this.components);
            this.txtOTD = new System.Windows.Forms.TextBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.textBox35 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel28.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckSaleDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckSale)).BeginInit();
            this.pnMTR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
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
            this.panel1.Controls.Add(this.ckSaleDetail);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lbSaleCaption);
            this.panel1.Controls.Add(this.ckSale);
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
            this.cboYear.Location = new System.Drawing.Point(947, 7);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(113, 53);
            this.cboYear.TabIndex = 7;
            this.cboYear.Text = "2021";
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Location = new System.Drawing.Point(746, 7);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(195, 53);
            this.cboMonth.TabIndex = 6;
            this.cboMonth.SelectionChangeCommitted += new System.EventHandler(this.cboMonth_SelectionChangeCommitted);
            // 
            // ckSaleDetail
            // 
            this.ckSaleDetail.Legend.Name = "Default Legend";
            this.ckSaleDetail.Location = new System.Drawing.Point(35, 432);
            this.ckSaleDetail.Name = "ckSaleDetail";
            this.ckSaleDetail.PaletteName = "Equity";
            this.ckSaleDetail.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckSaleDetail.Size = new System.Drawing.Size(1583, 371);
            this.ckSaleDetail.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(629, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(423, 45);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cumulated sales by project";
            // 
            // lbSaleCaption
            // 
            this.lbSaleCaption.AutoSize = true;
            this.lbSaleCaption.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lbSaleCaption.Location = new System.Drawing.Point(631, 10);
            this.lbSaleCaption.Name = "lbSaleCaption";
            this.lbSaleCaption.Size = new System.Drawing.Size(118, 45);
            this.lbSaleCaption.TabIndex = 2;
            this.lbSaleCaption.Text = "Sale in";
            // 
            // ckSale
            // 
            this.ckSale.Legend.Name = "Default Legend";
            this.ckSale.Location = new System.Drawing.Point(35, 69);
            this.ckSale.Name = "ckSale";
            this.ckSale.PaletteName = "Equity";
            this.ckSale.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckSale.Size = new System.Drawing.Size(1583, 277);
            this.ckSale.TabIndex = 0;
            // 
            // pnMTR
            // 
            this.pnMTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.pnMTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMTR.Controls.Add(this.txtForecast);
            this.pnMTR.Controls.Add(this.txtSaleAchievement);
            this.pnMTR.Controls.Add(this.txtCumulSale);
            this.pnMTR.Controls.Add(this.label7);
            this.pnMTR.Controls.Add(this.label8);
            this.pnMTR.Controls.Add(this.label9);
            this.pnMTR.Location = new System.Drawing.Point(24, 863);
            this.pnMTR.Name = "pnMTR";
            this.pnMTR.Size = new System.Drawing.Size(172, 148);
            this.pnMTR.TabIndex = 12;
            // 
            // txtForecast
            // 
            this.txtForecast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtForecast.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtForecast.Location = new System.Drawing.Point(4, 78);
            this.txtForecast.Name = "txtForecast";
            this.txtForecast.ReadOnly = true;
            this.txtForecast.Size = new System.Drawing.Size(72, 35);
            this.txtForecast.TabIndex = 0;
            this.txtForecast.Text = "17.2";
            this.txtForecast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSaleAchievement
            // 
            this.txtSaleAchievement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtSaleAchievement.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaleAchievement.Location = new System.Drawing.Point(91, 78);
            this.txtSaleAchievement.Name = "txtSaleAchievement";
            this.txtSaleAchievement.ReadOnly = true;
            this.txtSaleAchievement.Size = new System.Drawing.Size(72, 35);
            this.txtSaleAchievement.TabIndex = 0;
            this.txtSaleAchievement.Text = "59%";
            this.txtSaleAchievement.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCumulSale
            // 
            this.txtCumulSale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.txtCumulSale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCumulSale.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCumulSale.Location = new System.Drawing.Point(38, 42);
            this.txtCumulSale.Name = "txtCumulSale";
            this.txtCumulSale.ReadOnly = true;
            this.txtCumulSale.Size = new System.Drawing.Size(100, 28);
            this.txtCumulSale.TabIndex = 0;
            this.txtCumulSale.Text = "15.4 bd";
            this.txtCumulSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(72, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "Achievement";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 21);
            this.label8.TabIndex = 1;
            this.label8.Text = "Forecast";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(33, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 28);
            this.label9.TabIndex = 1;
            this.label9.Text = "Total Sale";
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
            // tmRefreshData
            // 
            this.tmRefreshData.Enabled = true;
            this.tmRefreshData.Interval = 160000;
            this.tmRefreshData.Tick += new System.EventHandler(this.tmRefreshData_Tick);
            // 
            // txtOTD
            // 
            this.txtOTD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtOTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtOTD.Location = new System.Drawing.Point(29, 324);
            this.txtOTD.Name = "txtOTD";
            this.txtOTD.ReadOnly = true;
            this.txtOTD.Size = new System.Drawing.Size(163, 29);
            this.txtOTD.TabIndex = 14;
            this.txtOTD.Text = "OTD: 100%";
            this.txtOTD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(29, 190);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(163, 131);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 16;
            this.pictureBox5.TabStop = false;
            // 
            // textBox35
            // 
            this.textBox35.BackColor = System.Drawing.Color.Green;
            this.textBox35.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox35.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox35.ForeColor = System.Drawing.Color.White;
            this.textBox35.Location = new System.Drawing.Point(24, 161);
            this.textBox35.Name = "textBox35";
            this.textBox35.ReadOnly = true;
            this.textBox35.Size = new System.Drawing.Size(172, 27);
            this.textBox35.TabIndex = 15;
            this.textBox35.Text = "Supply Chain";
            this.textBox35.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmKPISupplySale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.txtOTD);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.textBox35);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pnMTR);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel28);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKPISupplySale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckSaleDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckSale)).EndInit();
            this.pnMTR.ResumeLayout(false);
            this.pnMTR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
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
        private DevExpress.XtraCharts.ChartControl ckSale;
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.TextBox txtCumulSale;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSaleAchievement;
        private System.Windows.Forms.TextBox txtForecast;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.Label lbSaleCaption;
        private System.Windows.Forms.Timer tmRefreshData;
        private System.Windows.Forms.TextBox txtOTD;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.TextBox textBox35;
        private DevExpress.XtraCharts.ChartControl ckSaleDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
    }
}