namespace GreedySnake
{
    /// <summary>
    /// 蛇身体构造
    /// </summary>
    internal enum NodeType
    {
        /// <summary>
        /// 蛇头
        /// </summary>
        Head,
        /// <summary>
        /// 蛇身结点（未在拐弯处）
        /// </summary>
        Body,
        /// <summary>
        /// 蛇尾
        /// </summary>
        Foot,
        /// <summary>
        /// 蛇转弯时与身体构成90度的结点类型
        /// </summary>
        Turn
    }
}
