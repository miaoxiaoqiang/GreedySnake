namespace GreedySnake
{
    /// <summary>
    /// 定义坐标
    /// </summary>
    internal struct IndexPoint
    {
        public IndexPoint(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public byte X
        {
            get;
            set;
        }

        public byte Y
        {
            get;
            set;
        }

        public readonly bool Equals(IndexPoint point)
        {
            if (point.X == X && point.Y == Y)
            {
                return true;
            }

            return false;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is IndexPoint point)
            {
                return point.X == X && point.Y == Y;
            }

            return false;
        }

        public override readonly int GetHashCode()
        {
            int result = 17;
            result = 37 * result + X.GetHashCode();
            result = 37 * result + Y.GetHashCode();

            return result;
        }

        public override readonly string ToString() => $"X={X.ToString()}, Y={Y.ToString()}";

        public static bool operator ==(IndexPoint point1, IndexPoint point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        public static bool operator !=(IndexPoint point1, IndexPoint point2)
        {
            return point1.X != point2.X && point1.Y != point2.Y;
        }
    }
}
