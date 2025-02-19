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
    public partial class frmMüşteriListele : Form
    {
        public frmMüşteriListele()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;TrustServerCertificate=True");
        DataSet dset= new DataSet();


        private void frmMüşteriListele_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();

        }

        private void Kayıt_Göster()
        {
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from Müşteri", baglantı);
            adtr.Fill(dset, "Müşteri");
            dataGridView1.DataSource = dset.Tables["Müşteri"];
            baglantı.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txttc.Text = dataGridView1.CurrentRow.Cells["TC"].Value.ToString();
            txtadsoyad.Text = dataGridView1.CurrentRow.Cells["AdSoyad"].Value.ToString();
            txttelefon.Text = dataGridView1.CurrentRow.Cells["Telefon"].Value.ToString();
            txtadres.Text = dataGridView1.CurrentRow.Cells["Adres"].Value.ToString();
            txtmail.Text = dataGridView1.CurrentRow.Cells["EMail"].Value.ToString();
        }

        private void btngüncelle_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("update Müşteri set AdSoyad=@AdSoyad,Telefon=@Telefon,Adres=@Adres,EMail=@EMail where TC=@TC",baglantı);
            komut.Parameters.AddWithValue("@TC", Convert.ToInt32(txttc.Text));
            komut.Parameters.AddWithValue("@AdSoyad", txtadsoyad.Text);
            komut.Parameters.AddWithValue("@Telefon", txttelefon.Text);
            komut.Parameters.AddWithValue("@Adres", txtadres.Text);
            komut.Parameters.AddWithValue("EMail", txttelefon.Text);
            komut.ExecuteNonQuery();
            baglantı.Close();
            dset.Tables["Müşteri"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Müşteri Kaydı Güncellendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("delete from Müşteri where TC='" + dataGridView1.CurrentRow.Cells["TC"].Value.ToString()+"'", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            dset.Tables["Müşteri"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Kayıt Silindi");
        }

        private void txtTcAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo= new DataTable();
            baglantı.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from Müşteri where TC like'%"+txtTcAra.Text+"%'",baglantı);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglantı.Close() ;
        }
    }
}
