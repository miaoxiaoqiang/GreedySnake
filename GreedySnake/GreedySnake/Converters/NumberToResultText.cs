using System;
using System.Globalization;
using System.Windows.Data;

namespace GreedySnake.Converters
{
    internal sealed class NumberToResultText : IValueConverter
    {
        private static NumberToResultText _instance;
        public static NumberToResultText Instance => _instance ??= new NumberToResultText();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            int cout = System.Convert.ToInt32(value);
            string @params = parameter.ToString();
            return @params switch
            {
                "Level" => "等        级：lv " + cout.ToString() + "",
                "Bead" => "共吃珠块：" + cout.ToString() + "块",
                "Bombs" => "共吃炸弹：" + cout.ToString() + "个",
                "Score" => "获得积分：" + cout.ToString() + "分",
                "DestroyedItems" => "炸毁数量：" + cout.ToString() + "个",
                _ => null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
