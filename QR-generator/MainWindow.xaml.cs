using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using QRCoder;

namespace QR_generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
   
        QRCodeGenerator? qrC = null;

        byte[]? lastQR = null;
        public MainWindow()
        {
            InitializeComponent();
            qrC = new QRCodeGenerator();
        }

        private void ConvertBt_Click(object sender, RoutedEventArgs e)
        {
            if (inputText.Text != null && inputText.Text != string.Empty)
            {
                var qrData =  qrC.CreateQrCode(inputText.Text, QRCodeGenerator.ECCLevel.M);
                byte[] pngSrc = new PngByteQRCode(qrData).GetGraphic(20);
                var converter = new ImageSourceConverter();
                lastQR = pngSrc;
                ImageSource src = (ImageSource)converter.ConvertFrom(pngSrc);
                ImgQR.Source = src;
            }
        }

        private void SaveQR_Img(string path)
        {
            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    foreach (var b in lastQR)
                    {
                        fs.WriteByte(b);
                    }
                }
                MessageBox.Show("Saved! - " + path);
            }
            else
            {
                MessageBox.Show("Img already used this name!");
            }
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Images (*.PNG;*.JPG)|*.PNG;*.JPG|" +
        "All files (*.*)|*.*";
            ofd.DefaultDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            if (ofd.ShowDialog() == true)
            {
                Console.WriteLine(ofd.FileName);
                SaveQR_Img(ofd.FileName);
            }
        }
    }
}