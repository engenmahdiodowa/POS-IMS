using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using IMS_POS;
using IMS_POS.Models; // Assuming User class is in Models namespace
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IMS_POS.IMS_SaleForm;


namespace IMS_POS
{

    public partial class IMS_SaleForm : Form
    {
        private readonly DatabaseConnection dbConnection;
        private readonly User _loggedInUser;

        // Pagination state
        private const int pageSize = 10; // Rows per page

        public IMS_SaleForm(User user)
        {
            InitializeComponent();

            dbConnection = new DatabaseConnection(); // Initialize your DB connection


            user = _loggedInUser;


            this.Location = new Point(0, 0);
            this.Resize += IMS_SaleForm_Resized;
            this.LocationChanged += IMS_SaleForm_Resized; // reuse same handler

            LoadProductsToFetchGrid();
            LoadSalesToGrid();

            this.Text = "IMS_FormSales";
            ApplyRolePermissions();

        }
        private void IMS_SaleForm_Load(object sender, EventArgs e)
        {

            if (_loggedInUser != null)
            {
                lblWelcome.Text = $"Welcome, {_loggedInUser.FullName} ({_loggedInUser.Role})";
            }
            else
            {
                lblWelcome.Text = "Welcome, Guest";
                MessageBox.Show("User info not provided.");
            }
        }
        private void ApplyRolePermissions()
        {
            string user = Program.CurrentUser?.Role;

            if (user == "Admin")
            {
                btnSales.Enabled = true;
                btnPurchase.Enabled = true;
                btnRefund.Enabled = false;
                btnAddCustomer.Enabled = false;
                btnProPmt.Enabled = false;
                btnPrint.Enabled = true;
                btnProducts.Enabled = true;
                btnBalances.Enabled = true;
                btnRecievables.Enabled = true;
                btnLogout.Enabled = true;
                btnRefresh.Enabled = true;
                btnUpdProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
                btnMarkPaid.Enabled = true;
                btnDeleteOutSale.Enabled = true;
            }
            else if (user == "Accountant")
            {
                btnSales.Enabled = false;
                btnPurchase.Enabled = false;
                btnRefund.Enabled = true;
                btnAddCustomer.Enabled = false;
                btnProPmt.Enabled = true;
                btnPrint.Enabled = true;
                btnProducts.Enabled = true;
                btnBalances.Enabled = true;
                btnRecievables.Enabled = true;
                btnLogout.Enabled = true;
                btnRefresh.Enabled = true;
                btnUpdProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
                btnMarkPaid.Enabled = false;
                btnDeleteOutSale.Enabled = false;

            }
            else if (user == "Manager")
            {
                btnSales.Enabled = false;
                btnPurchase.Enabled = false;
                btnRefund.Enabled = true;
                btnAddCustomer.Enabled = true;
                btnProPmt.Enabled = true;
                btnPrint.Enabled = true;
                btnProducts.Enabled = true;
                btnBalances.Enabled = true;
                btnRecievables.Enabled = true;
                btnLogout.Enabled = true;
                btnRefresh.Enabled = true;
                btnUpdProduct.Enabled = true;
                btnDeleteProduct.Enabled = true;
                btnMarkPaid.Enabled = false;
                btnDeleteOutSale.Enabled = false;
            }
            else
            {
                MessageBox.Show("Invalid user role. The application will now exit.");
                Application.Exit();
            }
        }



        private void IMS_SaleForm_Resized(object sender, EventArgs e)
        {
            // Loop through all dynamic child forms in pnlMain
            foreach (var form in pnlMain.Controls.OfType<Form>())
            {
                form.Dock = DockStyle.Fill; // Automatically resizes to panel

                // Stretch internal panels and DataGridViews if any
                foreach (var panel in form.Controls.OfType<Panel>())
                {
                    panel.Dock = DockStyle.Fill;

                    foreach (var dgv in panel.Controls.OfType<DataGridView>())
                    {
                        dgv.Dock = DockStyle.Fill;
                    }
                }

                form.BringToFront();
            }
        }


        private async void LoadProductsToFetchGrid()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = "SELECT ProductID, ProductName, Cost, Quantity, VendorID, PurchaseDate FROM Products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvProductFetch.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
            dgvProductFetch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductFetch.RowHeadersVisible = false;
            dgvProductFetch.AllowUserToResizeRows = false;
            dgvProductFetch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductFetch.ReadOnly = true;
            dgvProductFetch.MultiSelect = false;
            dgvProductFetch.BackgroundColor = Color.White;

            // Optional: make rows auto-fit content height
            dgvProductFetch.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvProductFetch.RowTemplate.Height = 28;  // or whatever height looks best for your fontdgvProductFetch.ScrollBars = ScrollBars.Both;

            dgvProductFetch.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
        }

        private async void LoadSalesToGrid()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = @"
                SELECT 
                    SaleID,
                    ProductID,
                    CustomerID,
                    Quantity,
                    TotalPrice,
                    IsPaid,
                    PaymentMethod,
                    SaleDate
                FROM Sales";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvOutSales.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales: " + ex.Message);
            }
            dgvOutSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOutSales.RowHeadersVisible = false;
            dgvOutSales.AllowUserToResizeRows = false;
            dgvOutSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOutSales.ReadOnly = true;
            dgvOutSales.MultiSelect = false;
            dgvOutSales.BackgroundColor = Color.White;

            // Optional: make rows auto-fit content height
            dgvOutSales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvOutSales.RowTemplate.Height = 28;  // or whatever height looks best for your fontdgvProductFetch.ScrollBars = ScrollBars.Both;

            dgvOutSales.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var salesEntryForm = new IMS_POS.SalesEntryForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(salesEntryForm);
            salesEntryForm.Show();
            salesEntryForm.BringToFront();

        }

        private TextBox CreatePlaceholderTextBox(string placeholder, Point location)
        {
            var txt = new TextBox
            {
                Text = placeholder,
                Location = location,
                Width = 200,
                Font = new Font("Trebuchet MS", 10, FontStyle.Regular),
                ForeColor = Color.Gray
            };

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };

            return txt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var productsForm = new IMS_POS.ProductEditForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(productsForm);
            productsForm.Show();
            productsForm.BringToFront();
        }

        private async void btnPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                var purchaseForm = new IMS_POS.PurchaseInventoryForm();
                DynamicFormHelper.OpenInsidePanel(this, purchaseForm, pnlMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Purchase form:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                // Optional: Give pnlMain some internal padding so the child form sits inset
                pnlMain.Padding = new Padding(8);

                // Clear out any existing child controls
                pnlMain.Controls.Clear();

                // Instantiate the form
                var purchaseForm = new IMS_POS.PurchaseInventoryForm
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };

                // Optional: wait if you have asynchronous setup
                await Task.Run(() =>
                {
                    // Simulate any prep time if needed (not required unless constructor is heavy)
                });

                pnlMain.Controls.Add(purchaseForm);
                purchaseForm.Show();
                purchaseForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Purchase form:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var refundForm = new IMS_POS.DynamicRefundForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(refundForm);
            refundForm.Show();
            refundForm.BringToFront();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var customerForm = new IMS_POS.CustomerManagerForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(customerForm);
            customerForm.Show();
            customerForm.BringToFront();
        }

        private void btnProPmt_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var pmtForm = new IMS_POS.PaymentSyncForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(pmtForm);
            pmtForm.Show();
            pmtForm.BringToFront();
        }
        private DataTable GetReportData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1", typeof(string));
            dt.Columns.Add("Column2", typeof(int));

            // Example data
            dt.Rows.Add("Row1", 100);
            dt.Rows.Add("Row2", 200);

            return dt;
        }

        private void ShowPrintReportForm()
        {
            DataTable reportData = GetReportData(); // Get the data
            string reportHeader = "Sales Report";  // Set your header

            var repForm = new IMS_POS.PrintReportForm(reportData, reportHeader)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(repForm);
            repForm.Show();
            repForm.BringToFront();
        }

        private readonly DatabaseConnection db = new DatabaseConnection();

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            string query = "SELECT SaleID, ProductID, CustomerID, Quantity, TotalPrice, IsPaid, PaymentMethod, SaleDate FROM Sales WHERE IsPaid = 1";

            DataTable dtToPrint = new DataTable();

            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => adapter.Fill(dtToPrint)); // Fill is synchronous, so run in Task.Run
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load sales data: " + ex.Message);
                return;
            }

            if (dtToPrint.Rows.Count == 0)
            {
                MessageBox.Show("No paid sales data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string reportHeader = "Paid Sales Report\n" + DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");

            pnlMain.Padding = new Padding(8);
            pnlMain.Controls.Clear();

            var repForm = new IMS_POS.PrintReportForm(dtToPrint, reportHeader)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            pnlMain.Controls.Add(repForm);
            repForm.Show();
            repForm.BringToFront();
        }



        private void btnProducts_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var productsForm = new IMS_POS.ManageInventoryForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(productsForm);
            productsForm.Show();
            productsForm.BringToFront();
        }

        private void btnRefresh_Click(Form formToLoad)
        {

            var result = MessageBox.Show(
     "Do you want to reload the application and reset the session?",
     "Confirm Refresh",
     MessageBoxButtons.YesNo,          // Displays Yes and No buttons
     MessageBoxIcon.Question           // Displays a question mark icon
 );

            if (result == DialogResult.Yes)
            {
                // Logic for Yes
                Application.Restart();
            }
            else
            {
                // Logic for No (optional)
                MessageBox.Show("Refresh canceled.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var prodDeleForm = new IMS_POS.DynamicDeleteProductForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(prodDeleForm);
            prodDeleForm.Show();
            prodDeleForm.BringToFront();
        }

        private void btnMarkPaid_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var PaidSalForm = new IMS_POS.ReceivablesManagerForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(PaidSalForm);
            PaidSalForm.Show();
            PaidSalForm.BringToFront();

        }

        private void btnDeleteOutSale_Click(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var delSaleform = new IMS_POS.DeleteSalesForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(delSaleform);
            delSaleform.Show();
            delSaleform.BringToFront();
        }


        private void btnRecievables_Click_1(object sender, EventArgs e)
        {
            // Optional: Give pnlMain some internal padding so the child form sits inset
            pnlMain.Padding = new Padding(8);

            // Clear out any existing child controls
            pnlMain.Controls.Clear();

            // Instantiate and configure the SalesEntryForm to fill the panel
            var recievableForm = new IMS_POS.ReceivablesManagerForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill    // <-- fills pnlMain, respecting its Padding
            };

            // Add and show
            pnlMain.Controls.Add(recievableForm);
            recievableForm.Show();
            recievableForm.BringToFront();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
    "Do you want to reload the application and reset the session?",
    "Confirm Refresh",
    MessageBoxButtons.YesNo,          // Displays Yes and No buttons
    MessageBoxIcon.Question           // Displays a question mark icon
);

            if (result == DialogResult.Yes)
            {
                // Logic for Yes
                Application.Restart();
            }
            else
            {
                // Logic for No (optional)
                MessageBox.Show("Refresh canceled.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnBalances_Click(object sender, EventArgs e)
        {
            pnlMain.Padding = new Padding(8);
            pnlMain.Controls.Clear();

            var accBalances = new IMS_POS.AccountsBalanceForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            pnlMain.Controls.Add(accBalances);
            accBalances.Show();
            accBalances.BringToFront();
            // Disable the Mark as Paid button
        }

        private async void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchProduct.Text.Trim();


            if (string.IsNullOrEmpty(searchText))
            {
                dgvProductFetch.DataSource = null;
                return;
            }

            const string query = @"
        SELECT 
            ProductID, 
            ProductName, 
            Cost 
        FROM Products 
        WHERE ProductName LIKE @SearchText + '%'
        ORDER BY ProductName";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchText", searchText);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        dgvProductFetch.DataSource = dt;
                        if (dgvProductFetch.Columns.Contains("ProductID"))
                            dgvProductFetch.Columns["ProductID"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to search products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async void txtSearchOutSale_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchOutSale.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                dgvOutSales.DataSource = null; // Clear grid if search is empty
                return;
            }

            const string query = @"
        SELECT 
            s.SaleID, 
            p.ProductName, 
            c.CustomerName, 
            s.Quantity, 
            s.TotalPrice, 
            s.IsPaid, 
            pm.MethodName AS PaymentMethod, 
            s.SaleDate
        FROM Sales s
        INNER JOIN Products p ON s.ProductID = p.ProductID
        INNER JOIN Customers c ON s.CustomerID = c.CustomerID
        INNER JOIN PaymentMethods pm ON s.PaymentMethodID = pm.PaymentMethodID
        WHERE p.ProductName LIKE @SearchText + '%' OR c.CustomerName LIKE @SearchText + '%'
        ORDER BY s.SaleDate DESC";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchText", searchText);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        dgvOutSales.DataSource = dt;

                        if (dgvOutSales.Columns.Contains("SaleID"))
                            dgvOutSales.Columns["SaleID"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to search sales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Clear the current user session (assumed you have a global current user tracker)
                Program.CurrentUser = null;

                // Hide the current main form (this)
                this.Hide();

                // Show the login form modally
                using (IMS_LoginForm loginForm = new IMS_LoginForm())
                {
                    loginForm.ShowDialog();
                }

                // After login form closes, check if user logged in again
                if (Program.CurrentUser == null)
                {
                    // User did not log in again, exit app
                    Application.Exit();
                }
                else
                {
                    // User logged in again, show main form
                    this.Show();
                }
            }
        }


    }

    public class SalesEntryForm : Form
    {
        private ComboBox cmbProducts;
        private ComboBox cmbCustomers;
        private ComboBox cmbPaymentMethods;
        private TextBox txtQty;
        private TextBox txtPrice;
        private CheckBox chkIsPaid;
        private DateTimePicker dtpSaleDate;
        private Button btnSave;
        private Button btnCancel;
        private DataGridView dgvOutSales;

        private readonly DatabaseConnection db = new DatabaseConnection();

        public SalesEntryForm()
        {
            InitializeForm();
            InitializeControls();

            InitializeAsync(); // Start asynchronous loading
        }

        private async void InitializeAsync()
        {
            try
            {
                await LoadCustomersAsync();
                await LoadProductsAsync();
                await LoadPaymentMethodsAsync();
                await LoadSalesHistoryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeForm()
        {
            Text = "Sales Entry";
            BackColor = Color.White;
            Size = new Size(900, 580);
            this.Location = new Point(-10, 0);
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(8); // Form-level padding
        }

        private void InitializeControls()
        {
            // Top panel for DataGridView
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300,
                Padding = new Padding(4)
            };
            dgvOutSales = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            pnlTop.Controls.Add(dgvOutSales);

            // Bottom panel for inputs
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4)
            };

            int labelX = 10;
            int controlX = 120;
            int y = 10;
            int spacing = 30;

            // Customer
            var lblCustomer = new Label { Text = "Customer", Location = new Point(labelX, y), AutoSize = true };
            cmbCustomers = new ComboBox { Location = new Point(controlX, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            // Product
            var lblProduct = new Label { Text = "Product", Location = new Point(labelX, y), AutoSize = true };
            cmbProducts = new ComboBox { Location = new Point(controlX, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            // Quantity
            var lblQty = new Label { Text = "Quantity", Location = new Point(labelX, y), AutoSize = true };
            txtQty = new TextBox { Location = new Point(controlX, y), Width = 200 };
            y += spacing;

            // Price
            var lblPrice = new Label { Text = "Price", Location = new Point(labelX, y), AutoSize = true };
            txtPrice = new TextBox { Location = new Point(controlX, y), Width = 200 }; // Editable price
            y += spacing;

            // Payment Method
            var lblMethod = new Label { Text = "Payment Method", Location = new Point(labelX, y), AutoSize = true };
            cmbPaymentMethods = new ComboBox { Location = new Point(controlX, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            // Sale Date
            var lblDate = new Label { Text = "Sale Date", Location = new Point(labelX, y), AutoSize = true };
            dtpSaleDate = new DateTimePicker { Location = new Point(controlX, y), Width = 200 };
            y += spacing;

            // Is Paid Checkbox
            chkIsPaid = new CheckBox { Text = "Is Paid", Location = new Point(controlX, y), AutoSize = true };
            y += spacing;

            // Buttons
            btnSave = new Button { Text = "Save", Location = new Point(controlX, y), Width = 100 };
            btnSave.Click += async (s, e) => await SaveSaleAsync();
            btnCancel = new Button { Text = "Cancel", Location = new Point(controlX + 110, y), Width = 90 };
            btnCancel.Click += (s, e) =>
            {
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                    Dispose();
                }
            };

            // Add controls to bottom panel
            pnlBottom.Controls.AddRange(new Control[]
            {
            lblCustomer, cmbCustomers,
            lblProduct, cmbProducts,
            lblQty, txtQty,
            lblPrice, txtPrice,
            lblMethod, cmbPaymentMethods,
            lblDate, dtpSaleDate,
            chkIsPaid,
            btnSave, btnCancel
            });

            // Add panels to form
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private async Task LoadCustomersAsync()
        {
            const string query = "SELECT CustomerID, CustomerName FROM Customers";
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbCustomers.DataSource = dt;
                    cmbCustomers.DisplayMember = "CustomerName";
                    cmbCustomers.ValueMember = "CustomerID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductsAsync()
        {
            const string query = "SELECT ProductID, ProductName FROM Products";
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbProducts.DataSource = dt;
                    cmbProducts.DisplayMember = "ProductName";
                    cmbProducts.ValueMember = "ProductID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadPaymentMethodsAsync()
        {
            const string query = "SELECT PaymentMethodID, MethodName FROM PaymentMethods";
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbPaymentMethods.DataSource = dt;
                    cmbPaymentMethods.DisplayMember = "MethodName";
                    cmbPaymentMethods.ValueMember = "PaymentMethodID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load payment methods: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadSalesHistoryAsync()
        {
            const string query = @"
            SELECT 
                s.SaleID, 
                p.ProductName, 
                c.CustomerName, 
                s.Quantity, 
                s.TotalPrice, 
                s.IsPaid, 
                pm.MethodName AS PaymentMethod, 
                s.SaleDate
            FROM Sales s
            INNER JOIN Products p ON s.ProductID = p.ProductID
            INNER JOIN Customers c ON s.CustomerID = c.CustomerID
            INNER JOIN PaymentMethods pm ON s.PaymentMethodID = pm.PaymentMethodID
            ORDER BY s.SaleDate DESC";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvOutSales.DataSource = dt;
                    if (dgvOutSales.Columns.Contains("SaleID"))
                        dgvOutSales.Columns["SaleID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load sales history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveSaleAsync()
        {
            if (!int.TryParse(txtQty.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid positive quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid positive price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCustomers.SelectedValue == null || cmbProducts.SelectedValue == null || cmbPaymentMethods.SelectedValue == null)
            {
                MessageBox.Show("Please select customer, product, and payment method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = (int)cmbCustomers.SelectedValue;
            int productId = (int)cmbProducts.SelectedValue;
            int paymentMethodId = (int)cmbPaymentMethods.SelectedValue;
            bool isPaid = chkIsPaid.Checked;
            DateTime saleDate = dtpSaleDate.Value.Date;
            decimal totalPrice = quantity * price;

            const string insertQuery = @"
            INSERT INTO Sales 
            (ProductID, CustomerID, Quantity, TotalPrice, IsPaid, PaymentMethodID, SaleDate)
            VALUES 
            (@ProductID, @CustomerID, @Quantity, @TotalPrice, @IsPaid, @PaymentMethodID, @SaleDate)";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@IsPaid", isPaid);
                    cmd.Parameters.AddWithValue("@PaymentMethodID", paymentMethodId);
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show("Sale saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInputs();
                    await LoadSalesHistoryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save sale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtQty.Clear();
            txtPrice.Clear();
            chkIsPaid.Checked = false;
            dtpSaleDate.Value = DateTime.Today;

            if (cmbCustomers.Items.Count > 0) cmbCustomers.SelectedIndex = 0;
            if (cmbProducts.Items.Count > 0) cmbProducts.SelectedIndex = 0;
            if (cmbPaymentMethods.Items.Count > 0) cmbPaymentMethods.SelectedIndex = 0;
        }
    }
    public class DeleteSalesForm : Form
    {
        private DataGridView dgvOutStanSales;
        private Button btnDelete;
        private Button btnCancel;
        private int selectedSaleId = -1;
        private readonly DatabaseConnection db = new DatabaseConnection();
        public DeleteSalesForm()
        {
            this.Text = "Delete Sales";
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(180, 50);
            this.Size = new Size(700, 540);



            InitializeControls();
            _ = LoadSalesAsync();
        }

        private void InitializeControls()
        {
            dgvOutStanSales = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(660, 400),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvOutStanSales.CellClick += DgvOutStanSales_CellClick;
            this.Controls.Add(dgvOutStanSales);

            btnDelete = new Button
            {
                Text = "Delete Selected Sale",
                Location = new Point(20, 440),
                Size = new Size(200, 40)
            };
            btnDelete.Click += async (s, e) => await BtnDelete_ClickAsync();
            this.Controls.Add(btnDelete);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 440),
                Size = new Size(120, 40)
            };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void DgvOutStanSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedSaleId = Convert.ToInt32(dgvOutStanSales.Rows[e.RowIndex].Cells["SaleID"].Value);
            }
        }

        private async Task LoadSalesAsync()
        {
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = @"
                        SELECT 
                            s.SaleID,
                            s.ProductID,
                            s.Quantity,
                            s.TotalPrice,
                            s.PaymentMethod,
                            s.SaleDate,
                            c.CustomerName
                        FROM Sales s
                        LEFT JOIN Customers c ON s.CustomerID = c.CustomerID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvOutStanSales.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task BtnDelete_ClickAsync()
        {
            if (selectedSaleId == -1)
            {
                MessageBox.Show("Please select a sale to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this sale?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                    {
                        string deleteQuery = "DELETE FROM Sales WHERE SaleID = @SaleID";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@SaleID", selectedSaleId);
                            int rows = await cmd.ExecuteNonQueryAsync();

                            if (rows > 0)
                            {
                                MessageBox.Show("Sale deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                await LoadSalesAsync();
                                selectedSaleId = -1;
                            }
                            else
                            {
                                MessageBox.Show("Sale not found or already deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Loop through open forms and find by Text/title
            foreach (Form form in Application.OpenForms)
            {
                if (form is IMS_SaleForm mainForm)
                {
                    //mainForm.LoadProductsFetch();
                    //mainForm.LoadOutSales();
                    //mainForm.EnableButtons(true);
                    mainForm.BringToFront();
                    mainForm.Activate();

                    this.Close();
                    return;
                }
            }
        }
    }//SALES DELETING FROM SALES 
    public class ReceivablesManagerForm : Form
    {
        private DataGridView dgvReceivables;
        private ComboBox cmbPaymentMethods;
        private Button btnMarkPaid;
        private Button btnCancel;
        public void SetMarkPaidButtonVisibility(bool visible)
        {
            btnMarkPaid.Visible = false;
        }
        private int selectedSaleID = -1;
        private readonly DatabaseConnection db = new DatabaseConnection();

        public ReceivablesManagerForm()
        {
            InitializeForm();
            InitializeControls();
            _ = LoadPaymentMethodsAsync();
        }

        private void InitializeForm()
        {
            Text = "Accounts Receivable Manager";
            Size = new Size(900, 580);
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(8);
        }

        private void InitializeControls()
        {
            dgvReceivables = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvReceivables.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var cellValue = dgvReceivables.Rows[e.RowIndex].Cells["SaleID"].Value;
                    if (cellValue != DBNull.Value && cellValue != null && int.TryParse(cellValue.ToString(), out var saleId))
                    {
                        selectedSaleID = saleId;
                    }
                    else
                    {
                        selectedSaleID = -1;
                    }
                }
            };

            cmbPaymentMethods = new ComboBox
            {
                Location = new Point(20, 320),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPaymentMethods.SelectedIndexChanged += async (s, e) =>
            {
                selectedSaleID = -1;
                await LoadReceivablesAsync();
            };

            btnMarkPaid = new Button
            {
                Text = "Mark as Paid",
                Location = new Point(240, 320),
                Width = 120
            };
            btnMarkPaid.Click += async (s, e) => await MarkAsPaidAsync();

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(380, 320),
                Width = 100
            };
            btnCancel.Click += (s, e) =>
            {
                Parent?.Controls.Remove(this);
                Dispose();
            };

            Controls.Add(dgvReceivables);
            Controls.Add(cmbPaymentMethods);
            Controls.Add(btnMarkPaid);
            Controls.Add(btnCancel);
        }

        private async Task LoadPaymentMethodsAsync()
        {
            const string query = "SELECT PaymentMethodID, MethodName FROM PaymentMethods";
            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbPaymentMethods.DataSource = dt;
                    cmbPaymentMethods.DisplayMember = "MethodName";
                    cmbPaymentMethods.ValueMember = "PaymentMethodID";
                }
                if (cmbPaymentMethods.Items.Count > 0)
                {
                    cmbPaymentMethods.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment methods: " + ex.Message);
            }
        }

        private int GetSelectedPaymentMethodId()
        {
            // Safely get the selected payment method ID from the ComboBox
            if (cmbPaymentMethods.SelectedValue == null)
                return -1;

            if (cmbPaymentMethods.SelectedValue is DataRowView drv)
            {
                return Convert.ToInt32(drv["PaymentMethodID"]);
            }
            else if (cmbPaymentMethods.SelectedValue is int id)
            {
                return id;
            }
            else
            {
                return Convert.ToInt32(cmbPaymentMethods.SelectedValue);
            }
        }

        private async Task LoadReceivablesAsync()
        {
            int selectedPaymentMethodId = GetSelectedPaymentMethodId();

            if (selectedPaymentMethodId == -1)
            {
                dgvReceivables.DataSource = null;
                return;
            }

            // Load unpaid sales filtered by payment method
            const string query = @"
            SELECT s.SaleID, c.CustomerName, s.TotalPrice, s.SaleDate, s.IsPaid, s.PaymentMethodID
            FROM Sales s
            INNER JOIN Customers c ON s.CustomerID = c.CustomerID
            WHERE s.IsPaid = 0 AND (s.PaymentMethodID = @pmId OR s.PaymentMethodID IS NULL)
            ORDER BY s.SaleDate";

            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pmId", selectedPaymentMethodId);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvReceivables.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading receivables: " + ex.Message);
            }
        }

        private async Task MarkAsPaidAsync()
        {
            if (selectedSaleID == -1)
            {
                MessageBox.Show("Please select a sale first.");
                return;
            }

            int paymentMethodId = GetSelectedPaymentMethodId();

            if (paymentMethodId == -1)
            {
                MessageBox.Show("Please select a valid payment method.");
                return;
            }

            decimal amountDue = 0;
            if (dgvReceivables.CurrentRow != null &&
                dgvReceivables.CurrentRow.Cells["TotalPrice"].Value != DBNull.Value)
            {
                amountDue = Convert.ToDecimal(dgvReceivables.CurrentRow.Cells["TotalPrice"].Value);
            }
            else
            {
                MessageBox.Show("Could not determine the amount due for the selected sale.");
                return;
            }

            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                {
                    using (var trans = conn.BeginTransaction())
                    {
                        // Mark sale as paid
                        string updateSale = "UPDATE Sales SET IsPaid = 1, PaymentMethodID = @pm WHERE SaleID = @sid";
                        using (var cmd = new SqlCommand(updateSale, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@pm", paymentMethodId);
                            cmd.Parameters.AddWithValue("@sid", selectedSaleID);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Insert payment record
                        string insertPayment = @"
                        INSERT INTO Payments (SaleID, PaymentMethodID, Amount, PaymentDate)
                        VALUES (@sid, @pm, @amount, @date)";
                        using (var cmd = new SqlCommand(insertPayment, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@sid", selectedSaleID);
                            cmd.Parameters.AddWithValue("@pm", paymentMethodId);
                            cmd.Parameters.AddWithValue("@amount", amountDue);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Update Deposits: increment or insert if missing
                        string updateDeposit = @"
                        UPDATE Deposits
                        SET TotalAmount = TotalAmount + @amount, LastUpdated = GETDATE()
                        WHERE PaymentMethodID = @pm";
                        using (var cmd = new SqlCommand(updateDeposit, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@pm", paymentMethodId);
                            cmd.Parameters.AddWithValue("@amount", amountDue);
                            int rows = await cmd.ExecuteNonQueryAsync();

                            if (rows == 0)
                            {
                                string insertDeposit = @"
                                INSERT INTO Deposits (PaymentMethodID, TotalAmount, LastUpdated)
                                VALUES (@pm, @amount, GETDATE())";
                                using (var cmdInsert = new SqlCommand(insertDeposit, conn, trans))
                                {
                                    cmdInsert.Parameters.AddWithValue("@pm", paymentMethodId);
                                    cmdInsert.Parameters.AddWithValue("@amount", amountDue);
                                    await cmdInsert.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        trans.Commit();
                    }
                }

                MessageBox.Show("Sale marked as paid and payment synced successfully.");

                // Remove the paid row from DataGridView immediately
                if (dgvReceivables.CurrentRow != null)
                {
                    dgvReceivables.Rows.RemoveAt(dgvReceivables.CurrentRow.Index);
                }

                selectedSaleID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking sale as paid: " + ex.Message);
            }
        }
    }
    public class CustomerManagerForm : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtName, txtContact, txtAddress;
        private Button btnAdd, btnUpdate, btnDelete, btnClear;
        private int selectedCustomerId = -1;

        private readonly DatabaseConnection db = new DatabaseConnection();

        public CustomerManagerForm()
        {
            InitializeForm();
            InitializeControls();
            LoadCustomersAsync();
        }

        private void InitializeForm()
        {
            Text = "Customer Manager";
            BackColor = Color.White;
            Padding = new Padding(8);
            Size = new Size(900, 580);
            FormBorderStyle = FormBorderStyle.None;
        }

        private void InitializeControls()
        {
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300,
                Padding = new Padding(4)
            };

            dgvCustomers = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            };
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.CellClick += DgvCustomers_CellClick;
            pnlTop.Controls.Add(dgvCustomers);

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4)
            };

            int labelX = 10, controlX = 120, y = 10, spacing = 30;

            var lblName = new Label { Text = "Customer Name", Location = new Point(labelX, y), AutoSize = true };
            txtName = new TextBox { Location = new Point(controlX, y), Width = 250 };
            y += spacing;

            var lblContact = new Label { Text = "Contact Info", Location = new Point(labelX, y), AutoSize = true };
            txtContact = new TextBox { Location = new Point(controlX, y), Width = 250 };
            y += spacing;

            var lblAddress = new Label { Text = "Address", Location = new Point(labelX, y), AutoSize = true };
            txtAddress = new TextBox { Location = new Point(controlX, y), Width = 250 };
            y += spacing;

            btnAdd = new Button { Text = "Add", Location = new Point(controlX, y), Width = 90 };
            btnAdd.Click += async (s, e) => await AddCustomerAsync();

            btnUpdate = new Button { Text = "Update", Location = new Point(controlX + 100, y), Width = 90 };
            btnUpdate.Click += async (s, e) => await UpdateCustomerAsync();

            btnDelete = new Button { Text = "Delete", Location = new Point(controlX + 200, y), Width = 90 };
            btnDelete.Click += async (s, e) => await DeleteCustomerAsync();

            btnClear = new Button { Text = "Clear", Location = new Point(controlX + 300, y), Width = 90 };
            btnClear.Click += (s, e) => ClearFields();

            pnlBottom.Controls.AddRange(new Control[]
            {
                lblName, txtName,
                lblContact, txtContact,
                lblAddress, txtAddress,
                btnAdd, btnUpdate, btnDelete, btnClear
            });

            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private async void LoadCustomersAsync()
        {
            string query = "SELECT CustomerID, CustomerName, ContactInfo, Address FROM Customers ORDER BY CustomerName";
            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var adapter = new SqlDataAdapter(query, conn))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.Rows.Count > 0)
            {
                var row = dgvCustomers.Rows[e.RowIndex];
                selectedCustomerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
                txtName.Text = row.Cells["CustomerName"].Value.ToString();
                txtContact.Text = row.Cells["ContactInfo"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private async Task AddCustomerAsync()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Customer name is required.");
                return;
            }

            string query = "INSERT INTO Customers (CustomerName, ContactInfo, Address) VALUES (@Name, @Contact, @Address)";
            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    await cmd.ExecuteNonQueryAsync();
                }

                LoadCustomersAsync();
                ClearFields();
                MessageBox.Show("Customer added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer:\n" + ex.Message);
            }
        }

        private async Task UpdateCustomerAsync()
        {
            if (selectedCustomerId == -1)
            {
                MessageBox.Show("Select a customer to update.");
                return;
            }

            string query = "UPDATE Customers SET CustomerName=@Name, ContactInfo=@Contact, Address=@Address WHERE CustomerID=@ID";
            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@ID", selectedCustomerId);
                    await cmd.ExecuteNonQueryAsync();
                }

                LoadCustomersAsync();
                ClearFields();
                MessageBox.Show("Customer updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer:\n" + ex.Message);
            }
        }

        private async Task DeleteCustomerAsync()
        {
            if (selectedCustomerId == -1)
            {
                MessageBox.Show("Select a customer to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string query = "DELETE FROM Customers WHERE CustomerID=@ID";
            try
            {
                using (var conn = await db.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", selectedCustomerId);
                    await cmd.ExecuteNonQueryAsync();
                }

                LoadCustomersAsync();
                ClearFields();
                MessageBox.Show("Customer deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer:\n" + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            selectedCustomerId = -1;
        }
    }
    public class PurchaseInventoryForm : Form
    {
        private ComboBox cmbProducts;
        private ComboBox cmbVendors;
        private TextBox txtQuantity;
        private TextBox txtCost;
        private Button btnSave;
        private Button btnCancel;
        private DataGridView dgvProducts;

        private readonly DatabaseConnection dbConnection = new DatabaseConnection();

        public PurchaseInventoryForm()
        {
            InitializeForm();
            InitializeControls();

            FormResizer.Register(this);
            _ = LoadProductsAsync();
            _ = LoadVendorsAsync();
            _ = LoadProductsGridAsync();
        }

        private void InitializeForm()
        {
            Text = "Purchase Inventory";
            BackColor = Color.White;
            Size = new Size(900, 580);  // consistent with SalesEntryForm
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(8);
        }

        private void InitializeControls()
        {
            // Top Panel - DataGridView
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                Padding = new Padding(4)
            };

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            pnlTop.Controls.Add(dgvProducts);

            // Bottom Panel - Input Controls
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4)
            };

            int labelX = 10, controlX = 120, y = 10, spacing = 30;

            // Product
            var lblProduct = new Label { Text = "Product", Location = new Point(labelX, y), AutoSize = true };
            cmbProducts = new ComboBox { Location = new Point(controlX, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            // Vendor
            var lblVendor = new Label { Text = "Vendor", Location = new Point(labelX, y), AutoSize = true };
            cmbVendors = new ComboBox { Location = new Point(controlX, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            // Quantity
            var lblQty = new Label { Text = "Quantity", Location = new Point(labelX, y), AutoSize = true };
            txtQuantity = new TextBox { Location = new Point(controlX, y), Width = 200 };
            y += spacing;

            // Cost
            var lblCost = new Label { Text = "Cost", Location = new Point(labelX, y), AutoSize = true };
            txtCost = new TextBox { Location = new Point(controlX, y), Width = 200 };
            y += spacing;

            // Buttons
            btnSave = new Button { Text = "Save", Location = new Point(controlX, y), Width = 100 };
            btnSave.Click += async (s, e) => await BtnSave_ClickAsync();

            btnCancel = new Button { Text = "Cancel", Location = new Point(controlX + 110, y), Width = 90 };
            btnCancel.Click += (s, e) =>
            {
                if (this.Parent != null)
                {
                    this.Parent.Controls.Remove(this);
                    this.Dispose();
                }
            };

            pnlBottom.Controls.AddRange(new Control[]
            {
                lblProduct, cmbProducts,
                lblVendor, cmbVendors,
                lblQty, txtQuantity,
                lblCost, txtCost,
                btnSave, btnCancel
            });

            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private async Task LoadProductsAsync()
        {
            const string query = "SELECT ProductName FROM Products ORDER BY ProductName";
            try
            {
                using (var conn = await dbConnection.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbProducts.DataSource = dt;
                    cmbProducts.DisplayMember = "ProductName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private async Task LoadVendorsAsync()
        {
            const string query = "SELECT VendorName FROM Vendors ORDER BY VendorName";
            try
            {
                using (var conn = await dbConnection.OpenSqlConnectionAsync())
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    cmbVendors.DataSource = dt;
                    cmbVendors.DisplayMember = "VendorName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vendors: " + ex.Message);
            }
        }

        private async Task LoadProductsGridAsync()
        {
            const string query = @"
                SELECT ProductID, ProductName, Cost, Quantity, VendorID, PurchaseDate
                FROM Products
                ORDER BY ProductName";
            try
            {
                using (var conn = await dbConnection.OpenSqlConnectionAsync())
                using (var adapter = new SqlDataAdapter(query, conn))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products grid: " + ex.Message);
            }
        }

        private async Task BtnSave_ClickAsync()
        {
            if (cmbProducts.SelectedItem == null || cmbVendors.SelectedItem == null
                || string.IsNullOrWhiteSpace(txtQuantity.Text)
                || string.IsNullOrWhiteSpace(txtCost.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid positive quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost <= 0)
            {
                MessageBox.Show("Enter a valid positive cost.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var productName = cmbProducts.Text.Trim();
            var vendorName = cmbVendors.Text.Trim();
            var purchaseDate = DateTime.Now;

            try
            {
                int productId = await GetIdByNameAsync("Products", "ProductID", "ProductName", productName);
                int vendorId = await GetIdByNameAsync("Vendors", "VendorID", "VendorName", vendorName);

                if (productId == -1)
                {
                    // Insert new product
                    await InsertIntoProductsAsync(productName, cost, quantity, vendorId, purchaseDate);
                    MessageBox.Show("New product inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update quantity of existing product
                    await UpdateProductQuantityAsync(productId, quantity);
                    MessageBox.Show("Product quantity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                await LoadProductsAsync();
                await LoadProductsGridAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<int> GetIdByNameAsync(string table, string idCol, string nameCol, string name)
        {
            string query = $"SELECT {idCol} FROM {table} WHERE {nameCol} = @Name";
            using (var conn = await dbConnection.OpenSqlConnectionAsync())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                var result = await cmd.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        private async Task InsertIntoProductsAsync(string name, decimal cost, int qty, int vendorId, DateTime date)
        {
            const string query = @"
                INSERT INTO Products (ProductName, Cost, Quantity, VendorID, PurchaseDate)
                VALUES (@ProductName, @Cost, @Quantity, @VendorID, @PurchaseDate)";
            using (var conn = await dbConnection.OpenSqlConnectionAsync())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProductName", name);
                cmd.Parameters.AddWithValue("@Cost", cost);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                cmd.Parameters.AddWithValue("@VendorID", vendorId);
                cmd.Parameters.AddWithValue("@PurchaseDate", date);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task UpdateProductQuantityAsync(int productId, int qty)
        {
            const string query = "UPDATE Products SET Quantity = Quantity + @Qty WHERE ProductID = @ProductID";
            using (var conn = await dbConnection.OpenSqlConnectionAsync())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private void ClearInputs()
        {
            txtQuantity.Clear();
            txtCost.Clear();
            if (cmbProducts.Items.Count > 0) cmbProducts.SelectedIndex = 0;
            if (cmbVendors.Items.Count > 0) cmbVendors.SelectedIndex = 0;
        }
    }
    public class DynamicRefundForm : Form
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=FinGradProj_DB;Integrated Security=True";

        private ComboBox cmbCustomers;
        private DataGridView dgvSales;
        private TextBox txtRefundQuantity;
        private Button btnProcessRefund;
        private Label lblStatus;

        private bool isLoadingCustomers = false;

        public DynamicRefundForm()
        {
            InitializeForm();
            _ = LoadCustomersAsync();
        }

        private void InitializeForm()
        {
            this.Text = "Process Refund";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblCustomer = new Label() { Text = "Select Customer:", Location = new Point(20, 20), AutoSize = true };
            cmbCustomers = new ComboBox() { Location = new Point(140, 15), Width = 250 };
            cmbCustomers.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomers.SelectedIndexChanged += async (s, e) => await CmbCustomers_SelectedIndexChanged();

            dgvSales = new DataGridView()
            {
                Location = new Point(20, 50),
                Size = new Size(909, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            };
            dgvSales.BackgroundColor = Color.White;

            Label lblRefundQty = new Label() { Text = "Refund Quantity:", Location = new Point(20, 420), AutoSize = true };
            txtRefundQuantity = new TextBox() { Location = new Point(140, 415), Width = 100 };
            txtRefundQuantity.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            btnProcessRefund = new Button() { Text = "Process Refund", Location = new Point(260, 412), Width = 150 };
            btnProcessRefund.Click += async (s, e) => await ProcessRefundAsync();

            lblStatus = new Label() { Location = new Point(20, 460), ForeColor = Color.Red, AutoSize = true };

            this.Controls.Add(lblCustomer);
            this.Controls.Add(cmbCustomers);
            this.Controls.Add(dgvSales);
            this.Controls.Add(lblRefundQty);
            this.Controls.Add(txtRefundQuantity);
            this.Controls.Add(btnProcessRefund);
            this.Controls.Add(lblStatus);
        }

        private async Task LoadCustomersAsync()
        {
            isLoadingCustomers = true;
            cmbCustomers.DataSource = null;
            cmbCustomers.Items.Clear();
            lblStatus.Text = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string query = "SELECT CustomerID, CustomerName FROM Customers ORDER BY CustomerName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbCustomers.DataSource = dt;
                    cmbCustomers.DisplayMember = "CustomerName";
                    cmbCustomers.ValueMember = "CustomerID";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error loading customers: {ex.Message}";
                }
                finally
                {
                    isLoadingCustomers = false;
                }
            }
        }

        private async Task CmbCustomers_SelectedIndexChanged()
        {
            if (isLoadingCustomers)
                return;

            if (cmbCustomers.SelectedValue == null)
                return;

            // Prevent cast issues by checking type
            if (cmbCustomers.SelectedValue is DataRowView)
                return;

            if (!int.TryParse(cmbCustomers.SelectedValue.ToString(), out int customerId))
                return;

            await LoadCustomerSalesAsync(customerId);
        }

        private async Task LoadCustomerSalesAsync(int customerId)
        {
            dgvSales.DataSource = null;
            lblStatus.Text = "";
            txtRefundQuantity.Text = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT 
                            s.SaleID, 
                            p.ProductName, 
                            s.Quantity, 
                            pm.MethodName AS PaymentMethod, 
                            pm.PaymentMethodID,
                            s.SaleDate
                        FROM Sales s
                        INNER JOIN Products p ON s.ProductID = p.ProductID
                        INNER JOIN Payments pay ON s.SaleID = pay.SaleID
                        INNER JOIN PaymentMethods pm ON pay.PaymentMethodID = pm.PaymentMethodID
                        WHERE s.CustomerID = @CustomerID AND s.IsPaid = 1
                        ORDER BY s.SaleDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSales.DataSource = dt;

                        if (dgvSales.Columns.Contains("PaymentMethodID"))
                            dgvSales.Columns["PaymentMethodID"].Visible = false;

                        if (dgvSales.Columns.Contains("SaleDate"))
                            dgvSales.Columns["SaleDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error loading sales: {ex.Message}";
                }
            }
        }

        private async Task ProcessRefundAsync()
        {
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "";

            if (dgvSales.SelectedRows.Count == 0)
            {
                lblStatus.Text = "Please select a sale to refund.";
                return;
            }

            if (!int.TryParse(txtRefundQuantity.Text.Trim(), out int refundQty) || refundQty <= 0)
            {
                lblStatus.Text = "Please enter a valid positive integer for refund quantity.";
                return;
            }

            var confirm = MessageBox.Show($"Confirm refund of {refundQty} items?", "Confirm Refund", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var selectedRow = dgvSales.SelectedRows[0];
            int saleId = Convert.ToInt32(selectedRow.Cells["SaleID"].Value);
            int saleQty = Convert.ToInt32(selectedRow.Cells["Quantity"].Value);
            int paymentMethodId = Convert.ToInt32(selectedRow.Cells["PaymentMethodID"].Value);

            if (refundQty > saleQty)
            {
                lblStatus.Text = "Refund quantity cannot exceed the original sale quantity.";
                return;
            }

            btnProcessRefund.Enabled = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string insertQuery = @"
                        INSERT INTO Refunds (SaleID, RefundAmount, RefundDate, RefundMethod, ProcessedBy)
                        VALUES (@SaleID, @RefundAmount, @RefundDate, @RefundMethod, @ProcessedBy)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleId);
                        cmd.Parameters.AddWithValue("@RefundAmount", refundQty);
                        cmd.Parameters.AddWithValue("@RefundDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@RefundMethod", paymentMethodId);
                        cmd.Parameters.AddWithValue("@ProcessedBy", Environment.UserName);

                        int rows = await cmd.ExecuteNonQueryAsync();

                        if (rows > 0)
                        {
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Refund processed successfully.";
                            await LoadCustomerSalesAsync(Convert.ToInt32(cmbCustomers.SelectedValue));
                            txtRefundQuantity.Clear();
                        }
                        else
                        {
                            lblStatus.Text = "Failed to process refund.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error processing refund: {ex.Message}";
            }
            finally
            {
                btnProcessRefund.Enabled = true;
            }
        }

        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new DynamicRefundForm());
        //}
    }
    public partial class PrintReportForm : Form
    {
        private readonly DataTable _reportData;
        private readonly string _reportHeader;

        private Label lblHeader;
        private Label lblContact;
        private Label lblDate;
        private DataGridView dataGridViewReport;
        private Button btnPrint;

        private int _currentRow;
        private int _rowHeight = 30;
        private int _colWidth = 100;
        private int _currentPage = 1;

        public PrintReportForm(DataTable reportData, string reportHeader)
        {
            _reportData = reportData;
            _reportHeader = reportHeader;

            InitializeComponent();
            InitializeHeader();
            InitializeDataGridView();
            InitializePrintButton();

            Load += PrintReportForm_Load;
        }

        private void InitializeComponent()
        {
            this.Text = "Print Report";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
        }

        private void InitializeHeader()
        {
            lblHeader = new Label
            {
                Text = GetReportTitle(),
                Font = new Font("Trebuchet MS", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(50, 20)
            };
            this.Controls.Add(lblHeader);

            lblContact = new Label
            {
                Text = "Phone: +2526292242    Email: engabdiladif",
                Font = new Font("Trebuchet MS", 10),
                AutoSize = true,
                Location = new Point(50, 50)
            };
            this.Controls.Add(lblContact);

            lblDate = new Label
            {
                Text = GetReportDate(),
                Font = new Font("Trebuchet MS", 10),
                AutoSize = true,
                Location = new Point(50, 70)
            };
            this.Controls.Add(lblDate);
        }

        private string GetReportTitle()
        {
            return _reportHeader.Split('\n')[0];
        }

        private string GetReportDate()
        {
            return _reportHeader.Contains("\n") && _reportHeader.Split('\n').Length > 1
                ? _reportHeader.Split('\n')[1]
                : "Date: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
        }

        private void InitializeDataGridView()
        {
            dataGridViewReport = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(840, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            this.Controls.Add(dataGridViewReport);
        }

        private void InitializePrintButton()
        {
            btnPrint = new Button
            {
                Text = "Print",
                Location = new Point(780, 530),
                Size = new Size(80, 30)
            };
            btnPrint.Click += BtnPrint_Click;
            this.Controls.Add(btnPrint);
        }

        private void PrintReportForm_Load(object sender, EventArgs e)
        {
            if (_reportData == null || _reportData.Columns.Count == 0)
            {
                MessageBox.Show("No data available for this report.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            dataGridViewReport.DataSource = _reportData;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (_reportData == null || _reportData.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += PrintDoc_PrintPage;

            PrintDialog printDialog = new PrintDialog
            {
                Document = printDoc
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int marginLeft = 50;
            int marginTop = 100;
            int x = marginLeft;
            int y = marginTop;
            int colCount = dataGridViewReport.Columns.Count;

            AdjustColumnWidth(e.Graphics, e.MarginBounds.Width);

            // Draw header text
            e.Graphics.DrawString(lblHeader.Text, new Font("Trebuchet MS", 16, FontStyle.Bold), Brushes.Black, marginLeft, 20);
            e.Graphics.DrawString(lblContact.Text, new Font("Trebuchet MS", 10), Brushes.Black, marginLeft, 50);
            e.Graphics.DrawString(lblDate.Text, new Font("Trebuchet MS", 10), Brushes.Black, marginLeft, 70);

            // Draw column headers
            for (int i = 0; i < colCount; i++)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(x, y, _colWidth, _rowHeight));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, _colWidth, _rowHeight));
                string headerText = dataGridViewReport.Columns[i].HeaderText;
                e.Graphics.DrawString(headerText, dataGridViewReport.Font, Brushes.Black, new RectangleF(x + 2, y + 5, _colWidth, _rowHeight));
                x += _colWidth;
            }

            y += _rowHeight;
            x = marginLeft;

            // Draw rows until page end or all rows printed
            while (_currentRow < dataGridViewReport.Rows.Count)
            {
                if (y + _rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    _currentPage++;
                    return;
                }

                var row = dataGridViewReport.Rows[_currentRow];
                for (int col = 0; col < colCount; col++)
                {
                    string cellText = row.Cells[col].Value?.ToString() ?? "";
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, _colWidth, _rowHeight));
                    e.Graphics.DrawString(cellText, dataGridViewReport.Font, Brushes.Black, new RectangleF(x + 2, y + 5, _colWidth, _rowHeight));
                    x += _colWidth;
                }

                x = marginLeft;
                y += _rowHeight;
                _currentRow++;
            }

            e.HasMorePages = false;
            _currentRow = 0;
            _currentPage = 1;
        }

        private void AdjustColumnWidth(Graphics g, int pageWidth)
        {
            int totalColumns = dataGridViewReport.Columns.Count;
            _colWidth = pageWidth / totalColumns;
        }
    }
    public class PaymentSyncForm : Form
    {
        private DataGridView dgvPaidSales;
        private DataGridView dgvDeposits;
        private ComboBox cmbTransferFrom, cmbTransferTo;
        private Button btnSyncPayments, btnTransfer;
        private Label lblCashTotal, lblCardTotal, lblBankTotal;

        private readonly DatabaseConnection db = new DatabaseConnection();

        public PaymentSyncForm()
        {
            InitializeForm();
            InitializeControls();
            _ = LoadPaymentMethodsAsync();
            _ = LoadPaidSalesAsync();
            _ = LoadDepositsAsync();
        }

        private void InitializeForm()
        {
            this.Text = "Payment Sync & Transfer";
            this.Size = new Size(1200, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.White;
        }

        private void InitializeControls()
        {
            // Deposits DataGridView - Left panel
            dgvDeposits = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(200, 540),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(dgvDeposits);

            // Paid sales DataGridView - right side
            dgvPaidSales = new DataGridView
            {
                Location = new Point(220, 10),
                Size = new Size(950, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(dgvPaidSales);

            // Sync Payments Button
            btnSyncPayments = new Button
            {
                Text = "Sync Payments",
                Location = new Point(220, 370),
                Size = new Size(140, 40),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnSyncPayments.Click += BtnSyncPayments_Click;
            this.Controls.Add(btnSyncPayments);

            // Labels for totals
            lblCashTotal = new Label
            {
                Text = "Cash Total: $0.00",
                Location = new Point(380, 380),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblCashTotal);

            lblCardTotal = new Label
            {
                Text = "Card Total: $0.00",
                Location = new Point(500, 380),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblCardTotal);

            lblBankTotal = new Label
            {
                Text = "Bank Total: $0.00",
                Location = new Point(620, 380),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblBankTotal);

            // ComboBoxes and Transfer Button for transfer functionality
            Label lblFrom = new Label { Text = "Transfer From:", Location = new Point(220, 420), AutoSize = true };
            cmbTransferFrom = new ComboBox { Location = new Point(320, 415), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(lblFrom);
            this.Controls.Add(cmbTransferFrom);

            Label lblTo = new Label { Text = "Transfer To:", Location = new Point(490, 420), AutoSize = true };
            cmbTransferTo = new ComboBox { Location = new Point(580, 415), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(lblTo);
            this.Controls.Add(cmbTransferTo);

            btnTransfer = new Button
            {
                Text = "Transfer Amount",
                Location = new Point(750, 412),
                Size = new Size(150, 35),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White
            };
            btnTransfer.Click += BtnTransfer_Click;
            this.Controls.Add(btnTransfer);
        }

        private async Task LoadPaymentMethodsAsync()
        {
            string query = "SELECT PaymentMethodID, MethodName FROM PaymentMethods";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    cmbTransferFrom.DataSource = dt.Copy();
                    cmbTransferFrom.DisplayMember = "MethodName";
                    cmbTransferFrom.ValueMember = "PaymentMethodID";

                    cmbTransferTo.DataSource = dt;
                    cmbTransferTo.DisplayMember = "MethodName";
                    cmbTransferTo.ValueMember = "PaymentMethodID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load payment methods: " + ex.Message);
            }
        }

        private async Task LoadPaidSalesAsync()
        {
            string query = @"
            SELECT 
                s.SaleID, 
                c.CustomerName, 
                p.ProductName, 
                s.Quantity, 
                s.TotalPrice, 
                pm.MethodName AS PaymentMethod,
                s.SaleDate
            FROM Sales s
            INNER JOIN Customers c ON s.CustomerID = c.CustomerID
            INNER JOIN Products p ON s.ProductID = p.ProductID
            INNER JOIN PaymentMethods pm ON s.PaymentMethodID = pm.PaymentMethodID
            WHERE s.IsPaid = 1
            ORDER BY s.SaleDate DESC";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvPaidSales.DataSource = dt;

                    UpdateTotals(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading paid sales: " + ex.Message);
            }
        }

        private void UpdateTotals(DataTable salesTable)
        {
            decimal cashTotal = 0m;
            decimal cardTotal = 0m;
            decimal bankTotal = 0m;

            foreach (DataRow row in salesTable.Rows)
            {
                string paymentMethod = row["PaymentMethod"].ToString();
                decimal amount = row["TotalPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TotalPrice"]);

                switch (paymentMethod.ToLower())
                {
                    case "cash":
                        cashTotal += amount;
                        break;
                    case "card":
                        cardTotal += amount;
                        break;
                    case "bank":
                        bankTotal += amount;
                        break;
                }
            }

            lblCashTotal.Text = $"Cash Total: ${cashTotal:N2}";
            lblCardTotal.Text = $"Card Total: ${cardTotal:N2}";
            lblBankTotal.Text = $"Bank Total: ${bankTotal:N2}";
        }

        private async Task LoadDepositsAsync()
        {
            string query = @"
            SELECT d.DepositID, pm.MethodName AS PaymentMethod, d.TotalAmount, d.LastUpdated
            FROM Deposits d
            INNER JOIN PaymentMethods pm ON d.PaymentMethodID = pm.PaymentMethodID
            ORDER BY pm.MethodName";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvDeposits.DataSource = dt;

                    // Hide DepositID, LastUpdated if you want
                    if (dgvDeposits.Columns.Contains("DepositID"))
                        dgvDeposits.Columns["DepositID"].Visible = false;
                    if (dgvDeposits.Columns.Contains("LastUpdated"))
                        dgvDeposits.Columns["LastUpdated"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading deposits: " + ex.Message);
            }
        }

        private async void BtnSyncPayments_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    // Get all paid sales to sync
                    string querySales = "SELECT SaleID, PaymentMethodID, TotalPrice FROM Sales WHERE IsPaid = 1";

                    var salesToSync = new System.Collections.Generic.List<(int SaleID, int PaymentMethodID, decimal Amount)>();

                    using (SqlCommand cmdSales = new SqlCommand(querySales, conn))
                    using (SqlDataReader reader = await cmdSales.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader.IsDBNull(0) || reader.IsDBNull(1) || reader.IsDBNull(2))
                                continue;

                            int saleId = reader.GetInt32(0);
                            int paymentMethodId = reader.GetInt32(1);
                            decimal amount = reader.GetDecimal(2);

                            salesToSync.Add((saleId, paymentMethodId, amount));
                        }
                    }

                    // Insert payments if not existing
                    foreach (var sale in salesToSync)
                    {
                        string checkPaymentQuery = @"SELECT COUNT(*) FROM Payments WHERE SaleID = @SaleID AND PaymentMethodID = @PaymentMethodID";

                        using (SqlCommand checkCmd = new SqlCommand(checkPaymentQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@SaleID", sale.SaleID);
                            checkCmd.Parameters.AddWithValue("@PaymentMethodID", sale.PaymentMethodID);

                            int count = (int)await checkCmd.ExecuteScalarAsync();

                            if (count == 0)
                            {
                                string insertPaymentQuery = @"
                                INSERT INTO Payments (SaleID, PaymentMethodID, Amount, PaymentDate)
                                VALUES (@SaleID, @PaymentMethodID, @Amount, @PaymentDate)";

                                using (SqlCommand insertCmd = new SqlCommand(insertPaymentQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@SaleID", sale.SaleID);
                                    insertCmd.Parameters.AddWithValue("@PaymentMethodID", sale.PaymentMethodID);
                                    insertCmd.Parameters.AddWithValue("@Amount", sale.Amount);
                                    insertCmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);

                                    await insertCmd.ExecuteNonQueryAsync();
                                }
                            }
                        }
                    }

                    // Now update Deposits table by aggregating Payments grouped by PaymentMethodID
                    string getPaymentsSumQuery = @"
                    SELECT PaymentMethodID, SUM(Amount) AS TotalAmount
                    FROM Payments
                    GROUP BY PaymentMethodID";

                    var paymentSums = new System.Collections.Generic.Dictionary<int, decimal>();

                    using (SqlCommand sumCmd = new SqlCommand(getPaymentsSumQuery, conn))
                    using (SqlDataReader sumReader = await sumCmd.ExecuteReaderAsync())
                    {
                        while (await sumReader.ReadAsync())
                        {
                            int paymentMethodId = sumReader.GetInt32(0);
                            decimal totalAmount = sumReader.IsDBNull(1) ? 0m : sumReader.GetDecimal(1);
                            paymentSums[paymentMethodId] = totalAmount;
                        }
                    }

                    foreach (var kvp in paymentSums)
                    {
                        // Upsert logic for Deposits table:
                        string upsertDeposit = @"
                        IF EXISTS (SELECT 1 FROM Deposits WHERE PaymentMethodID = @PaymentMethodID)
                            UPDATE Deposits 
                            SET TotalAmount = @TotalAmount, LastUpdated = @LastUpdated 
                            WHERE PaymentMethodID = @PaymentMethodID
                        ELSE
                            INSERT INTO Deposits (PaymentMethodID, TotalAmount, LastUpdated)
                            VALUES (@PaymentMethodID, @TotalAmount, @LastUpdated)";

                        using (SqlCommand upsertCmd = new SqlCommand(upsertDeposit, conn))
                        {
                            upsertCmd.Parameters.AddWithValue("@PaymentMethodID", kvp.Key);
                            upsertCmd.Parameters.AddWithValue("@TotalAmount", kvp.Value);
                            upsertCmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                            await upsertCmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                MessageBox.Show("Payments synced and deposits updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadPaidSalesAsync();
                await LoadDepositsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error syncing payments or updating deposits: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTransfer_Click(object sender, EventArgs e)
        {
            if (cmbTransferFrom.SelectedValue == null || cmbTransferTo.SelectedValue == null)
            {
                MessageBox.Show("Please select both transfer source and target payment methods.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbTransferFrom.SelectedValue.Equals(cmbTransferTo.SelectedValue))
            {
                MessageBox.Show("Transfer source and target cannot be the same.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fromMethodId = (int)cmbTransferFrom.SelectedValue;
            int toMethodId = (int)cmbTransferTo.SelectedValue;

            decimal fromTotal = await GetTotalDepositAmountByPaymentMethodAsync(fromMethodId);
            if (fromTotal <= 0)
            {
                MessageBox.Show("No available amount to transfer from the selected source.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal transferAmount = fromTotal; // For now transfer full amount; extend UI if partial is needed

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    // Insert negative payment to remove from source method
                    string insertNegativePayment = @"
                    INSERT INTO Payments (SaleID, PaymentMethodID, Amount, PaymentDate)
                    VALUES (NULL, @FromMethodID, @Amount, @Date);";

                    // Insert positive payment to add to target method
                    string insertPositivePayment = @"
                    INSERT INTO Payments (SaleID, PaymentMethodID, Amount, PaymentDate)
                    VALUES (NULL, @ToMethodID, @Amount, @Date);";

                    using (SqlCommand cmdNeg = new SqlCommand(insertNegativePayment, conn))
                    {
                        cmdNeg.Parameters.AddWithValue("@FromMethodID", fromMethodId);
                        cmdNeg.Parameters.AddWithValue("@Amount", -transferAmount);
                        cmdNeg.Parameters.AddWithValue("@Date", DateTime.Now);
                        await cmdNeg.ExecuteNonQueryAsync();
                    }

                    using (SqlCommand cmdPos = new SqlCommand(insertPositivePayment, conn))
                    {
                        cmdPos.Parameters.AddWithValue("@ToMethodID", toMethodId);
                        cmdPos.Parameters.AddWithValue("@Amount", transferAmount);
                        cmdPos.Parameters.AddWithValue("@Date", DateTime.Now);
                        await cmdPos.ExecuteNonQueryAsync();
                    }
                }

                // Update deposits after transfer
                await UpdateDepositsTableAsync();

                MessageBox.Show($"Transferred ${transferAmount:N2} from {cmbTransferFrom.Text} to {cmbTransferTo.Text}.", "Transfer Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadPaidSalesAsync();
                await LoadDepositsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during transfer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateDepositsTableAsync()
        {
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string getPaymentsSumQuery = @"
                    SELECT PaymentMethodID, SUM(Amount) AS TotalAmount
                    FROM Payments
                    GROUP BY PaymentMethodID";

                    var paymentSums = new System.Collections.Generic.Dictionary<int, decimal>();

                    using (SqlCommand sumCmd = new SqlCommand(getPaymentsSumQuery, conn))
                    using (SqlDataReader sumReader = await sumCmd.ExecuteReaderAsync())
                    {
                        while (await sumReader.ReadAsync())
                        {
                            int paymentMethodId = sumReader.GetInt32(0);
                            decimal totalAmount = sumReader.IsDBNull(1) ? 0m : sumReader.GetDecimal(1);
                            paymentSums[paymentMethodId] = totalAmount;
                        }
                    }

                    foreach (var kvp in paymentSums)
                    {
                        string upsertDeposit = @"
                        IF EXISTS (SELECT 1 FROM Deposits WHERE PaymentMethodID = @PaymentMethodID)
                            UPDATE Deposits 
                            SET TotalAmount = @TotalAmount, LastUpdated = @LastUpdated 
                            WHERE PaymentMethodID = @PaymentMethodID
                        ELSE
                            INSERT INTO Deposits (PaymentMethodID, TotalAmount, LastUpdated)
                            VALUES (@PaymentMethodID, @TotalAmount, @LastUpdated)";

                        using (SqlCommand upsertCmd = new SqlCommand(upsertDeposit, conn))
                        {
                            upsertCmd.Parameters.AddWithValue("@PaymentMethodID", kvp.Key);
                            upsertCmd.Parameters.AddWithValue("@TotalAmount", kvp.Value);
                            upsertCmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                            await upsertCmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating deposits: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<decimal> GetTotalDepositAmountByPaymentMethodAsync(int paymentMethodId)
        {
            string query = @"
            SELECT ISNULL(SUM(TotalAmount), 0) FROM Deposits WHERE PaymentMethodID = @PaymentMethodID";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentMethodID", paymentMethodId);
                    object result = await cmd.ExecuteScalarAsync();
                    return result == DBNull.Value ? 0m : Convert.ToDecimal(result);
                }
            }
            catch
            {
                return 0m;
            }
        }
    }
    public class ManageInventoryForm : Form
    {
        private DataGridView dgvInventory;
        private Button btnAddVendor, btnEditVendor, btnDeleteProduct, btnUpdateProduct, btnRefresh, btnCancel;
        private Label lblCOGS, lblOnHandQty, lblOnHandCost;
        private readonly IMS_POS.DatabaseConnection db = new IMS_POS.DatabaseConnection();

        public ManageInventoryForm()
        {
            this.Text = "Inventory and Vendor Manager";
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300, 14);
            this.AutoScroll = true;
            this.BackColor = Color.White;
            this.Size = new Size(1100, 600);

            InitializeControls();
            LoadInventoryData();
        }

        private void InitializeControls()
        {
            dgvInventory = new DataGridView
            {
                Location = new Point(10, 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Size = new Size(1050, 300),
                ReadOnly = true,
                AllowUserToAddRows = false


            };
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.BackgroundColor = Color.White;
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            btnAddVendor = new Button { Text = "Add Vendor", Location = new Point(10, 320), Size = new Size(100, 30) };
            btnEditVendor = new Button { Text = "Edit Vendor", Location = new Point(120, 320), Size = new Size(100, 30) };
            btnDeleteProduct = new Button { Text = "Delete Product", Location = new Point(230, 320), Size = new Size(120, 30) };
            btnUpdateProduct = new Button { Text = "Update Product", Location = new Point(360, 320), Size = new Size(120, 30) };
            btnRefresh = new Button { Text = "Refresh", Location = new Point(490, 320), Size = new Size(100, 30) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(600, 320), Size = new Size(100, 30) };


            lblCOGS = new Label { Text = "Total COGS: $0", Location = new Point(10, 360), AutoSize = true };
            lblOnHandQty = new Label { Text = "Total On-Hand Qty: 0", Location = new Point(10, 390), AutoSize = true };
            lblOnHandCost = new Label { Text = "Total On-Hand Cost: $0", Location = new Point(10, 420), AutoSize = true };

            btnAddVendor.Click += (s, e) => new VendorManagementForm().ShowDialog();
            btnEditVendor.Click += (s, e) => new VendorManagementForm().ShowDialog();
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            btnUpdateProduct.Click += BtnUpdateProduct_Click;
            btnRefresh.Click += (s, e) => LoadInventoryData();
            btnCancel.Click += (s, e) => btnCancel_Click();

            Controls.AddRange(new Control[] {
            dgvInventory, btnAddVendor, btnEditVendor, btnDeleteProduct, btnUpdateProduct, btnRefresh, btnCancel,
            lblCOGS, lblOnHandQty, lblOnHandCost
        });
        }

        private void LoadInventoryData()
        {
            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();

                string query = @"
                SELECT 
                    P.ProductID,
                    P.ProductName,
                    P.Quantity AS PurchasedQty,
                    ISNULL(SUM(S.Quantity), 0) AS SoldQty,
                    (P.Quantity - ISNULL(SUM(S.Quantity), 0)) AS OnHandQty,
                    P.Cost,
                    (P.Cost * (P.Quantity - ISNULL(SUM(S.Quantity), 0))) AS OnHandCost,
                    (P.Cost * ISNULL(SUM(S.Quantity), 0)) AS COGS
                FROM Products P
                LEFT JOIN Sales S ON S.ProductID = P.ProductID
                GROUP BY P.ProductID, P.ProductName, P.Quantity, P.Cost
            ";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvInventory.DataSource = dt;

                decimal totalCOGS = 0, totalOnHandCost = 0;
                int totalQty = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalCOGS += Convert.ToDecimal(row["COGS"]);
                    totalOnHandCost += Convert.ToDecimal(row["OnHandCost"]);
                    totalQty += Convert.ToInt32(row["OnHandQty"]);
                }

                lblCOGS.Text = $"Total COGS: ${totalCOGS}";
                lblOnHandCost.Text = $"Total On-Hand Cost: ${totalOnHandCost}";
                lblOnHandQty.Text = $"Total On-Hand Qty: {totalQty}";
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvInventory.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvInventory.CurrentRow.Cells["ProductID"].Value);
                using (SqlConnection conn = db.GetSqlConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }
                LoadInventoryData();
            }
        }
        private void btnCancel_Click()
        {
            this.Close();
        }

        private void BtnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (dgvInventory.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvInventory.CurrentRow.Cells["ProductID"].Value);
                new ProductUpdateForm(productId).ShowDialog();
                LoadInventoryData();
            }
        }
    }//PRODUCTS  LIST AND MANAGEMENT FORM A 
    public class ProductEditForm : Form
    {
        private DataGridView dgvProductList;
        private TextBox txtProductName, txtCost, txtQuantity, txtVendorID, txtPurchaseDate;
        private Button btnUpdate, btnCancel;
        private int selectedProductId = -1;
        private readonly DatabaseConnection db = new DatabaseConnection();

        public ProductEditForm()
        {
            InitializeForm();
            LoadProductsAsync();
        }

        private void InitializeForm()
        {
            // Form settings
            this.Text = "Product Inventory - Edit Product";
            this.Size = new Size(980, 590);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // DataGridView
            dgvProductList = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(810, 300),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvProductList.SelectionChanged += DgvProductList_SelectionChanged;
            this.Controls.Add(dgvProductList);

            // Labels and TextBoxes positions
            int labelX = 20;
            int textboxX = 150;
            int startY = 320;
            int verticalSpacing = 35;

            // ProductName
            var lblProductName = new Label() { Text = "Product Name:", Location = new Point(labelX, startY), AutoSize = true };
            txtProductName = new TextBox() { Location = new Point(textboxX, startY - 3), Width = 200 };

            // Cost
            var lblCost = new Label() { Text = "Cost:", Location = new Point(labelX, startY + verticalSpacing), AutoSize = true };
            txtCost = new TextBox() { Location = new Point(textboxX, startY + verticalSpacing - 3), Width = 200 };

            // Quantity
            var lblQuantity = new Label() { Text = "Quantity:", Location = new Point(labelX, startY + verticalSpacing * 2), AutoSize = true };
            txtQuantity = new TextBox() { Location = new Point(textboxX, startY + verticalSpacing * 2 - 3), Width = 200 };

            // VendorID
            var lblVendorID = new Label() { Text = "Vendor ID:", Location = new Point(labelX, startY + verticalSpacing * 3), AutoSize = true };
            txtVendorID = new TextBox() { Location = new Point(textboxX, startY + verticalSpacing * 3 - 3), Width = 200 };

            // PurchaseDate
            var lblPurchaseDate = new Label() { Text = "Purchase Date (yyyy-MM-dd):", Location = new Point(labelX, startY + verticalSpacing * 4), AutoSize = true };
            txtPurchaseDate = new TextBox() { Location = new Point(textboxX + 130, startY + verticalSpacing * 4 - 3), Width = 120 };

            // Buttons
            btnUpdate = new Button() { Text = "Update", Location = new Point(150, startY + verticalSpacing * 5 + 5), Width = 100 };
            btnCancel = new Button() { Text = "Cancel", Location = new Point(270, startY + verticalSpacing * 5 + 5), Width = 100 };

            btnUpdate.Click += BtnUpdate_Click;
            btnCancel.Click += (s, e) => this.Close();

            // Add all controls
            this.Controls.AddRange(new Control[]
            {
                lblProductName, txtProductName,
                lblCost, txtCost,
                lblQuantity, txtQuantity,
                lblVendorID, txtVendorID,
                lblPurchaseDate, txtPurchaseDate,
                btnUpdate, btnCancel
            });
        }

        private async void LoadProductsAsync()
        {
            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = "SELECT ProductID, ProductName, Cost, Quantity, VendorID, PurchaseDate FROM Products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvProductList.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void DgvProductList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProductList.SelectedRows[0];
                selectedProductId = Convert.ToInt32(row.Cells["ProductID"].Value);
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtCost.Text = row.Cells["Cost"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
                txtVendorID.Text = row.Cells["VendorID"].Value.ToString();
                txtPurchaseDate.Text = Convert.ToDateTime(row.Cells["PurchaseDate"].Value).ToString("yyyy-MM-dd");
            }
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProductId == -1)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            string productName = txtProductName.Text.Trim();
            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Product Name cannot be empty.");
                return;
            }

            if (!decimal.TryParse(txtCost.Text.Trim(), out decimal cost))
            {
                MessageBox.Show("Invalid Cost value.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Invalid Quantity value.");
                return;
            }

            if (!int.TryParse(txtVendorID.Text.Trim(), out int vendorId))
            {
                MessageBox.Show("Invalid Vendor ID value.");
                return;
            }

            if (!DateTime.TryParse(txtPurchaseDate.Text.Trim(), out DateTime purchaseDate))
            {
                MessageBox.Show("Invalid Purchase Date. Use yyyy-MM-dd format.");
                return;
            }

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string updateQuery = @"
                        UPDATE Products
                        SET ProductName = @productName,
                            Cost = @cost,
                            Quantity = @quantity,
                            VendorID = @vendorId,
                            PurchaseDate = @purchaseDate
                        WHERE ProductID = @productId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productName", productName);
                        cmd.Parameters.AddWithValue("@cost", cost);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@vendorId", vendorId);
                        cmd.Parameters.AddWithValue("@purchaseDate", purchaseDate);
                        cmd.Parameters.AddWithValue("@productId", selectedProductId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully.");
                            LoadProductsAsync(); // Reload grid
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Product not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
        }
        //private async Task<List<string>> GetTableColumnValues(string tableName, string columnName)
        //{
        //    var values = new List<string>();
        //    using (SqlConnection conn = await dbConnection.OpenSqlConnectionAsync())
        //    {
        //        string query = $"SELECT {columnName} FROM {tableName} ORDER BY {columnName}";
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                if (!reader.IsDBNull(0))
        //                    values.Add(reader.GetString(0));
        //            }
        //        }
        //    }
        //    return values;
        //}
    }//PRODUCTS EDITING FORM
    public class ProductUpdateForm : Form
    {
        private int _productId;
        private TextBox txtProductName, txtQuantity, txtCost;
        private ComboBox cmbVendors;
        private Button btnSave, btnCancel;

        private readonly IMS_POS.DatabaseConnection db = new IMS_POS.DatabaseConnection();

        public ProductUpdateForm(int productId)
        {
            _productId = productId;
            InitializeComponents();
            LoadProductData();
            LoadVendors();
        }

        private void InitializeComponents()
        {
            this.Text = "Update Product";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label { Text = "Product Name:", Location = new Point(20, 30), AutoSize = true };
            txtProductName = new TextBox { Location = new Point(150, 30), Width = 200 };

            Label lblQty = new Label { Text = "Quantity:", Location = new Point(20, 70), AutoSize = true };
            txtQuantity = new TextBox { Location = new Point(150, 70), Width = 200 };

            Label lblCost = new Label { Text = "Cost:", Location = new Point(20, 110), AutoSize = true };
            txtCost = new TextBox { Location = new Point(150, 110), Width = 200 };

            Label lblVendor = new Label { Text = "Vendor:", Location = new Point(20, 150), AutoSize = true };
            cmbVendors = new ComboBox { Location = new Point(150, 150), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            btnSave = new Button { Text = "Save", Location = new Point(150, 200), Width = 80 };
            btnCancel = new Button { Text = "Cancel", Location = new Point(270, 200), Width = 80 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblName, txtProductName, lblQty, txtQuantity, lblCost, txtCost, lblVendor, cmbVendors, btnSave, btnCancel });
        }

        private void LoadProductData()
        {
            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();
                string query = "SELECT ProductName, Quantity, Cost, VendorID FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", _productId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtProductName.Text = reader["ProductName"].ToString();
                            txtQuantity.Text = reader["Quantity"].ToString();
                            txtCost.Text = reader["Cost"].ToString();
                            int vendorId = Convert.ToInt32(reader["VendorID"]);
                            cmbVendors.SelectedValue = vendorId;
                        }
                    }
                }
            }
        }

        private void LoadVendors()
        {
            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();
                string query = "SELECT VendorID, VendorName FROM Vendors";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbVendors.DataSource = dt;
                cmbVendors.ValueMember = "VendorID";
                cmbVendors.DisplayMember = "VendorName";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost < 0)
            {
                MessageBox.Show("Cost must be a non-negative decimal.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int vendorId = (int)cmbVendors.SelectedValue;

            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();
                string query = @"
                UPDATE Products SET 
                    ProductName = @ProductName,
                    Quantity = @Quantity,
                    Cost = @Cost,
                    VendorID = @VendorID
                WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Cost", cost);
                    cmd.Parameters.AddWithValue("@VendorID", vendorId);
                    cmd.Parameters.AddWithValue("@ProductID", _productId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }//PRODUCTS UPDATE FORM 
    public class VendorManagementForm : Form
    {
        private int? _vendorId = null; // null means add new
        private TextBox txtVendorName, txtVendorAddress;
        private Button btnSave, btnCancel;

        private readonly IMS_POS.DatabaseConnection db = new IMS_POS.DatabaseConnection();

        public VendorManagementForm()
        {
            InitializeComponents();
        }

        public VendorManagementForm(int vendorId) : this()
        {
            _vendorId = vendorId;
            LoadVendorData();
        }

        private void InitializeComponents()
        {
            this.Text = _vendorId.HasValue ? "Edit Vendor" : "Add Vendor";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label { Text = "Vendor Name:", Location = new Point(20, 30), AutoSize = true };
            txtVendorName = new TextBox { Location = new Point(150, 30), Width = 200 };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, 70), AutoSize = true };
            txtVendorAddress = new TextBox { Location = new Point(150, 70), Width = 200 };

            btnSave = new Button { Text = "Save", Location = new Point(150, 130), Width = 80 };
            btnCancel = new Button { Text = "Cancel", Location = new Point(270, 130), Width = 80 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblName, txtVendorName, lblAddress, txtVendorAddress, btnSave, btnCancel });
        }

        private void LoadVendorData()
        {
            if (!_vendorId.HasValue) return;

            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();
                string query = "SELECT VendorName, VendorAddress FROM Vendors WHERE VendorID = @VendorID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@VendorID", _vendorId.Value);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtVendorName.Text = reader["VendorName"].ToString();
                            txtVendorAddress.Text = reader["VendorAddress"].ToString();
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVendorName.Text))
            {
                MessageBox.Show("Vendor Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string vendorName = txtVendorName.Text.Trim();
            string vendorAddress = txtVendorAddress.Text.Trim();

            using (SqlConnection conn = db.GetSqlConnection())
            {
                conn.Open();

                SqlCommand cmd;

                if (_vendorId.HasValue)
                {
                    // Update existing
                    string updateQuery = @"
                    UPDATE Vendors SET 
                        VendorName = @VendorName,
                        VendorAddress = @VendorAddress
                    WHERE VendorID = @VendorID";

                    cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@VendorID", _vendorId.Value);
                }
                else
                {
                    // Insert new
                    string insertQuery = @"
                    INSERT INTO Vendors (VendorName, VendorAddress) 
                    VALUES (@VendorName, @VendorAddress)";
                    cmd = new SqlCommand(insertQuery, conn);
                }

                cmd.Parameters.AddWithValue("@VendorName", vendorName);
                cmd.Parameters.AddWithValue("@VendorAddress", vendorAddress);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show(_vendorId.HasValue ? "Vendor updated successfully!" : "Vendor added successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Operation failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }//VENDOR MANAGEMENT FORM
    public class DynamicDeleteProductForm : Form
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=FinGradProj_DB;Integrated Security=True";
        private DataGridView dataGridViewProducts;
        private Button btnConfirm, btnCancel;

        public DynamicDeleteProductForm()
        {
            InitializeDynamicForm();
            LoadProducts();
        }

        private void InitializeDynamicForm()
        {
            // Form properties
            this.Text = "Delete Product";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dataGridViewProducts = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(740, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            };

            // Confirm Button
            btnConfirm = new Button
            {
                Text = "Confirm",
                Location = new Point(540, 400),
                Size = new Size(100, 30),
            };
            btnConfirm.Click += BtnConfirm_Click;

            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(660, 400),
                Size = new Size(100, 30),
            };
            btnCancel.Click += BtnCancel_Click;

            // Add controls to the form
            this.Controls.Add(dataGridViewProducts);
            this.Controls.Add(btnConfirm);
            this.Controls.Add(btnCancel);
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    ProductID, 
                    ProductName, 
                    Cost, 
                    Quantity, 
                    VendorID, 
                    PurchaseDate 
                FROM Products";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable productsTable = new DataTable();
                adapter.Fill(productsTable);

                dataGridViewProducts.DataSource = productsTable;

                // Hide ProductID column
                if (dataGridViewProducts.Columns["ProductID"] != null)
                {
                    dataGridViewProducts.Columns["ProductID"].Visible = false;
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            int selectedProductId = Convert.ToInt32(dataGridViewProducts.SelectedRows[0].Cells["ProductID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if product is related to the Sales table
                string checkQuery = "SELECT COUNT(*) FROM Sales WHERE ProductID = @ProductID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ProductID", selectedProductId);
                    int relatedCount = (int)checkCmd.ExecuteScalar();

                    if (relatedCount > 0)
                    {
                        MessageBox.Show("This product cannot be deleted as it is related to sales transactions.");
                        return;
                    }
                }

                // Delete the product
                string deleteQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@ProductID", selectedProductId);
                    deleteCmd.ExecuteNonQuery();
                    MessageBox.Show("Product deleted successfully.");
                }

                // Refresh the product list
                LoadProducts();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new DynamicDeleteProductForm());
        //}
    }
    public class AccountsBalanceForm : Form
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=FinGradProj_DB;Integrated Security=True";

        private DataGridView dataGridViewBalances;
        private Button btnPrint;
        private Button btnRefresh;
        private PrintDocument printDocument;
        private DataTable balanceTable;

        public AccountsBalanceForm()
        {
            InitializeDynamicForm();
            _ = LoadBalancesAsync();
        }

        private void InitializeDynamicForm()
        {
            this.Text = "Accounts Balances with Vendors";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridViewBalances = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(840, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            };
            dataGridViewBalances.BackgroundColor = Color.White;
            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(640, 440),
                Size = new Size(100, 30),
            };
            btnRefresh.Click += async (s, e) => await LoadBalancesAsync();

            btnPrint = new Button
            {
                Text = "Print",
                Location = new Point(760, 440),
                Size = new Size(100, 30),
            };
            btnPrint.Click += BtnPrint_Click;

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            this.Controls.Add(dataGridViewBalances);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnPrint);
        }

        private async Task LoadBalancesAsync()
        {
            balanceTable = new DataTable();
            balanceTable.Columns.Add("VendorName", typeof(string));
            balanceTable.Columns.Add("PaymentMethodName", typeof(string));
            balanceTable.Columns.Add("TotalDeposits", typeof(decimal));
            balanceTable.Columns.Add("TotalPayments", typeof(decimal));
            balanceTable.Columns.Add("TotalRefunds", typeof(decimal));
            balanceTable.Columns.Add("NetBalance", typeof(decimal));

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"
                SELECT 
                    v.VendorName,
                    pm.MethodName AS PaymentMethodName,
                    ISNULL(d.TotalDeposits, 0) AS TotalDeposits,
                    ISNULL(p.TotalPayments, 0) AS TotalPayments,
                    ISNULL(r.TotalRefunds, 0) AS TotalRefunds,
                    ISNULL(d.TotalDeposits, 0) - ISNULL(p.TotalPayments, 0) - ISNULL(r.TotalRefunds, 0) AS NetBalance
                FROM 
                    Vendors v
                CROSS JOIN 
                    PaymentMethods pm
                LEFT JOIN
                (
                    SELECT VendorID, PaymentMethodID, SUM(TotalAmount) AS TotalDeposits
                    FROM Deposits
                    GROUP BY VendorID, PaymentMethodID
                ) d ON d.VendorID = v.VendorID AND d.PaymentMethodID = pm.PaymentMethodID
                LEFT JOIN
                (
                    SELECT PaymentMethodID, SUM(Amount) AS TotalPayments
                    FROM Payments
                    GROUP BY PaymentMethodID
                ) p ON p.PaymentMethodID = pm.PaymentMethodID
                LEFT JOIN
                (
                    SELECT pm.PaymentMethodID, SUM(r.RefundAmount) AS TotalRefunds
                    FROM Refunds r
                    INNER JOIN PaymentMethods pm ON r.RefundMethod = pm.MethodName
                    GROUP BY pm.PaymentMethodID
                ) r ON r.PaymentMethodID = pm.PaymentMethodID
                WHERE
                    ISNULL(d.TotalDeposits, 0) + ISNULL(p.TotalPayments, 0) + ISNULL(r.TotalRefunds, 0) > 0
                ORDER BY v.VendorName, pm.MethodName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(balanceTable);
                    }

                    dataGridViewBalances.DataSource = balanceTable;

                    // Format currency columns
                    foreach (var colName in new[] { "TotalDeposits", "TotalPayments", "TotalRefunds", "NetBalance" })
                    {
                        if (dataGridViewBalances.Columns.Contains(colName))
                            dataGridViewBalances.Columns[colName].DefaultCellStyle.Format = "C2";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading balances: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            using (PrintPreviewDialog previewDialog = new PrintPreviewDialog())
            {
                previewDialog.Document = printDocument;
                previewDialog.Width = 800;
                previewDialog.Height = 600;
                previewDialog.ShowDialog();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int startX = 10, startY = 10;
            int offsetY = 30;

            graphics.DrawString("Account Balances with Vendors", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, startX, startY);
            offsetY += 40;

            if (balanceTable == null || balanceTable.Rows.Count == 0)
            {
                graphics.DrawString("No data available.", new Font("Arial", 12), Brushes.Black, startX, startY + offsetY);
                return;
            }

            // Print header row
            string header = $"Vendor               Payment Method       Deposits     Payments    Refunds    Net Balance";
            graphics.DrawString(header, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, startX, startY + offsetY);
            offsetY += 25;

            foreach (DataRow row in balanceTable.Rows)
            {
                string line = string.Format("{0,-20}  {1,-18}  {2,10:C2}  {3,10:C2}  {4,10:C2}  {5,12:C2}",
                    TruncateString(row.Field<string>("VendorName"), 20),
                    TruncateString(row.Field<string>("PaymentMethodName"), 18),
                    row.Field<decimal>("TotalDeposits"),
                    row.Field<decimal>("TotalPayments"),
                    row.Field<decimal>("TotalRefunds"),
                    row.Field<decimal>("NetBalance"));

                graphics.DrawString(line, new Font("Consolas", 10), Brushes.Black, startX, startY + offsetY);
                offsetY += 20;

                if (offsetY > e.MarginBounds.Height - 40)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }

        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }
    }
    public class DepositsForm : Form
    {
        private DataGridView dgvDeposits;
        private Label lblCashTotal, lblCardTotal, lblBankTotal;
        private Button btnPrintReport, btnRefresh;
        private readonly DatabaseConnection db = new DatabaseConnection();

        public DepositsForm()
        {
            InitializeForm();
            InitializeControls();
            LoadDepositsAsync();
        }

        private void InitializeForm()
        {
            this.Text = "Deposits Summary";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void InitializeControls()
        {
            dgvDeposits = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(900, 300),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };
            dgvDeposits.BackgroundColor = Color.White;
            lblCashTotal = new Label
            {
                Text = "Cash Total: $0.00",
                Location = new Point(20, 330),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblCardTotal = new Label
            {
                Text = "Card Total: $0.00",
                Location = new Point(20, 360),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblBankTotal = new Label
            {
                Text = "Bank Total: $0.00",
                Location = new Point(20, 390),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnPrintReport = new Button
            {
                Text = "Print Report",
                Location = new Point(20, 440),
                Size = new Size(120, 30),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };
            btnPrintReport.Click += BtnPrintReport_Click;

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(160, 440),
                Size = new Size(120, 30),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };
            btnRefresh.Click += (s, e) => LoadDepositsAsync();

            this.Controls.AddRange(new Control[] { dgvDeposits, lblCashTotal, lblCardTotal, lblBankTotal, btnPrintReport, btnRefresh });
        }

        private async void LoadDepositsAsync()
        {
            const string query = @"
                SELECT 
                    pm.MethodName AS PaymentMethod,
                    d.TotalAmount,
                    d.LastUpdated
                FROM Deposits d
                INNER JOIN PaymentMethods pm ON d.PaymentMethodID = pm.PaymentMethodID";

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvDeposits.DataSource = dt;

                    decimal cashTotal = 0m, cardTotal = 0m, bankTotal = 0m;

                    foreach (DataRow row in dt.Rows)
                    {
                        string paymentMethod = row["PaymentMethod"].ToString().ToLower();
                        decimal amount = 0m;
                        if (row["TotalAmount"] != DBNull.Value)
                            amount = Convert.ToDecimal(row["TotalAmount"]);

                        switch (paymentMethod)
                        {
                            case "cash":
                                cashTotal += amount;
                                break;
                            case "card":
                                cardTotal += amount;
                                break;
                            case "bank":
                                bankTotal += amount;
                                break;
                        }
                    }

                    lblCashTotal.Text = $"Cash Total: ${cashTotal:N2}";
                    lblCardTotal.Text = $"Card Total: ${cardTotal:N2}";
                    lblBankTotal.Text = $"Bank Total: ${bankTotal:N2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading deposits: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintReport_Click(object sender, EventArgs e)
        {
            DataTable dt = dgvDeposits.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reportHeader = "Deposits Summary Report\n" + DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
            using (var reportForm = new PrintReportForm(dt, reportHeader))
            {
                reportForm.ShowDialog();
            }
        }
    }


    public class ForgotPasswordForm : Form
        {
            private TextBox txtUsername;
            private TextBox txtNewPassword;
            private Button btnResetPassword;
            private Label lblStatus;
            private Label lblTitle;

            private readonly string connectionString = "Data Source=.;Initial Catalog=FinProjGrad_DB;Integrated Security=True"; // Adjust if needed

            public ForgotPasswordForm()
            {
                InitializeDynamicForm();
            }

            private void InitializeDynamicForm()
            {
                this.Text = "Forgot Password";
                this.Size = new Size(400, 300);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;

                lblTitle = new Label()
                {
                    Text = "Reset Your Password",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(100, 20)
                };
                this.Controls.Add(lblTitle);

                var lblUsername = new Label()
                {
                    Text = "Username:",
                    Location = new Point(40, 70),
                    AutoSize = true
                };
                this.Controls.Add(lblUsername);

                txtUsername = new TextBox()
                {
                    Location = new Point(140, 65),
                    Width = 200
                };
                this.Controls.Add(txtUsername);

                var lblNewPassword = new Label()
                {
                    Text = "New Password:",
                    Location = new Point(40, 110),
                    AutoSize = true
                };
                this.Controls.Add(lblNewPassword);

                txtNewPassword = new TextBox()
                {
                    Location = new Point(140, 105),
                    Width = 200,
                    UseSystemPasswordChar = true
                };
                this.Controls.Add(txtNewPassword);

                btnResetPassword = new Button()
                {
                    Text = "Reset Password",
                    Location = new Point(140, 150),
                    Width = 120,
                    Height = 30
                };
                btnResetPassword.Click += BtnResetPassword_Click;
                this.Controls.Add(btnResetPassword);

                lblStatus = new Label()
                {
                    ForeColor = Color.DarkRed,
                    AutoSize = true,
                    Location = new Point(40, 200)
                };
                this.Controls.Add(lblStatus);
            }

            private void BtnResetPassword_Click(object sender, EventArgs e)
            {
                string username = txtUsername.Text.Trim();
                string newPassword = txtNewPassword.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(newPassword))
                {
                    lblStatus.Text = "Please fill in all fields.";
                    return;
                }

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        //string hashedPassword = HashPassword(newPassword);
                        string query = "UPDATE Users SET Password = @Password WHERE Username = @Username";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            //cmd.Parameters.AddWithValue("@Password", hashedPassword);
                            cmd.Parameters.AddWithValue("@Username", username);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "✅ Password reset successfully!";
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "⚠️ Username not found.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Error: " + ex.Message;
                }
            }

      
    }
        public static class FormResizer
    {
        private static Size originalSize;

        public static void Register(Form form)
        {
            originalSize = form.Size;

            form.Resize += (s, e) =>
            {
                float scaleX = (float)form.Width / originalSize.Width;
                float scaleY = (float)form.Height / originalSize.Height;

                ScaleControls(form.Controls, scaleX, scaleY);
            };
        }

        private static void ScaleControls(Control.ControlCollection controls, float scaleX, float scaleY)
        {
            foreach (Control control in controls)
            {
                var oldFont = control.Font;
                control.Width = (int)(control.Width * scaleX);
                control.Height = (int)(control.Height * scaleY);
                control.Left = (int)(control.Left * scaleX);
                control.Top = (int)(control.Top * scaleY);

                control.Font = new Font(oldFont.FontFamily, oldFont.Size * scaleY, oldFont.Style);

                if (control.HasChildren)
                    ScaleControls(control.Controls, scaleX, scaleY);
            }
        }
    }

    public static class DynamicFormHelper
    {
        public static void OpenInsidePanel(Form containerForm, Form childForm, Panel targetPanel)
        {
            try
            {
                targetPanel.Padding = new Padding(8);
                targetPanel.Controls.Clear();

                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                targetPanel.Controls.Add(childForm);
                childForm.Show();
                childForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message);
            }
        }
    }



}
