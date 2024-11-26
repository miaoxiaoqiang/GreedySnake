using System;
using System.Globalization;
using System.Windows.Data;

namespace GreedySnake.Converters
{
    internal sealed class SecondToHMS : IValueConverter
    {
        private static SecondToHMS _instance;
        public static SecondToHMS Instance => _instance ??= new SecondToHMS();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "00:00:00";
            }

            string @params = parameter.ToString();
            long duration = System.Convert.ToInt64(value);
            TimeSpan time = TimeSpan.FromSeconds(duration);
            string timestring = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            if (@params == "Time")
            {
                return timestring;
            }
            else
            {
                return "用时：" + timestring;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
