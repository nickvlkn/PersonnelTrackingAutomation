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
    public partial class raporAl : Form
    {
        public raporAl()
        {
            InitializeComponent();
        }

        //Veri tabanı yolu ve provier 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=personel.accdb");
        //
        //
        //Kullanıcıları veri tabanından getir
        //
        private void personelGoster(string ek)
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullanicilari_listele = new OleDbDataAdapter("SELECT tcno AS[TC NO],ad AS [İSİM],soyad AS[SOYİSİM] FROM personeller "+ek+" Order By ad ASC", baglantim);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "volkan yıldız personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        //
        //load
        //
        private void raporAl_Load(object sender, EventArgs e)
        {
            if (label3.Text == "")
            {
                personelGoster("");
                sahsiPersonel("");
                radioButton1.Checked = true;
            }
            else
            {
               // MessageBox.Show(label3.Text);
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                txtTcisim.Text = label3.Text;
                string ekkod = " where tcno ='" + label3.Text + "' ";
                detaypers();

            }
        }

        private void txtTcisim_TextChanged(object sender, EventArgs e)
        {
            string ekkod = "";
            if (txtTcisim.Text != "")
            {
                if (radioButton1.Checked)
                {
                 

                    ekkod = " where tcno like '" + txtTcisim.Text + "%' ";

                        
                }
                else if (radioButton2.Checked)
                {
                    ekkod = " where ad like '" + txtTcisim.Text + "%' ";
                }
                else
                {
                    ekkod = " where tcno ='" + label3.Text + "' ";

                }
            }

            personelGoster(ekkod);


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtTcisim.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtTcisim.Text = "";
        }

        private void txtTcisim_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (radioButton1.Checked)
            {

                //sadece raakam
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                txtTcisim.MaxLength = 11;

             


            }
            else if (radioButton2.Checked)
            {
                //sadace harf girişi
                e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
                txtTcisim.MaxLength = 99;
            }
          
        }


        //
        //personel bilgilerini getir
        //
        private void sahsiPersonel(string ekkod2)
        {
            //tarih sorgusu
            DateTime tarih1 = dateTimePicker1.Value;
            string t1 = tarih1.ToString("MM/dd/yyyy");
            string t1s = "";
            DateTime tarih2 = dateTimePicker2.Value;
            string t2 = tarih2.ToString("MM/dd/yyyy");
            string t2s = "";

            for (int i = 0; i < t1.Length; i++)
            {
                if (t1[i] == '.')
                    t1s = t1s + '/';
                else if (t1[i] == ' ')
                    break;
                else
                    t1s = t1s + t1[i];

            }
            for (int a = 0; a < t2.Length; a++)
            {
                if (t2[a] == '.')
                    t2s = t2s + '/';
                else if (t2[a] == ' ')
                    break;
                else
                    t2s = t2s + t2[a];

            }
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleriListele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],tarih AS[TARİH],gsaat AS[GİRİŞ SAATİ]," +
                    "csaat AS[ÇIKIŞ SAATİ] from personeller  where tarih BETWEEN #" + t1s + "# and #" + t2s + "# "+ ekkod2+" Order By tcno ASC", baglantim); 
                DataSet dsHafiza = new DataSet();
                //fill dolduruyor
                personelleriListele.Fill(dsHafiza);
                dataGridView2.DataSource = dsHafiza.Tables[0];


                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "VOLKAN YILDIZ personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }



        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            sahsiPersonel("");
        }



              //// Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
              //  if (e.RowIndex == -1) return;

              //  txtTc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void detaypers()
        {

            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleriListele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],tarih AS[TARİH],gsaat AS[GİRİŞ SAATİ]," +
                    "csaat AS[ÇIKIŞ SAATİ] from personeller  where tcno='" + txtTcisim.Text + "'", baglantim);
                DataSet dsHafiza = new DataSet();
                //fill dolduruyor
                personelleriListele.Fill(dsHafiza);
                dataGridView2.DataSource = dsHafiza.Tables[0];


                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "VOLKAN YILDIZ personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string tcno= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                radioButton1.Checked = true;
                txtTcisim.Text = tcno;
                detaypers();


            }
        }
    }
}
