using System;
using System.Windows;

namespace GreedySnake.Model
{
    internal sealed class GameData : NotifyPropertyBase
    {
        public GameData()
        {
            EndText = "        ";
        }

        private int level;
        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                RaiseChanged(ref level, value);
            }
        }

        private int bombs;
        /// <summary>
        /// 吃雷数
        /// </summary>
        public int Bombs
        {
            get
            {
                return bombs;
            }
            set
            {
                RaiseChanged(ref bombs, value);
            }
        }

        private int score;
        /// <summary>
        /// 积分
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                RaiseChanged(ref score, value);

                //switch (value)
                //{
                //    case  >= 100:
                //        Level = 1;
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        private bool playmusic;
        /// <summary>
        /// 背景音乐
        /// </summary>
        public bool PlayMusic
        {
            get
            {
                return playmusic;
            }
            set
            {
                RaiseChanged(ref playmusic, value);
            }
        }

        private string endtext;
        public string EndText
        {
            get
            {
                return endtext;
            }
            set
            {
                RaiseChanged(ref endtext, value);
            }
        }

        private int beads;
        /// <summary>
        /// 吃掉珠块数
        /// </summary>
        public int Beads
        {
            get
            {
                return beads;
            }
            set
            {
                RaiseChanged(ref beads, value);
            }
        }

        private int destroyeditems;
        /// <summary>
        /// 炸毁物品数量
        /// </summary>
        public int DestroyedItems
        {
            get
            {
                return destroyeditems;
            }
            set
            {
                RaiseChanged(ref destroyeditems, value);
            }
        }

        private long duration;
        /// <summary>
        /// 游戏用时
        /// </summary>
        public long Duration
        {
            get
            {
                return duration;
            }
            set
            {
                RaiseChanged(ref duration, value);
            }
        }

        public void Clear()
        {
            Duration = 0;
            Level = 0;
            Bombs = 0;
            Score = 0;
            Beads = 0;
            EndText = "        ";
            DestroyedItems = 0;
        }
    }
}
