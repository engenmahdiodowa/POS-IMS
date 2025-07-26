namespace IMS_POS
{
    partial class IMS_SaleForm
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Trebuchet MS", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(20, 20); // Adjust location as needed
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(150, 30);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, ...";

            // 
            // IMS_SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450); // Adjust as needed
            this.Controls.Add(this.lblWelcome);
            this.Name = "IMS_SaleForm";
            this.Text = "Sales Dashboard";
            this.Load += new System.EventHandler(this.IMS_SaleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            this.pnltop = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtSearchOutSale = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSearchProduct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvOutSales = new System.Windows.Forms.DataGridView();
            this.dgvProductFetch = new System.Windows.Forms.DataGridView();
            this.btnMarkPaid = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnDeleteOutSale = new System.Windows.Forms.Button();
            this.btnUpdProduct = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRecievables = new System.Windows.Forms.Button();
            this.btnBalances = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnProPmt = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnAddCustomer = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.btnPurchase = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.pnltop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductFetch)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnltop
            // 
            this.pnltop.Controls.Add(this.pictureBox2);
            this.pnltop.Controls.Add(this.pictureBox1);
            this.pnltop.Controls.Add(this.label1);
            this.pnltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnltop.Location = new System.Drawing.Point(0, 0);
            this.pnltop.Name = "pnltop";
            this.pnltop.Size = new System.Drawing.Size(978, 42);
            this.pnltop.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::IMS_POS.Properties.Resources.maximize_window_20px;
            this.pictureBox2.Location = new System.Drawing.Point(908, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(26, 29);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::IMS_POS.Properties.Resources.close_window_20px;
            this.pictureBox1.Location = new System.Drawing.Point(940, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 29);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(357, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome Sales Session";
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Controls.Add(this.txtSearchOutSale);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.txtSearchProduct);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.dgvOutSales);
            this.pnlMain.Controls.Add(this.dgvProductFetch);
            this.pnlMain.Controls.Add(this.btnMarkPaid);
            this.pnlMain.Controls.Add(this.btnDeleteProduct);
            this.pnlMain.Controls.Add(this.btnDeleteOutSale);
            this.pnlMain.Controls.Add(this.btnUpdProduct);
            this.pnlMain.Location = new System.Drawing.Point(187, 42);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(791, 586);
            this.pnlMain.TabIndex = 0;
            // 
            // txtSearchOutSale
            // 
            this.txtSearchOutSale.Location = new System.Drawing.Point(188, 539);
            this.txtSearchOutSale.Name = "txtSearchOutSale";
            this.txtSearchOutSale.Size = new System.Drawing.Size(210, 28);
            this.txtSearchOutSale.TabIndex = 2;
            this.txtSearchOutSale.TextChanged += new System.EventHandler(this.txtSearchOutSale_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 542);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Search Sales:";
            // 
            // txtSearchProduct
            // 
            this.txtSearchProduct.Location = new System.Drawing.Point(188, 242);
            this.txtSearchProduct.Name = "txtSearchProduct";
            this.txtSearchProduct.Size = new System.Drawing.Size(210, 28);
            this.txtSearchProduct.TabIndex = 2;
            this.txtSearchProduct.TextChanged += new System.EventHandler(this.txtSearchProduct_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Search Product:";
            // 
            // dgvOutSales
            // 
            this.dgvOutSales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOutSales.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvOutSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutSales.Location = new System.Drawing.Point(117, 288);
            this.dgvOutSales.Name = "dgvOutSales";
            this.dgvOutSales.RowHeadersWidth = 62;
            this.dgvOutSales.RowTemplate.Height = 28;
            this.dgvOutSales.Size = new System.Drawing.Size(654, 236);
            this.dgvOutSales.TabIndex = 0;
            // 
            // dgvProductFetch
            // 
            this.dgvProductFetch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductFetch.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvProductFetch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductFetch.Location = new System.Drawing.Point(117, 14);
            this.dgvProductFetch.Name = "dgvProductFetch";
            this.dgvProductFetch.RowHeadersWidth = 62;
            this.dgvProductFetch.RowTemplate.Height = 28;
            this.dgvProductFetch.Size = new System.Drawing.Size(654, 218);
            this.dgvProductFetch.TabIndex = 0;
            // 
            // btnMarkPaid
            // 
            this.btnMarkPaid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.btnMarkPaid.FlatAppearance.BorderSize = 0;
            this.btnMarkPaid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkPaid.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMarkPaid.ForeColor = System.Drawing.Color.White;
            this.btnMarkPaid.Image = global::IMS_POS.Properties.Resources.paycheque_10px;
            this.btnMarkPaid.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMarkPaid.Location = new System.Drawing.Point(9, 288);
            this.btnMarkPaid.Name = "btnMarkPaid";
            this.btnMarkPaid.Size = new System.Drawing.Size(83, 41);
            this.btnMarkPaid.TabIndex = 0;
            this.btnMarkPaid.Text = "Pay";
            this.btnMarkPaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMarkPaid.UseVisualStyleBackColor = false;
            this.btnMarkPaid.Click += new System.EventHandler(this.btnMarkPaid_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.btnDeleteProduct.FlatAppearance.BorderSize = 0;
            this.btnDeleteProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProduct.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteProduct.ForeColor = System.Drawing.Color.White;
            this.btnDeleteProduct.Image = global::IMS_POS.Properties.Resources.delete_document_10px;
            this.btnDeleteProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteProduct.Location = new System.Drawing.Point(9, 61);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(83, 41);
            this.btnDeleteProduct.TabIndex = 0;
            this.btnDeleteProduct.Text = "Delete";
            this.btnDeleteProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteProduct.UseVisualStyleBackColor = false;
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            // 
            // btnDeleteOutSale
            // 
            this.btnDeleteOutSale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.btnDeleteOutSale.FlatAppearance.BorderSize = 0;
            this.btnDeleteOutSale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteOutSale.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteOutSale.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOutSale.Image = global::IMS_POS.Properties.Resources.update_10px1;
            this.btnDeleteOutSale.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteOutSale.Location = new System.Drawing.Point(9, 335);
            this.btnDeleteOutSale.Name = "btnDeleteOutSale";
            this.btnDeleteOutSale.Size = new System.Drawing.Size(83, 41);
            this.btnDeleteOutSale.TabIndex = 0;
            this.btnDeleteOutSale.Text = "Delete";
            this.btnDeleteOutSale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteOutSale.UseVisualStyleBackColor = false;
            this.btnDeleteOutSale.Click += new System.EventHandler(this.btnDeleteOutSale_Click);
            // 
            // btnUpdProduct
            // 
            this.btnUpdProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.btnUpdProduct.FlatAppearance.BorderSize = 0;
            this.btnUpdProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdProduct.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdProduct.ForeColor = System.Drawing.Color.White;
            this.btnUpdProduct.Image = global::IMS_POS.Properties.Resources.update_10px;
            this.btnUpdProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdProduct.Location = new System.Drawing.Point(9, 14);
            this.btnUpdProduct.Name = "btnUpdProduct";
            this.btnUpdProduct.Size = new System.Drawing.Size(83, 41);
            this.btnUpdProduct.TabIndex = 0;
            this.btnUpdProduct.Text = "Update";
            this.btnUpdProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdProduct.UseVisualStyleBackColor = false;
            this.btnUpdProduct.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlLeft.Controls.Add(this.btnRefresh);
            this.pnlLeft.Controls.Add(this.btnLogout);
            this.pnlLeft.Controls.Add(this.btnRecievables);
            this.pnlLeft.Controls.Add(this.btnBalances);
            this.pnlLeft.Controls.Add(this.btnProducts);
            this.pnlLeft.Controls.Add(this.btnProPmt);
            this.pnlLeft.Controls.Add(this.btnPrint);
            this.pnlLeft.Controls.Add(this.btnAddCustomer);
            this.pnlLeft.Controls.Add(this.btnRefund);
            this.pnlLeft.Controls.Add(this.btnPurchase);
            this.pnlLeft.Controls.Add(this.btnSales);
            this.pnlLeft.Location = new System.Drawing.Point(0, 42);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(187, 586);
            this.pnlLeft.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Image = global::IMS_POS.Properties.Resources.refresh_50px;
            this.btnRefresh.Location = new System.Drawing.Point(124, 533);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(57, 41);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Image = global::IMS_POS.Properties.Resources.logout_rounded_left_20px;
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogout.Location = new System.Drawing.Point(12, 533);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(97, 41);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnRecievables
            // 
            this.btnRecievables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecievables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnRecievables.FlatAppearance.BorderSize = 0;
            this.btnRecievables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecievables.ForeColor = System.Drawing.Color.White;
            this.btnRecievables.Image = global::IMS_POS.Properties.Resources.general_ledger_40px;
            this.btnRecievables.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRecievables.Location = new System.Drawing.Point(23, 405);
            this.btnRecievables.Name = "btnRecievables";
            this.btnRecievables.Size = new System.Drawing.Size(158, 41);
            this.btnRecievables.TabIndex = 0;
            this.btnRecievables.Text = "A/Recievable";
            this.btnRecievables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecievables.UseVisualStyleBackColor = false;
            this.btnRecievables.Click += new System.EventHandler(this.btnRecievables_Click_1);
            // 
            // btnBalances
            // 
            this.btnBalances.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBalances.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnBalances.FlatAppearance.BorderSize = 0;
            this.btnBalances.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBalances.ForeColor = System.Drawing.Color.White;
            this.btnBalances.Image = global::IMS_POS.Properties.Resources.transaction_20px;
            this.btnBalances.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBalances.Location = new System.Drawing.Point(23, 358);
            this.btnBalances.Name = "btnBalances";
            this.btnBalances.Size = new System.Drawing.Size(158, 41);
            this.btnBalances.TabIndex = 0;
            this.btnBalances.Text = "Acc. Balances";
            this.btnBalances.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBalances.UseVisualStyleBackColor = false;
            this.btnBalances.Click += new System.EventHandler(this.btnBalances_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnProducts.FlatAppearance.BorderSize = 0;
            this.btnProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProducts.ForeColor = System.Drawing.Color.White;
            this.btnProducts.Image = global::IMS_POS.Properties.Resources.trolley_20px;
            this.btnProducts.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProducts.Location = new System.Drawing.Point(23, 311);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(158, 41);
            this.btnProducts.TabIndex = 0;
            this.btnProducts.Text = "Products";
            this.btnProducts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProducts.UseVisualStyleBackColor = false;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // btnProPmt
            // 
            this.btnProPmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProPmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnProPmt.FlatAppearance.BorderSize = 0;
            this.btnProPmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProPmt.ForeColor = System.Drawing.Color.White;
            this.btnProPmt.Image = global::IMS_POS.Properties.Resources.payment_history_20px1;
            this.btnProPmt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProPmt.Location = new System.Drawing.Point(23, 217);
            this.btnProPmt.Name = "btnProPmt";
            this.btnProPmt.Size = new System.Drawing.Size(158, 41);
            this.btnProPmt.TabIndex = 0;
            this.btnProPmt.Text = "Payment";
            this.btnProPmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProPmt.UseVisualStyleBackColor = false;
            this.btnProPmt.Click += new System.EventHandler(this.btnProPmt_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::IMS_POS.Properties.Resources.print_20px;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.Location = new System.Drawing.Point(23, 264);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(158, 41);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnAddCustomer
            // 
            this.btnAddCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnAddCustomer.FlatAppearance.BorderSize = 0;
            this.btnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCustomer.ForeColor = System.Drawing.Color.White;
            this.btnAddCustomer.Image = global::IMS_POS.Properties.Resources.customer_20px;
            this.btnAddCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddCustomer.Location = new System.Drawing.Point(23, 170);
            this.btnAddCustomer.Name = "btnAddCustomer";
            this.btnAddCustomer.Size = new System.Drawing.Size(158, 41);
            this.btnAddCustomer.TabIndex = 0;
            this.btnAddCustomer.Text = "Customer";
            this.btnAddCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCustomer.UseVisualStyleBackColor = false;
            this.btnAddCustomer.Click += new System.EventHandler(this.btnAddCustomer_Click);
            // 
            // btnRefund
            // 
            this.btnRefund.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefund.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnRefund.FlatAppearance.BorderSize = 0;
            this.btnRefund.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefund.ForeColor = System.Drawing.Color.White;
            this.btnRefund.Image = global::IMS_POS.Properties.Resources.refund_20px;
            this.btnRefund.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefund.Location = new System.Drawing.Point(23, 123);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(158, 41);
            this.btnRefund.TabIndex = 0;
            this.btnRefund.Text = "Refund";
            this.btnRefund.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefund.UseVisualStyleBackColor = false;
            this.btnRefund.Click += new System.EventHandler(this.btnRefund_Click);
            // 
            // btnPurchase
            // 
            this.btnPurchase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPurchase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnPurchase.FlatAppearance.BorderSize = 0;
            this.btnPurchase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchase.ForeColor = System.Drawing.Color.White;
            this.btnPurchase.Image = global::IMS_POS.Properties.Resources.add_shopping_cart_20px1;
            this.btnPurchase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPurchase.Location = new System.Drawing.Point(23, 76);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(158, 41);
            this.btnPurchase.TabIndex = 0;
            this.btnPurchase.Text = "Purchase";
            this.btnPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPurchase.UseVisualStyleBackColor = false;
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // btnSales
            // 
            this.btnSales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnSales.FlatAppearance.BorderSize = 0;
            this.btnSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSales.ForeColor = System.Drawing.Color.White;
            this.btnSales.Image = global::IMS_POS.Properties.Resources.total_sales_20px;
            this.btnSales.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSales.Location = new System.Drawing.Point(23, 29);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(158, 41);
            this.btnSales.TabIndex = 0;
            this.btnSales.Text = "Sales";
            this.btnSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSales.UseVisualStyleBackColor = false;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            // 
            // IMS_SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 628);
            this.ControlBox = false;
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnltop);
            this.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IMS_SaleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnltop.ResumeLayout(false);
            this.pnltop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductFetch)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnltop;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnAddCustomer;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.Button btnPurchase;
        private System.Windows.Forms.Button btnProPmt;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridView dgvProductFetch;
        private System.Windows.Forms.DataGridView dgvOutSales;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Button btnUpdProduct;
        private System.Windows.Forms.Button btnMarkPaid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearchProduct;
        private System.Windows.Forms.TextBox txtSearchOutSale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDeleteOutSale;
        private System.Windows.Forms.Button btnBalances;
        private System.Windows.Forms.Button btnRecievables;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblWelcome;

    }
}