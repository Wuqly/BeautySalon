using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
    public partial class Sign : Form
    {
        SqlConnection sqlConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Aarone\\Desktop\\BeautySalon\\BeautySalonDb.mdf;Integrated Security=True;Connect Timeout=30");
        int Id = 0;
        private Size _initialFormSize;
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        public Sign()
        {
            InitializeComponent();
            // Загрузка шрифта
            byte[] fontData = Properties.Resources.midium; // Измените "YourFontFile" на имя вашего файла ресурса шрифта
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, fontData.Length);
            AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, out dummy);
            Marshal.FreeCoTaskMem(fontPtr);
            Font myFont = new Font(fonts.Families[0], 14.0F);


            label2.Font = myFont;
            label1.Font = myFont;
            label3.Font = myFont;
            button1.Font = myFont;


            textBox2.UseSystemPasswordChar = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
            _initialFormSize = this.Size;

        }



        private void Sign_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Close();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Вход в программу не совершен",
                    "Введите логин", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Вход в программу не совершен",
                    "Введите пароль", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var login = textBox1.Text;
            var password = textBox2.Text;
            sqlConn.Open();
            string query = $"SELECT * FROM Stuff where Логин = '{login}' and Пароль = '{password}'";
            SqlDataAdapter ad = new SqlDataAdapter(query, sqlConn);
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand($"select * from Stuff where Логин = '{login}' and Пароль = '{password}'", sqlConn);
            ad.Fill(dataTable);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader[7].ToString() == "Администратор")
                {
                    MyConnection.type = "A";
                }
                if (reader[7].ToString() == "Менеджер")
                {
                    MyConnection.type = "M";
                }
                if (reader[7].ToString() == "Кассир")
                {
                    MyConnection.type = "K";
                }
                Main Main = new Main();
                Main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Пользователь с такими данными не зарегистрирован или неверный логин или пароль",
                "Вход в программу не совершен", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlConn.Close();
        }
    }
}
