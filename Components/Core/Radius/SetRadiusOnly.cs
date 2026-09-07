namespace Uinity
{
    public readonly struct SetRadiusOnly
    {
        public readonly float TopLeft;
        public readonly float TopRight;
        public readonly float BottomRight;
        public readonly float BottomLeft;

        public SetRadiusOnly(
            float leftTop = 0f,
            float rightTop = 0f,
            float leftBottom = 0f,
            float rightBottom = 0f,
            float topLeft = -1f,
            float topRight = -1f,
            float bottomLeft = -1f,
            float bottomRight = -1f)
        {
            TopLeft = topLeft >= 0f ? topLeft : leftTop;
            TopRight = topRight >= 0f ? topRight : rightTop;
            BottomLeft = bottomLeft >= 0f ? bottomLeft : leftBottom;
            BottomRight = bottomRight >= 0f ? bottomRight : rightBottom;
        }

        public static implicit operator Radius(SetRadiusOnly only)
        {
            return new Radius(only.TopLeft, only.TopRight, only.BottomRight, only.BottomLeft);
        }
    }
}
