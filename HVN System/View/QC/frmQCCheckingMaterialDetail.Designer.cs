namespace HVN_System.View.QC
{
    partial class frmQCCheckingMaterialDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQCCheckingMaterialDetail));
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.dtpLotNo = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQTYNG = new System.Windows.Forms.TextBox();
            this.txtQTYOK = new System.Windows.Forms.TextBox();
            this.txtPN = new System.Windows.Forms.TextBox();
            this.txtLabelCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nmOth = new System.Windows.Forms.NumericUpDown();
            this.nmDim = new System.Windows.Forms.NumericUpDown();
            this.nmSha = new System.Windows.Forms.NumericUpDown();
            this.nmEle = new System.Windows.Forms.NumericUpDown();
            this.nmRus = new System.Windows.Forms.NumericUpDown();
            this.nmBur = new System.Windows.Forms.NumericUpDown();
            this.nmScr = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmOth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmSha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmEle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmScr)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "WEIGHT (G)";
            this.gridColumn4.FieldName = "weight";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Width = 149;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.dtpLotNo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtQTYNG);
            this.groupBox1.Controls.Add(this.txtQTYOK);
            this.groupBox1.Controls.Add(this.txtPN);
            this.groupBox1.Controls.Add(this.txtLabelCode);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(9, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 223);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "THÔNG TIN TEM";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(28, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 19);
            this.label13.TabIndex = 4;
            this.label13.Text = "NHẬP SCAN";
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.White;
            this.txtBarcode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.ForeColor = System.Drawing.Color.Blue;
            this.txtBarcode.Location = new System.Drawing.Point(179, 29);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(200, 26);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // dtpLotNo
            // 
            this.dtpLotNo.CustomFormat = " dd/MM/yyyy";
            this.dtpLotNo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpLotNo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotNo.Location = new System.Drawing.Point(179, 123);
            this.dtpLotNo.Name = "dtpLotNo";
            this.dtpLotNo.Size = new System.Drawing.Size(200, 26);
            this.dtpLotNo.TabIndex = 4;
            this.dtpLotNo.ValueChanged += new System.EventHandler(this.dtpLotNo_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "SỐ LƯỢNG LỖI";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "SỐ LƯỢNG OK";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "LOT NO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "MÃ VẬT TƯ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "MÃ TEM";
            // 
            // txtQTYNG
            // 
            this.txtQTYNG.BackColor = System.Drawing.Color.White;
            this.txtQTYNG.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQTYNG.ForeColor = System.Drawing.Color.Blue;
            this.txtQTYNG.Location = new System.Drawing.Point(179, 185);
            this.txtQTYNG.Name = "txtQTYNG";
            this.txtQTYNG.ReadOnly = true;
            this.txtQTYNG.Size = new System.Drawing.Size(200, 26);
            this.txtQTYNG.TabIndex = 6;
            // 
            // txtQTYOK
            // 
            this.txtQTYOK.BackColor = System.Drawing.Color.White;
            this.txtQTYOK.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQTYOK.ForeColor = System.Drawing.Color.Blue;
            this.txtQTYOK.Location = new System.Drawing.Point(179, 154);
            this.txtQTYOK.Name = "txtQTYOK";
            this.txtQTYOK.ReadOnly = true;
            this.txtQTYOK.Size = new System.Drawing.Size(200, 26);
            this.txtQTYOK.TabIndex = 5;
            // 
            // txtPN
            // 
            this.txtPN.BackColor = System.Drawing.Color.White;
            this.txtPN.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPN.ForeColor = System.Drawing.Color.Blue;
            this.txtPN.Location = new System.Drawing.Point(179, 92);
            this.txtPN.Name = "txtPN";
            this.txtPN.ReadOnly = true;
            this.txtPN.Size = new System.Drawing.Size(200, 26);
            this.txtPN.TabIndex = 3;
            // 
            // txtLabelCode
            // 
            this.txtLabelCode.BackColor = System.Drawing.Color.White;
            this.txtLabelCode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabelCode.ForeColor = System.Drawing.Color.Blue;
            this.txtLabelCode.Location = new System.Drawing.Point(179, 61);
            this.txtLabelCode.Name = "txtLabelCode";
            this.txtLabelCode.ReadOnly = true;
            this.txtLabelCode.Size = new System.Drawing.Size(200, 26);
            this.txtLabelCode.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "LỖI XƯỚC";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(28, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 19);
            this.label7.TabIndex = 1;
            this.label7.Text = "BAVIA";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(28, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "GỈ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(28, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 19);
            this.label9.TabIndex = 1;
            this.label9.Text = "LỖI ĐIỆN";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(28, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 19);
            this.label10.TabIndex = 1;
            this.label10.Text = "SAI HÌNH DẠNG";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(28, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(136, 19);
            this.label11.TabIndex = 1;
            this.label11.Text = "SAI KÍCH THƯỚC";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(28, 210);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 19);
            this.label12.TabIndex = 1;
            this.label12.Text = "LỖI KHÁC";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nmOth);
            this.groupBox2.Controls.Add(this.nmDim);
            this.groupBox2.Controls.Add(this.nmSha);
            this.groupBox2.Controls.Add(this.nmEle);
            this.groupBox2.Controls.Add(this.nmRus);
            this.groupBox2.Controls.Add(this.nmBur);
            this.groupBox2.Controls.Add(this.nmScr);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(9, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 248);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "THÔNG TIN HÀNG LỖI";
            // 
            // nmOth
            // 
            this.nmOth.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmOth.Location = new System.Drawing.Point(179, 208);
            this.nmOth.Name = "nmOth";
            this.nmOth.Size = new System.Drawing.Size(200, 26);
            this.nmOth.TabIndex = 15;
            this.nmOth.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmDim
            // 
            this.nmDim.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmDim.Location = new System.Drawing.Point(179, 178);
            this.nmDim.Name = "nmDim";
            this.nmDim.Size = new System.Drawing.Size(200, 26);
            this.nmDim.TabIndex = 13;
            this.nmDim.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmSha
            // 
            this.nmSha.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmSha.Location = new System.Drawing.Point(179, 148);
            this.nmSha.Name = "nmSha";
            this.nmSha.Size = new System.Drawing.Size(200, 26);
            this.nmSha.TabIndex = 12;
            this.nmSha.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmEle
            // 
            this.nmEle.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmEle.Location = new System.Drawing.Point(179, 118);
            this.nmEle.Name = "nmEle";
            this.nmEle.Size = new System.Drawing.Size(200, 26);
            this.nmEle.TabIndex = 11;
            this.nmEle.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmRus
            // 
            this.nmRus.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmRus.Location = new System.Drawing.Point(179, 88);
            this.nmRus.Name = "nmRus";
            this.nmRus.Size = new System.Drawing.Size(200, 26);
            this.nmRus.TabIndex = 10;
            this.nmRus.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmBur
            // 
            this.nmBur.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmBur.Location = new System.Drawing.Point(179, 58);
            this.nmBur.Name = "nmBur";
            this.nmBur.Size = new System.Drawing.Size(200, 26);
            this.nmBur.TabIndex = 9;
            this.nmBur.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // nmScr
            // 
            this.nmScr.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmScr.Location = new System.Drawing.Point(179, 28);
            this.nmScr.Name = "nmScr";
            this.nmScr.Size = new System.Drawing.Size(200, 26);
            this.nmScr.TabIndex = 8;
            this.nmScr.ValueChanged += new System.EventHandler(this.nmScr_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.Location = new System.Drawing.Point(287, 495);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 35);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "BỎ QUA";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.ImageOptions.Image")));
            this.btnConfirm.Location = new System.Drawing.Point(187, 495);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(98, 35);
            this.btnConfirm.TabIndex = 16;
            this.btnConfirm.Text = "XÁC NHẬN";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmQCCheckingMaterialDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 551);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQCCheckingMaterialDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CHECKING RAW MATERIAL";
            this.Load += new System.EventHandler(this.frmWHMaterialIssueToPD_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmOth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmSha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmEle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmScr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.TextBox txtQTYNG;
        private System.Windows.Forms.TextBox txtQTYOK;
        private System.Windows.Forms.TextBox txtPN;
        private System.Windows.Forms.TextBox txtLabelCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpLotNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nmOth;
        private System.Windows.Forms.NumericUpDown nmDim;
        private System.Windows.Forms.NumericUpDown nmSha;
        private System.Windows.Forms.NumericUpDown nmEle;
        private System.Windows.Forms.NumericUpDown nmRus;
        private System.Windows.Forms.NumericUpDown nmBur;
        private System.Windows.Forms.NumericUpDown nmScr;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBarcode;
    }
}