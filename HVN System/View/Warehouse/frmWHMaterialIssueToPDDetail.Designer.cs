namespace HVN_System.View.Warehouse
{
    partial class frmWHMaterialIssueToPDDetail
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
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.dtpLotNo = new System.Windows.Forms.DateTimePicker();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpSupplyDate = new System.Windows.Forms.DateTimePicker();
            this.txtMName = new System.Windows.Forms.TextBox();
            this.txtProdCustCode = new System.Windows.Forms.TextBox();
            this.txtPLine = new System.Windows.Forms.TextBox();
            this.txtPShift = new System.Windows.Forms.TextBox();
            this.txtReceiveLabelCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRemaining = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtQtyInBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbNote = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtWeight.ForeColor = System.Drawing.Color.Magenta;
            this.txtWeight.Location = new System.Drawing.Point(1378, 269);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(361, 40);
            this.txtWeight.TabIndex = 6;
            this.txtWeight.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(50, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 33);
            this.label1.TabIndex = 16;
            this.label1.Text = "PRODUCTION LINE/ CHUYỀN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(50, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(382, 33);
            this.label2.TabIndex = 16;
            this.label2.Text = "PART NUMBER/ MÃ SẢN PHẨM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(50, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(380, 33);
            this.label3.TabIndex = 16;
            this.label3.Text = "MATERIAL/ NGUYÊN VẬT LIỆU";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(50, 456);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 33);
            this.label4.TabIndex = 16;
            this.label4.Text = "LOT NO";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(924, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(310, 33);
            this.label6.TabIndex = 16;
            this.label6.Text = "WEIGHT/TRỌNG LƯỢNG";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 664);
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
            // dtpLotNo
            // 
            this.dtpLotNo.CustomFormat = " ";
            this.dtpLotNo.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpLotNo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotNo.Location = new System.Drawing.Point(504, 453);
            this.dtpLotNo.Name = "dtpLotNo";
            this.dtpLotNo.Size = new System.Drawing.Size(361, 40);
            this.dtpLotNo.TabIndex = 26;
            this.dtpLotNo.ValueChanged += new System.EventHandler(this.dtpLotNo_ValueChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.btnConfirm.ForeColor = System.Drawing.Color.Blue;
            this.btnConfirm.Location = new System.Drawing.Point(1580, 460);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(159, 93);
            this.btnConfirm.TabIndex = 27;
            this.btnConfirm.Text = "CONFIRM\r\nXÁC NHẬN";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(50, 396);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(269, 33);
            this.label8.TabIndex = 16;
            this.label8.Text = "SHIFT/ CA SẢN XUẤT";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(50, 516);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(397, 33);
            this.label9.TabIndex = 16;
            this.label9.Text = "SUPPLY DATE/ NGÀY SẢN XUẤT";
            // 
            // dtpSupplyDate
            // 
            this.dtpSupplyDate.CustomFormat = " dd/MM/yyyy";
            this.dtpSupplyDate.Enabled = false;
            this.dtpSupplyDate.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpSupplyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSupplyDate.Location = new System.Drawing.Point(504, 513);
            this.dtpSupplyDate.Name = "dtpSupplyDate";
            this.dtpSupplyDate.Size = new System.Drawing.Size(361, 40);
            this.dtpSupplyDate.TabIndex = 26;
            this.dtpSupplyDate.ValueChanged += new System.EventHandler(this.dtpSupplyDate_ValueChanged);
            // 
            // txtMName
            // 
            this.txtMName.BackColor = System.Drawing.SystemColors.Window;
            this.txtMName.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtMName.ForeColor = System.Drawing.Color.Black;
            this.txtMName.Location = new System.Drawing.Point(504, 213);
            this.txtMName.Name = "txtMName";
            this.txtMName.ReadOnly = true;
            this.txtMName.Size = new System.Drawing.Size(361, 40);
            this.txtMName.TabIndex = 6;
            // 
            // txtProdCustCode
            // 
            this.txtProdCustCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtProdCustCode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtProdCustCode.ForeColor = System.Drawing.Color.Black;
            this.txtProdCustCode.Location = new System.Drawing.Point(504, 273);
            this.txtProdCustCode.Name = "txtProdCustCode";
            this.txtProdCustCode.ReadOnly = true;
            this.txtProdCustCode.Size = new System.Drawing.Size(361, 40);
            this.txtProdCustCode.TabIndex = 6;
            // 
            // txtPLine
            // 
            this.txtPLine.BackColor = System.Drawing.SystemColors.Window;
            this.txtPLine.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtPLine.ForeColor = System.Drawing.Color.Black;
            this.txtPLine.Location = new System.Drawing.Point(504, 333);
            this.txtPLine.Name = "txtPLine";
            this.txtPLine.ReadOnly = true;
            this.txtPLine.Size = new System.Drawing.Size(361, 40);
            this.txtPLine.TabIndex = 6;
            // 
            // txtPShift
            // 
            this.txtPShift.BackColor = System.Drawing.SystemColors.Window;
            this.txtPShift.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtPShift.ForeColor = System.Drawing.Color.Black;
            this.txtPShift.Location = new System.Drawing.Point(504, 393);
            this.txtPShift.Name = "txtPShift";
            this.txtPShift.ReadOnly = true;
            this.txtPShift.Size = new System.Drawing.Size(361, 40);
            this.txtPShift.TabIndex = 6;
            // 
            // txtReceiveLabelCode
            // 
            this.txtReceiveLabelCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtReceiveLabelCode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtReceiveLabelCode.ForeColor = System.Drawing.Color.Black;
            this.txtReceiveLabelCode.Location = new System.Drawing.Point(504, 154);
            this.txtReceiveLabelCode.Name = "txtReceiveLabelCode";
            this.txtReceiveLabelCode.ReadOnly = true;
            this.txtReceiveLabelCode.Size = new System.Drawing.Size(361, 40);
            this.txtReceiveLabelCode.TabIndex = 6;
            this.txtReceiveLabelCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIssueLabelCode_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(50, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(449, 33);
            this.label10.TabIndex = 16;
            this.label10.Text = "RECEIVE LABEL/ MÃ TEM LINH KIỆN";
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.SystemColors.Window;
            this.txtResult.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtResult.ForeColor = System.Drawing.Color.Blue;
            this.txtResult.Location = new System.Drawing.Point(1378, 329);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(361, 40);
            this.txtResult.TabIndex = 6;
            this.txtResult.Text = "0/0";
            this.txtResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(924, 336);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(423, 33);
            this.label11.TabIndex = 16;
            this.label11.Text = "TOTAL ISSUED/ TỔNG SỐ ĐÃ CẤP";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label12.ForeColor = System.Drawing.Color.Blue;
            this.label12.Location = new System.Drawing.Point(924, 399);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(431, 33);
            this.label12.TabIndex = 29;
            this.label12.Text = "REMAINING/ SỐ CÒN LẠI CẦN CẤP";
            // 
            // txtRemaining
            // 
            this.txtRemaining.BackColor = System.Drawing.SystemColors.Window;
            this.txtRemaining.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtRemaining.ForeColor = System.Drawing.Color.Blue;
            this.txtRemaining.Location = new System.Drawing.Point(1378, 396);
            this.txtRemaining.Name = "txtRemaining";
            this.txtRemaining.ReadOnly = true;
            this.txtRemaining.Size = new System.Drawing.Size(361, 40);
            this.txtRemaining.TabIndex = 6;
            this.txtRemaining.Text = "0";
            this.txtRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(924, 213);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(409, 33);
            this.label13.TabIndex = 31;
            this.label13.Text = "QTY IN BOX/ SỐ CÒN LẠI Ở HỘP";
            // 
            // txtQtyInBox
            // 
            this.txtQtyInBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtQtyInBox.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtQtyInBox.ForeColor = System.Drawing.Color.Magenta;
            this.txtQtyInBox.Location = new System.Drawing.Point(1378, 213);
            this.txtQtyInBox.Name = "txtQtyInBox";
            this.txtQtyInBox.ReadOnly = true;
            this.txtQtyInBox.Size = new System.Drawing.Size(361, 40);
            this.txtQtyInBox.TabIndex = 30;
            this.txtQtyInBox.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label14.ForeColor = System.Drawing.Color.ForestGreen;
            this.label14.Location = new System.Drawing.Point(924, 157);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(339, 33);
            this.label14.TabIndex = 34;
            this.label14.Text = "Ô NHẬP LIỆU/ INPUT TEXT";
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.SystemColors.Window;
            this.txtBarcode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtBarcode.ForeColor = System.Drawing.Color.Black;
            this.txtBarcode.Location = new System.Drawing.Point(1378, 154);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(361, 40);
            this.txtBarcode.TabIndex = 33;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbNote
            // 
            this.lbNote.AutoSize = true;
            this.lbNote.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.lbNote.ForeColor = System.Drawing.Color.Red;
            this.lbNote.Location = new System.Drawing.Point(50, 580);
            this.lbNote.Name = "lbNote";
            this.lbNote.Size = new System.Drawing.Size(512, 33);
            this.lbNote.TabIndex = 35;
            this.lbNote.Text = "*LƯU Ý:  KHÔNG CẦN DÙNG CÂN ĐỂ CẤP";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Maroon;
            this.label15.Location = new System.Drawing.Point(459, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(924, 77);
            this.label15.TabIndex = 37;
            this.label15.Text = "CẤP LINH KIỆN CHO SẢN XUẤT";
            // 
            // frmWHMaterialIssueToPDDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1831, 686);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbNote);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtQtyInBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dtpSupplyDate);
            this.Controls.Add(this.dtpLotNo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPShift);
            this.Controls.Add(this.txtPLine);
            this.Controls.Add(this.txtProdCustCode);
            this.Controls.Add(this.txtReceiveLabelCode);
            this.Controls.Add(this.txtRemaining);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtMName);
            this.Controls.Add(this.txtWeight);
            this.Name = "frmWHMaterialIssueToPDDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CẤP LINH KIỆN CHO SẢN XUẤT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWHMaterialIssueToPDDetail_FormClosing);
            this.Load += new System.EventHandler(this.frmWHCCManagement_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.DateTimePicker dtpLotNo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpSupplyDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPShift;
        private System.Windows.Forms.TextBox txtPLine;
        private System.Windows.Forms.TextBox txtProdCustCode;
        private System.Windows.Forms.TextBox txtReceiveLabelCode;
        private System.Windows.Forms.TextBox txtMName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRemaining;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtQtyInBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbNote;
        private System.Windows.Forms.Label label15;
    }
}