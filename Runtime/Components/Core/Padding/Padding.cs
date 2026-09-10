namespace Uinity
{
    public readonly struct Padding
    {
        public readonly float Left;
        public readonly float Top;
        public readonly float Right;
        public readonly float Bottom;
        public readonly bool HasValue;

        public Padding(float value)
        {
            Left = Top = Right = Bottom = value;
            HasValue = value != 0f;
        }

        public Padding(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            HasValue = left != 0f || top != 0f || right != 0f || bottom != 0f;
        }

        public static implicit operator Padding(float value)
        {
            return new Padding(value);
        }
    }

    public readonly struct SetPadding
    {
        public readonly Padding Value;

        public SetPadding(float value)
        {
            Value = new Padding(value);
        }

        public static implicit operator Padding(SetPadding p) => p.Value;
    }

    public readonly struct SetPaddingOnly
    {
        public readonly Padding Value;

        public SetPaddingOnly(float left = 0f, float top = 0f, float right = 0f, float bottom = 0f)
        {
            Value = new Padding(left, top, right, bottom);
        }

        public static implicit operator Padding(SetPaddingOnly p) => p.Value;
    }

    public readonly struct SetPaddingX
    {
        public readonly Padding Value;

        public SetPaddingX(float left = 0f, float right = -1f)
        {
            float r = right >= 0f ? right : left;
            Value = new Padding(left, 0f, r, 0f);
        }

        public static implicit operator Padding(SetPaddingX p) => p.Value;
    }

    public readonly struct SetPaddingY
    {
        public readonly Padding Value;

        public SetPaddingY(float top = 0f, float bottom = -1f)
        {
            float b = bottom >= 0f ? bottom : top;
            Value = new Padding(0f, top, 0f, b);
        }

        public static implicit operator Padding(SetPaddingY p) => p.Value;
    }

    public readonly struct SetPaddingXY
    {
        public readonly Padding Value;

        public SetPaddingXY(float x = 0f, float y = 0f)
        {
            Value = new Padding(x, y, x, y);
        }

        public static implicit operator Padding(SetPaddingXY p) => p.Value;
    }
}
