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
    public partial class frmSatışListele : Form
    {
        public frmSatışListele()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-CPR4UEO\\SQLYBS;Initial Catalog=Stok_Takip;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        DataSet daset = new DataSet();

        private void satışlistele()
        {
            baglan.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from satis", baglan);
            adtr.Fill(daset, "satis");
            dataGridView1.DataSource = daset.Tables["satis"];
            baglan.Close();
        }

        private void frmSatışListele_Load(object sender, EventArgs e)
        {
            satışlistele();
        }
    }
}
