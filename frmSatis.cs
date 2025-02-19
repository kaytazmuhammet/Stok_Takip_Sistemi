﻿using System;
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
    public partial class frmSatis : Form
    {
        public frmSatis()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        DataSet daset=new DataSet();

        private void sepetlistele()
        {
            baglan.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet",baglan);
            adtr.Fill(daset,"sepet");
            dataGridView1.DataSource= daset.Tables["sepet"];
            dataGridView1.Columns[0].Visible=false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglan.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from sepet",baglan);
            SqlDataReader read=komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString())
                {
                    durum=false;
                }
            }
            baglan.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if(durum==true)
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(TC,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@TC,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglan);
                komut.Parameters.AddWithValue("@TC", txtTc.Text.Trim());
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunadi", txtÜrünAdı.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMikari.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatışFiyatı.Text));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToplamFiyati.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglan.Close();
            }
            else
            {
                baglan.Open();
                SqlCommand komut2 = new SqlCommand("UPDATE sepet SET miktari = miktari + @miktar WHERE barkodno = @barkod", baglan);
                komut2.Parameters.AddWithValue("@miktar", int.Parse(txtMikari.Text));
                komut2.Parameters.AddWithValue("@barkod", txtBarkodNo.Text);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("UPDATE sepet SET toplamfiyati = miktari * satisfiyati WHERE barkodno = @barkod", baglan);
                komut3.Parameters.AddWithValue("@barkod", txtBarkodNo.Text);
                komut3.ExecuteNonQuery();
                baglan.Close();
            }
            txtMikari.Text = "1";
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMikari)
                    {
                        item.Text = "";
                    }
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMüşteriEkle ekle= new frmMüşteriEkle();
            ekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMüşteriListele listele= new frmMüşteriListele();
            listele.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmÜrünEkle ekle=new frmÜrünEkle();
            ekle.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmKategori kategori =new frmKategori();
            kategori.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmÜrünListeleme listele= new frmÜrünListeleme();
            listele.ShowDialog();
        }

        private void hesapla()
        {
            try
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyati) from sepet",baglan);
                lblGenelTolam.Text=komut.ExecuteScalar()+" TL";
                baglan.Close();
            }
            catch(Exception)
            {
                ;
            }
        }
        private void frmSatis_Load(object sender, EventArgs e)
        {
            sepetlistele();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";

            }

            baglan.Open();
            SqlCommand komut= new SqlCommand("select * from Müşteri where TC like '"+txtTc.Text+"'",baglan);
            SqlDataReader read=komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text =read["AdSoyad"].ToString();
                txtTelefon.Text = read["Telefon"].ToString();
            }
            baglan.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            Temizle();

            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like '" + txtBarkodNo.Text + "'", baglan);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtÜrünAdı.Text = read["urunadi"].ToString();
                txtSatışFiyatı.Text = read["satisfiyati"].ToString();
            }
            baglan.Close();
        }

        private void Temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMikari)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }

        private void txtMikari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyati.Text= (double.Parse(txtMikari.Text) * double.Parse(txtSatışFiyatı.Text)).ToString();
            }
            catch(Exception) 
            {
                ;
            }
        }

        private void txtSatışFiyatı_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyati.Text = (double.Parse(txtMikari.Text) * double.Parse(txtSatışFiyatı.Text)).ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut= new SqlCommand("delete from sepet where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString() +"'",baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Ürün Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void btnSatışİptal_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("delete from sepet ", baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Ürünler Sepetten Çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatışListele listele= new frmSatışListele();
            listele.ShowDialog();
        }

        private void btnSatışYap_Click(object sender, EventArgs e)
        {
            for(int i=0;i<dataGridView1.Rows.Count-1;i++)
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("insert into satis(TC,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@TC,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglan);
                komut.Parameters.AddWithValue("@TC", txtTc.Text.Trim());
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery(); 
                SqlCommand komut2 = new SqlCommand("update urun set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "'where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "'", baglan);
                komut2.ExecuteNonQuery();
                baglan.Close();
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
                MessageBox.Show("Satış İşlemi Gerçekleşti");
            }
            baglan.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet ", baglan);
            komut3.ExecuteNonQuery();
            baglan.Close();
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void txtÜrünAdı_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtToplamFiyati_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
