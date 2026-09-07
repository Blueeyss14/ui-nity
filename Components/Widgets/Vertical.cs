using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public class Vertical : UIElement
    {
        private readonly float _gap;
        private readonly UIElement[] _children;

        public Vertical(params UIElement[] children)
            : this(0f, children)
        {
        }

        public Vertical(float gap, params UIElement[] children)
        {
            _gap = gap;
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