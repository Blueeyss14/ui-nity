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
                float w = Screen.width;
#if UNITY_EDITOR
                if (!Application.isPlaying || w <= 0)
                {
                    if (w <= 0)
                    {
                        var sceneView = UnityEditor.SceneView.lastActiveSceneView;
                        w = (sceneView != null && sceneView.position.width > 0) ? sceneView.position.width : 1920f;
                    }
                }
#else
                if (w <= 0) w = 1920f;
#endif
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