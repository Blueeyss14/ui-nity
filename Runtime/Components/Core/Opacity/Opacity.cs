using UnityEngine;

namespace Uinity
{
    public readonly struct Opacity
    {
        public readonly float Value;
        public readonly bool HasValue;

        public Opacity(float value)
        {
            Value = Mathf.Clamp01(value);
            HasValue = true;
        }

        public Opacity(double value) : this((float)value) { }

        public static implicit operator Opacity(float value)
        {
            return new Opacity(value);
        }

        public static implicit operator Opacity(double value)
        {
            return new Opacity(value);
        }

        public static implicit operator float(Opacity opacity)
        {
            return opacity.Value;
        }
    }
}
