namespace Uinity
{
    public readonly struct Margin
    {
        public readonly float Left;
        public readonly float Top;
        public readonly float Right;
        public readonly float Bottom;
        public readonly bool HasValue;

        public Margin(float value)
        {
            Left = Top = Right = Bottom = value;
            HasValue = value != 0f;
        }

        public Margin(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            HasValue = left != 0f || top != 0f || right != 0f || bottom != 0f;
        }

        public static implicit operator Margin(float value)
        {
            return new Margin(value);
        }
    }

    public readonly struct SetMargin
    {
        public readonly Margin Value;

        public SetMargin(float value)
        {
            Value = new Margin(value);
        }

        public static implicit operator Margin(SetMargin m) => m.Value;
    }

    public readonly struct SetMarginOnly
    {
        public readonly Margin Value;

        public SetMarginOnly(float left = 0f, float top = 0f, float right = 0f, float bottom = 0f)
        {
            Value = new Margin(left, top, right, bottom);
        }

        public static implicit operator Margin(SetMarginOnly m) => m.Value;
    }

    public readonly struct SetMarginX
    {
        public readonly Margin Value;

        public SetMarginX(float left = 0f, float right = -1f)
        {
            float r = right >= 0f ? right : left;
            Value = new Margin(left, 0f, r, 0f);
        }

        public static implicit operator Margin(SetMarginX m) => m.Value;
    }

    public readonly struct SetMarginY
    {
        public readonly Margin Value;

        public SetMarginY(float top = 0f, float bottom = -1f)
        {
            float b = bottom >= 0f ? bottom : top;
            Value = new Margin(0f, top, 0f, b);
        }

        public static implicit operator Margin(SetMarginY m) => m.Value;
    }

    public readonly struct SetMarginXY
    {
        public readonly Margin Value;

        public SetMarginXY(float x = 0f, float y = 0f)
        {
            Value = new Margin(x, y, x, y);
        }

        public static implicit operator Margin(SetMarginXY m) => m.Value;
    }
}
