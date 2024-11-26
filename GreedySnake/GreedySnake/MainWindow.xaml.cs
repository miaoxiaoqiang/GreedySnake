using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace GreedySnake
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly Key[] controlkeys;
        private readonly Timer _gameThreadingTimer;
        private readonly SnakeGame game;
        private readonly DispatcherTimer stopWatch;

        public MainWindow()
        {
            controlkeys = [Key.Up, Key.Down, Key.Left, Key.Right];
            _gameThreadingTimer = new Timer(GameThreadTimerCallback, null, Timeout.Infinite, 400);

            InitializeComponent();

            KeyDown += Window_KeyDown;
            BtnStart.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Btn_StartGame), true);
            BtnPause.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Btn_PauseGame), true);
            BtnStop.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Btn_StopGame), true);
            BtnExit.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Btn_ExitApp), true);
            Logo.AddHandler(Image.MouseMoveEvent, new MouseEventHandler(WindowMouseMove));

            game = new(gameArea);
            DataContext = game.GameData;
            game.DisplayFailTextEvent += DisplayFailText;
            game.ExplosionEvent += BeginExploding;

            SolidColorBrush brush1 = new((Color)ColorConverter.ConvertFromString("#FF1F4A80"));
            SolidColorBrush brush2 = new((Color)ColorConverter.ConvertFromString("#FF22538A"));
            SolidColorBrush brush3 = new((Color)ColorConverter.ConvertFromString("#FF2B6AA4"));

            //游戏区域画背景
            for (int x = 0; x < 15; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    Path path = new()
                    {
                        StrokeThickness = 1,
                        Visibility = Visibility.Visible,
                        Stroke = brush1,
                        Data = new RectangleGeometry() { Rect = new Rect(0, 0, 40, 40) }
                    };

                    if ((y & 1) == 1)//奇数
                    {
                        if ((x & 1) == 1)//奇数
                        {
                            path.Fill = brush2;
                        }
                        else
                        {
                            path.Fill = brush3;
                        }
                    }
                    else
                    {
                        if ((x & 1) == 1)//奇数
                        {
                            path.Fill = brush3;
                        }
                        else
                        {
                            path.Fill = brush2;
                        }
                    }

                    Canvas.SetLeft(path, x * 40);
                    Canvas.SetTop(path, y * 40);
                    Canvas.SetZIndex(path, 10);
                    gameArea.Children.Add(path);
                }
            }

            stopWatch = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            stopWatch.Tick += StopWatch_Tick;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (game.Signal == GameSignal.GAMEING)
            {
                if (e.Key == Key.Space)
                {
                    BtnPause.Content = "继续";
                    game.PauseGame(true);
                    stopWatch.Stop();
                    _gameThreadingTimer.Change(-1, Timeout.Infinite);
                }
                else
                {
                    if (game.GreedySnake.SnakeNodes.Count > 0)
                    {
                        if (Array.Exists(controlkeys, p => { return p == e.Key && (Math.Abs((int)e.Key - (int)game.MoveDirection) == 1 || Math.Abs((int)e.Key - (int)game.MoveDirection) == 3); }))
                        {
                            Direction olddire = game.MoveDirection;
                            Direction newdire = (Direction)(int)e.Key;
                            game.KeysQueue.Enqueue(newdire);

                            if (olddire != newdire)
                            {
                                SnakeNode headnode = game.GreedySnake.SnakeNodes.Last();
                                if (!game.TurnPoints.Any(p => p.Key == headnode.Pos))
                                {
                                    game.TurnPoints.Add(headnode.Pos, newdire);
                                }
                            }
                        }
                    }
                }
            }
            else if (game.Signal == GameSignal.PAUSE)
            {
                if (e.Key == Key.Space)
                {
                    BtnPause.Content = "暂停";
                    game.PauseGame(false);
                    stopWatch.Start();
                    _gameThreadingTimer.Change(200, 400);
                }
            }
        }

        private void Btn_StartGame(object sender, MouseButtonEventArgs e)
        {
            BtnStart.IsEnabled = false;
            BtnPause.IsEnabled = true;
            BtnStop.IsEnabled = true;

            game.Init();

            stopWatch.Start();
            _gameThreadingTimer.Change(200, 400);
        }

        private void Btn_PauseGame(object sender, MouseButtonEventArgs e)
        {
            if (game.Signal == GameSignal.PAUSE)
            {
                BtnPause.Content = "继续";
                stopWatch.Stop();
                _gameThreadingTimer.Change(-1, Timeout.Infinite);
            }
            else if (game.Signal == GameSignal.GAMEING)
            {
                BtnPause.Content = "暂停";
                stopWatch.Start();
                _gameThreadingTimer.Change(200, 400);
            }
        }

        private void Btn_StopGame(object sender, MouseButtonEventArgs e)
        {
            stopWatch.Stop();
            _gameThreadingTimer.Change(-1, Timeout.Infinite);

            BtnStart.IsEnabled = true;
            BtnPause.IsEnabled = false;
            BtnStop.IsEnabled = false;

            game.StopGame();
        }

        private void Btn_ExitApp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("确定退出此程序？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void GameThreadTimerCallback(object state)
        {
            if (game.Signal == GameSignal.GAMEING)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    game.SnakeMove();
                }));
            }
        }

        private void DisplayFailText(string text)
        {
            Btn_StopGame(null, null);

            game.GameData.EndText = text;
            Mask.Visibility = Visibility.Visible;
        }

        private void BeginExploding(bool obj)
        {
            if (obj)
            {
                _gameThreadingTimer.Change(-1, Timeout.Infinite);
            }
            else
            {
                _gameThreadingTimer.Change(10, 400);
            }
        }

        private void StopWatch_Tick(object sender, EventArgs e)
        {
            game.GameData.Duration += 1;
        }
    }
}
