using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IMS_POS.Models;

namespace IMS_POS
{
    public partial class IMS_LoginForm : Form
    {
        private readonly DatabaseConnection db = new DatabaseConnection();

        public IMS_LoginForm()
        {
            InitializeComponent();
        }

        private async void IMS_LoginForm_Load(object sender, EventArgs e)
        {
            await LoadRolesFromDatabaseAsync();
            txtUsername.Focus();
        }

        private async Task LoadRolesFromDatabaseAsync()
        {
            cmbRole.Items.Clear();

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = "SELECT DISTINCT Role FROM Users WHERE Role IS NOT NULL AND Role <> ''";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string role = reader["Role"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(role))
                                cmbRole.Items.Add(role);
                        }
                    }
                }

                if (cmbRole.Items.Count > 0)
                    cmbRole.SelectedIndex = 0;
                else
                    MessageBox.Show("No roles found in the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles from database:\n" + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string rawPassword = txtPassword.Text.Trim();
            string selectedRole = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(rawPassword) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please enter username, password, and select a role.");
                return;
            }

            string hashedPassword = ComputeSha256Hex(rawPassword);

            try
            {
                using (SqlConnection conn = await db.OpenSqlConnectionAsync())
                {
                    string query = @"
                        SELECT Username, Role, FullName 
                        FROM Users 
                        WHERE Username = @username AND Password = @password AND Role = @role";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@role", selectedRole);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Login success - save user
                                Program.CurrentUser = new User
                                {
                                    Username = reader["Username"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    FullName = reader["FullName"].ToString()
                                };

                                // Open main form based on role (example: SaleForm)
                                IMS_SaleForm saleForm = new IMS_SaleForm(Program.CurrentUser);
                                saleForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username, password, or role.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ComputeSha256Hex(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));  // Hex format

                return builder.ToString();
            }
        }
    }
}
