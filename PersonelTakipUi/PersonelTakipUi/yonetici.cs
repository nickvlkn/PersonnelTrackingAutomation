using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using MetroFramework.Forms;
//using MaterialSkin.Controls;
//using MaterialSkin.Animations;
//using MaterialSkin;


namespace PersonelTakipUi
{
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();
        }
        //
        //form load
        //
        private void yonetici_Load(object sender, EventArgs e)
        {
           
        }
        //
        //
        //
        int Move;
        int Mouse_X;
        int Mouse_Y;
        //
        // üst panel
        //
        private void guna2CustomGradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        //
        // üst panel
        //
        private void guna2CustomGradientPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }
        //
        // üst panel
        //
        private void guna2CustomGradientPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        //
        //form getir metodu
        //
        private void FormGetir(Form frm)
        {
            panelGovde.Controls.Clear();
            frm.MdiParent = this;
            frm.FormBorderStyle = FormBorderStyle.None;
            panelGovde.Controls.Add(frm);
            frm.Show();
        }
        //
        //buton 1 
        //
        private void button1_Click(object sender, EventArgs e)
        {
            icerikKullanici kullaniciEkle = new icerikKullanici();
            FormGetir(kullaniciEkle);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            icerikPersonel personelEkle=new icerikPersonel();
            FormGetir(personelEkle);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            raporAl raporla = new raporAl();
            FormGetir(raporla);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            girisCikis girisCikis = new girisCikis();
            FormGetir(girisCikis);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            iletisim girisCikis = new iletisim();
            FormGetir(girisCikis);
        }
        //
        //çıkış butonu
        //
        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
          this.WindowState = FormWindowState.Minimized;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            panelGovde.Controls.Clear();
            
         
        }
        //
        //aşşa al
        //
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

      
    }
}
