using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace ImageToEmbeddedString
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SourceImagePathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                ImagePreview.Source = WpfHelper.ImageSourceFromFullLocator(((TextBox) sender).Text);
            }
            catch
            {
                ImagePreview.Source = null;
            }

            ConvertButton.IsEnabled = (ImagePreview.Source != null);
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            EmbeddedImageTextBlock.Text = ConvertImageToEmbeddedString(SourceImagePathTextBox.Text);
            CopyEmbeddedImageButton.IsEnabled = !string.IsNullOrWhiteSpace(EmbeddedImageTextBlock.Text);
        }

        private string ConvertImageToEmbeddedString(string source)
        {
            // find the image type
            var imageType = "png";

            if (source.ToLower().IndexOf("jpg") > -1) 
                imageType = "jpg";

            if (source.ToLower().IndexOf("ico") > -1) 
                imageType = "ico";

            // initialize the return value
            var ret = "";
            
            using (var stream = File.Open(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (stream != null)
                {
                    // put the image into an Image object
                    var image = System.Drawing.Image.FromStream(stream);

                    if (image != null)
                    {
                        // prepend the text for Data URI scheme
                        ret = "data:image/" + imageType + ";base64,";

                        using (var ms = new MemoryStream())
                        {
                            // convert the image into a MemoryStream
                            image.Save(ms, image.RawFormat);
                            // convert the image to base64 encoding
                            ret += Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }

            return ret;
        }

        private void CopyEmbeddedImageButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(EmbeddedImageTextBlock.Text);
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog {Multiselect = false};

            var result = dialog.ShowDialog();

            if (result == true)
            {
                SourceImagePathTextBox.Text = dialog.FileName;
            }
        }
    }
}
