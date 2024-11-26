using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GreedySnake
{
    /// <summary>
    /// 蛇
    /// </summary>
    internal sealed class Snake
    {
        private readonly IReadOnlyDictionary<double, RotateTransform> Rotations;
        private readonly ImageBrush bodyimage;
        private readonly ImageBrush turnimage;

        public Snake()
        {
            Rotations = new Dictionary<double, RotateTransform>()
            {
                { 0, new RotateTransform(0, 20, 20) },
                { 180, new RotateTransform(180, 20, 20) },
                { 90, new RotateTransform(90, 20, 20) },
                { -90, new RotateTransform(-90, 20, 20) },
                { -180, new RotateTransform(-180, 20, 20) },
                { 360, new RotateTransform(360, 20, 20) },
                { 270, new RotateTransform(270, 20, 20) },
                { -270, new RotateTransform(-270, 20, 20) },
                { -360, new RotateTransform(-360, 20, 20) }
            };
            SnakeNodes = new List<SnakeNode>();

            bodyimage = (ImageBrush)Application.Current.Resources["Body"];
            turnimage = (ImageBrush)Application.Current.Resources["Corner"];
        }

        /// <summary>
        /// 蛇身结点集合
        /// </summary>
        public IList<SnakeNode> SnakeNodes
        {
            get;
        }

        /// <summary>
        /// 生成蛇身结点
        /// </summary>
        /// <param name="nodeTye">蛇身结点图片类型</param>
        /// <param name="point">结点在画布中的偏移量</param>
        public SnakeNode CreateNode(NodeType nodeTye, IndexPoint point)
        {
            SnakeNode node = new(nodeTye, point);
            if (SnakeNodes.Count >= 4)
            {
                SnakeNodes.Insert(SnakeNodes.Count - 1, node);
            }
            else
            {
                if (SnakeNodes.Count > 0)
                {
                    SnakeNodes.Insert(0, node);
                }
                else
                {
                    SnakeNodes.Add(node);
                }
            }

            return node;
        }

        public void Move(Direction targetDire, IDictionary<IndexPoint, Direction> cornerPoints)
        {
            if (SnakeNodes.Count > 0)
            {
                for (int i = 0; i < SnakeNodes.Count - 1; i++)
                {
                    SnakeNodes[i].SetPos(SnakeNodes[i + 1].Pos.X, SnakeNodes[i + 1].Pos.Y);
                    //改变蛇尾及蛇身图像转向
                    if (i == 0)
                    {
                        if (cornerPoints.Any(p => p.Key == SnakeNodes[i].Pos))
                        {
                            Direction direction = cornerPoints[SnakeNodes[i].Pos];
                            cornerPoints.Remove(SnakeNodes[i].Pos);
                            if (direction == Direction.Left || direction == Direction.Right)
                            {
                                RotateElement(SnakeNodes[i].Rect, 90 * (24 - (int)direction));
                            }
                            else
                            {
                                RotateElement(SnakeNodes[i].Rect, 90 * (26 - (int)direction));
                            }
                        }
                    }
                    else
                    {
                        //if (cornerPoints.Any(p => p.Key == SnakeNodes[i].Pos))
                        //{
                        //    Direction direction = cornerPoints[SnakeNodes[i].Pos];

                        //    //旋转角度多少请根据所使用的图片为准，下同
                        //    if (direction == Direction.Left || direction == Direction.Right)
                        //    {
                        //        int sub = SnakeNodes[i].Pos.Y - SnakeNodes[i - 1].Pos.Y;
                        //        SnakeNodes[i].Rect.Fill = turnimage;
                        //        RotateElement(SnakeNodes[i].Rect, -45 * (((int)direction - 24) * (6 + sub) + 1));
                        //    }
                        //    else
                        //    {
                        //        int sub = SnakeNodes[i].Pos.X - SnakeNodes[i - 1].Pos.X;
                        //        SnakeNodes[i].Rect.Fill = turnimage;
                        //        RotateElement(SnakeNodes[i].Rect, -45 * (((int)direction - 25) * (6 - sub) - 1));
                        //    }
                        //}
                        //else
                        //{
                        //    if (SnakeNodes[i].Pos.X == SnakeNodes[i - 1].Pos.X)
                        //    {
                        //        int sub = SnakeNodes[i].Pos.Y - SnakeNodes[i - 1].Pos.Y;
                        //        SnakeNodes[i].Rect.Fill = bodyimage;
                        //        RotateElement(SnakeNodes[i].Rect, 90 * sub);
                        //    }
                        //    else
                        //    {
                        //        int sub = SnakeNodes[i].Pos.X - SnakeNodes[i - 1].Pos.X;
                        //        SnakeNodes[i].Rect.Fill = bodyimage;
                        //        RotateElement(SnakeNodes[i].Rect, 90 * (sub - 1));
                        //    }
                        //}

                        RotateBodyImage(SnakeNodes[i], SnakeNodes[i - 1], cornerPoints);
                    }
                }

                SnakeNode headnode = SnakeNodes.Last();
                byte nextX = headnode.Pos.X;
                byte nextY = headnode.Pos.Y;
                string directionstring = headnode.Rect.Tag.ToString();

                if (targetDire == Direction.Left || targetDire == Direction.Right)
                {
                    nextX = Convert.ToByte(nextX + (int)targetDire - 24);
                }
                else
                {
                    nextY = Convert.ToByte(nextY + (int)targetDire - 25);
                }

                if (directionstring != targetDire.ToString())
                {
                    RotateElement(headnode.Rect, 90 * ((int)targetDire - 24)); //改变蛇头图像转向
                }

                headnode.SetPos(nextX, nextY);
                headnode.Rect.Tag = targetDire.ToString();
            }
        }

        public void RotateElement(System.Windows.Shapes.Rectangle element, double angle)
        {
            element.ClearValue(UIElement.RenderTransformProperty);
            element.RenderTransform = Rotations[angle];
        }

        public void RotateBodyImage(SnakeNode node1, SnakeNode node2, IDictionary<IndexPoint, Direction> cornerPoints)
        {
            if (cornerPoints.Any(p => p.Key == node1.Pos))
            {
                Direction direction = cornerPoints[node1.Pos];

                //旋转角度多少请根据所使用的图片为准，下同
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    int sub = node1.Pos.Y - node2.Pos.Y;
                    node1.Rect.Fill = turnimage;
                    RotateElement(node1.Rect, -45 * (((int)direction - 24) * (6 + sub) + 1));
                    node1.NodeType = NodeType.Turn;
                }
                else
                {
                    int sub = node1.Pos.X - node2.Pos.X;
                    node1.Rect.Fill = turnimage;
                    RotateElement(node1.Rect, -45 * (((int)direction - 25) * (6 - sub) - 1));
                    node1.NodeType = NodeType.Turn;
                }
            }
            else
            {
                if (node1.Pos.X == node2.Pos.X)
                {
                    //if (node1.NodeType == NodeType.Turn)
                    //{
                        int sub = node1.Pos.Y - node2.Pos.Y;
                        node1.Rect.Fill = bodyimage;
                        RotateElement(node1.Rect, 90 * sub);
                        node1.NodeType = NodeType.Body;
                    //}
                    
                }
                else
                {
                    //if (node1.NodeType == NodeType.Turn)
                    //{
                        int sub = node1.Pos.X - node2.Pos.X;
                        node1.Rect.Fill = bodyimage;
                        RotateElement(node1.Rect, 90 * (sub - 1));
                        node1.NodeType = NodeType.Body;
                    //}
                    
                }
            }
        }
    }
}
