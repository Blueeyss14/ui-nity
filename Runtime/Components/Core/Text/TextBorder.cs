using UnityEngine;

namespace Uinity
{
    public readonly struct TextBorder
    {
        public readonly float Thickness;
        public readonly Color Color;
        public readonly bool HasValue;

        public TextBorder(float thickness, Color color)
        {
            Thickness = thickness;
            this.Color = color;
            HasValue = true;
        }

        public TextBorder(float thickness)
        {
            Thickness = thickness;
            Color = Color.black;
            HasValue = true;
        }

        public static TextBorder None => default;
    }
}
