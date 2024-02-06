namespace HVN_System.View.Warehouse
{
    partial class frmWHMaterialIssueToPD
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboScaleType = new System.Windows.Forms.ComboBox();
            this.txtPIC = new System.Windows.Forms.TextBox();
            this.dtpSupplyDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.dgvDetail = new DevExpress.XtraGrid.GridControl();
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lbError = new System.Windows.Forms.Label();
            this.dgvInfo = new DevExpress.XtraGrid.GridControl();
            this.gvInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnRefresh);
            this.layoutControl1.Controls.Add(this.cboScaleType);
            this.layoutControl1.Controls.Add(this.txtPIC);
            this.layoutControl1.Controls.Add(this.dtpSupplyDate);
            this.layoutControl1.Controls.Add(this.label3);
            this.layoutControl1.Controls.Add(this.btnStart);
            this.layoutControl1.Controls.Add(this.dgvDetail);
            this.layoutControl1.Controls.Add(this.lbError);
            this.layoutControl1.Controls.Add(this.dgvInfo);
            this.layoutControl1.Controls.Add(this.label2);
            this.layoutControl1.Controls.Add(this.label1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1430, 570);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(942, 57);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(201, 95);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "REFRESH\r\nCẬP NHẬT MỚI";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboScaleType
            // 
            this.cboScaleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScaleType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboScaleType.FormattingEnabled = true;
            this.cboScaleType.Items.AddRange(new object[] {
            "CAN TO",
            "CAN NHO",
            "KHONG DUNG CAN"});
            this.cboScaleType.Location = new System.Drawing.Point(185, 123);
            this.cboScaleType.Name = "cboScaleType";
            this.cboScaleType.Size = new System.Drawing.Size(318, 33);
            this.cboScaleType.TabIndex = 20;
            // 
            // txtPIC
            // 
            this.txtPIC.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPIC.Location = new System.Drawing.Point(185, 90);
            this.txtPIC.Name = "txtPIC";
            this.txtPIC.Size = new System.Drawing.Size(318, 29);
            this.txtPIC.TabIndex = 19;
            // 
            // dtpSupplyDate
            // 
            this.dtpSupplyDate.CustomFormat = "dd/MM/yyyy";
            this.dtpSupplyDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpSupplyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSupplyDate.Location = new System.Drawing.Point(185, 57);
            this.dtpSupplyDate.Name = "dtpSupplyDate";
            this.dtpSupplyDate.Size = new System.Drawing.Size(318, 31);
            this.dtpSupplyDate.TabIndex = 18;
            this.dtpSupplyDate.ValueChanged += new System.EventHandler(this.dtpSupplyDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 20.25F);
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1406, 41);
            this.label3.TabIndex = 17;
            this.label3.Text = "CẤP NGUYÊN VẬT LIỆU NGOÀI KẾ HOẠCH CHO SẢN XUẤT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(1147, 57);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(271, 95);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "START\r\nBẮT ĐẦU";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dgvDetail
            // 
            this.dgvDetail.Location = new System.Drawing.Point(951, 192);
            this.dgvDetail.MainView = this.gvDetail;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.Size = new System.Drawing.Size(467, 366);
            this.dgvDetail.TabIndex = 13;
            this.dgvDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetail});
            // 
            // gvDetail
            // 
            this.gvDetail.Appearance.FocusedRow.BackColor = System.Drawing.Color.Transparent;
            this.gvDetail.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetail.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.gvDetail.Appearance.Row.Options.UseFont = true;
            this.gvDetail.Appearance.SelectedRow.BackColor = System.Drawing.Color.Transparent;
            this.gvDetail.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvDetail.GridControl = this.dgvDetail;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "MÃ TEM";
            this.gridColumn2.FieldName = "whmi_code";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 199;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "SỐ LƯỢNG";
            this.gridColumn3.FieldName = "quantity";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 136;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "K LƯỢNG (G)";
            this.gridColumn4.FieldName = "weight";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 149;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "STATUS";
            this.gridColumn5.FieldName = "Status";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // lbError
            // 
            this.lbError.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lbError.ForeColor = System.Drawing.Color.Red;
            this.lbError.Location = new System.Drawing.Point(507, 57);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(431, 95);
            this.lbError.TabIndex = 12;
            // 
            // dgvInfo
            // 
            this.dgvInfo.Location = new System.Drawing.Point(12, 192);
            this.dgvInfo.MainView = this.gvInfo;
            this.dgvInfo.Name = "dgvInfo";
            this.dgvInfo.Size = new System.Drawing.Size(935, 366);
            this.dgvInfo.TabIndex = 11;
            this.dgvInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInfo});
            // 
            // gvInfo
            // 
            this.gvInfo.Appearance.FocusedRow.BackColor = System.Drawing.Color.Transparent;
            this.gvInfo.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvInfo.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvInfo.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvInfo.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.gvInfo.Appearance.Row.Options.UseFont = true;
            this.gvInfo.Appearance.SelectedRow.BackColor = System.Drawing.Color.Transparent;
            this.gvInfo.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn1,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn6});
            this.gvInfo.GridControl = this.dgvInfo;
            this.gvInfo.Name = "gvInfo";
            this.gvInfo.OptionsBehavior.Editable = false;
            this.gvInfo.OptionsSelection.MultiSelect = true;
            this.gvInfo.OptionsView.ShowGroupPanel = false;
            this.gvInfo.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvInfo_RowClick);
            this.gvInfo.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvInfo_RowCellStyle);
            this.gvInfo.DoubleClick += new System.EventHandler(this.gvInfo_DoubleClick);
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "MÃ LINH KIỆN";
            this.gridColumn9.FieldName = "M_name";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 3;
            this.gridColumn9.Width = 200;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "MÃ HÀNG SX";
            this.gridColumn10.FieldName = "Product_customer_code";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 171;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "CHUYỀN";
            this.gridColumn11.FieldName = "P_line";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 98;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "STATUS";
            this.gridColumn1.FieldName = "Status";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "SỐ YÊU CẦU";
            this.gridColumn7.FieldName = "M_demand";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 139;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "SỐ ĐÃ CẤP";
            this.gridColumn8.FieldName = "Actual_qty";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            this.gridColumn8.Width = 157;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "MÃ YÊU CẦU";
            this.gridColumn6.FieldName = "M_doc_id";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 145;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(951, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(467, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "THÔNG TIN CHI TIẾT";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(935, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "THÔNG TIN CẤP HÀNG";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem8,
            this.layoutControlItem6,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem9,
            this.layoutControlItem1,
            this.layoutControlItem7});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1430, 570);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.label1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(939, 36);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.label2;
            this.layoutControlItem4.Location = new System.Drawing.Point(939, 144);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(471, 36);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.dgvInfo;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 180);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(939, 370);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.dgvDetail;
            this.layoutControlItem6.Location = new System.Drawing.Point(939, 180);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(471, 370);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnStart;
            this.layoutControlItem2.Location = new System.Drawing.Point(1135, 45);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(275, 99);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lbError;
            this.layoutControlItem5.Location = new System.Drawing.Point(495, 45);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(24, 24);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(435, 99);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.label3;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(1410, 45);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem11.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem11.Control = this.dtpSupplyDate;
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 45);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(495, 33);
            this.layoutControlItem11.Text = "Ngày cấp hàng";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(170, 29);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 18F);
            this.layoutControlItem9.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem9.Control = this.txtPIC;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 78);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(495, 33);
            this.layoutControlItem9.Text = "Người thực hiện";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(170, 29);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 18F);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.cboScaleType;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 111);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(495, 33);
            this.layoutControlItem1.Text = "Chọn loại cân";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(170, 29);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnRefresh;
            this.layoutControlItem7.Location = new System.Drawing.Point(930, 45);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(205, 99);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // frmWHMaterialIssueToPD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 570);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmWHMaterialIssueToPD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supply to Production";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmWHMaterialIssueToPD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.GridControl dgvInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraGrid.GridControl dgvDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private System.Windows.Forms.Button btnStart;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Label lbError;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.TextBox txtPIC;
        private System.Windows.Forms.DateTimePicker dtpSupplyDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private System.Windows.Forms.ComboBox cboScaleType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.Button btnRefresh;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}