using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace BeautySalon
{
    public partial class Stuff : Form
    {
        int Id = 0;

        private Size _initialFormSize;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        public Stuff()
        {
            InitializeComponent();
            textBox5.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox2.Visible = false;

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
            label9.Font = myFont;


            byte[] fontData1 = Properties.Resources.midium; // Измените "YourFontFile" на имя вашего файла ресурса шрифта
            IntPtr fontPtr1 = Marshal.AllocCoTaskMem(fontData1.Length);
            Marshal.Copy(fontData1, 0, fontPtr1, fontData1.Length);
            uint dummy1 = 0;
            fonts.AddMemoryFont(fontPtr1, fontData1.Length);
            AddFontMemResourceEx(fontPtr1, (uint)fontData1.Length, IntPtr.Zero, out dummy1);
            Marshal.FreeCoTaskMem(fontPtr1);
            Font myFont1 = new Font(fonts.Families[0], 10.0F);
            groupBox1.Font = myFont1;
            groupBox2.Font = myFont1;
            groupBox3.Font = myFont1;
            label1.Font = myFont1;
            label2.Font = myFont1;
            label3.Font = myFont1;
            label4.Font = myFont1;
            label5.Font = myFont1;
            label6.Font = myFont1;
            label7.Font = myFont1;
            label8.Font = myFont1;
            button1.Font = myFont1;
            button2.Font = myFont1;
            button3.Font = myFont1;
            button4.Font = myFont1;
        }

        public Boolean chekUser()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            DataTable dataTable = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Stuff where Логин = '" + textBox4.Text + "'", ProjectConnection.sqlConn);
            ad.SelectCommand = sqlCommand;
            ad.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такой логин уже существует",
                "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }

        }
        public Boolean chekPhone()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            DataTable dataTable = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Stuff where [Номер телефона] = '" + maskedTextBox1.Text + "'", ProjectConnection.sqlConn);
            ad.SelectCommand = sqlCommand;
            ad.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такой номер уже существует",
                "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }

        }

        public Boolean chekPassport()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            DataTable dataTable = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Stuff where Паспорт = '" + maskedTextBox2.Text + "'", ProjectConnection.sqlConn);
            ad.SelectCommand = sqlCommand;
            ad.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такой паспорт уже существует",
                "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }

        }

        private void ClearControls()
        {
            Id = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            maskedTextBox1.Text = "";
            comboBox1.Text = "";
            maskedTextBox2.Text = "";
            pictureBox1.Image = null;
        }

        private void populate()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            ProjectConnection.sqlConn.Open();
            string Myquary = "select * from Stuff";
            SqlCommand cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView2.DataSource = ds;
            dataGridView2.Columns["Фото"].Visible = false;
            ProjectConnection.sqlConn.Close();
        }

        private void populatePost()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            ProjectConnection.sqlConn.Open();
            string Myquary = "select * from Posts";
            SqlCommand cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable ds = new DataTable();
            da.Fill(ds);
            comboBox1.DisplayMember = "Название";
            comboBox1.DataSource = ds;
            comboBox1.SelectedIndex = -1;
            ProjectConnection.sqlConn.Close();
        }
        public byte[] imageToByteArray(Image imageIn)//конвертировать картинку в массив байт
        {
            using (var ms = new MemoryStream())//инициализация потока памяти(буфер обмена условно)
            {
                imageIn.Save(ms, imageIn.RawFormat);//сохранить картинку
                return ms.ToArray();//вернуть массив байт
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                Id = Convert.ToInt32(row.Cells[0].Value.ToString());
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                textBox5.Text = row.Cells[5].Value.ToString();
                maskedTextBox1.Text = row.Cells[6].Value.ToString();
                comboBox1.Text = row.Cells[7].Value.ToString();
                maskedTextBox2.Text = row.Cells[8].Value.ToString();
                Byte[] picture = (Byte[])row.Cells[9].Value;
                MemoryStream ms = new MemoryStream(picture);
                pictureBox1.Image = Image.FromStream(ms);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text)
                || string.IsNullOrWhiteSpace(textBox5.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox2.Text)
                || pictureBox1.Image == null)
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (chekUser())
            {
                return;
            }
            if (chekPhone())
            {
                return;
            }
            if (chekPassport())
            {
                return;
            }

            if (textBox1.Text != ""
              || textBox2.Text != ""
              || textBox3.Text != ""
              || textBox4.Text != ""
              || textBox5.Text != ""
              || maskedTextBox1.Text != ""
              || comboBox1.Text != ""
              || maskedTextBox2.Text != ""
              || pictureBox1.Image != null)
            {
                ProjectConnection NewConnection = new ProjectConnection();
                NewConnection.Connection_Today();
                SqlCommand com = new SqlCommand("INSERT INTO Stuff (Фамилия, Имя, Отчество, Логин, Пароль, [Номер телефона], Должность, Паспорт, Фото)" +
                    " VALUES (@fName, @sName, @tName, @Login, @Pass, @Phone, @Post, @Passport, @Foto)", ProjectConnection.sqlConn);
                ProjectConnection.sqlConn.Open();
                com.Parameters.AddWithValue("@fName", textBox1.Text);
                com.Parameters.AddWithValue("@sName", textBox2.Text);
                com.Parameters.AddWithValue("@tName", textBox3.Text);
                com.Parameters.AddWithValue("@Login", textBox4.Text);
                com.Parameters.AddWithValue("@Pass", textBox5.Text);
                com.Parameters.AddWithValue("@Phone", maskedTextBox1.Text);
                com.Parameters.AddWithValue("@Post", comboBox1.Text);
                com.Parameters.AddWithValue("@Passport", maskedTextBox2.Text);
                com.Parameters.Add("@Foto", SqlDbType.Image).Value = imageToByteArray(pictureBox1.Image);
                com.ExecuteNonQuery();
                ProjectConnection.sqlConn.Close();
                populate();
                ClearControls();
                MessageBox.Show("Данные успешно добавлены", "Добавление",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text)
                || string.IsNullOrWhiteSpace(textBox5.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox2.Text)
                || pictureBox1.Image == null)
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Вы хотите отредактировать cсотрудника?",
            "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                if (Id != 0
                    || textBox1.Text != ""
                    || textBox2.Text != ""
                    || textBox3.Text != ""
                    || textBox4.Text != ""
                    || textBox5.Text != ""
                    || maskedTextBox1.Text != ""
                    || comboBox1.Text != ""
                    || maskedTextBox2.Text != ""
                    || pictureBox1.Image != null)
                {
                    ProjectConnection NewConnection = new ProjectConnection();
                    NewConnection.Connection_Today();
                    SqlCommand com = new SqlCommand("UPDATE Stuff set Фамилия = @fName, Имя = @sName, Отчество = @tName, Логин = @Login, Пароль = @Pass,[Номер телефона] = @Phone, Должность = @Post, Паспорт = @Passport, Фото = @Photo Where Id = @Id", ProjectConnection.sqlConn);
                    ProjectConnection.sqlConn.Open();
                    com.Parameters.AddWithValue("@Id", Id);
                    com.Parameters.AddWithValue("@fName", textBox1.Text);
                    com.Parameters.AddWithValue("@sName", textBox2.Text);
                    com.Parameters.AddWithValue("@tName", textBox3.Text);
                    com.Parameters.AddWithValue("@Login", textBox4.Text);
                    com.Parameters.AddWithValue("@Pass", textBox5.Text);
                    com.Parameters.AddWithValue("@Phone", maskedTextBox1.Text);
                    com.Parameters.AddWithValue("@Post", comboBox1.Text);
                    com.Parameters.AddWithValue("@Passport", maskedTextBox2.Text);
                    com.Parameters.Add("@Photo", SqlDbType.Image).Value = imageToByteArray(pictureBox1.Image);
                    com.ExecuteNonQuery();
                    ProjectConnection.sqlConn.Close();
                    populate();
                    ClearControls();
                    MessageBox.Show("Данные успешно редактированы", "Редактирование",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Произошла ошибка");
                }
            }
            if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text)
                || string.IsNullOrWhiteSpace(textBox5.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox2.Text)
                || pictureBox1.Image == null)
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Вы хотите удалить запиcь?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                if (Id != 0)
                {
                    ProjectConnection NewConnection = new ProjectConnection();
                    NewConnection.Connection_Today();
                    ProjectConnection.sqlConn.Open();
                    SqlCommand command = new SqlCommand("DELETE Stuff where Id = @Id", ProjectConnection.sqlConn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    ProjectConnection.sqlConn.Close();
                    populate();
                    ClearControls();
                    MessageBox.Show("Вы успешно удалили запись",
                    "Удаление", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
            }
            if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);//установить картинку из файла
            }
        }

        private void Stuff_Load(object sender, EventArgs e)
        {
            populate();
            populatePost();
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

        private void Stuff_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                maskedTextBox1.Select(0, 0);
            });
        }

        private void maskedTextBox2_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                maskedTextBox2.Select(0, 0);
            });
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
            if (!string.IsNullOrEmpty(textBox8.Text))
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToUpper().Contains(textBox8.Text.ToUpper()))
                        {
                            row.Selected = true;
                            dataGridView2.CurrentCell = cell;
                            break;
                        }
                    }
                    if (dataGridView2.SelectedCells.Count > 0)
                    {
                        break;
                    }
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                e.Value = new String('*', e.Value.ToString().Length);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox5.UseSystemPasswordChar = true;

            pictureBox3.Visible = true;
            pictureBox2.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox5.UseSystemPasswordChar = false;

            pictureBox3.Visible = false;
            pictureBox2.Visible = true;
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
