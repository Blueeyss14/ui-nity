using UnityEngine;

namespace Uinity
{
    public readonly struct Height
    {
        public readonly float Value;
        public readonly bool IsFull;

        public Height(float value, bool isFull = false)
        {
            Value = value;
            IsFull = isFull;
        }

        public static Height full => new Height(0f, true);
        public static Height Full => full;

        public static Height screen
        {
            get
            {
                float h = 0f;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Vector2 gameViewSize = UnityEditor.Handles.GetMainGameViewSize();
                    if (gameViewSize.y > 0f)
                    {
                        h = gameViewSize.y;
                    }
                }
#endif
                if (h <= 0f)
                {
                    h = Screen.height;
                }
                if (h <= 0f)
                {
                    h = 1080f;
                }
                return new Height(h, false);
            }
        }

        public static implicit operator Height(float value)
        {
            return new Height(value, false);
        }

        public static implicit operator float(Height height)
        {
            return height.Value;
        }

        public static Height operator +(Height a, Height b)
        {
            return new Height(a.Value + b.Value, a.IsFull || b.IsFull);
        }

        public static Height operator -(Height a, Height b)
        {
            return new Height(a.Value - b.Value, a.IsFull && b.IsFull);
        }

        public static Height operator *(Height a, float scalar)
        {
            return new Height(a.Value * scalar, a.IsFull);
        }

        public static Height operator *(float scalar, Height a)
        {
            return new Height(a.Value * scalar, a.IsFull);
        }

        public static Height operator /(Height a, float scalar)
        {
            return new Height(a.Value / scalar, a.IsFull);
        }
    }
}