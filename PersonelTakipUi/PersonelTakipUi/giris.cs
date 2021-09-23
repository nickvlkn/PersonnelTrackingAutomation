using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//kütüphane ekliyorum veri tabnı
using System.Data.OleDb;

namespace PersonelTakipUi
{
    public partial class giris : Form
    {
        public giris()
        {
            InitializeComponent();
            lbltimer.Text = "30";
            timer1.Start();
        }
        //
        //veri tabanı -- dosya yolu
        readonly OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=personel.accdb");

        //form lar arası değişkenler
        public static string tcno, adi, soyadi, yetki;

        //bu form değişkeni
        int hak = 3;
        bool durum = false;

    

        //
        //form load
        //
        private void giris_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            //entere basıldığında (giriş butonu çalışşın)
            this.AcceptButton = btnGiris;
            lblhak.Text = Convert.ToString(hak);
            label2.Visible = false;
            lblhak.Visible = false;
            lbltimer.Visible = false;
            //yönetici olarak otamik seçili gelsin 
            radioButton1.Checked = true;



        }
        //
        //panel hareketi
        //
        int Move;
        int Mouse_X;
        int Mouse_Y;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        //
        // cikis_Click
        //
        private void cikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //
        //kucult_Click
        //
        private void kucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //esc ile formdan cıkmak
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }
        //
        //Giriş buton
        //
        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (txtTc.Text == "" || txtSifre.Text == "" || radioButton1.Checked == true && radioButton2.Checked == true)
            {
                //if (radioButton1.Checked == true && radioButton2.Checked == true)
                //{

                //}
                MessageBox.Show("Tüm Alanları Doğru Bir Şekilde Doldurun.", "PERSONEL TAKİP ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {  
                if (hak <= 3)
                {

                    label2.Visible = true;
                    lblhak.Visible = true;
                }
                if (hak != 0)
                {
                    //veri tabanını açıyorız
                    baglantim.Open();
                    OleDbCommand selectSorgu = new OleDbCommand("select * from kullanicilar", baglantim);
                    //kuallnıcıalr tablosundaki tüm bilgileri getir ver kayit okuma reader da sakaldık 
                    OleDbDataReader kayitOkuma = selectSorgu.ExecuteReader();

                    while (kayitOkuma.Read())
                    {
                        raporAl frm3 = new raporAl();
                        if (radioButton1.Checked == true)
                        {//tablodaki veriler ile girilen veriyi kıyaslayıp giriş yapıyoruz
                            if (kayitOkuma["kullaniciadi"].ToString() == txtTc.Text &&
                                kayitOkuma["parola"].ToString() == txtSifre.Text
                                && kayitOkuma["yetki"].ToString() == "Yönetici")
                            {
                                durum = true;
                                //herşey doğru ise veri tabanınıdaki 0. alanı alıp tc no ya aktardık
                                tcno = kayitOkuma.GetValue(0).ToString();
                                adi = kayitOkuma.GetValue(1).ToString();
                                soyadi = kayitOkuma.GetValue(2).ToString();
                                yetki = kayitOkuma.GetValue(3).ToString();
                                //yuakrıdaki bilgileri başka formlara aktarmak için giridik 
                                this.Hide();//bşaarılı giriş olduğu için formu gizle 
                                yonetici frm2 = new yonetici(); // form 1 i gizleyip form 2 yi açıyoruz .
                                frm2.Show();
                                frm3.label3.Text = "";
                                break;// kayıt oldukdan sorna çıkışımızı yapıyoruz 
                            }
                        }

                        if (radioButton2.Checked == true)
                        {
                            if (kayitOkuma["kullaniciadi"].ToString() == txtTc.Text && kayitOkuma["parola"].ToString() == txtSifre.Text && kayitOkuma["yetki"].ToString() == "Kullanici")
                            {
                                durum = true;
                                //herşey doğru ise veri tabanınıdaki 0. alanı alıp tc no ya aktardık
                                tcno = kayitOkuma.GetValue(0).ToString();
                                adi = kayitOkuma.GetValue(1).ToString();
                                soyadi = kayitOkuma.GetValue(2).ToString();
                                yetki = kayitOkuma.GetValue(3).ToString();
                                //yuakrıdaki bilgileri başka formlara aktarmak için giridik 
                                this.Hide();//bşaarılı giriş olduğu için formu gizle 

                                
                                
                                frm3.label3.Text = tcno;
                                frm3.Show();

                                break;
                            }
                        }

                    }

                    if (durum == false)
                    {
                        hak--;

                    }
                    baglantim.Close();
                }
            }
            lblhak.Text = Convert.ToString(hak);
            if (hak == 0)
            {  //hakkı btiitiğinde giriş butonunu pasif yapıyoruz
                btnGiris.Enabled = false;
                MessageBox.Show("Giriş hakkı kalmadı", "PERSONEL TAKİP ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //lbltimer.Visible = true;
                //if (int.Parse(lbltimer.Text) != 0)
                //{
                //     btnGiris.Enabled=false;

                    
                //}
            
               this.Close();
            }
        }
        //
        
        //timer sayım
        //
        DateTime time = new DateTime(0);
        private void timer1_Tick(object sender, EventArgs e)
        {
            //time = time.AddSeconds(1);
            ////int timerdeger = int.Parse(lbltimer.Text);
            ////timerdeger++;
            //lbltimer.Text = time.ToString();

            lbltimer.Text = (int.Parse(lbltimer.Text) - 1).ToString(); //lowering the value - explained above
            if (int.Parse(lbltimer.Text) == 1)  //if the countdown reaches '0', we stop it
                timer1.Stop();
            btnGiris.Enabled = true;


        }

        //
        //şifre göster
        //
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txtSifre.UseSystemPasswordChar == true)
            {
                txtSifre.UseSystemPasswordChar = false;
            }
            else
                txtSifre.UseSystemPasswordChar = true;
        }

     
    }
}
