namespace HVN_System.View.Warehouse
{
    partial class frmWHRubberPrintStockLabel
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
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPalletWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNetWeight = new System.Windows.Forms.TextBox();
            this.cboRubberName = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPIC = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboRubberName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtWeight.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtWeight.ForeColor = System.Drawing.Color.Black;
            this.txtWeight.Location = new System.Drawing.Point(1465, 277);
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
            this.label3.Location = new System.Drawing.Point(23, 143);
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
            this.label4.Location = new System.Drawing.Point(23, 217);
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
            this.label5.Location = new System.Drawing.Point(909, 277);
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
            this.txtRawData.Location = new System.Drawing.Point(1465, 346);
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
            this.label7.Location = new System.Drawing.Point(909, 349);
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
            this.dtpLotNo.Location = new System.Drawing.Point(477, 207);
            this.dtpLotNo.Name = "dtpLotNo";
            this.dtpLotNo.Size = new System.Drawing.Size(361, 40);
            this.dtpLotNo.TabIndex = 26;
            this.dtpLotNo.ValueChanged += new System.EventHandler(this.dtpLotNo_ValueChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.btnConfirm.ForeColor = System.Drawing.Color.Blue;
            this.btnConfirm.Location = new System.Drawing.Point(1563, 426);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(159, 93);
            this.btnConfirm.TabIndex = 27;
            this.btnConfirm.Text = "IN TEM\r\nPRINT";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(23, 277);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(389, 33);
            this.label9.TabIndex = 16;
            this.label9.Text = "EXPIRED DATE/NGÀY HẾT HẠN";
            // 
            // dtpExpiredDate
            // 
            this.dtpExpiredDate.CustomFormat = "dd/MM/yyyy";
            this.dtpExpiredDate.Enabled = false;
            this.dtpExpiredDate.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.dtpExpiredDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpiredDate.Location = new System.Drawing.Point(477, 274);
            this.dtpExpiredDate.Name = "dtpExpiredDate";
            this.dtpExpiredDate.Size = new System.Drawing.Size(361, 40);
            this.dtpExpiredDate.TabIndex = 26;
            this.dtpExpiredDate.ValueChanged += new System.EventHandler(this.dtpSupplyDate_ValueChanged);
            // 
            // ckInputManual
            // 
            this.ckInputManual.AutoSize = true;
            this.ckInputManual.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.ckInputManual.Location = new System.Drawing.Point(915, 426);
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
            this.lbError.Location = new System.Drawing.Point(23, 426);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(39, 33);
            this.lbError.TabIndex = 35;
            this.lbError.Text = "...";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Maroon;
            this.label15.Location = new System.Drawing.Point(464, 23);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(855, 77);
            this.label15.TabIndex = 37;
            this.label15.Text = "IN TEM CAO SU TRONG KHO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(909, 210);
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
            this.txtPalletWeight.Location = new System.Drawing.Point(1465, 210);
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
            this.label2.Location = new System.Drawing.Point(909, 143);
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
            this.txtNetWeight.Location = new System.Drawing.Point(1465, 143);
            this.txtNetWeight.Name = "txtNetWeight";
            this.txtNetWeight.ReadOnly = true;
            this.txtNetWeight.Size = new System.Drawing.Size(257, 40);
            this.txtNetWeight.TabIndex = 40;
            this.txtNetWeight.Text = "0";
            // 
            // cboRubberName
            // 
            this.cboRubberName.Location = new System.Drawing.Point(477, 140);
            this.cboRubberName.Name = "cboRubberName";
            this.cboRubberName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.cboRubberName.Properties.Appearance.Options.UseFont = true;
            this.cboRubberName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboRubberName.Properties.NullText = "Select Rubber";
            this.cboRubberName.Properties.PopupView = this.searchLookUpEdit1View;
            this.cboRubberName.Size = new System.Drawing.Size(361, 40);
            this.cboRubberName.TabIndex = 42;
            this.cboRubberName.EditValueChanged += new System.EventHandler(this.cboRubberName_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(23, 346);
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
            this.txtPIC.Location = new System.Drawing.Point(477, 346);
            this.txtPIC.Name = "txtPIC";
            this.txtPIC.Size = new System.Drawing.Size(361, 40);
            this.txtPIC.TabIndex = 5;
            this.txtPIC.TextChanged += new System.EventHandler(this.txtWeight_TextChanged);
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.SystemColors.Window;
            this.txtBarcode.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.txtBarcode.ForeColor = System.Drawing.Color.Black;
            this.txtBarcode.Location = new System.Drawing.Point(477, 556);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(361, 40);
            this.txtBarcode.TabIndex = 5;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(23, 559);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(357, 33);
            this.label8.TabIndex = 16;
            this.label8.Text = "RE PRINT/ SCAN IN LẠI TEM";
            // 
            // frmWHRubberPrintStockLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1831, 641);
            this.Controls.Add(this.cboRubberName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNetWeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPalletWeight);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbError);
            this.Controls.Add(this.ckInputManual);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dtpExpiredDate);
            this.Controls.Add(this.dtpLotNo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRawData);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.txtPIC);
            this.Controls.Add(this.txtWeight);
            this.Name = "frmWHRubberPrintStockLabel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IN TEM CAO SU (NGOÀI)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWHMaterialIssueToPDDetail_FormClosing);
            this.Load += new System.EventHandler(this.frmWHCCManagement_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboRubberName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
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
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPalletWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNetWeight;
        private DevExpress.XtraEditors.SearchLookUpEdit cboRubberName;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPIC;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label8;
    }
}