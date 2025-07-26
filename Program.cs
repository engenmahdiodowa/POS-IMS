// Program.cs
using System;
using System.Windows.Forms;
using IMS_POS.Models;
using ClosedXML.Excel;

namespace IMS_POS
{
    internal static class Program
    {
        public static User CurrentUser { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show splash screen
            using (SplashForm splash = new SplashForm())
            {
                splash.Show();
                Application.DoEvents(); // Allow UI to render
                System.Threading.Thread.Sleep(3000); // Duration matches SplashForm timer
            }

            // Show login form
            using (var loginForm = new IMS_LoginForm())
            {
                Application.Run(loginForm);
            }

            // Proceed if login successful
            if (CurrentUser != null)
            {
                Application.Run(new IMS_SaleForm(CurrentUser));
                CreateExcelFile();
            }
            else
            {
                Application.Exit();
            }
        }

        static void CreateExcelFile()
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    worksheet.Cell("A1").Value = "Hello, World!";
                    workbook.SaveAs("Output.xlsx");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excel export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
