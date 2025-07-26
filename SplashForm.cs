using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS_POS
{
    public partial class SplashForm : Form
    {
        private Timer splashTimer;

        public SplashForm()
        {
            InitializeComponent();
            Timersplash=new Timer();
            Timersplash.Tick += Timersplash_Tick;
            Timersplash.Interval = 100;
            splashProgBar.Step = 2;
            Timersplash.Start();
        }

        private void Timersplash_Tick(object sender, EventArgs e)
        {
            //splashProgBar.PerformStep();

            if (splashProgBar.Value >= splashProgBar.Maximum)
            {
                splashTimer.Stop();           // ✅ Stop timer
                this.Hide();                  // Hide splash
                using (IMS_LoginForm loginForm = new IMS_LoginForm())
                {
                    loginForm.ShowDialog();   // Show login form once
                }
                this.Close();                 // Close splash after login
            }
        }
    }
}
