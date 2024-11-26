using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GreedySnake.Converters
{
    internal sealed class PathToImageBrush : IValueConverter
    {
        private static PathToImageBrush _instance;
        public static PathToImageBrush Instance => _instance ??= new PathToImageBrush();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageBrush imageBrush = new()
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,," + parameter.ToString(), UriKind.RelativeOrAbsolute))
            };
            return imageBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
