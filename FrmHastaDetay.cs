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

namespace Proje_Hastane
{
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }

        public string TC;

        SqlBaglantisi bgl = new SqlBaglantisi();
        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            lblTC.Text = TC;

            // Ad Soyad Cekme
            SqlCommand komut = new SqlCommand("select hastaAd, hastaSoyad from tblHastalar where hastaTC = @p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TC);
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                lblAdSoyad.Text = dr[0] + " " + dr[1];
            }

            bgl.baglanti().Close();

            // Randevu Gecmisi
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from tblRandevular where hastaTC = " + TC, bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            // Branslari Cekme
            SqlCommand komut2 = new SqlCommand("select bransAd from tblBranslar", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();

            while (dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }

            bgl.baglanti().Close();
        }

        private void cmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDoktor.Items.Clear();

            SqlCommand komut3 = new SqlCommand("select doktorAd, doktorSoyad from tblDoktorlar where doktorBrans = @p1", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", cmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader();

            while (dr3.Read())
            {
                cmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }

            bgl.baglanti().Close();
        }

        private void cmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from tblRandevular where randevuBrans = '" + cmbBrans.Text + "'" + "and randevuDoktor = '" + cmbDoktor.Text + "' and randevuDurum = 0", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void lnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDuzenle fr = new FrmBilgiDuzenle();
            fr.TC = lblTC.Text;
            fr.Show();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            txtID.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }

        private void btnRandevuAl_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("update tblRandevular set randevuDurum = 1, hastaTC = @p1, hastaSikayet = @p2 where randevuID = @p3", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", lblTC.Text);
            komut.Parameters.AddWithValue("@p2", rchSikayet.Text);
            komut.Parameters.AddWithValue("@p3", txtID.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu Alindi", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
