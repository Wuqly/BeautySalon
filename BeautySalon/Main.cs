using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeautySalon
{
    public partial class Main : Form
    {
        private Size _initialFormSize;
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();
        public Main()
        {
            InitializeComponent();
            _initialFormSize = this.Size;
            // Загрузка шрифта
            byte[] fontData = Properties.Resources.midium; // Измените "YourFontFile" на имя вашего файла ресурса шрифта
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, fontData.Length);
            AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, out dummy);
            Marshal.FreeCoTaskMem(fontPtr);
            Font myFont = new Font(fonts.Families[0], 14.0F);
            menuStrip2.Font = myFont;
            label3.Font = myFont;


            byte[] fontData1 = Properties.Resources.Vigran; // Измените "YourFontFile" на имя вашего файла ресурса шрифта
            IntPtr fontPtr1 = Marshal.AllocCoTaskMem(fontData1.Length);
            Marshal.Copy(fontData1, 0, fontPtr1, fontData1.Length);
            uint dummy1 = 0;
            fonts.AddMemoryFont(fontPtr1, fontData1.Length);
            AddFontMemResourceEx(fontPtr1, (uint)fontData1.Length, IntPtr.Zero, out dummy1);
            Marshal.FreeCoTaskMem(fontPtr1);
            Font myFont1 = new Font(fonts.Families[0], 16.0F);
            label1.Font = myFont1;

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Sign s = new Sign();
            this.Hide();
            s.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Main M = new Main();
            this.Hide();
            M.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Records R = new Records();
            this.Hide();
            R.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Service Serv = new Service();
            this.Hide();
            Serv.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Client Client = new Client();
            this.Hide();
            Client.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Stuff Stuff = new Stuff();
            this.Hide();
            Stuff.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Product Product = new Product();
            this.Hide();
            Product.Show();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            ServicePayment SP = new ServicePayment();
            this.Hide();
            SP.Show();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            ProductPayment PP = new ProductPayment();
            this.Hide();
            PP.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (MyConnection.type == "M")
            {
                toolStripMenuItem7.Visible = false;
            }

            if (MyConnection.type == "K")
            {
                toolStripMenuItem7.Visible = false;
                toolStripMenuItem4.Visible = false;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Sign s = new Sign();
            this.Hide();
            s.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.Size = _initialFormSize;
                this.CenterToScreen();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
