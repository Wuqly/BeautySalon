using Microsoft.Office.Interop.Word;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Fonts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Collections;

namespace BeautySalon
{
    public partial class ProductPayment : Form
    {

        SqlDataReader reader;
        SqlCommand cmd;
        int Id = 0;
        private Size _initialFormSize;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, out uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        public ProductPayment()
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
            label6.Font = myFont1;
            button1.Font = myFont1;
            button3.Font = myFont1;
            button4.Font = myFont1;
            button5.Font = myFont1;
        }



        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Sign s = new Sign();
            this.Hide();
            s.Show();
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
            string relativePath = @"docs\ProductShablon.doc";
            string relativePath1 = @"docs\ProductShablon1.doc";

            // Строим полный путь к файлу docx
            string fullPath = Path.Combine(exeFolderPath, relativePath);
            string fullPath1 = Path.Combine(exeFolderPath1, relativePath1);

            var product = comboBox2.Text;
            var data1 = dateTimePicker1.Value.ToShortDateString();
            var price = textBox1.Text;
            var sale = textBox2.Text;
            var kolvo = textBox3.Text;
            var itog = textBox4.Text;
            var IdClient = comboBox1.Text;
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = false;
            try
            {
                var wordDocument = wordApp.Documents.Open(fullPath);
                ReplaceWordStub("{product}", product, wordDocument);
                ReplaceWordStub("{data1}", data1, wordDocument);
                ReplaceWordStub("{price}", price, wordDocument);
                ReplaceWordStub("{sale}", sale, wordDocument);
                ReplaceWordStub("{kolvo}", kolvo, wordDocument);
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
        private void ClearControls()
        {
            Id = 0;
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Text = "";
            comboBox2.SelectedIndex = -1;
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void ClearControlsKol()
        {
            textBox3.Text = "";
        }

        private void populate()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            string Myquary = "select * from ProductPayment";
            SqlCommand cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            System.Data.DataTable ds = new System.Data.DataTable();
            da.Fill(ds);
            dataGridView2.DataSource = ds;
            ProjectConnection.sqlConn.Close();
        }

        private void populateClient()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            string Myquary = "select * from Clients";
            cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            ProjectConnection.sqlConn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["Id"]);
            }
            ProjectConnection.sqlConn.Close();
        }

        private void populateProduct()
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            string Myquary = "select * from Product";
            cmd = new SqlCommand(Myquary, ProjectConnection.sqlConn);
            ProjectConnection.sqlConn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["Наименование"]);
            }
            ProjectConnection.sqlConn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            ProjectConnection.sqlConn.Open();
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmd = new SqlCommand("SELECT * FROM Clients WHERE Id = @Id", ProjectConnection.sqlConn);
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
            ProjectConnection.sqlConn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectConnection NewConnection = new ProjectConnection();
            NewConnection.Connection_Today();
            ProjectConnection.sqlConn.Open();
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmd = new SqlCommand("SELECT * FROM Product WHERE Наименование = @Name", ProjectConnection.sqlConn);
            cmd.Parameters.AddWithValue("@Name", comboBox2.Text);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string price = reader["Цена (₽)"].ToString();
                textBox1.Text = price;
            }
            ProjectConnection.sqlConn.Close();
        }

        private void ProductPayment_Load(object sender, EventArgs e)
        {
            populate();
            populateProduct();
            populateClient();
            if (MyConnection.type == "M")
            {
                toolStripMenuItem7.Visible = false;
            }

            if (MyConnection.type == "K")
            {
                toolStripMenuItem7.Visible = false;
            }
        }

        private void IfKol0()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=WUQLY\\SQLEXPRESS;Initial Catalog=BeautySalonDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                try
                {
                    // Устанавливаем соединение с базой данных
                    connection.Open();

                    // SQL-запрос для обновления данных
                    string query = "Delete From Product Where [Количество (шт)] = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Выполняем запрос к БД
                        int rowsAffected = command.ExecuteNonQuery();
                        // Проверяем, что запрос на обновление коснулся рядов
                    }
                }
                catch (Exception ex)
                {
                    // В случае ошибки выводим сообщение
                    MessageBox.Show($"Ошибка: {ex.Message}");
                    return;
                }
            }
        }

        private void plusKol()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=WUQLY\\SQLEXPRESS;Initial Catalog=BeautySalonDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                try
                {
                    // Устанавливаем соединение с базой данных
                    connection.Open();

                    // SQL-запрос для обновления данных
                    string query = "UPDATE Product SET [Количество (шт)] = [Количество (шт)] + @QuantitySubtract WHERE Наименование = @Product";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuantitySubtract", textBox3.Text);
                        command.Parameters.AddWithValue("@Product", comboBox2.SelectedItem.ToString());
                        // Выполняем запрос к БД
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверяем, что запрос на обновление коснулся рядов
                        if (rowsAffected > 0)
                        {

                            DialogResult dialogResult = MessageBox.Show("Вы хотите удалить запиcь?",
                                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (Id != 0)
                                {
                                    ProjectConnection NewConnection = new ProjectConnection();
                                    NewConnection.Connection_Today();
                                    ProjectConnection.sqlConn.Open();
                                    SqlCommand com = new SqlCommand("DELETE ProductPayment where Id = @Id", ProjectConnection.sqlConn);
                                    com.Parameters.AddWithValue("@Id", Id);
                                    com.ExecuteNonQuery();
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
                        else
                        {
                            MessageBox.Show("Обновление не затронуло ни одного ряда.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // В случае ошибки выводим сообщение
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void vichetKol()
        {

            using (SqlConnection connection = new SqlConnection("Data Source=WUQLY\\SQLEXPRESS;Initial Catalog=BeautySalonDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                try
                {
                    // Устанавливаем соединение с базой данных
                    connection.Open();

                    // SQL-запрос для обновления данных
                    string query = "UPDATE Product SET [Количество (шт)] = [Количество (шт)] - @QuantitySubtract WHERE Наименование = @Product";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuantitySubtract", textBox3.Text);
                        command.Parameters.AddWithValue("@Product", comboBox2.SelectedItem.ToString());
                        // Выполняем запрос к БД
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверяем, что запрос на обновление коснулся рядов
                        if (rowsAffected > 0)
                        {
                            
                            if (comboBox1.Text != ""
                              || dateTimePicker1.Text != ""
                              || textBox1.Text != ""
                              || comboBox2.Text != ""
                              || textBox3.Text != ""
                              || textBox4.Text != "")
                            {
                                ProjectConnection NewConnection = new ProjectConnection();
                                NewConnection.Connection_Today();
                                SqlCommand com = new SqlCommand("INSERT INTO ProductPayment ([ID Клиента],Товар, [Цена товара], Дата, [Количество (шт)], Итог) VALUES (@ClientID, @Prod, @PriceProd, @Date,@KolVo, @Itog)", ProjectConnection.sqlConn);
                                ProjectConnection.sqlConn.Open();
                                com.Parameters.AddWithValue("@ClientID", comboBox1.Text);
                                com.Parameters.AddWithValue("@Prod", comboBox2.Text);
                                com.Parameters.AddWithValue("@PriceProd", textBox1.Text);
                                com.Parameters.AddWithValue("@Date", dateTimePicker1.Text);
                                com.Parameters.AddWithValue("@KolVo", textBox3.Text);
                                com.Parameters.AddWithValue("@Itog", textBox4.Text);
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
                        else
                        {
                            MessageBox.Show("Обновление не затронуло ни одного ряда.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrWhiteSpace(comboBox1.Text)
                        || string.IsNullOrWhiteSpace(comboBox2.Text)
                        || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                        || string.IsNullOrWhiteSpace(textBox3.Text)
                        || string.IsNullOrWhiteSpace(textBox4.Text)
                        || string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("Есть незаполненные поля",
                                        "Ошибка ввода", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    return;
                }
            }
        }

        private void chekKol()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=WUQLY\\SQLEXPRESS;Initial Catalog=BeautySalonDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT [Количество (шт)] FROM Product WHERE Наименование = @Product";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Product", comboBox2.SelectedItem.ToString());
                        try
                        {
                            int stockQuantity = Convert.ToInt32(command.ExecuteScalar());
                            int requestedQuantity = int.Parse(textBox3.Text);
                            if (requestedQuantity > stockQuantity)
                            {
                                MessageBox.Show("Введённое значение больше значения из базы.",
                                "Ошибка ввода", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                ClearControlsKol();
                                return;
                            }
                            else
                            {
                                decimal price, sail, total, kolVo;
                                price = Decimal.Parse(textBox1.Text);
                                sail = Decimal.Parse(textBox2.Text);
                                kolVo = Decimal.Parse(textBox3.Text);

                                if (textBox2.Visible == false)
                                {
                                    textBox4.Text = (price * kolVo).ToString();
                                }
                                else
                                {
                                    total = Convert.ToInt32((price * sail) / 100);
                                    textBox4.Text = ((price - total) * kolVo).ToString();
                                }
                            }
                        }
                        catch
                        {
                            if (string.IsNullOrWhiteSpace(comboBox1.Text)
                                            || string.IsNullOrWhiteSpace(comboBox2.Text)
                                            || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                                            || string.IsNullOrWhiteSpace(textBox3.Text)
                                            || string.IsNullOrWhiteSpace(textBox4.Text)
                                            || string.IsNullOrWhiteSpace(textBox1.Text))
                            {
                                MessageBox.Show("Есть незаполненные поля",
                                                "Ошибка ввода", MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    if (string.IsNullOrWhiteSpace(comboBox1.Text)
                        || string.IsNullOrWhiteSpace(comboBox2.Text)
                        || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                        || string.IsNullOrWhiteSpace(textBox3.Text)
                        || string.IsNullOrWhiteSpace(textBox4.Text)
                        || string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("Есть незаполненные поля",
                                        "Ошибка ввода", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chekKol();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vichetKol();
            IfKol0();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text)
                 || string.IsNullOrWhiteSpace(comboBox2.Text)
                 || string.IsNullOrWhiteSpace(dateTimePicker1.Text)
                 || string.IsNullOrWhiteSpace(textBox3.Text)
                 || string.IsNullOrWhiteSpace(textBox4.Text)
                 || string.IsNullOrWhiteSpace(textBox1.Text))
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
                    || textBox3.Text != ""
                    || textBox4.Text != "")
                {
                    ProjectConnection NewConnection = new ProjectConnection();
                    NewConnection.Connection_Today();
                    ProjectConnection.sqlConn.Open();
                    SqlCommand com = new SqlCommand("UPDATE ProductPayment set [ID Клиента] = @ClientID, Товар = @Prod, [Цена товара] = @PriceProd, Дата = @Date, [Количество (шт)] = @KolVo, Итог = @Itog where Id = @Id", ProjectConnection.sqlConn);
                    com.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Id;
                    com.Parameters.AddWithValue("@ClientID", comboBox1.Text);
                    com.Parameters.AddWithValue("@Prod", comboBox2.Text);
                    com.Parameters.AddWithValue("@PriceProd", textBox1.Text);
                    com.Parameters.AddWithValue("@Date", dateTimePicker1.Text);
                    com.Parameters.AddWithValue("@KolVo", textBox3.Text);
                    com.Parameters.AddWithValue("@Itog", textBox4.Text);  
                    com.ExecuteNonQuery();
                    ProjectConnection.sqlConn.Close();
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
            plusKol();
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
                comboBox1.Text = row.Cells[1].Value.ToString();
                comboBox2.Text = row.Cells[2].Value.ToString();
                textBox1.Text = row.Cells[3].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[4].Value.ToString());
                textBox3.Text = row.Cells[5].Value.ToString();
                textBox4.Text = row.Cells[6].Value.ToString();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                // Запрет на ввод более одной десятичной точки.
                if (e.KeyChar != '.' || textBox3.Text.IndexOf(".") != 0)
                {
                    e.Handled = true;
                }
                else if(textBox3.Text.Length == 0)
                {
                    if (e.KeyChar == '0') e.Handled = true;
                }
            }

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                var selectionStart = textBox3.SelectionStart;
                if (textBox3.SelectionLength > 0)
                {
                    textBox3.Text = textBox3.Text.Substring(0, selectionStart) + textBox3.Text.Substring(selectionStart + textBox3.SelectionLength);
                    textBox3.SelectionStart = selectionStart;
                }
                else if (selectionStart > 0)
                {
                    textBox3.Text = textBox3.Text.Substring(0, selectionStart - 1) + textBox3.Text.Substring(selectionStart);
                    textBox3.SelectionStart = selectionStart - 1;
                }

                e.Handled = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void типУслугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypeOfProd TP = new TypeOfProd();
            this.Hide();
            TP.Show();
        }
    }
}
