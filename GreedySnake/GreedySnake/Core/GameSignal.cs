using System;

namespace GreedySnake
{
    /// <summary>
    /// 游戏信号
    /// </summary>
    internal enum GameSignal
    {
        /// <summary>
        /// 停止游戏
        /// </summary>
        STOP,
        /// <summary>
        /// 游戏中
        /// </summary>
        GAMEING,
        /// <summary>
        /// 暂停游戏
        /// </summary>
        PAUSE,
        /// <summary>
        /// 爆炸处理
        /// </summary>
        BOMBING
    }
}
