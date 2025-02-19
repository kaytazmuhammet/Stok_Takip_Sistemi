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
    public partial class frmÜrünListeleme : Form
    {
        public frmÜrünListeleme()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        DataSet daset= new DataSet();

        private void kategorigetir()
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from kategoribilgileri", baglan);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["kategori"].ToString());
            }
            baglan.Close();
        }

        private void frmÜrünListeleme_Load(object sender, EventArgs e)
        {
            ÜrünListele();
            kategorigetir();
        }

        private void ÜrünListele()
        {
            baglan.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglan);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglan.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BarkodNotxt.Text= dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString();
            Kategoritxt.Text = dataGridView1.CurrentRow.Cells["kategori"].Value.ToString();
            Markatxt.Text = dataGridView1.CurrentRow.Cells["marka"].Value.ToString();
            ÜrünAdıtxt.Text = dataGridView1.CurrentRow.Cells["urunadi"].Value.ToString();
            Miktartxt.Text = dataGridView1.CurrentRow.Cells["miktari"].Value.ToString();
            AlışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["alisfiyatı"].Value.ToString();
            SatışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["satisfiyati"].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("update urun set urunadi=@urunadi,miktari=@miktari,alisfiyatı=@alisfiyatı,satisfiyati=@satisfiyati where barkodno=@barkodno",baglan);
            komut.Parameters.AddWithValue("@barkodno",BarkodNotxt.Text);
            komut.Parameters.AddWithValue("@urunadi", ÜrünAdıtxt.Text);
            komut.Parameters.AddWithValue("@miktari",int.Parse(Miktartxt.Text));
            komut.Parameters.AddWithValue("@alisfiyatı",double.Parse(AlışFiyatıtxt.Text));
            komut.Parameters.AddWithValue("@satisfiyati",double.Parse(SatışFiyatıtxt.Text));
            komut.ExecuteNonQuery();
            baglan.Close();
            daset.Tables["urun"].Clear();
            ÜrünListele();
            MessageBox.Show("Güncelleme Yapıldı");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnMarkaGuncelle_Click(object sender, EventArgs e)
        {
            if(BarkodNotxt.Text!="")
            {
                baglan.Open();
                SqlCommand komut = new SqlCommand("update urun set kategori=@kategori,marka=@marka where barkodno=@barkodno", baglan);
                komut.Parameters.AddWithValue("@barkodno", BarkodNotxt.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Güncelleme Yapıldı");
                daset.Tables["urun"].Clear();
                ÜrünListele();
            }
            else
            {
                MessageBox.Show("Barkod No Yazılı Değil!");
            }
            
            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from markabilgileri where kategori='" + comboKategori.SelectedItem + "'", baglan);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglan.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("delete from urun where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString() + "'", baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            daset.Tables["urun"].Clear();
            ÜrünListele();
            MessageBox.Show("Kayıt Silindi");
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglan.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where barkodno like'%" + txtBarkodNoAra.Text + "%'", baglan);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglan.Close();
        }
    }
}
