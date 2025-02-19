using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stok_Otomasyon
{
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        bool durum;
        private void markakontrol()
        {
            durum = true;
            baglan.Open();
            SqlCommand komut = new SqlCommand(
                "SELECT COUNT(*) FROM markabilgileri WHERE kategori COLLATE SQL_Latin1_General_CP1_CS_AS = @kategori AND marka COLLATE SQL_Latin1_General_CP1_CS_AS = @marka", baglan);
            komut.Parameters.AddWithValue("@kategori", comboBox1.Text);
            komut.Parameters.AddWithValue("@marka", textBox1.Text);

            int count = (int)komut.ExecuteScalar();
            if (count > 0 || string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                durum = false;
            }
            baglan.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            markakontrol();
            if(durum==true)
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("insert into markabilgileri(kategori,marka) values('" + comboBox1.Text + "','" + textBox1.Text + "')", baglan);
                komut.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Marka Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle Bir Kategori ve Marka Var","Uyarı");
            }
            textBox1.Text = "";
            comboBox1.Text = "";
            
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategorigetir();
        }

        private void kategorigetir()
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from kategoribilgileri", baglan);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["kategori"].ToString());
            }
            baglan.Close();
        }
    }
}
