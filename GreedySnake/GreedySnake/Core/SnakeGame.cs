using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

using GreedySnake.Model;

namespace GreedySnake
{
    internal sealed class SnakeGame
    {
        public event Action<string> DisplayFailTextEvent;
        public event Action<bool> ExplosionEvent;

        private readonly MediaPlayer eatplayer;
        private readonly MediaPlayer bombplayer;
        private readonly Canvas _canvas;
        private readonly List<IndexPoint> Indexs;
        private readonly List<IndexPoint> exitIndexs;
        private readonly IReadOnlyList<ItemType> miscItems;

        private string failtext = string.Empty;

        public SnakeGame(Canvas canvas)
        {
            eatplayer = new MediaPlayer()
            {
                Volume = 1
            };
            bombplayer = new MediaPlayer()
            {
                Volume = 1
            };
            TurnPoints = new Dictionary<IndexPoint, Direction>();
            miscItems = new List<ItemType>() { ItemType.Wall, ItemType.Bomb, ItemType.Potion, ItemType.Diamond };
            _canvas = canvas;
            GameItems = new List<GameItem>();
            GameData = new();

            exitIndexs = new List<IndexPoint>();
            Indexs = new List<IndexPoint>();
            for (byte y = 0; y < 15; y++)
            {
                for (byte x = 0; x < 15; x++)
                {
                    Indexs.Add(new IndexPoint(x, y));
                }
            }

            GreedySnake = new();
            KeysQueue = new Queue<Direction>();
        }

        public Snake GreedySnake
        {
            get;
        }

        public GameItem FoodItem
        {
            get;
            private set;
        }

        public GameData GameData
        {
            get;
        }

        /// <summary>
        /// 记录蛇转弯的索引位置
        /// </summary>
        public IDictionary<IndexPoint, Direction> TurnPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 按键键入队列
        /// </summary>
        /// <remarks>
        /// 在游戏时，假如蛇的方向为D，快速敲击键盘W、D时，程序会来不及进入计时器定义的事件(在时间间隔内)，<para/>
        /// 这时蛇的状态不能经历W，直接到D，造成后面的程序判断蛇咬到了自己。<para/>
        /// 加入T类型为 <seealso cref="Direction"/> 的 <seealso cref="T:System.Collections.Generic.Queue`1"/> 队列集合当作键盘输入队列，在timer事件中取其第一个元素
        /// </remarks>
        public Queue<Direction> KeysQueue
        {
            get;
        }

        public Direction MoveDirection
        {
            get;
            private set;
        }

        public GameSignal Signal
        {
            get;
            private set;
        }

        public IList<GameItem> GameItems
        {
            get;
        }

        public void Init()
        {
            TurnPoints.Clear();
            Signal = GameSignal.GAMEING;
            MoveDirection = (Direction)Utils.GenerateRandom(23, 27);
            GameData.Clear();

            if (MoveDirection == Direction.Down || MoveDirection == Direction.Up)
            {
                int sub = (int)MoveDirection - 25;
                //随机蛇位置
                int x = Utils.GenerateRandom(0, 15);
                int y = Utils.GenerateRandom(Convert.ToInt32(1.5 + 1.5 * sub), Convert.ToInt32(13.5 + 1.5 * sub));

                SnakeNode newheadnode = GreedySnake.CreateNode(NodeType.Head, new IndexPoint(Convert.ToByte(x), Convert.ToByte(y)));
                _canvas.Children.Add(newheadnode.Rect);

                if (sub > 0)
                {
                    GreedySnake.RotateElement(GreedySnake.SnakeNodes[0].Rect, 180);
                }

                for (int i = 1; i <= 3; i++)
                {
                    if (i == 3)
                    {
                        SnakeNode footnode = GreedySnake.CreateNode(NodeType.Foot, new IndexPoint(Convert.ToByte(x), Convert.ToByte(y + sub * i * -1)));
                        _canvas.Children.Add(footnode.Rect);
                        if (sub < 0)
                        {
                            GreedySnake.RotateElement(footnode.Rect, 180);
                        }
                    }
                    else
                    {
                        SnakeNode newnode = GreedySnake.CreateNode(NodeType.Body, new IndexPoint(Convert.ToByte(x), Convert.ToByte(y + sub * i * -1)));
                        _canvas.Children.Add(newnode.Rect);
                        GreedySnake.RotateElement(newnode.Rect, 90 * sub);
                    }
                }
            }
            else
            {
                int sub = (int)MoveDirection - 24;
                //随机蛇位置
                int x = Utils.GenerateRandom(Convert.ToInt32(1.5 + 1.5 * sub), Convert.ToInt32(13.5 + 1.5 * sub));
                int y = Utils.GenerateRandom(0, 15);

                SnakeNode newheadnode = GreedySnake.CreateNode(NodeType.Head, new IndexPoint(Convert.ToByte(x), Convert.ToByte(y)));
                _canvas.Children.Add(newheadnode.Rect);
                GreedySnake.RotateElement(newheadnode.Rect, 90 * sub);

                for (int i = 1; i <= 3; i++)
                {
                    if (i == 3)
                    {
                        SnakeNode footnode = GreedySnake.CreateNode(NodeType.Foot, new IndexPoint(Convert.ToByte(x + sub * i * -1), Convert.ToByte(y)));
                        _canvas.Children.Add(footnode.Rect);
                        GreedySnake.RotateElement(footnode.Rect, -90 * sub);
                    }
                    else
                    {
                        SnakeNode newnode = GreedySnake.CreateNode(NodeType.Body, new IndexPoint(Convert.ToByte(x + sub * i * -1), Convert.ToByte(y)));
                        _canvas.Children.Add(newnode.Rect);
                        GreedySnake.RotateElement(newnode.Rect, 270 + 90 * sub);
                    }
                }
            }

            GreedySnake.SnakeNodes.Last().Rect.Tag = MoveDirection.ToString();
            //创建珠块
            CreateFood();
        }

        /// <summary>
        /// 创建珠块或改变珠块位置
        /// </summary>
        private void CreateFood()
        {
            IndexPoint point = GetRandomAvailablePos();
            ItemType foodtype = (ItemType)Utils.GenerateRandom(1, 9);

            if (FoodItem == null)
            {
                FoodItem = new(foodtype, point);
                _canvas.Children.Add(FoodItem.Rect);
                _canvas.Children.Add(FoodItem.BombObject);
                GameItems.Add(FoodItem);
            }
            else
            {
                FoodItem.SetPos(foodtype, point);
            }
        }

        /// <summary>
        /// 创建除了珠块之外的游戏物品
        /// </summary>
        private void CreateItem()
        {
            IndexPoint point = GetRandomAvailablePos();

            int Potioncount = GameItems.Count(p => p.Type == ItemType.Potion);
            int Diamondcount = GameItems.Count(p => p.Type == ItemType.Diamond);
            int Bombcount = GameItems.Count(p => p.Type == ItemType.BlackOrbs);
            int Wallcount = GameItems.Count(p => p.Type == ItemType.Wall);

            if (GameData.Score >= 1000)
            {
                int i = Utils.GenerateRandom(0, 200);
                if (i >= 193)
                {
                    GameItem gameitem = new(ItemType.Potion, point);
                    GameItems.Add(gameitem);
                    _canvas.Children.Add(gameitem.Rect);

                    return;
                }
            }

            if (Diamondcount == 0)
            {
                int i = Utils.GenerateRandom(0, 200);

                if (i >= 195 && i <= 197)
                {
                    GameItem gameitem = new(ItemType.Diamond, point);
                    GameItems.Add(gameitem);
                    _canvas.Children.Add(gameitem.Rect);

                    return;
                }
            }

            if ((Potioncount > 0 || Diamondcount > 0) && Bombcount == 0 || (Bombcount == 0 && Wallcount > 0))
            {
                int i = Utils.GenerateRandom(0, 100);

                if (i >= 63 && i <= 70)
                {
                    GameItem gameitem = new(ItemType.Bomb, point);
                    GameItems.Add(gameitem);
                    _canvas.Children.Add(gameitem.Rect);

                    return;
                }
            }

            int j = Utils.GenerateRandom(0, 100);
            if (j >= 49 && j <= 51)
            {
                GameItem gameitemwall = new(ItemType.Wall, point);
                GameItems.Add(gameitemwall);
                _canvas.Children.Add(gameitemwall.Rect);
            }
        }

        public void SnakeMove()
        {
            CreateItem();

            if (!CollisionCheck(out failtext))
            {
                GreedySnake.Move(MoveDirection, TurnPoints);
            }
            else
            {
                if (string.Compare(failtext, "        ", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (GameData.PlayMusic)
                    {
                        eatplayer.Open(new Uri("pack://siteoforigin:,,,/Resources/Sounds/Dead.mp3"));
                        eatplayer.Play();
                    }
                    DisplayFailTextEvent?.Invoke(failtext);
                }
            }
        }

        public void PauseGame(bool ispause)
        {
            if (ispause)
            {
                Signal = GameSignal.PAUSE;
            }
            else
            {
                Signal = GameSignal.GAMEING;
            }
        }

        public void StopGame()
        {
            KeysQueue.Clear();
            _canvas.Children.Remove(FoodItem.Rect);
            _canvas.Children.Remove(FoodItem.BombObject);

            foreach (var node in GreedySnake.SnakeNodes)
            {
                _canvas.Children.Remove(node.Rect);
            }
            foreach (var item in GameItems)
            {
                _canvas.Children.Remove(item.Rect);
                if (item.BombObject != null)
                {
                    _canvas.Children.Remove(item.BombObject);
                }
            }
            GreedySnake.SnakeNodes.Clear();
            GameItems.Clear();
            FoodItem = null;
        }

        private bool CollisionCheck(out string text)
        {
            text = "        ";

            if (KeysQueue.Count > 0)
            {
                MoveDirection = KeysQueue.Dequeue();
            }

            SnakeNode snakeHead = GreedySnake.SnakeNodes.Last();

            int nextX;
            int nextY;
            if (MoveDirection == Direction.Left || MoveDirection == Direction.Right)
            {
                nextX = snakeHead.Pos.X + ((int)MoveDirection - 24);
                nextY = snakeHead.Pos.Y;
            }
            else
            {
                nextX = snakeHead.Pos.X;
                nextY = snakeHead.Pos.Y + ((int)MoveDirection - 25);
            }

            //边界碰撞
            if ((nextY < 0) || (nextY > 14) || (nextX < 0) || (nextX > 14))
            {
                text = "原因：超出边界了";
                return true;
            }

            //自身碰撞
            foreach (SnakeNode snakeBodyPart in GreedySnake.SnakeNodes.Take(GreedySnake.SnakeNodes.Count - 1))
            {
                if ((nextX == snakeBodyPart.Pos.X) && (nextY == snakeBodyPart.Pos.Y))
                {
                    text = "原因：咬到自己了";
                    return true;
                }
            }

            //游戏物品碰撞
            for (int i = 0; i < GameItems.Count; i++)
            {
                if ((Convert.ToDouble(nextX * 40) == Canvas.GetLeft(GameItems[i].Rect))
                    && (Convert.ToDouble(nextY * 40) == Canvas.GetTop(GameItems[i].Rect)))
                {
                    if (GameItems[i].Type == ItemType.Wall)
                    {
                        text = "原因：撞到砖墙了";
                        return true;
                    }
                    else
                    {
                        EatItem(GameItems[i]);
                        return true;
                    }
                }
            }

            return false;
        }

        private IndexPoint GetRandomAvailablePos()
        {
            foreach (SnakeNode node in GreedySnake.SnakeNodes)
            {
                exitIndexs.Add(node.Pos);
            }
            foreach (GameItem item in GameItems)
            {
                exitIndexs.Add(item.Pos);
            }

            Indexs.RemoveAll(p => exitIndexs.Exists(t => t == p));
            IndexPoint point = Indexs[Utils.GenerateRandom(0, Indexs.Count)];

            exitIndexs.ForEach(p =>
            {
                Indexs.Add(p);
            });
            exitIndexs.Clear();

            return point;
        }

        private void EatItem(GameItem item)
        {
            if (item.Type == ItemType.Bomb)
            {
                GameData.Bombs += 1;
                GameData.DestroyedItems += GameItems.Count(p => p.Type != ItemType.Bomb);
                GreedySnake.Move(MoveDirection, TurnPoints);

                ExplosionEvent?.Invoke(true);
                if (GameData.PlayMusic)
                {
                    bombplayer.Open(new Uri("pack://siteoforigin:,,,/Resources/Sounds/Bombing.mp3"));
                    bombplayer.Play();
                }
                item.Rect.Visibility = System.Windows.Visibility.Collapsed;
                foreach (GameItem item2 in GameItems)
                {
                    item2.Rect.Visibility = System.Windows.Visibility.Collapsed;
                    if (item2.BombObject != null)
                    {
                        item2.BombObject.BombImage.Visibility = System.Windows.Visibility.Visible;
                    }
                }

                _canvas.Dispatcher.BeginInvoke(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(800);

                    GameItems.Clear();

                    FoodItem = null;
                    CreateFood();
                    ExplosionEvent?.Invoke(false);
                });
            }
            else
            {
                if (GameData.PlayMusic)
                {
                    eatplayer.Open(new Uri("pack://siteoforigin:,,,/Resources/Sounds/Eat.mp3"));
                    eatplayer.Play();
                }
                GameData.Score += (int)item.Type * 10;
                if ((int)item.Type <= 8 && (int)item.Type >= 1)
                {
                    SnakeNode headnode = GreedySnake.SnakeNodes.Last();
                    SnakeNode newnode = GreedySnake.CreateNode(NodeType.Body, headnode.Pos);
                    _canvas.Children.Add(newnode.Rect);
                    GreedySnake.RotateBodyImage(newnode, GreedySnake.SnakeNodes[GreedySnake.SnakeNodes.Count - 3], TurnPoints);


                    byte newheadx = headnode.Pos.X;
                    byte newheady = headnode.Pos.Y;
                    if (MoveDirection == Direction.Left || MoveDirection == Direction.Right)
                    {
                        newheadx = Convert.ToByte(newheadx + (int)MoveDirection - 24);
                        GreedySnake.RotateElement(headnode.Rect, 90 * ((int)MoveDirection - 24));
                    }
                    else
                    {
                        newheady = Convert.ToByte(newheady + (int)MoveDirection - 25);
                        GreedySnake.RotateElement(headnode.Rect, 90 * (1 + (int)MoveDirection - 25));
                    }

                    headnode.SetPos(newheadx, newheady);
                    CreateFood();
                }
                else
                {
                    item.Rect.Visibility = System.Windows.Visibility.Collapsed;
                    item.BombObject.Visibility = System.Windows.Visibility.Collapsed;

                    GameItems.Remove(item);
                }
            }
        }
    }
}
