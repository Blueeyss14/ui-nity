using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public class Container : UIElement
    {
        private readonly Color? _color;
        private readonly Width _width;
        private readonly Height _height;
        private readonly Alignment _alignment;
        private readonly UIElement _child;

        public Container(
            Color? color = null,
            Width width = default,
            Height height = default,
            Alignment alignment = Alignment.TopLeft,
            UIElement child = null)
        {
            _color = color;
            _width = width;
            _height = height;
            _alignment = alignment;
            _child = child;
        }

        public override GameObject Build(Transform parent)
        {
            GameObject obj = UIContext.CreateObject(
                "Container",
                parent
            );

            RectTransform rect =
                UIContext.GetRect(obj);

            ApplyDefaults(rect);

            LayoutElement layoutElement =
                obj.AddComponent<LayoutElement>();

            if (_color.HasValue)
            {
                UIContext.AddImage(
                    obj,
                    _color.Value
                );
            }

            if (_width.IsFull)
            {
                ApplyWidth(rect);
                layoutElement.flexibleWidth = 1f;
            }
            else if (_width.Value > 0)
            {
                ApplyWidth(rect);
                layoutElement.preferredWidth = _width.Value;
                layoutElement.minWidth = _width.Value;
                layoutElement.flexibleWidth = 0f;
            }

            if (_height.IsFull)
            {
                ApplyHeight(rect);
                layoutElement.flexibleHeight = 1f;
            }
            else if (_height.Value > 0)
            {
                ApplyHeight(rect);
                layoutElement.preferredHeight = _height.Value;
                layoutElement.minHeight = _height.Value;
                layoutElement.flexibleHeight = 0f;
            }

            if (_child != null)
            {
                GameObject child =
                    _child.Build(obj.transform);

                RectTransform childRect =
                    child.GetComponent<RectTransform>();

                ApplyChildAlignment(childRect);
            }

            return obj;
        }

        private void ApplyDefaults(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void ApplyWidth(RectTransform rect)
        {
            if (_width.IsFull)
            {
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(1f, 1f);

                rect.offsetMin =
                    new Vector2(0f, rect.offsetMin.y);

                rect.offsetMax =
                    new Vector2(0f, rect.offsetMax.y);
            }
            else
            {
                rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    _width.Value
                );
            }
        }

        private void ApplyHeight(RectTransform rect)
        {
            if (_height.IsFull)
            {
                rect.anchorMin = new Vector2(
                    rect.anchorMin.x,
                    0f
                );

                rect.anchorMax = new Vector2(
                    rect.anchorMax.x,
                    1f
                );

                rect.offsetMin = new Vector2(
                    rect.offsetMin.x,
                    0f
                );

                rect.offsetMax = new Vector2(
                    rect.offsetMax.x,
                    0f
                );
            }
            else
            {
                rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    _height.Value
                );
            }
        }

        private void ApplyChildAlignment(
            RectTransform child)
        {
            Vector2 anchor =
                GetAnchor(_alignment);

            bool isStretchX = child.anchorMin.x != child.anchorMax.x;
            bool isStretchY = child.anchorMin.y != child.anchorMax.y;

            float minX = isStretchX ? child.anchorMin.x : anchor.x;
            float maxX = isStretchX ? child.anchorMax.x : anchor.x;

            float minY = isStretchY ? child.anchorMin.y : anchor.y;
            float maxY = isStretchY ? child.anchorMax.y : anchor.y;

            child.anchorMin = new Vector2(minX, minY);
            child.anchorMax = new Vector2(maxX, maxY);
            child.pivot = anchor;

            if (isStretchX && isStretchY)
            {
                child.offsetMin = Vector2.zero;
                child.offsetMax = Vector2.zero;
            }
            else if (isStretchX)
            {
                child.offsetMin = new Vector2(0f, child.offsetMin.y);
                child.offsetMax = new Vector2(0f, child.offsetMax.y);
                child.anchoredPosition = new Vector2(0f, child.anchoredPosition.y);
            }
            else if (isStretchY)
            {
                child.offsetMin = new Vector2(child.offsetMin.x, 0f);
                child.offsetMax = new Vector2(child.offsetMax.x, 0f);
                child.anchoredPosition = new Vector2(child.anchoredPosition.x, 0f);
            }
            else
            {
                child.anchoredPosition = Vector2.zero;
            }
        }

        private Vector2 GetAnchor(
            Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0f, 1f);

                case Alignment.TopCenter:
                    return new Vector2(0.5f, 1f);

                case Alignment.TopRight:
                    return new Vector2(1f, 1f);

                case Alignment.CenterLeft:
                    return new Vector2(0f, 0.5f);

                case Alignment.Center:
                    return new Vector2(0.5f, 0.5f);

                case Alignment.CenterRight:
                    return new Vector2(1f, 0.5f);

                case Alignment.BottomLeft:
                    return new Vector2(0f, 0f);

                case Alignment.BottomCenter:
                    return new Vector2(0.5f, 0f);

                case Alignment.BottomRight:
                    return new Vector2(1f, 0f);

                default:
                    return new Vector2(0f, 1f);
            }
        }
    }
}