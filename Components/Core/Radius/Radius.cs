namespace Uinity
{
    public readonly struct Radius
    {
        public readonly float TopLeft;
        public readonly float TopRight;
        public readonly float BottomRight;
        public readonly float BottomLeft;
        public readonly bool IsFull;
        public readonly bool HasValue;

        public static Radius full => new Radius(0f, 0f, 0f, 0f, isFull: true);
        public static Radius Full => full;

        public Radius(float value)
        {
            if (value < 0f)
            {
                TopLeft = TopRight = BottomRight = BottomLeft = 0f;
                IsFull = true;
            }
            else
            {
                TopLeft = TopRight = BottomRight = BottomLeft = value;
                IsFull = false;
            }
            HasValue = true;
        }

        public Radius(float topLeft, float topRight, float bottomRight, float bottomLeft, bool isFull = false)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
            IsFull = isFull;
            HasValue = true;
        }

        public Radius(Radius other)
        {
            TopLeft = other.TopLeft;
            TopRight = other.TopRight;
            BottomRight = other.BottomRight;
            BottomLeft = other.BottomLeft;
            IsFull = other.IsFull;
            HasValue = other.HasValue;
        }

        public Radius(SetRadiusOnly only)
        {
            TopLeft = only.TopLeft;
            TopRight = only.TopRight;
            BottomRight = only.BottomRight;
            BottomLeft = only.BottomLeft;
            IsFull = false;
            HasValue = true;
        }

        public Radius(SetRadiusX x)
        {
            TopLeft = x.Left;
            BottomLeft = x.Left;
            TopRight = x.Right;
            BottomRight = x.Right;
            IsFull = false;
            HasValue = true;
        }

        public Radius(SetRadiusY y)
        {
            TopLeft = y.Top;
            TopRight = y.Top;
            BottomLeft = y.Bottom;
            BottomRight = y.Bottom;
            IsFull = false;
            HasValue = true;
        }

        public static implicit operator Radius(float value)
        {
            return new Radius(value);
        }
    }
}
