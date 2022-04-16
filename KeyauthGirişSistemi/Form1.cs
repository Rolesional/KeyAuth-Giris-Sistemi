using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyAuth;

namespace KeyauthGirişSistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public static api KeyAuthApp = new api(
            name: "", //Buraya Uygulama İsmini Yazıyorsunuz.
            ownerid: "", //Ayarlarınızda Bulunan Owner ID'nizi Yazıyorsunuz.
            secret: "", //Uygulamanızın Secret Code'unu Yazıyorsunuz.
            version: "1.0" //Uygulamanızın Sürümüdür.
        );

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();

            if (KeyAuthApp.response.message == "invalidver")
            {
                if (!string.IsNullOrEmpty(KeyAuthApp.app_data.downloadLink))
                {
                    DialogResult dialogResult = MessageBox.Show("Yes to open file in browser\nNo to download file automatically", "Auto update", MessageBoxButtons.YesNo); 
                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            Process.Start(KeyAuthApp.app_data.downloadLink);
                            Environment.Exit(0);
                            break;
                        case DialogResult.No:
                            WebClient webClient = new WebClient();
                            string destFile = Application.ExecutablePath;

                            string rand = random_string();

                            destFile = destFile.Replace(".exe", $"-{rand}.exe");
                            webClient.DownloadFile(KeyAuthApp.app_data.downloadLink, destFile);

                            Process.Start(destFile);
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                FileName = "cmd.exe"
                            });
                            Environment.Exit(0);

                            break;
                        default:
                            MessageBox.Show("Invalid option");
                            Environment.Exit(0);
                            break;
                    }
                }
                MessageBox.Show("Version of this program does not match the one online. Furthermore, the download link online isn't set. You will need to manually obtain the download link from the developer");
                Thread.Sleep(2500);
                Environment.Exit(0);
            }

            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
                Environment.Exit(0);
            }

            KeyAuthApp.check();
            label2.Text = $"Current Session Validation Status: {KeyAuthApp.response.success}"; //Bunun Pek Bir Önemi Yok İsteğinize Bağlı Olarak Ekleyebilirsiniz.
        
    }

        private string random_string()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)  //Giriş Yap Butonu
        {
            KeyAuthApp.license(textBox1.Text);
            if (KeyAuthApp.response.success)
            {
                Main main = new Main();
                MessageBox.Show("Başarıyla Giriş Yapıldı!","Giriş Sistemi"); //Giriş Yaptığınızda Main Panel Açılacaktır.
                main.Show();
                this.Hide();
            }
            else
                label3.Text = "Durum: " + KeyAuthApp.response.message; //Giriş Yanlış vb. İse Ne Olduğu Yazacaktır.
        
    }
    }
}
