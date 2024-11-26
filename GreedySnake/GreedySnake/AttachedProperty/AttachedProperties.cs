using System.Windows;

namespace GreedySnake.AttachedProperty
{
    internal sealed class AttachedProperties : DependencyObject
    {
        static AttachedProperties()
        {

        }

        public static readonly DependencyProperty HeadTextProperty =
        DependencyProperty.RegisterAttached("HeadText", typeof(string), typeof(AttachedProperties), new PropertyMetadata(string.Empty));

        public static string GetHeadText(DependencyObject obj)
        {
            return (string)obj.GetValue(HeadTextProperty);
        }

        public static void SetHeadText(DependencyObject obj, string value)
        {
            obj.SetValue(HeadTextProperty, value);
        }
    }
}
