using UnityEngine;

namespace Uinity
{
    public readonly struct Border
    {
        public readonly float Thickness;
        public readonly Color Color;
        public readonly bool HasValue;

        public static Border None => default;

        public Border(float thickness, Color? color = null)
        {
            Thickness = thickness;
            Color = color ?? UnityEngine.Color.black;
            HasValue = thickness > 0f;
        }

        public Border(float thickness, Color color)
        {
            Thickness = thickness;
            Color = color;
            HasValue = thickness > 0f;
        }
    }
}
