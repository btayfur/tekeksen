using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;



namespace LabArdwatch
{
    public partial class Form1 : Form
    {

        string portadi1 = "";
        private Thread okumath;
        bool okuma = false;
        int baudrate = 57600;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            try
            {
                if (comboBox1.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Port seçimi yapılmadı.");
                }
                else
                {
                    
                    try
                    {
                        baudrate = System.Convert.ToInt32(textBox2.Text.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Geçerli bir baudrate giriniz.");
                        textBox2.Text = "57600";
                    }
                    portadi1 = comboBox1.SelectedItem.ToString();
                    okumath = new Thread(new ThreadStart(this.Okumat));
                    okumath.IsBackground = true;
                    okumath.Start();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Okumat()
        {
            string gelenveri = "";
            try
            {
                button3.Enabled = true;
                okuma = true;
                

                portadi1 = comboBox1.SelectedItem.ToString();

                SerialPort port = new SerialPort(portadi1, baudrate);


                if (!port.IsOpen)
                    port.Open();


                while (okuma)
                {
                    gelenveri = port.ReadLine();
                    yazdir(gelenveri);
                }
                port.DiscardInBuffer();
                port.DiscardInBuffer();
                port.Close();

            }
            catch (TimeoutException)
            {
                MessageBox.Show("Bir hata oluştu.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Bağlantı Sorunu!"+e);
            }

        }
        private void yazdir(string gelenveri)
        {
            richTextBox1.AppendText(gelenveri);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            okuma = false;
            button3.Enabled = false;
            button2.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string dosyaadi = "a";
                if (textBox1.Text == "")
                {
                    dosyaadi = DateTime.Now.ToString("MM_dd_yyyy HH_mm_ss");
                }
                else
                {
                    dosyaadi = textBox1.Text.ToString();
                }
                string fileName = dosyaadi+".txt";
                string path = @"C:\Text_Files\my_file.txt";

                while (File.Exists(fileName))
                {
                    fileName = fileName + "_";
                    MessageBox.Show("Aynı adda kayıtlı dosya mevcut! \n Dosya şu isimle kaydedildi:" + fileName + "_");
                }

                StreamWriter sw = new StreamWriter(fileName);
                string metin = richTextBox1.Text.ToString();
                sw.WriteLine(metin);
                sw.Close();

                //File.WriteAllText(path, richTextBox1.Text.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Verilen dosya adı uygun değil farklı bir isimle deneyin.");
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
