using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Fonts;
using Microsoft.Office.Interop.Word;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace BeautySalon
{
    public partial class ServicePayment : Form
    {
        SqlConnection sqlConn = new SqlConnection("");
        SqlDataReader reader;
        SqlCommand cmd;
        int Id = 0;
        private Size _initialFormSize;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        public ServicePayment()
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
            System.Drawing.Font myFont = new System.Drawing.Font(fonts.Families[0], 14.0F);
            menuStrip2.Font = myFont;
            label4.Font = myFont;


            byte[] fontData1 = Properties.Resources.midium; // Измените "YourFontFile" на имя вашего файла ресурса шрифта
            IntPtr fontPtr1 = Marshal.AllocCoTaskMem(fontData1.Length);
            Marshal.Copy(fontData1, 0, fontPtr1, fontData1.Length);
            uint dummy1 = 0;
            fonts.AddMemoryFont(fontPtr1, fontData1.Length);
            AddFontMemResourceEx(fontPtr1, (uint)fontData1.Length, IntPtr.Zero, out dummy1);
            Marshal.FreeCoTaskMem(fontPtr1);
            System.Drawing.Font myFont1 = new System.Drawing.Font(fonts.Families[0], 10.0F);
            groupBox1.Font = myFont1;
            groupBox2.Font = myFont1;
            groupBox3.Font = myFont1;
            groupBox4.Font = myFont1;
            label3.Font = myFont1;
            label5.Font = myFont1;
            label8.Font = myFont1;
            label9.Font = myFont1;
            label1.Font = myFont1;
            label2.Font = myFont1;
            button1.Font = myFont1;
            button2.Font = myFont1;
            button3.Font = myFont1;
            button4.Font = myFont1;
            button5.Font = myFont1;

        }
        private void ClearControls()
        {
            Id = 0;
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Text = "";
            comboBox2.SelectedIndex = -1;
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void populate()
        {
            sqlConn.Open();
            string Myquary = "select * from ServicePayment";
            SqlCommand cmd = new SqlCommand(Myquary, sqlConn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            System.Data.DataTable ds = new System.Data.DataTable();
            da.Fill(ds);
            dataGridView2.DataSource = ds;
            sqlConn.Close();
        }

        private void populateClient()
        {
            
            string Myquary = "select * from Clients";
            cmd = new SqlCommand(Myquary, sqlConn);
            sqlConn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["Id"]);
            }
            sqlConn.Close();
        }

        private void populateService()
        {
            string Myquary = "select * from Service";
            cmd = new SqlCommand(Myquary, sqlConn);
            sqlConn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["Наименование"]);
            }
            sqlConn.Close();
        }
        private void ReplaceWordStub(string stubToReplace, string text, Microsoft.Office.Interop.Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        private void OpenDocxFile()
        {
            // Получаем полный путь к директории исполняемого файла приложения
            string exeFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string exeFolderPath1 = AppDomain.CurrentDomain.BaseDirectory;

            // Определяем относительный путь к файлу docx. Предположим, файл находится в папке "docs" внутри папки с EXE.
            string relativePath = @"docs\ServiceShablon.doc";
            string relativePath1 = @"docs\ServiceShablon1.doc";

            // Строим полный путь к файлу docx
            string fullPath = Path.Combine(exeFolderPath, relativePath);
            string fullPath1 = Path.Combine(exeFolderPath1, relativePath1);

            var service = comboBox2.Text;
            var data1 = dateTimePicker1.Value.ToShortDateString();
            var price = textBox1.Text;
            var sale = textBox2.Text;
            var itog = textBox4.Text;
            var IdClient = comboBox1.Text;
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = false;
            try
            {
                var wordDocument = wordApp.Documents.Open(fullPath);
                ReplaceWordStub("{service}", service, wordDocument);
                ReplaceWordStub("{data1}", data1, wordDocument);
                ReplaceWordStub("{price}", price, wordDocument);
                ReplaceWordStub("{sale}", sale, wordDocument);
                ReplaceWordStub("{itog}", itog, wordDocument);
                ReplaceWordStub("{IdClient}", IdClient, wordDocument);
                ReplaceWordStub("{itog1}", itog, wordDocument);

                wordDocument.SaveAs(fullPath1);
                wordDocument.Close();
                Process.Start(fullPath1);

            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }


            finally
            {
                wordApp.Quit();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlConn.Open();
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmd = new SqlCommand("SELECT * FROM Service WHERE Наименование = @Name", sqlConn);
            cmd.Parameters.AddWithValue("@Name", comboBox2.Text);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string price = reader["Цена (₽)"].ToString();
                textBox1.Text = price;
            }
            sqlConn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlConn.Open();
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmd = new SqlCommand("SELECT * FROM Clients WHERE Id = @Id", sqlConn);
            cmd.Parameters.AddWithValue("@Id", comboBox1.Text);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string sail = reader["Скидка (%)"].ToString();
                textBox2.Text = sail;
            }
            if (textBox2.Text == "0")
            {

                textBox2.Visible = false;
            }
            else
            {
                textBox2.Visible = true;
            }
            sqlConn.Close();
        }

        private void ServicePayment_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ServicePayment_Load(object sender, EventArgs e)
        {
            populate();
            populateService();
            populateClient();
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""
            || textBox2.Text == "")
            {
                MessageBox.Show("Есть незаполненные поля",
                "Ошибка ввода", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            decimal price, sail, total;
            price = Decimal.Parse(textBox1.Text);
            sail = Decimal.Parse(textBox2.Text);
            if (textBox2.Visible == false)
            {
                textBox4.Text = (price).ToString();
            }
            else
            {
                total = Convert.ToInt32((price * sail) / 100);
                textBox4.Text = (price - total).ToString();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                Id = Convert.ToInt32(row.Cells[0].Value.ToString());
                comboBox1.Text = row.Cells[1].Value.ToString();
                comboBox2.Text = row.Cells[2].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[3].Value.ToString());
                textBox4.Text = row.Cells[4].Value.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox2.Text)
                || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (comboBox1.Text != ""
              || dateTimePicker1.Text != ""
              || comboBox2.Text != ""
              || textBox4.Text != "")
            {
                SqlCommand com = new SqlCommand("INSERT INTO ServicePayment ([ID Клиента],Услуги, Дата, Итог) VALUES (@ClientID, @Service, @Date, @Itog)", sqlConn);
                sqlConn.Open();
                com.Parameters.AddWithValue("@ClientID", comboBox1.Text);
                com.Parameters.AddWithValue("@Service", comboBox2.Text);
                com.Parameters.AddWithValue("@Date", dateTimePicker1.Text);
                com.Parameters.AddWithValue("@Itog", textBox4.Text);
                com.ExecuteNonQuery();
                sqlConn.Close();
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
            if (string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox2.Text)
                || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Есть незаполненные поля",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Вы хотите отредактировать запись?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                if (comboBox1.Text != ""
                    || dateTimePicker1.Text != ""
                    || comboBox2.Text != ""
                    || textBox4.Text != "")
                {
                    sqlConn.Open();
                    SqlCommand com = new SqlCommand("UPDATE ServicePayment set [ID Клиента] = @ClientID, Услуги = @Service, Дата = @Date, Итог = @Itog where Id = @Id", sqlConn);
                    com.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Id;
                    com.Parameters.AddWithValue("@ClientID", comboBox1.Text);
                    com.Parameters.AddWithValue("@Service", comboBox2.Text);
                    com.Parameters.AddWithValue("@Date", dateTimePicker1.Text);
                    com.Parameters.AddWithValue("@Itog", textBox4.Text);
                    com.ExecuteNonQuery();
                    sqlConn.Close();
                    populate();
                    ClearControls();
                    MessageBox.Show("Вы успешно отредактировали запись",
                    "Редактирование", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text)
                || string.IsNullOrWhiteSpace(comboBox2.Text)
                || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
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
                    sqlConn.Open();
                    SqlCommand command = new SqlCommand("DELETE ServicePayment where Id = @Id", sqlConn);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                    sqlConn.Close();
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

        private void button5_Click(object sender, EventArgs e)
        {
            OpenDocxFile();
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
    }
}
