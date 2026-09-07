using UnityEngine;

namespace Uinity
{
    public readonly struct Width
    {
        public readonly float Value;
        public readonly bool IsFull;

        public Width(float value, bool isFull = false)
        {
            Value = value;
            IsFull = isFull;
        }

        public static Width full => new Width(0f, true);
        public static Width Full => full;

        public static Width screen => new Width(Screen.width, false);

        public static implicit operator Width(float value)
        {
            return new Width(value, false);
        }

        public static implicit operator float(Width width)
        {
            return width.Value;
        }

        public static Width operator +(Width a, Width b)
        {
            return new Width(a.Value + b.Value, a.IsFull || b.IsFull);
        }

        public static Width operator -(Width a, Width b)
        {
            return new Width(a.Value - b.Value, a.IsFull && b.IsFull);
        }

        public static Width operator *(Width a, float scalar)
        {
            return new Width(a.Value * scalar, a.IsFull);
        }

        public static Width operator *(float scalar, Width a)
        {
            return new Width(a.Value * scalar, a.IsFull);
        }

        public static Width operator /(Width a, float scalar)
        {
            return new Width(a.Value / scalar, a.IsFull);
        }
    }
}