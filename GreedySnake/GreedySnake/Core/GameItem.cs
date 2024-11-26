using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using GreedySnake.Componets;

namespace GreedySnake
{
    /// <summary>
    /// 游戏物品（珠块，炸弹，墙壁）
    /// </summary>
    internal sealed class GameItem
    {
        public GameItem(ItemType type, IndexPoint point)
        {
            Type = type;
            Pos = point;

            if (type == ItemType.Diamond)
            {
                Rect = (Rectangle)Application.Current.Resources["Diamond"];
            }
            else
            {
                Rect = new Rectangle()
                {
                    Width = 40,
                    Height = 40,
                    Fill = (ImageBrush)Application.Current.Resources[type.ToString()]
                };
            }
            
            Canvas.SetLeft(Rect, Convert.ToDouble(point.X * 40));
            Canvas.SetTop(Rect, Convert.ToDouble(point.Y * 40));
            Canvas.SetZIndex(Rect, 40);

            if (type != ItemType.Bomb)
            {
                BombObject = new BombPlay();
                Canvas.SetLeft(BombObject, Convert.ToDouble(point.X * 40));
                Canvas.SetTop(BombObject, Convert.ToDouble(point.Y * 40));
                Canvas.SetZIndex(BombObject, 35);
            }
        }

        /// <summary>
        /// 物品类型
        /// </summary>
        public ItemType Type
        {
            get;
            set;
        }

        public Rectangle Rect
        {
            get;
        }

        public BombPlay BombObject
        {
            get;
        }

        public IndexPoint Pos
        {
            get;
            set;
        }

        public void SetPos(IndexPoint pos)
        {
            Pos = pos;

            Canvas.SetLeft(Rect, Convert.ToDouble(Pos.X * 40));
            Canvas.SetTop(Rect, Convert.ToDouble(Pos.Y * 40));

            if (BombObject != null)
            {
                Canvas.SetLeft(BombObject, Convert.ToDouble(Pos.X * 40));
                Canvas.SetTop(BombObject, Convert.ToDouble(Pos.Y * 40));
            }
        }

        public void SetPos(ItemType type, IndexPoint pos)
        {
            Type = type;
            Pos = pos;

            Canvas.SetLeft(Rect, Convert.ToDouble(Pos.X * 40));
            Canvas.SetTop(Rect, Convert.ToDouble(Pos.Y * 40));

            if (BombObject != null)
            {
                Canvas.SetLeft(BombObject, Convert.ToDouble(Pos.X * 40));
                Canvas.SetTop(BombObject, Convert.ToDouble(Pos.Y * 40));
            }
        }
    }
}
