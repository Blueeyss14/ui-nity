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

        public static Width screen
        {
            get
            {
                float w = 0f;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Vector2 gameViewSize = UnityEditor.Handles.GetMainGameViewSize();
                    if (gameViewSize.x > 0f)
                    {
                        w = gameViewSize.x;
                    }
                }
#endif
                if (w <= 0f)
                {
                    w = Screen.width;
                }
                if (w <= 0f)
                {
                    w = 1920f;
                }
                return new Width(w, false);
            }
        }

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