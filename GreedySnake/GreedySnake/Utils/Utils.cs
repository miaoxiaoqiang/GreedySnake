using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows;

namespace GreedySnake
{
    internal sealed class Utils
    {
        private static readonly RNGCryptoServiceProvider rngCSP;

        static Utils()
        {
            rngCSP = new RNGCryptoServiceProvider();
            //STimeLineDict = new List<Tuple<double, double, KeyTime>>()
            //{
            //    { Tuple.Create<double, double, KeyTime>(-4, -5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(30))) },
            //    { Tuple.Create<double, double, KeyTime>(-53, -5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(80))) },
            //    { Tuple.Create<double, double, KeyTime>(-105, -5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(130))) },
            //    { Tuple.Create<double, double, KeyTime>(-155, -5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(180))) },
            //    { Tuple.Create<double, double, KeyTime>(-4, -54, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(230))) },
            //    { Tuple.Create<double, double, KeyTime>(-54, -54, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(280))) },
            //    { Tuple.Create<double, double, KeyTime>(-104, -54, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(330))) },
            //    { Tuple.Create<double, double, KeyTime>(-154, -54, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(380))) },
            //    { Tuple.Create<double, double, KeyTime>(-4, -103, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(430))) },
            //    { Tuple.Create<double, double, KeyTime>(-54, -103, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(480))) },
            //    { Tuple.Create<double, double, KeyTime>(-102, -103, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(530))) },
            //    { Tuple.Create<double, double, KeyTime>(-152, -103, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(580))) },
            //    { Tuple.Create<double, double, KeyTime>(0, -150, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(630))) },
            //    { Tuple.Create<double, double, KeyTime>(-50, -150, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(680))) },
            //    { Tuple.Create<double, double, KeyTime>(-100, -150, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(730))) },
            //    { Tuple.Create<double, double, KeyTime>(-150, -150, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(780))) }
            //};
        }

        //public static IList<Tuple<double, double, KeyTime>> STimeLineDict
        //{
        //    get;
        //}

        /// <summary>
        /// 在指定的连续范围生成随机数
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>
        /// <paramref name="minValue"/> &lt;= 随机数值 &lt; <paramref name="maxValue"/>
        /// </returns>
        public static int GenerateRandom(int minValue, int maxValue)
        {
            int m = maxValue - minValue;
            decimal _base = long.MaxValue;
            byte[] rndSeries = new byte[8];
            rngCSP.GetBytes(rndSeries);
            long l = BitConverter.ToInt64(rndSeries, 0);
            int rnd = (int)(Math.Abs(l) / _base * m);
            return minValue + rnd;
        }

        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            List<T> childList = new();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }

                childList.AddRange(GetChildObjects<T>(child, ""));
            }

            return childList;

        }
    }
}
