using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public class Vertical : UIElement
    {
        private readonly float _gap;
        private readonly Padding _padding;
        private readonly UIElement[] _children;

        public Vertical(params UIElement[] children)
        {
            _gap = 0f;
            _padding = default;
            _children = children ?? System.Array.Empty<UIElement>();
        }

        public Vertical(float gap, params UIElement[] children)
        {
            _gap = gap;
            _padding = default;
            _children = children ?? System.Array.Empty<UIElement>();
        }

        public Vertical(Padding padding, params UIElement[] children)
        {
            _gap = 0f;
            _padding = padding;
            _children = children ?? System.Array.Empty<UIElement>();
        }

        public Vertical(float gap, Padding padding, params UIElement[] children)
        {
            _gap = gap;
            _padding = padding;
            _children = children ?? System.Array.Empty<UIElement>();
        }

        public Vertical(Padding padding, float gap, params UIElement[] children)
        {
            _gap = gap;
            _padding = padding;
            _children = children ?? System.Array.Empty<UIElement>();
        }

        public override GameObject Build(
            Transform parent)
        {
            GameObject obj =
                UIContext.CreateObject(
                    "Vertical",
                    parent
                );

            RectTransform rect =
                UIContext.GetRect(obj);

            rect.anchorMin =
                new Vector2(0f, 0f);

            rect.anchorMax =
                new Vector2(1f, 1f);

            rect.pivot =
                new Vector2(0f, 1f);

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            VerticalLayoutGroup layout =
                obj.AddComponent<VerticalLayoutGroup>();

            layout.childAlignment =
                TextAnchor.UpperLeft;

            layout.childControlWidth = true;
            layout.childControlHeight = true;

            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            layout.spacing = _gap;

            if (_padding.HasValue)
            {
                layout.padding = new RectOffset(
                    Mathf.RoundToInt(_padding.Left),
                    Mathf.RoundToInt(_padding.Right),
                    Mathf.RoundToInt(_padding.Top),
                    Mathf.RoundToInt(_padding.Bottom)
                );
            }

            foreach (UIElement child in _children)
            {
                if (child != null)
                {
                    child.Build(obj.transform);
                }
            }

            if (_gap > 0f)
            {
                UILayoutGapAdapter adapter = obj.AddComponent<UILayoutGapAdapter>();
                adapter.targetGap = _gap;
                adapter.isVertical = true;
                adapter.UpdateSpacing();
            }

            return obj;
        }
    }
}