﻿namespace HVN_System.View.PlantKPI
{
    partial class frmKPIQualityNQC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKPIQualityNQC));
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label33 = new System.Windows.Forms.Label();
            this.lbCurrentDateTime = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.txtPPM = new System.Windows.Forms.TextBox();
            this.panel28 = new System.Windows.Forms.Panel();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckNQC = new DevExpress.XtraCharts.ChartControl();
            this.ckNQCTotal = new DevExpress.XtraCharts.ChartControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnMTR = new System.Windows.Forms.Panel();
            this.txtMTRLastMonth = new System.Windows.Forms.TextBox();
            this.txtMTRThisMonth = new System.Windows.Forms.TextBox();
            this.txtMTRLastday = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnHome = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel28.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckNQC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckNQCTotal)).BeginInit();
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
            this.textBox33.Text = "Quality";
            this.textBox33.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.panel1.Controls.Add(this.ckNQC);
            this.panel1.Controls.Add(this.ckNQCTotal);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(222, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1651, 850);
            this.panel1.TabIndex = 11;
            // 
            // ckNQC
            // 
            this.ckNQC.Legend.Name = "Default Legend";
            this.ckNQC.Location = new System.Drawing.Point(80, 278);
            this.ckNQC.Name = "ckNQC";
            this.ckNQC.PaletteName = "Equity";
            this.ckNQC.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckNQC.Size = new System.Drawing.Size(1232, 353);
            this.ckNQC.TabIndex = 19;
            // 
            // ckNQCTotal
            // 
            this.ckNQCTotal.Legend.Name = "Default Legend";
            this.ckNQCTotal.Location = new System.Drawing.Point(1373, 278);
            this.ckNQCTotal.Name = "ckNQCTotal";
            this.ckNQCTotal.PaletteName = "Equity";
            this.ckNQCTotal.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ckNQCTotal.Size = new System.Drawing.Size(213, 353);
            this.ckNQCTotal.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(589, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 45);
            this.label3.TabIndex = 2;
            this.label3.Text = "NQC DAILY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(1379, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 45);
            this.label1.TabIndex = 2;
            this.label1.Text = "NQC TOTAL";
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
            // frmKPIQualityNQC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pnMTR);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel28);
            this.Controls.Add(this.txtPPM);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.textBox33);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKPIQualityNQC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plant KPI";
            this.Load += new System.EventHandler(this.frmDashboardPlantKPI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckNQC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckNQCTotal)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox txtPPM;
        private System.Windows.Forms.Panel panel28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnMTR;
        private System.Windows.Forms.TextBox txtMTRLastday;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMTRThisMonth;
        private System.Windows.Forms.TextBox txtMTRLastMonth;
        private DevExpress.XtraEditors.SimpleButton btnHome;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraCharts.ChartControl ckNQCTotal;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraCharts.ChartControl ckNQC;
    }
}