using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GreedySnake.Componets
{
    /// <summary>
    /// BombPlay.xaml 的交互逻辑
    /// </summary>
    public sealed partial class BombPlay : UserControl
    {
        //private readonly Storyboard sb;

        public BombPlay()
        {
            //sb = new Storyboard();
            
            InitializeComponent();
            //DoubleAnimationUsingKeyFrames bombAnimationX = new();
            //DoubleAnimationUsingKeyFrames bombAnimationY = new();

            //sb.Children.Clear();
            //sb.Children.Add(bombAnimationX);
            //sb.Children.Add(bombAnimationY);
            //Storyboard.SetTarget(bombAnimationX, BombImage);
            //Storyboard.SetTarget(bombAnimationY, BombImage);
            //Storyboard.SetTargetProperty(bombAnimationX, new PropertyPath("(Canvas.Left)"));
            //Storyboard.SetTargetProperty(bombAnimationY, new PropertyPath("(Canvas.Top)"));
            //foreach (var pair in Utils.STimeLineDict)
            //{
            //    DoubleKeyFrame kf_left = new DiscreteDoubleKeyFrame() { KeyTime = pair.Item3, Value = pair.Item1 };
            //    bombAnimationX.KeyFrames.Add(kf_left);
            //    DoubleKeyFrame kf_top = new DiscreteDoubleKeyFrame() { KeyTime = pair.Item3, Value = pair.Item2 };
            //    bombAnimationY.KeyFrames.Add(kf_top);
            //}
        }

        //public void Play()
        //{
        //    Visibility = Visibility.Visible;
        //    sb.Completed += Sb_Completed;
        //    sb.Begin();
        //}

        //private void Sb_Completed(object sender, EventArgs e)
        //{
        //    sb.Completed -= Sb_Completed;
        //    Visibility = Visibility.Collapsed;
        //}
    }
}
