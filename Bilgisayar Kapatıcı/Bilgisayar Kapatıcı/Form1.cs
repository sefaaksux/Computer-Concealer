using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bilgisayar_Kapatıcı
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        

        // ComboBox içeriğini doldur.
        public void cmbToFill()
        {
            for (int i = 0; i < 24; i++)
            {
                cmbBox_hour.Items.Add(i.ToString("D2"));
            }

            for (int j = 0; j < 60; j++)
            {

                cmbBox_minute.Items.Add(j.ToString("D2"));
            }
            for (int k = 0; k < 60; k++)
            {
                cmbBox_second.Items.Add(k.ToString("D2"));
            }
            // Default değer ataması
            cmbBox_hour.SelectedIndex = 0;
            cmbBox_minute.SelectedIndex = 0;
            cmbBox_second.SelectedIndex = 0;

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // ComboBox içeriğini doldur
            cmbToFill();
            timer1.Enabled = true;

            btn_iptalEt.Visible = false;
            txt_Ayarlanan.Enabled = false;
            txt_nowClock.Enabled = false;
            txt_kalanSure.Enabled = false;
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            txt_nowClock.Text =DateTime.Now.ToString("HH:mm:ss");
        }

        private void btn_baslat_Click(object sender, EventArgs e)
        {   // kullanıcının girdiği saati aldık.
            int closeHour = Convert.ToInt32(cmbBox_hour.Text);
            int closeMinute = Convert.ToInt32(cmbBox_minute.Text);
            int closeSecond = Convert.ToInt32(cmbBox_second.Text);
            TimeSpan closeTime = new TimeSpan(closeHour, closeMinute, closeSecond);
            // kullanıcının girdiği saati aldık.

            DateTime dateTime = DateTime.Now;       

            if (closeTime < dateTime.TimeOfDay)
            {
                MessageBox.Show("Zamanda geriye gittiniz. Lütfen doğru şekilde saat seçiminizi yapınız.");
                iptal();
            }
            else
            {
                MessageBox.Show("Bilgisayar kapatma zamanlayıcısı başladı.", "BAŞLATILDI");

                btn_baslat.Visible = false;
                btn_iptalEt.Visible = true;
                timer2.Enabled = true;
                cmbBox_hour.Enabled = false;
                cmbBox_minute.Enabled = false;
                cmbBox_second.Enabled = false;
                btn_baslat.Enabled = false;           
            }
            

        }
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            int closeHour = Convert.ToInt32(cmbBox_hour.Text);
            int closeMinute = Convert.ToInt32(cmbBox_minute.Text);
            int closeSecond = Convert.ToInt32(cmbBox_second.Text);
            DateTime dateTime = DateTime.Now;

            TimeSpan closeTime = new TimeSpan(closeHour, closeMinute, closeSecond);
            txt_Ayarlanan.Text = closeTime.ToString();

            TimeSpan difference = closeTime - dateTime.TimeOfDay;

            txt_kalanSure.Text = difference.ToString(@"hh\:mm\:ss");
            
            if(difference.TotalSeconds < 901 && difference.TotalSeconds > 900)
            {
                timer2.Enabled = false;
                MessageBox.Show("Sürenizin bitmesine 30 dakika kaldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer2.Enabled = true;
            }else if(difference.TotalSeconds < 301 && difference.TotalSeconds > 300)
            {
                timer2.Enabled = false;
                MessageBox.Show("Sürenizin bitmesine 5 dakika kaldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                timer2.Enabled = true;
            }
            

            if (difference.TotalSeconds <= 0)
            {
                timer1.Stop();
                timer2.Stop();

                DialogResult result = MessageBox.Show("Süreniz doldu. Kapatma işlemi gerçekleşiyor. Bilgisayar kapatılsın mı?", "Shutdown", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    MessageBox.Show("Kapatma işlemi durduruldu");
                    timer1.Enabled = true;
                    iptal();

                }
                else
                {
                    MessageBox.Show("Bilgisayarınız 4 saniye sonra kapanıyor.");
                    Process shutdownProcess = Process.Start("shutdown", "/s /t 4");
                    this.Close();
                }
            }

        }

        public void iptal()
        {
            timer2.Enabled = false;
            txt_kalanSure.Text = "";
            txt_Ayarlanan.Text = "";

            cmbBox_hour.Enabled = true;
            cmbBox_minute.Enabled = true;
            cmbBox_second.Enabled = true;

            btn_iptalEt.Visible = false;
            btn_baslat.Visible = true;
            btn_baslat.Enabled = true;

            cmbBox_hour.SelectedIndex = 0;
            cmbBox_minute.SelectedIndex = 0;
            cmbBox_second.SelectedIndex = 0;

        }

        public void btn_iptalEt_Click(object sender, EventArgs e)
        {
            iptal();
        }
    }
}
