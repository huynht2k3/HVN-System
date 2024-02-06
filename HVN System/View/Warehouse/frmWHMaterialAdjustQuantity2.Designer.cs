namespace HVN_System.View.Warehouse
{
    partial class frmWHMaterialAdjustQuantity2
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
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtRawData = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpLotNo = new System.Windows.Forms.DateTimePicker();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtMName = new System.Windows.Forms.TextBox();
            this.txtReceiveLabelCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtQtyInBox = new System.Windows.Forms.TextBox();
            this.ckInputManual = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbNote = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtPIC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboScaleType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtWeight.ForeColor = System.Drawing.Color.Magenta;
            this.txtWeight.Location = new System.Drawing.Point(1359, 248);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(361, 40);
            this.txtWeight.TabIndex = 5;
            this.txtWeight.Text = "0";
            this.txtWeight.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // txtQuantity
            // 
            this.txtQuantity.BackColor = System.Drawing.SystemColors.Window;
            this.txtQuantity.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtQuantity.ForeColor = System.Drawing.Color.Magenta;
            this.txtQuantity.Location = new System.Drawing.Point(1359, 188);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.ReadOnly = true;
            this.txtQuantity.Size = new System.Drawing.Size(361, 40);
            this.txtQuantity.TabIndex = 6;
            this.txtQuantity.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(31, 195);
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
            this.label4.Location = new System.Drawing.Point(31, 258);
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
            this.label5.Location = new System.Drawing.Point(905, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(356, 33);
            this.label5.TabIndex = 16;
            this.label5.Text = "WEIGHT(G)/ TRỌNG LƯỢNG";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(905, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(369, 33);
            this.label6.TabIndex = 16;
            this.label6.Text = "QUANTITY/ SỐ LƯỢNG THỰC";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 665);
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
            this.txtRawData.ForeColor = System.Drawing.Color.Magenta;
            this.txtRawData.Location = new System.Drawing.Point(1359, 308);
            this.txtRawData.Name = "txtRawData";
            this.txtRawData.ReadOnly = true;
            this.txtRawData.Size = new System.Drawing.Size(361, 40);
            this.txtRawData.TabIndex = 6;
            this.txtRawData.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(905, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 33);
            this.label7.TabIndex = 16;
            this.label7.Text = "RAW DATA";
            // 
            // dtpLotNo
            // 
            this.dtpLotNo.CustomFormat = " ";
            this.dtpLotNo.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpLotNo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLotNo.Location = new System.Drawing.Point(485, 255);
            this.dtpLotNo.Name = "dtpLotNo";
            this.dtpLotNo.Size = new System.Drawing.Size(361, 40);
            this.dtpLotNo.TabIndex = 26;
            this.dtpLotNo.ValueChanged += new System.EventHandler(this.dtpLotNo_ValueChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.btnConfirm.ForeColor = System.Drawing.Color.Blue;
            this.btnConfirm.Location = new System.Drawing.Point(1561, 445);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(159, 93);
            this.btnConfirm.TabIndex = 27;
            this.btnConfirm.Text = "CONFIRM\r\nXÁC NHẬN";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Visible = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtMName
            // 
            this.txtMName.BackColor = System.Drawing.SystemColors.Window;
            this.txtMName.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtMName.ForeColor = System.Drawing.Color.Black;
            this.txtMName.Location = new System.Drawing.Point(485, 192);
            this.txtMName.Name = "txtMName";
            this.txtMName.ReadOnly = true;
            this.txtMName.Size = new System.Drawing.Size(361, 40);
            this.txtMName.TabIndex = 6;
            // 
            // txtReceiveLabelCode
            // 
            this.txtReceiveLabelCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtReceiveLabelCode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtReceiveLabelCode.ForeColor = System.Drawing.Color.Black;
            this.txtReceiveLabelCode.Location = new System.Drawing.Point(485, 133);
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
            this.label10.Location = new System.Drawing.Point(31, 136);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(449, 33);
            this.label10.TabIndex = 16;
            this.label10.Text = "RECEIVE LABEL/ MÃ TEM LINH KIỆN";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label13.Location = new System.Drawing.Point(905, 369);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(397, 33);
            this.label13.TabIndex = 31;
            this.label13.Text = "QTY IN BOX/ SỐ LƯƠNG Ở HỘP";
            // 
            // txtQtyInBox
            // 
            this.txtQtyInBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtQtyInBox.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtQtyInBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.txtQtyInBox.Location = new System.Drawing.Point(1359, 369);
            this.txtQtyInBox.Name = "txtQtyInBox";
            this.txtQtyInBox.ReadOnly = true;
            this.txtQtyInBox.Size = new System.Drawing.Size(361, 40);
            this.txtQtyInBox.TabIndex = 30;
            this.txtQtyInBox.Text = "0";
            this.txtQtyInBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ckInputManual
            // 
            this.ckInputManual.AutoSize = true;
            this.ckInputManual.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.ckInputManual.Location = new System.Drawing.Point(911, 445);
            this.ckInputManual.Name = "ckInputManual";
            this.ckInputManual.Size = new System.Drawing.Size(250, 37);
            this.ckInputManual.TabIndex = 32;
            this.ckInputManual.Text = "INPUT MANUALLY";
            this.ckInputManual.UseVisualStyleBackColor = true;
            this.ckInputManual.CheckedChanged += new System.EventHandler(this.ckInputManual_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label14.ForeColor = System.Drawing.Color.ForestGreen;
            this.label14.Location = new System.Drawing.Point(905, 136);
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
            this.txtBarcode.Location = new System.Drawing.Point(1359, 133);
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
            this.lbNote.Location = new System.Drawing.Point(31, 600);
            this.lbNote.Name = "lbNote";
            this.lbNote.Size = new System.Drawing.Size(122, 33);
            this.lbNote.TabIndex = 35;
            this.lbNote.Text = "*LƯU Ý: ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Maroon;
            this.label15.Location = new System.Drawing.Point(512, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(866, 77);
            this.label15.TabIndex = 37;
            this.label15.Text = "KIỂM TRA NGUYÊN VẬT LIỆU";
            // 
            // txtPIC
            // 
            this.txtPIC.BackColor = System.Drawing.SystemColors.Window;
            this.txtPIC.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtPIC.ForeColor = System.Drawing.Color.Black;
            this.txtPIC.Location = new System.Drawing.Point(485, 313);
            this.txtPIC.Name = "txtPIC";
            this.txtPIC.Size = new System.Drawing.Size(361, 40);
            this.txtPIC.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(31, 316);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 33);
            this.label1.TabIndex = 39;
            this.label1.Text = "PIC/ NGƯỜI THỰC HIỆN";
            // 
            // cboScaleType
            // 
            this.cboScaleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScaleType.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboScaleType.FormattingEnabled = true;
            this.cboScaleType.Items.AddRange(new object[] {
            "BIG SCALE/ CAN TO",
            "SMALL SCALE/ CAN NHO"});
            this.cboScaleType.Location = new System.Drawing.Point(485, 369);
            this.cboScaleType.Name = "cboScaleType";
            this.cboScaleType.Size = new System.Drawing.Size(361, 41);
            this.cboScaleType.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(31, 372);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 33);
            this.label2.TabIndex = 39;
            this.label2.Text = "CHỌN LOẠI CÂN";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.btnStart.ForeColor = System.Drawing.Color.Blue;
            this.btnStart.Location = new System.Drawing.Point(687, 445);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(159, 93);
            this.btnStart.TabIndex = 27;
            this.btnStart.Text = "START\r\nBẮT ĐẦU";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // frmWHMaterialAdjustQuantity2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1831, 687);
            this.Controls.Add(this.cboScaleType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPIC);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbNote);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.ckInputManual);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtQtyInBox);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dtpLotNo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRawData);
            this.Controls.Add(this.txtReceiveLabelCode);
            this.Controls.Add(this.txtMName);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtWeight);
            this.Name = "frmWHMaterialAdjustQuantity2";
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRawData;
        private System.Windows.Forms.DateTimePicker dtpLotNo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtReceiveLabelCode;
        private System.Windows.Forms.TextBox txtMName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtQtyInBox;
        private System.Windows.Forms.CheckBox ckInputManual;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbNote;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtPIC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboScaleType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
    }
}