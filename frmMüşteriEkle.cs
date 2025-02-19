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
    public partial class frmMüşteriEkle : Form
    {
        public frmMüşteriEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglantı= new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;TrustServerCertificate=True");

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmMüşteriEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("insert into Müşteri(TC,AdSoyad,Telefon,Adres,EMail) values(@TC,@AdSoyad,@Telefon,@Adres,@EMail)", baglantı);
            komut.Parameters.AddWithValue("@TC",Convert.ToInt32(txttc.Text));
            komut.Parameters.AddWithValue("@AdSoyad",txtadsoyad.Text);
            komut.Parameters.AddWithValue("@Telefon",txttelefon.Text);
            komut.Parameters.AddWithValue("@Adres",txtadres.Text);
            komut.Parameters.AddWithValue("EMail", txttelefon.Text);
            komut.ExecuteNonQuery();
            baglantı.Close();
            MessageBox.Show("Müşteri Kaydı Eklendi");
            foreach(Control item in this.Controls)
            {
                if(item is TextBox)
                {
                    item.Text ="";
                }
            }
        }
    }
}
