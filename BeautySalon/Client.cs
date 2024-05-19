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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace BeautySalon
{
    public partial class Client : Form
    {
        int Id = 0;
        private Size _initialFormSize;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        public Client()
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
            label1.Font = myFont;


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
            label3.Font = myFont1;
            label5.Font = myFont1;
            label6.Font = myFont1;
            label7.Font = myFont1;
            label8.Font = myFont1;
            button1.Font = myFont1;
            button2.Font = myFont1;
            button3.Font = myFont1;
        }

        private void Client_Load(object sender, EventArgs e)
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
            populate();
        }

        private Boolean chekPhone()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            DataTable dataTable = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Clients where [Номер телефона] = '" + maskedTextBox1.Text + "'", ProjectConnection.sqlConn);
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

        private void populate()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            ProjectConnection.sqlConn.Open();
            string Myquary = "select * from Clients";
            SqlCommand cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView2.DataSource = ds;
            ProjectConnection.sqlConn.Close();
        }

        private void ClearControls()
        {
            Id = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            maskedTextBox1.Text = "";
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(textBox1.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (chekPhone())
            {
                return;
            }

            if (textBox1.Text != ""
              || textBox2.Text != ""
              || textBox3.Text != ""
              || maskedTextBox1.Text != ""
              || textBox4.Text != "")
            {
                ProjectConnection NewConnection = new ProjectConnection();
                NewConnection.Connection_Today();
                SqlCommand com = new SqlCommand("INSERT INTO Clients (Фамилия, Имя, Отчество, [Номер телефона], [Скидка (%)])" +
                    "VALUES (@fName, @sName, @tName, @Phone, @Sail)", ProjectConnection.sqlConn);
                ProjectConnection.sqlConn.Open();
                com.Parameters.AddWithValue("@fName", textBox1.Text);
                com.Parameters.AddWithValue("@sName", textBox2.Text);
                com.Parameters.AddWithValue("@tName", textBox3.Text);
                com.Parameters.AddWithValue("@Phone", maskedTextBox1.Text);
                com.Parameters.AddWithValue("@Sail", textBox4.Text);
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
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (Id != 0
              || textBox1.Text != ""
              || textBox2.Text != ""
              || textBox3.Text != ""
              || maskedTextBox1.Text != ""
              || textBox4.Text != "")
            {
                ProjectConnection NewConnection = new ProjectConnection();
                NewConnection.Connection_Today();
                SqlCommand com = new SqlCommand("UPDATE Clients set Фамилия = @fName, Имя = @sName, Отчество = @tName, [Номер телефона] = @Phone, [Скидка (%)] = @Sail Where Id = @Id", ProjectConnection.sqlConn);
                ProjectConnection.sqlConn.Open();
                com.Parameters.AddWithValue("@Id", Id);
                com.Parameters.AddWithValue("@fName", textBox1.Text);
                com.Parameters.AddWithValue("@sName", textBox2.Text);
                com.Parameters.AddWithValue("@tName", textBox3.Text);
                com.Parameters.AddWithValue("@Phone", maskedTextBox1.Text);
                com.Parameters.AddWithValue("@Sail", textBox4.Text);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text)
                || string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(maskedTextBox1.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text))
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
                    SqlCommand command = new SqlCommand("DELETE Clients where Id = @Id", ProjectConnection.sqlConn);
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

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                Id = Convert.ToInt32(row.Cells[0].Value.ToString());
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                maskedTextBox1.Text = row.Cells[4].Value.ToString();
                textBox4.Text = row.Cells[5].Value.ToString();

            }
        }


        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                maskedTextBox1.Select(0, 0);
            });
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

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
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

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                var selectionStart = textBox4.SelectionStart;
                if (textBox4.SelectionLength > 0)
                {
                    textBox4.Text = textBox4.Text.Substring(0, selectionStart) + textBox4.Text.Substring(selectionStart + textBox4.SelectionLength);
                    textBox4    .SelectionStart = selectionStart;
                }
                else if (selectionStart > 0)
                {
                    textBox4.Text = textBox4.Text.Substring(0, selectionStart - 1) + textBox4.Text.Substring(selectionStart);
                    textBox4.SelectionStart = selectionStart - 1;
                }

                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                // Запрет на ввод более одной десятичной точки.
                if (e.KeyChar != '.' || textBox4.Text.IndexOf(".") != 0)
                {
                    e.Handled = true;
                }
            }
        }
    }
}

