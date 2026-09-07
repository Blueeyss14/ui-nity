namespace Uinity
{
    public readonly struct SetRadiusY
    {
        public readonly float Top;
        public readonly float Bottom;

        public SetRadiusY(float top = 0f, float bottom = 0f)
        {
            Top = top;
            Bottom = bottom;
        }

        public static implicit operator Radius(SetRadiusY y)
        {
            return new Radius(topLeft: y.Top, topRight: y.Top, bottomRight: y.Bottom, bottomLeft: y.Bottom);
        }
    }
}
