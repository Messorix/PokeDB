using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Pokemon
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Uri source = new Uri(value.ToString() + ".png", UriKind.Relative);
                BitmapImage img = new BitmapImage(source);
                return img;
            }
            catch (Exception ex)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }
        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
