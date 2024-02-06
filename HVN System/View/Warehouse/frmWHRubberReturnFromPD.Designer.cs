namespace HVN_System.View.Warehouse
{
    partial class frmWHRubberReturnFromPD
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
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtRawData = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpLotNo = new System.Windows.Forms.DateTimePicker();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpExpiredDate = new System.Windows.Forms.DateTimePicker();
            this.ckInputManual = new System.Windows.Forms.CheckBox();
            this.lbError = new System.Windows.Forms.Label();
            this.lbFormName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPalletWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNetWeight = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPIC = new System.Windows.Forms.TextBox();
            this.txtRubberName = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWHRRCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtWeight.ForeColor = System.Drawing.Color.Black;
            this.txtWeight.Location = new System.Drawing.Point(1468, 335);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(257, 40);
            this.txtWeight.TabIndex = 5;
            this.txtWeight.Text = "0";
            this.txtWeight.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(23, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(310, 33);
            this.label3.TabIndex = 16;
            this.label3.Text = "RUBBER PN/ MÃ CAO SU";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(23, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 33);
            this.label4.TabIndex = 16;
            this.label4.Text = "LOT NO";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(912, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(464, 33);
            this.label5.TabIndex = 16;
            this.label5.Text = "GROSS WEIGHT/ KHỐI LƯỢNG TỔNG";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 619);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1831, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbStatus
            // 
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(59, 17);
            this.lbStatus.Text = "Loading...";
            // 
            // txtRawData
            // 
            this.txtRawData.BackColor = System.Drawing.SystemColors.Window;
            this.txtRawData.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtRawData.ForeColor = System.Drawing.Color.Black;
            this.txtRawData.Location = new System.Drawing.Point(1468, 400);
            this.txtRawData.Name = "txtRawData";
            this.txtRawData.ReadOnly = true;
            this.txtRawData.Size = new System.Drawing.Size(257, 40);
            this.txtRawData.TabIndex = 6;
            this.txtRawData.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(912, 403);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 33);
            this.label7.TabIndex = 16;
            this.label7.Text = "RAW DATA";
            // 
            // dtpLotNo
            // 
            this.dtpLotNo.CustomFormat = "dd/MM/yyyy";
            this.dtpLotNo.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpLotNo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotNo.Location = new System.Drawing.Point(477, 263);
            this.dtpLotNo.Name = "dtpLotNo";
            this.dtpLotNo.Size = new System.Drawing.Size(361, 40);
            this.dtpLotNo.TabIndex = 26;
            this.dtpLotNo.ValueChanged += new System.EventHandler(this.dtpLotNo_ValueChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.btnConfirm.ForeColor = System.Drawing.Color.Blue;
            this.btnConfirm.Location = new System.Drawing.Point(1468, 476);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(257, 93);
            this.btnConfirm.TabIndex = 27;
            this.btnConfirm.Text = "XÁC NHẬN\r\nCONFIRM";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(23, 334);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(397, 33);
            this.label9.TabIndex = 16;
            this.label9.Text = "EXPIRED DATE/ NGÀY HẾT HẠN";
            // 
            // dtpExpiredDate
            // 
            this.dtpExpiredDate.CustomFormat = "dd/MM/yyyy";
            this.dtpExpiredDate.Enabled = false;
            this.dtpExpiredDate.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpExpiredDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpiredDate.Location = new System.Drawing.Point(477, 328);
            this.dtpExpiredDate.Name = "dtpExpiredDate";
            this.dtpExpiredDate.Size = new System.Drawing.Size(361, 40);
            this.dtpExpiredDate.TabIndex = 26;
            this.dtpExpiredDate.ValueChanged += new System.EventHandler(this.dtpSupplyDate_ValueChanged);
            // 
            // ckInputManual
            // 
            this.ckInputManual.AutoSize = true;
            this.ckInputManual.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.ckInputManual.Location = new System.Drawing.Point(918, 476);
            this.ckInputManual.Name = "ckInputManual";
            this.ckInputManual.Size = new System.Drawing.Size(250, 37);
            this.ckInputManual.TabIndex = 32;
            this.ckInputManual.Text = "INPUT MANUALLY";
            this.ckInputManual.UseVisualStyleBackColor = true;
            this.ckInputManual.CheckedChanged += new System.EventHandler(this.ckInputManual_CheckedChanged);
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.lbError.ForeColor = System.Drawing.Color.Red;
            this.lbError.Location = new System.Drawing.Point(23, 476);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(39, 33);
            this.lbError.TabIndex = 35;
            this.lbError.Text = "...";
            // 
            // lbFormName
            // 
            this.lbFormName.AutoSize = true;
            this.lbFormName.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFormName.ForeColor = System.Drawing.Color.Maroon;
            this.lbFormName.Location = new System.Drawing.Point(464, 23);
            this.lbFormName.Name = "lbFormName";
            this.lbFormName.Size = new System.Drawing.Size(801, 77);
            this.lbFormName.TabIndex = 37;
            this.lbFormName.Text = "TRẢ CAO SU TỪ SẢN XUẤT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(912, 273);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(451, 33);
            this.label1.TabIndex = 39;
            this.label1.Text = "STD PACKING/ KHỐI LƯỢNG BAO BÌ";
            // 
            // txtPalletWeight
            // 
            this.txtPalletWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtPalletWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtPalletWeight.ForeColor = System.Drawing.Color.Black;
            this.txtPalletWeight.Location = new System.Drawing.Point(1468, 270);
            this.txtPalletWeight.Name = "txtPalletWeight";
            this.txtPalletWeight.Size = new System.Drawing.Size(257, 40);
            this.txtPalletWeight.TabIndex = 38;
            this.txtPalletWeight.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(912, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(504, 33);
            this.label2.TabIndex = 41;
            this.label2.Text = "NET WEIGHT/ KHỐI LƯỢNG CAO SU(KG)";
            // 
            // txtNetWeight
            // 
            this.txtNetWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtNetWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtNetWeight.ForeColor = System.Drawing.Color.Magenta;
            this.txtNetWeight.Location = new System.Drawing.Point(1468, 205);
            this.txtNetWeight.Name = "txtNetWeight";
            this.txtNetWeight.ReadOnly = true;
            this.txtNetWeight.Size = new System.Drawing.Size(257, 40);
            this.txtNetWeight.TabIndex = 40;
            this.txtNetWeight.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(23, 401);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(305, 33);
            this.label6.TabIndex = 16;
            this.label6.Text = "PIC/ NGƯỜI THỰC HIỆN";
            // 
            // txtPIC
            // 
            this.txtPIC.BackColor = System.Drawing.SystemColors.Window;
            this.txtPIC.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtPIC.ForeColor = System.Drawing.Color.Black;
            this.txtPIC.Location = new System.Drawing.Point(477, 396);
            this.txtPIC.Name = "txtPIC";
            this.txtPIC.Size = new System.Drawing.Size(361, 40);
            this.txtPIC.TabIndex = 5;
            this.txtPIC.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // txtRubberName
            // 
            this.txtRubberName.BackColor = System.Drawing.SystemColors.Window;
            this.txtRubberName.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtRubberName.ForeColor = System.Drawing.Color.Black;
            this.txtRubberName.Location = new System.Drawing.Point(477, 201);
            this.txtRubberName.Name = "txtRubberName";
            this.txtRubberName.ReadOnly = true;
            this.txtRubberName.Size = new System.Drawing.Size(361, 40);
            this.txtRubberName.TabIndex = 5;
            this.txtRubberName.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.SystemColors.Window;
            this.txtBarcode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtBarcode.ForeColor = System.Drawing.Color.Black;
            this.txtBarcode.Location = new System.Drawing.Point(1468, 140);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(257, 40);
            this.txtBarcode.TabIndex = 6;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(912, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(315, 33);
            this.label8.TabIndex = 16;
            this.label8.Text = "INPUT DATA/ NHẬP LIỆU";
            // 
            // txtWHRRCode
            // 
            this.txtWHRRCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtWHRRCode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtWHRRCode.ForeColor = System.Drawing.Color.Black;
            this.txtWHRRCode.Location = new System.Drawing.Point(477, 140);
            this.txtWHRRCode.Name = "txtWHRRCode";
            this.txtWHRRCode.ReadOnly = true;
            this.txtWHRRCode.Size = new System.Drawing.Size(361, 40);
            this.txtWHRRCode.TabIndex = 5;
            this.txtWHRRCode.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(23, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(332, 33);
            this.label10.TabIndex = 16;
            this.label10.Text = "MÃ SỐ PALLET/ PALLET ID";
            // 
            // frmWHRubberReturnFromPD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1831, 641);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNetWeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPalletWeight);
            this.Controls.Add(this.lbFormName);
            this.Controls.Add(this.lbError);
            this.Controls.Add(this.ckInputManual);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dtpExpiredDate);
            this.Controls.Add(this.dtpLotNo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.txtRawData);
            this.Controls.Add(this.txtWHRRCode);
            this.Controls.Add(this.txtRubberName);
            this.Controls.Add(this.txtPIC);
            this.Controls.Add(this.txtWeight);
            this.Name = "frmWHRubberReturnFromPD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IN TEM CAO SU (NGOÀI)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWHMaterialIssueToPDDetail_FormClosing);
            this.Load += new System.EventHandler(this.frmWHCCManagement_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRawData;
        private System.Windows.Forms.DateTimePicker dtpLotNo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.DateTimePicker dtpExpiredDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox ckInputManual;
        private System.Windows.Forms.Label lbError;
        private System.Windows.Forms.Label lbFormName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPalletWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNetWeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPIC;
        private System.Windows.Forms.TextBox txtRubberName;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWHRRCode;
        private System.Windows.Forms.Label label10;
    }
}