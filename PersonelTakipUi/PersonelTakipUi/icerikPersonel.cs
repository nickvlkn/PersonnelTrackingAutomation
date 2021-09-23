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

namespace PersonelTakipUi
{
    public partial class icerikPersonel : Form
    {
        public icerikPersonel()
        {
            InitializeComponent();
        }
        //Veri tabanı yolu ve provier 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=personel.accdb");

        private void sayi()
        {
        //    DataSet ds2 = new DataSet();
             
        //    OleDbCommand komut = new OleDbCommand("SELECT COUNT(tcno) FROM  personeller", baglantim);
        //    baglantim.Open();
        //    OleDbDataReader o1 = komut.ExecuteReader();


         }
        //
        //load
        //
        private void icerikPersonel_Load(object sender, EventArgs e)
        {
            personelleriGoster();


            dateTimeDogumt.Format = DateTimePickerFormat.Custom;
            dateTimeDogumt.CustomFormat = "dd/MM/yyyy";

            comboCinsiyet.SelectedIndex = 0;
            comboEgitim.SelectedIndex = 0;
            comboGorev.SelectedIndex = 0;  
            comboGorevyeri.SelectedIndex = 0;

            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));

            dateTimeDogumt.MinDate = new DateTime(1950, 1, 1);
            dateTimeDogumt.MaxDate = new DateTime(yil - 18, ay, gun);
            dateTimeDogumt.Format = DateTimePickerFormat.Short;

        }
        //
        // personelleri getir
        //
        private void personelleriGoster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleriListele = new OleDbDataAdapter(
                    "select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],cinsiyet AS[CİNSİYET],mezuniyet AS[MEZUNİYET]," +
                    "dogumtarihi AS[DOGUM TARİHİ],gorevi AS[GOREV],gorevyeri AS[GÖREV YERİ],maasi AS[MAAŞI] from personeller Order By tcno ASC", baglantim);
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
        //kaydet buton
        //
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            bool kayitKontrol = false;
            //ekleyeceğimiz kişi daha önce varmı yokmu bakıyoruz
            baglantim.Open();
            OleDbCommand selectSorgu = new OleDbCommand("select * from personeller where tcno='" + txtTc.Text + "'", baglantim);
            OleDbDataReader kayitOkuma = selectSorgu.ExecuteReader();
            // eğer kayır gelmişşe kayıt okuma olduysa başlangıçdaki  kayitKontrol true yapıyoruz
            while (kayitOkuma.Read())
            {
                kayitKontrol = true;
                break;
            }
            baglantim.Close();

            //girilen de veri tabanın da yok ise kayıt yapıyoz
            if (kayitKontrol == false)
            {
                if (txtTc.Text.Length < 11 || txtTc.Text == "")
                    lbltc.ForeColor = Color.Red;
                else
                    lbltc.ForeColor = Color.Black;

                //ad veri kontrol

                if (txtad.Text.Length < 2 || txtad.Text == "")
                    lblad.ForeColor = Color.Red;
                else
                    lblad.ForeColor = Color.Black;

                //soyad veri kontrol

                if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                    lblsoyad.ForeColor = Color.Red;
                else
                    lblsoyad.ForeColor = Color.Black;


                if (comboCinsiyet.SelectedIndex == 0)
                    lblCinsiyet.ForeColor = Color.Red;
                else
                    lblCinsiyet.ForeColor = Color.Black;


                if (comboEgitim.SelectedIndex == 0)
                    lblegitim.ForeColor = Color.Red;
                else
                    lblegitim.ForeColor = Color.Black;

                if (comboGorev.SelectedIndex == 0)
                    lblgorev.ForeColor = Color.Red;
                else
                    lblgorev.ForeColor = Color.Black;

                if (comboGorevyeri.SelectedIndex == 0)
                    lblgorevyeri.ForeColor = Color.Red;
                else
                    lblgorev.ForeColor = Color.Black;


                if (txtMaas.Text.Length < 3 || txtMaas.Text == "")
                    lblmaas.ForeColor = Color.Red;
                else
                    lblmaas.ForeColor = Color.Black;


                //                                          //                                               //                    //                                           //

                if (txtTc.Text.Length == 11 && txtTc.Text != "" && txtad.Text != "" && txtad.Text.Length > 1 && txtsoyad.Text != "" &&
                      txtsoyad.Text.Length > 1 && comboCinsiyet.SelectedIndex != 0 && comboEgitim.SelectedIndex != 0 && comboGorev.SelectedIndex != 0 &&
                      comboGorevyeri.SelectedIndex != 0 && txtMaas.Text != "")
                {

                    try
                    {
                        baglantim.Open();
                        OleDbCommand ekleKomutu = new OleDbCommand("insert into personeller(tcno,ad,soyad,cinsiyet,mezuniyet,dogumtarihi,gorevi,gorevyeri,maasi)values "+
                            "('"+txtTc.Text+ "','" + txtad.Text + "','" + txtsoyad.Text + "','" + comboCinsiyet.Text + "','" + comboEgitim.Text + "','" + dateTimeDogumt.Text + "','" + comboGorev.Text + "','" + comboGorevyeri.Text + "','" + txtMaas.Text + "')", baglantim);
                        
                        ekleKomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Yeni personel kaydı oluşturuldu. ", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        personelleriGoster();
                        icerikTemizle();
                      

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
            {
                MessageBox.Show("Girilen tc daha önceden kayıtlıdır", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        //
        //formu temizle button
        //
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            icerikTemizle();

        }
        //
        //sayfaları temizleme
        //
        private void icerikTemizle()//kullanıcı işlemleri
        {
            txtTc.Clear();
            txtad.Clear();
            txtsoyad.Clear();
            txtMaas.Clear();
            comboCinsiyet.SelectedIndex = 0;
            comboEgitim.SelectedIndex = 0;
            comboGorev.SelectedIndex = 0;
            comboGorevyeri.SelectedIndex = 0;
        }
        //
        //ara butonu
        //
        private void guna2Button2_Click(object sender, EventArgs e)
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
                    txtad.Text = kayitOkuma.GetValue(1).ToString();
                    txtsoyad.Text = kayitOkuma.GetValue(2).ToString();
                    comboCinsiyet.Text = kayitOkuma.GetValue(3).ToString();
                    comboEgitim.Text = kayitOkuma.GetValue(4).ToString();
                    dateTimeDogumt.Text = kayitOkuma.GetValue(5).ToString();
                    comboGorev.Text = kayitOkuma.GetValue(6).ToString();
                    comboGorevyeri.Text = kayitOkuma.GetValue(7).ToString();
                    txtMaas.Text = kayitOkuma.GetValue(8).ToString();
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
        //
        //sil metodu
        //
        private void personelSil()
        {
            if (txtTc.Text.Length == 11)
            {
                bool kayitAramaDurumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())
                {
                    kayitAramaDurumu = true;
                    OleDbCommand deleteSorgu = new OleDbCommand("delete from personeller where tcno='" + txtTc.Text + "'", baglantim);
                    deleteSorgu.ExecuteNonQuery();

                    MessageBox.Show("Personel Kaydı Silindi.", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baglantim.Close();
                    personelleriGoster();
                    icerikTemizle();
                    break;

                }

                if (kayitAramaDurumu == false)
                    MessageBox.Show("Personel Kaydı Bulunamadı!!", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                baglantim.Close();
                icerikTemizle();
                personelleriGoster();

            }

            else
                MessageBox.Show("Tc no 11 karakterden oluşmalı!!", "Volkan Yıldız Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        //
        // sil butonu
        //
        private void guna2Button3_Click(object sender, EventArgs e)
        {

            if (txtTc.Text.Length == 11)
            {
                DialogResult sil = new DialogResult();
                sil = MessageBox.Show("Personeli silmek istiyormusunuz ?", "Uyarı", MessageBoxButtons.YesNo);
                if (sil == DialogResult.Yes)
                {
                    personelSil();
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
        //güncelle butonu
        //
        private void guna2Button4_Click(object sender, EventArgs e)
        {
                if (txtTc.Text.Length < 11 || txtTc.Text == "")
                    lbltc.ForeColor = Color.Red;
                else
                    lbltc.ForeColor = Color.Black;

                //ad veri kontrol
                if (txtad.Text.Length < 2 || txtad.Text == "")
                    lblad.ForeColor = Color.Red;
                else
                    lblad.ForeColor = Color.Black;

                //soyad veri kontrol
                if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                    lblsoyad.ForeColor = Color.Red;
                else
                    lblsoyad.ForeColor = Color.Black;

                if (comboCinsiyet.SelectedIndex == 0)
                    lblCinsiyet.ForeColor = Color.Red;
                else
                    lblCinsiyet.ForeColor = Color.Black;

                if (comboEgitim.SelectedIndex == 0)
                    lblegitim.ForeColor = Color.Red;
                else
                    lblegitim.ForeColor = Color.Black;

                if (comboGorev.SelectedIndex == 0)
                    lblgorev.ForeColor = Color.Red;
                else
                    lblgorev.ForeColor = Color.Black;

                if (comboGorevyeri.SelectedIndex == 0)
                    lblgorevyeri.ForeColor = Color.Red;
                else
                    lblgorev.ForeColor = Color.Black;

                if (txtMaas.Text.Length < 3 || txtMaas.Text == "")
                    lblmaas.ForeColor = Color.Red;
                else
                    lblmaas.ForeColor = Color.Black;
            //                                          //                                               //                    //                                           //

            if (txtTc.Text.Length == 11 && txtTc.Text != "" && txtad.Text != "" && txtad.Text.Length > 1 && txtsoyad.Text != "" &&
          txtsoyad.Text.Length > 1 && comboCinsiyet.SelectedIndex != 0 && comboEgitim.SelectedIndex != 0 && comboGorev.SelectedIndex != 0 &&
          comboGorevyeri.SelectedIndex != 0 && txtMaas.Text != "")
            { 
                try
                {
                    baglantim.Open();
                    OleDbCommand guncelleKomutu = new OleDbCommand("UPDATE  personeller set ad='" + txtad.Text + "',soyad='" + txtsoyad.Text + "'" +
                        ",cinsiyet='" + comboCinsiyet.Text+"',mezuniyet='" + comboEgitim.Text + "',dogumtarihi='" + dateTimeDogumt.Text + "'"+ 
                        ",gorevi='" + comboGorev.Text + "',gorevyeri='" + comboGorevyeri.Text + "',maasi='" + txtMaas.Text+ "'where tcno='" + txtTc.Text + "'", baglantim);
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

        //private void rakamGirisi()
        //{

        //}
        //
        //key press maas
        //
        private void txtMaas_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        //
        //key press tc 
        //
        private void txtTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtsoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)&& !char.IsSeparator(e.KeyChar);

        }
        //
        //data grid  eleman seçme 
        //
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
                // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
                if (e.RowIndex == -1) return;

                txtTc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }


 
    }
}


//e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
