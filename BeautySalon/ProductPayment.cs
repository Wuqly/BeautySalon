using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace BeautySalon
{
    public partial class ProductPayment : Form
    {

        SqlConnection sqlConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Aarone\\Desktop\\BeautySalon\\BeautySalonDb.mdf;Integrated Security=True;Connect Timeout=30");
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
            Font myFont = new Font(fonts.Families[0], 14.0F);
            menuStrip2.Font = myFont;
            label4.Font = myFont;


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
            DataTable ds = new DataTable();
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

        private void populateProduct()
        {
            string Myquary = "select * from Product";
            cmd = new SqlCommand(Myquary, sqlConn);
            sqlConn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["Наименование"]);
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlConn.Open();
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmd = new SqlCommand("SELECT * FROM Product WHERE Наименование = @Name", sqlConn);
            cmd.Parameters.AddWithValue("@Name", comboBox2.Text);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string price = reader["Цена (₽)"].ToString();
                textBox1.Text = price;
            }
            sqlConn.Close();
        }

        private void ProductPayment_Load(object sender, EventArgs e)
        {
            populate();
            populateProduct();
            populateClient();
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

        }
    }
}
