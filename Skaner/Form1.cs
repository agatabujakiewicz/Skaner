using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WIA;

namespace Skaner
{
    public partial class Form1 : Form
    {
        WIA.CommonDialog wia_dialog;
        Device scanner = null;
        ImageFile scan;


        public Form1()
        {
            InitializeComponent();
            //loadScanner();

        }

        private void loadScanner()
        {
            wia_dialog = new WIA.CommonDialog();
            scanner = wia_dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);
            if(scanner == null)
            {
                MessageBox.Show("Nie wykryto skanera");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Item item = scanner.Items[1];
            object image = wia_dialog.ShowTransfer(item, "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}", false);
            scan = (ImageFile)image;

            var file_bytes = (byte[])scan.FileData.get_BinaryData();
            var stream = new MemoryStream(file_bytes);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromStream(stream);
        }

        private void changeSettings(IItem s, int dpi, int width, int hight, int mode)
        {
            changeProp(s.Properties, "6147", dpi);
            changeProp(s.Properties, "6151", width);
            changeProp(s.Properties, "6152", hight);
            changeProp(s.Properties, "6146", mode);
        }

        private void changeProp(IProperties p, object name, object v)
        {
            Property property = p.get_Item(ref name);
            property.set_Value(ref v);
            p.get_Item(ref name).set_Value(ref v);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Item item = scanner.Items[1];
            int s;
            if (radioButton1.Checked == true)
                s = 1;
            else if (radioButton2.Checked == true)
            {
                s = 2;
            }
            else
                s = 4;

            changeSettings(item, Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text), s);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            saveFileDialog1.Filter = "Png File (*.png)|*.png;";
            saveFileDialog1.ShowDialog();
            string name = saveFileDialog1.FileName;
            if(name != "")
            scan.SaveFile(name);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            scan = wia_dialog.ShowAcquireImage(
                        WiaDeviceType.ScannerDeviceType,
                        WiaImageIntent.UnspecifiedIntent,
                        WiaImageBias.MaximizeQuality,
                        "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}",
                        true, true, false);

            var file_bytes = (byte[])scan.FileData.get_BinaryData();
            var stream = new MemoryStream(file_bytes);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromStream(stream);
        }

    }
}
