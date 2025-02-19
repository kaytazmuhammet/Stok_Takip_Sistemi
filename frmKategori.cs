using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Stok_Otomasyon
{
    public partial class frmKategori : Form
    {
        public frmKategori()
        {
            InitializeComponent();
        }

        SqlConnection baglan= new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        
        bool durum;
        private void kategorikontrol()
        {
            durum = true;
            baglan.Open();
            SqlCommand komut = new SqlCommand("SELECT COUNT(*) FROM kategoribilgileri WHERE LOWER(kategori) = LOWER(@kategori)", baglan);
            komut.Parameters.AddWithValue("@kategori", textBox1.Text);
            int count = (int)komut.ExecuteScalar();
            if (count > 0 || string.IsNullOrEmpty(textBox1.Text))
            {
                durum = false;
            }
            baglan.Close();
        }
        private void frmKategori_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategorikontrol();
            if(durum==true)
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("insert into kategoribilgileri(kategori) values('" + textBox1.Text + "')", baglan);
                komut.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Kategori Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle Bir Kategori Var","Uyarı");
            }
            textBox1.Text = "";


        }
    }
}
