using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GreedySnake.Componets
{
    /// <summary>
    /// MaskMessageBox.xaml 的交互逻辑
    /// </summary>
    public sealed partial class MaskMessageBox : UserControl
    {
        public MaskMessageBox()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
