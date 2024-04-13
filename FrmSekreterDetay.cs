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
    public partial class FrmSekreterDetay : Form
    {
        public FrmSekreterDetay()
        {
            InitializeComponent();
        }

        public string TC;
        SqlBaglantisi bgl = new SqlBaglantisi();

        private void FrmSekreterDetay_Load(object sender, EventArgs e)
        {
            lblTC.Text = TC;

            // Ad Soyad
            SqlCommand komut1 = new SqlCommand("select sekreterAdSoyad from tblSekreterler where sekreterTC = @p1", bgl.baglanti());
            komut1.Parameters.AddWithValue("@p1", TC);
            SqlDataReader dr1 = komut1.ExecuteReader();

            while (dr1.Read())
            {
                lblAdSoyad.Text = dr1[0].ToString();
            }

            bgl.baglanti().Close();

            // Branslari DataGride Aktarma
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from tblBranslar", bgl.baglanti());
            da.Fill(dt1);
            dataGridView1.DataSource = dt1;

            // Doktorlari Listeye Aktarma
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select (doktorAd + ' ' + doktorSoyad) as 'Doktorlar', doktorBrans from tblDoktorlar", bgl.baglanti());
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;

            // Bransi Combobox'a Aktarma
            SqlCommand komut2 = new SqlCommand("select bransAd from tblBranslar", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();

            while (dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }

            bgl.baglanti().Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komutKaydet = new SqlCommand("insert into tblRandevular (randevuTarih, randevuSaat, randevuBrans, randevuDoktor) values (@r1, @r2, @r3, @r4)", bgl.baglanti());
            komutKaydet.Parameters.AddWithValue("@r1", mskTarih.Text);
            komutKaydet.Parameters.AddWithValue("@r2", mskSaat.Text);
            komutKaydet.Parameters.AddWithValue("@r3", cmbBrans.Text);
            komutKaydet.Parameters.AddWithValue("@r4", cmbDoktor.Text);
            komutKaydet.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu Olusturuldu");
        }

        private void cmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDoktor.Items.Clear();

            SqlCommand komut = new SqlCommand("select doktorAd, doktorSoyad from tblDoktorlar where doktorBrans = @p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", cmbBrans.Text);
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                cmbDoktor.Items.Add(dr[0] + " " + dr[1]);
            }

            bgl.baglanti().Close();
        }

        private void btnDuyuruOlustur_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into tblDuyurular (duyuru) values (@d1)", bgl.baglanti());
            komut.Parameters.AddWithValue("@d1", rchDuyuru.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Duyuru Olusturuldu");
        }

        private void btnDoktorPanel_Click(object sender, EventArgs e)
        {
            FrmDoktorPaneli drp = new FrmDoktorPaneli();
            drp.Show();
        }

        private void btnBransPanel_Click(object sender, EventArgs e)
        {
            FrmBransPaneli frb = new FrmBransPaneli();
            frb.Show();
        }

        private void btnRandevuListe_Click(object sender, EventArgs e)
        {
            FrmRandevuListesi frl = new FrmRandevuListesi();
            frl.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Duyurular fr = new Duyurular();
            fr.Show();
        }
    }
}
