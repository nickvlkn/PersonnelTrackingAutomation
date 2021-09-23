using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//kütüphane veri tabanı  ekliyorum
using System.Data.OleDb;
// regex güvenli parola oluşturma kütüphanesi
//using System.Text.RegularExpressions;

//klasörleme işlemleri için
using System.IO;

namespace PersonelTakipUi
{
    public partial class icerikKullanici : Form
    {
        public icerikKullanici()
        {
            InitializeComponent();
        }
        //Veri tabanı yolu ve provier 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=personel.accdb");
        //
        //Kullanıcıları veri tabanından getir
        //
        private void kullanicilari_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullanicilari_listele = new OleDbDataAdapter("SELECT" +
                " tcno AS[TC NO],ad AS[AD],soyad AS[SOYAD],kullaniciadi AS[KULLANICI ADI],parola AS[ŞİFRE],yetki AS[YETKİ] FROM kullanicilar Order By ad ASC", baglantim);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();

            }
            catch(Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "volkan yıldız personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        //
        //sayfaları temizleme
        //
        private void icerikTemizle()//kullanıcı işlemleri
        {
            txtTc.Clear();
            txtad.Clear();
            txtsoyad.Clear();
            txtkadi.Clear();
            txtsifret.Clear();
            txtsifre.Clear();
        }
        //
        //TextBox1_TextChanged
        //
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text.Length < 11)
                errorProvider1.SetError(txtTc, "TC Kimlik No 11 karakter olmalıdır");
            else
                errorProvider1.Clear();
        }
        //
        //kaydet butonu
        //
        private void btnkaydet_Click(object sender, EventArgs e)
        {
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            string yetki = "";
            bool kayitKontrol1 = false;
            bool kayitKontrol2 = false;
            //ekleyeceğimiz kişi daha önce varmı yokmu bakıyoruz
            baglantim.Open();
            OleDbCommand selectSorgu = new OleDbCommand("select * from kullanicilar where tcno='" + txtTc.Text + "'", baglantim);
            OleDbDataReader kayitOkuma = selectSorgu.ExecuteReader();
            while (kayitOkuma.Read())
            {
                kayitKontrol1 = true;
                break;
            }
            OleDbCommand selectSorgu2 = new OleDbCommand("select * from kullanicilar where kullaniciadi='" + txtkadi.Text + "'", baglantim);
            OleDbDataReader kayitOkuma2 = selectSorgu2.ExecuteReader();
            while (kayitOkuma2.Read())
            {
                kayitKontrol2 = true;
                break;
            }


            baglantim.Close();

            //girilen de veri tabanın da yok ise kayıt yapıyoz
            if (kayitKontrol1 == false)
            {
                if (kayitKontrol2 == false)
                {
                    if (txtTc.Text.Length < 11 || txtTc.Text == "")
                        label2.ForeColor = Color.Red;
                    else
                        label2.ForeColor = Color.Black;

                    //ad veri kontrol

                    if (txtad.Text.Length < 2 || txtad.Text == "")
                        label3.ForeColor = Color.Red;
                    else
                        label3.ForeColor = Color.Black;

                    //soyad veri kontrol

                    if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                        label4.ForeColor = Color.Red;
                    else
                        label4.ForeColor = Color.Black;


                    //kullanıcı adı veri kontrol

                    if (txtkadi.Text.Length != 8 || txtkadi.Text == "")
                        label5.ForeColor = Color.Red;
                    else
                        label5.ForeColor = Color.Black;

                    //parola  veri kontrol

                    if (txtsifre.Text == "")
                        label6.ForeColor = Color.Red;
                    else
                        label6.ForeColor = Color.Black;

                    //parola tekrar veri kontrol

                    if (txtsifret.Text == "" || txtsifret != txtsifre)
                        label7.ForeColor = Color.Red;
                    else
                        label7.ForeColor = Color.Black;
                    //                                          //                                               //                    //                                           //

                    if (txtTc.Text.Length == 11 && txtTc.Text != "" && txtad.Text != "" && txtad.Text.Length > 1 && txtsoyad.Text != "" &&
                            txtsoyad.Text.Length > 1 && txtkadi.Text != "" && txtsifre.Text != "" && txtsifret.Text != "" &&
                                txtsifre.Text == txtsifret.Text && txtsifre.Text == txtsifret.Text)
                    {
                        if (radiobtnY.Checked == true)
                            yetki = "Yönetici";

                        else if (radiobtnK.Checked == true)
                            yetki = "Kullanici";

                        try
                        {
                            baglantim.Open();
                            OleDbCommand ekleKomutu = new OleDbCommand("insert into kullanicilar (tcno,ad,soyad,kullaniciadi,parola,yetki) values " +
                                "('" + txtTc.Text + "','" + txtad.Text + "','" + txtsoyad.Text + "','" + txtkadi.Text + "','" + txtsifre.Text + "','" + yetki + "')", baglantim);
                            ekleKomutu.ExecuteNonQuery();
                            baglantim.Close();
                            MessageBox.Show("Yeni kullanıcı kaydı oluşturuldu. ", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // temizleme metodunu çağırıyoruz

                            icerikTemizle();
                            kullanicilari_goster();
                            //
                            label7.ForeColor = Color.Black;

                        }
                        catch (Exception hatamsj)
                        {
                            MessageBox.Show(hatamsj.Message);
                            baglantim.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kırmızı alanları doğru girin", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Girilen kuullanıcı adı daha önceden kayıtlıdır", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Girilen tc daha önceden kayıtlıdır", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void txtTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //kullanıcının silme ve rakam dışında veir girmesini engelliyoruz
            //klavyeden basılan tuşun ne olduğunu bulup 48 il 57 arasına bakıyoruz bakıyorus asscı 
            //8 backspace tuşu
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }
        //
        // Load load
        //
        private void icerikKullanici_Load(object sender, EventArgs e)
        {
            kullanicilari_goster();
            txtTc.MaxLength = 11;
            txtkadi.MaxLength = 8;
          //  toolTip1.SetToolTip(this.txtTc, "tc no 11 karakter olmalı");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        //
        //Formu temizle
        //
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            icerikTemizle();
        }
        //
        // Sil
        //
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (txtTc.Text.Length == 11)
            {
                bool kayitAramaDurumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())
                {
                    kayitAramaDurumu = true;
                    OleDbCommand deleteSorgu = new OleDbCommand("delete from kullanicilar where tcno='" + txtTc.Text + "'", baglantim);
                    deleteSorgu.ExecuteNonQuery();

                    MessageBox.Show("Kullanıcı Kaydı Silindi.", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    kullanicilari_goster();
                    icerikTemizle();
                    break;

                }

                if (kayitAramaDurumu == false)
                    MessageBox.Show("Kullanıcı Kaydı Bulunamadı!!", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                baglantim.Close();
                icerikTemizle();

            }

            else
                MessageBox.Show("Tc no 11 karakterden oluşmalı!!", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        //
        //Güncelle
        //
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string yetki = "";

            
                if (txtTc.Text.Length < 11 || txtTc.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;

                //ad veri kontrol

                if (txtad.Text.Length < 2 || txtad.Text == "")
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;

                //soyad veri kontrol

                if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                    label4.ForeColor = Color.Red;
                else
                    label4.ForeColor = Color.Black;


                //kullanıcı adı veri kontrol

                if (txtkadi.Text.Length != 8 || txtkadi.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;

                //parola  veri kontrol

                if (txtsifre.Text == "")
                    label6.ForeColor = Color.Red;
                else
                    label6.ForeColor = Color.Black;

                //parola tekrar veri kontrol

                if (txtsifret.Text == "" || txtsifret != txtsifre)
                    label7.ForeColor = Color.Red;
                else
                    label7.ForeColor = Color.Black;
                //                                          //                                               //                    //                                           //

                if (txtTc.Text.Length == 11 && txtTc.Text != "" && txtad.Text != "" && txtad.Text.Length > 1 && txtsoyad.Text != "" && txtsoyad.Text.Length > 1 && txtkadi.Text != "" && txtsifre.Text != "" && txtsifret.Text != "" && txtsifre.Text == txtsifret.Text )
                 {
                if (radiobtnY.Checked == true)
                    yetki = "Yönetici";

                else if (radiobtnK.Checked == true)
                    yetki = "Kullanici";

                try
                {
                    baglantim.Open();
                    OleDbCommand guncelleKomutu = new OleDbCommand("update kullanicilar set ad='" +
                        txtad.Text + "',soyad='" + txtsoyad.Text + "',yetki='" + yetki +
                        "',kullaniciadi='" + txtkadi.Text + "',parola='" + txtsifre.Text +
                        "'where tcno='" + txtTc.Text + "'", baglantim);
                    //güncelle komudunların sonucunu veri tabanına işle
                    guncelleKomutu.ExecuteNonQuery();
                    baglantim.Close();
                    MessageBox.Show("Kullanıcı bilgileri güncellendi. ", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // temizleme metodunu çağırıyoruz
                    //  icerikTemizle();
                    kullanicilari_goster();

                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "volkan yıldız personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
            else
            {
                MessageBox.Show("Kırmızı alanları doğru girin", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
         }
        //
        //Ara butonu
        //
        private void guna2Button2_Click(object sender, EventArgs e)
        {

            bool kayitAramaDurumu = false;
            if (txtTc.Text.Length == 11)
            {
                baglantim.Open();
                //kulalniclar tablosundaki tüm alanları seç where tcno alanındaki eşit olanalrı getir 
                OleDbCommand selecsorgu = new OleDbCommand("select * from kullanicilar where tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitOkuma = selecsorgu.ExecuteReader();
                while (kayitOkuma.Read())
                {
                    kayitAramaDurumu = true;
                    //veri tabanından 1 nolur elemanı alıp stringe dönüştürüp yazdırıyoruz
                    txtad.Text = kayitOkuma.GetValue(1).ToString();
                    txtsoyad.Text = kayitOkuma.GetValue(2).ToString();
                    if (kayitOkuma.GetValue(3).ToString() == "Yönetici")
                    {
                        radiobtnK.Checked = true;
                    }
                    else
                        radiobtnY.Checked = true;
                    txtkadi.Text = kayitOkuma.GetValue(4).ToString();
                    txtsifre.Text = kayitOkuma.GetValue(5).ToString();
                    txtsifret.Text = kayitOkuma.GetValue(5).ToString();
                    break;
                }
                //eğer yok ise folse ise
                if (kayitAramaDurumu == false)
                {  //Exclamation bilgi msg box
                    MessageBox.Show("Kayıt Bulunamadı", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                baglantim.Close();

            }
            else
            {
                MessageBox.Show("11 haneli tc gir", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                icerikTemizle();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
            if (e.RowIndex == -1) return;
            // data grid batığımızda seçme
            txtTc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtsoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtkadi.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
            if (e.RowIndex == -1) return;

            txtTc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
        //
        //ad key pres
        //
        private void txtad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        //
        //soyad key pres
        //
        private void txtsoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        //
        //kullanıcı adı
        //
        private void txtkadi_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Boşluk engelleme
            e.Handled = Char.IsWhiteSpace(e.KeyChar);
            //özel karakter engelleme
            if (e.KeyChar == '£' || e.KeyChar == '½' ||
              e.KeyChar == '€' || e.KeyChar == '₺' ||
              e.KeyChar == '¨' || e.KeyChar == 'æ' ||
              e.KeyChar == 'ß' || e.KeyChar == '´')
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 33 && (int)e.KeyChar <= 47)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 58 && (int)e.KeyChar <= 64)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 91 && (int)e.KeyChar <= 96)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 123 && (int)e.KeyChar <= 127)
            {
                e.Handled = true;
            }


        }
 
        //
        //personelleri veri tabanından getir
        //
        //private void personelleriGoster()
        //{
        //    try
        //    {
        //        baglantim.Open();
        //        OleDbDataAdapter personelleriListele = new OleDbDataAdapter(
        //            "select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],cinsiyet AS[CİNSİYET],mezuniyet AS[MEZUNİYET]," +
        //            "dogumtarihi AS[DOGUM TARİHİ],gorevi AS[GOREV],gorevyeri AS[GÖREV YERİ],maasi AS[MAAŞI] from personeller Order By ad ASC", baglantim);
        //        DataSet dsHafiza = new DataSet();
        //        //fill dolduruyor
        //        personelleriListele.Fill(dsHafiza);
        //        dataGridView2.DataSource = dsHafiza.Tables[0];
        //        baglantim.Close();

        //    }
        //    catch (Exception hatamsj)
        //    {
        //        MessageBox.Show(hatamsj.Message, "VOLKAN YILDIZ personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        baglantim.Close();
        //    }
        //}













    }
}
