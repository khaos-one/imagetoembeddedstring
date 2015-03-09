using System.Windows.Media;

namespace ImageToEmbeddedString
{
    public static class WpfHelper
    {
        public static ImageSource ImageSourceFromFullLocator(string locator)
        {
            return
                new ImageSourceConverter().ConvertFromString(
                    locator) as
                    ImageSource;
        }

        public static ImageSource ImageSourceFromRelLocator(string relativeLocator)
        {
            return
                new ImageSourceConverter().ConvertFromString(
                    "pack://application:,,,/ImageToEmbeddedString;component/" + relativeLocator) as
                    ImageSource;
        }
    }
}