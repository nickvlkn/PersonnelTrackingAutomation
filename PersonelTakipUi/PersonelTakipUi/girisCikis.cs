using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PersonelTakipUi
{
    public partial class girisCikis : Form
    {
        public girisCikis()
        {
            InitializeComponent();
        }

        //Veri tabanı yolu ve provier 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=personel.accdb");
        //
        //Kullanıcıları veri tabanından getir
        //
        private void personelleriGoster()
        {
            try
            {
                //SELECT format(satis.tarih,'dd.mm.yyyy') as 'satis tarihi'
                baglantim.Open();
                OleDbDataAdapter personelleriListele = new OleDbDataAdapter(
                    "select tcno AS[TC KİMLİK NO],gsaat AS[Giriş Saati],csaat AS[Çıkış Saati],format(tarih,'dd.mm.yyyy') AS[TARİH] from personeller Order By tcno ASC", baglantim);
                DataSet dsHafiza = new DataSet();
                //fill dolduruyor
                personelleriListele.Fill(dsHafiza);
                dataGridView1.DataSource = dsHafiza.Tables[0];


                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "VOLKAN YILDIZ personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        //
        //load
        //
        private void girisCikis_Load(object sender, EventArgs e)
        {
            personelleriGoster();

            dateGiris.Format = DateTimePickerFormat.Custom;
            dateGiris.CustomFormat = "HH:mm";

            dateCikis.Format = DateTimePickerFormat.Custom;
            dateCikis.CustomFormat = "HH:mm";

        }
        //
        //ekle buton
        //
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            bool kayitKontrol = true;
            //ekleyeceğimiz kişi daha önce varmı yokmu bakıyoruz
            baglantim.Open();
            OleDbCommand selectSorgu = new OleDbCommand("select * from personeller where tcno='" + txtTc.Text + "'", baglantim);
            OleDbDataReader kayitOkuma = selectSorgu.ExecuteReader();
            // eğer kayır gelmişşe kayıt okuma olduysa başlangıçdaki  kayitKontrol true yapıyoruz
            while (kayitOkuma.Read())
            {
                kayitKontrol = false;
                break;
            }
            baglantim.Close();
            if (kayitKontrol==false)
            {

           
            if (txtTc.Text.Length < 11 || txtTc.Text == "")
                    lbltc.ForeColor = Color.Red;
                else
                    lbltc.ForeColor = Color.Black;

                //if (txtGsaat.Text.Length < 2 || txtGsaat.Text == "")
                //    lblad.ForeColor = Color.Red;
                //else
                //    lblad.ForeColor = Color.Black;

                //if (txtCsaat.Text.Length < 2 || txtCsaat.Text == "")
                //    lblsoyad.ForeColor = Color.Red;
                //else
                //    lblsoyad.ForeColor = Color.Black;



                //                                          //                                               //                    //                                           //

                if (txtTc.Text.Length == 11 && txtTc.Text != "" /*&& txtGsaat.Text != "" && txtGsaat.Text.Length > 1 && txtCsaat.Text != "" &&  txtCsaat.Text.Length > 1*/ )
                {

                    try
                    {
                        DateTime t2 = dateTimetTarih.Value;
                        string tarih2 = t2.ToString("MM/dd/yyyy");
                        //SELECT format(satis.tarih,'dd.mm.yyyy') as 'satis tarihi'
                        baglantim.Open();
                    OleDbCommand guncelleKomutu = new OleDbCommand("UPDATE  personeller set gsaat='" + dateGiris.Text + "',csaat='" + dateCikis.Text + "',tarih='" + tarih2+ "' where tcno='" + txtTc.Text + "'", baglantim);
                    //güncelle komudunların sonucunu veri tabanına işle
                    guncelleKomutu.ExecuteNonQuery();
                    baglantim.Close();
                    MessageBox.Show("Personel bilgileri güncellendi. ", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // temizleme metodunu çağırıyoruz
                    //  icerikTemizle();
                    personelleriGoster();
                  
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
                MessageBox.Show("Girilen tc kayıtlı değildir!.", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

 
        //
        //sil metodu
        //
        private void personelbilgiSil()
        {
            if (txtTc.Text.Length < 11 || txtTc.Text == "")
                lbltc.ForeColor = Color.Red;
            else
                lbltc.ForeColor = Color.Black;

            //if (txtGsaat.Text.Length < 2 || txtGsaat.Text == "")
            //    lblad.ForeColor = Color.Red;
            //else
            //    lblad.ForeColor = Color.Black;

            //if (txtCsaat.Text.Length < 2 || txtCsaat.Text == "")
            //    lblsoyad.ForeColor = Color.Red;
            //else
            //    lblsoyad.ForeColor = Color.Black;


            //                                          //                                               //                    //                                           //

            if (txtTc.Text.Length == 11 && txtTc.Text != "" /*&& txtGsaat.Text != "" && txtGsaat.Text.Length > 1 && txtCsaat.Text != "" && txtCsaat.Text.Length > 1*/)
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand guncelleKomutu = new OleDbCommand("UPDATE  personeller set gsaat='" + txtBos.Text + "',csaat='" + txtBos.Text + "',tarih='" + txtBos.Text + "' where tcno='" + txtTc.Text + "'", baglantim);
                    //güncelle komudunların sonucunu veri tabanına işle
                    guncelleKomutu.ExecuteNonQuery();
                    baglantim.Close();
                    MessageBox.Show("Personel bilgileri güncellendi. ", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // temizleme metodunu çağırıyoruz
                    //  icerikTemizle();
                    personelleriGoster();
                    icerikTemizle();

                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "Volkan Vıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
            else
            {
                MessageBox.Show("Kırmızı alanları doğru girin", "Volkan Vıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //
        //sil btn
        //
        private void guna2Button3_Click(object sender, EventArgs e)
            {

            if (txtTc.Text.Length == 11)
            {
                DialogResult sil = new DialogResult();
                sil = MessageBox.Show("Personelin bilgilerini silmek istiyormusunuz ?", "Uyarı", MessageBoxButtons.YesNo);
                if (sil == DialogResult.Yes)
                {
                    personelbilgiSil();
                }
                if (sil == DialogResult.No)
                {
                    MessageBox.Show("Personel silinmedi.");
                }
            }
            else
            {
                MessageBox.Show("Tc no 11 karakterden oluşmalı!!", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        //
        //btn ARa
        //
        private void btnAra_Click(object sender, EventArgs e)
            {

            bool kayitAramaDurumu = false;
            if (txtTc.Text.Length == 11)
            {
                baglantim.Open();
                //kulalniclar tablosundaki tüm alanları seç where tcno alanındaki eşit olanalrı getir 
                OleDbCommand selecsorgu = new OleDbCommand("select * from personeller where tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitOkuma = selecsorgu.ExecuteReader();
                while (kayitOkuma.Read())
                {
                    kayitAramaDurumu = true;
                    //veri tabanından 1 nolur elemanı alıp stringe dönüştürüp yazdırıyoruz
      
                    dateGiris.Text = kayitOkuma.GetValue(9).ToString();
                    dateCikis.Text = kayitOkuma.GetValue(10).ToString();
                    dateTimetTarih.Text = kayitOkuma.GetValue(11).ToString();
                
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
                //  icerikTemizle();
            }
        }
        //temizle metot
        private void icerikTemizle()//kullanıcı işlemleri
        {
            txtTc.Clear();
            dateCikis.Text = "00:00";
            dateGiris.Text = "00:00";
        }
        //
        //temizle btn
        //
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            icerikTemizle();
        }
        //
        //data grid  eleman seçme 
        //
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
             if (e.RowIndex == -1) return;

                txtTc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();   
               // txtGsaat.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();   
              //  txtCsaat.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();   
               // dateTimetTarih.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();   
        }

        private void txtGsaat_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece raakam
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

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
    }
}
