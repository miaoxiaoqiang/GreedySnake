using System;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GreedySnake
{
    /// <summary>
    /// 蛇身结点
    /// </summary>
    internal sealed class SnakeNode
    {
        public SnakeNode(NodeType nodeTye, IndexPoint point)
        {
            NodeType = nodeTye;
            Pos = point;

            Rect = new Rectangle
            {
                Width = 40,
                Height = 40,
                Fill = (ImageBrush)Application.Current.Resources[nodeTye.ToString()] // new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF83F783"))// Color.FromArgb(0, 0, 0, 0))
            };

            Canvas.SetLeft(Rect, Convert.ToDouble(Pos.X * 40));
            Canvas.SetTop(Rect, Convert.ToDouble(Pos.Y * 40));
            Canvas.SetZIndex(Rect, 50);
        }

        public NodeType NodeType
        {
            get;
            set;
        }

        public IndexPoint Pos
        {
            get;
            private set;
        }

        public Rectangle Rect
        {
            get;
        }

        public void SetPos(byte x, byte y)
        {
            Pos = new IndexPoint(x, y);
            Canvas.SetLeft(Rect, Convert.ToDouble(x * 40));
            Canvas.SetTop(Rect, Convert.ToDouble(y * 40));
        }
    }
}
