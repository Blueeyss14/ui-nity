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
        private readonly Radius _radius;
        private readonly bool _clip;
        private readonly Padding _padding;
        private readonly Margin _margin;
        private readonly Opacity _opacity;
        private readonly Border _border;

        public Container(
            Color? color = null,
            Width width = default,
            Height height = default,
            Alignment alignment = Alignment.TopLeft,
            UIElement child = null,
            Radius radius = default,
            bool clip = false,
            Padding padding = default,
            Margin margin = default,
            Opacity opacity = default,
            Border border = default)
        {
            _color = color;
            _width = width;
            _height = height;
            _alignment = alignment;
            _child = child;
            _radius = radius;
            _clip = clip;
            _padding = padding;
            _margin = margin;
            _opacity = opacity;
            _border = border;
        }

        public override GameObject Build(Transform parent)
        {
            GameObject containerObj;
            RectTransform containerRect;
            LayoutElement layoutElement;

            bool hasMargin = _margin.HasValue;
            bool hasBorder = _border.HasValue && _border.Thickness > 0f;
            float b = hasBorder ? _border.Thickness : 0f;

            float marginLeft = hasMargin ? _margin.Left : 0f;
            float marginRight = hasMargin ? _margin.Right : 0f;
            float marginTop = hasMargin ? _margin.Top : 0f;
            float marginBottom = hasMargin ? _margin.Bottom : 0f;

            float extraLeft = marginLeft + b;
            float extraRight = marginRight + b;
            float extraTop = marginTop + b;
            float extraBottom = marginBottom + b;

            float totalExtraX = extraLeft + extraRight;
            float totalExtraY = extraTop + extraBottom;

            if (hasMargin || hasBorder)
            {
                GameObject marginObj = UIContext.CreateObject(
                    "ContainerMargin",
                    parent
                );
                RectTransform marginRect = UIContext.GetRect(marginObj);
                ApplyDefaults(marginRect);

                layoutElement = marginObj.AddComponent<LayoutElement>();

                if (_width.IsFull)
                {
                    ApplyWidth(marginRect);
                    layoutElement.flexibleWidth = 1f;
                }
                else if (_width.Value > 0)
                {
                    ApplyWidth(marginRect, totalExtraX);
                    layoutElement.preferredWidth = _width.Value + totalExtraX;
                    layoutElement.minWidth = _width.Value + totalExtraX;
                    layoutElement.flexibleWidth = 0f;
                }

                if (_height.IsFull)
                {
                    ApplyHeight(marginRect);
                    layoutElement.flexibleHeight = 1f;
                }
                else if (_height.Value > 0)
                {
                    ApplyHeight(marginRect, totalExtraY);
                    layoutElement.preferredHeight = _height.Value + totalExtraY;
                    layoutElement.minHeight = _height.Value + totalExtraY;
                    layoutElement.flexibleHeight = 0f;
                }

                containerObj = UIContext.CreateObject(
                    "Container",
                    marginObj.transform
                );
                containerRect = UIContext.GetRect(containerObj);
                containerRect.anchorMin = Vector2.zero;
                containerRect.anchorMax = Vector2.one;
                containerRect.offsetMin = new Vector2(extraLeft, extraBottom);
                containerRect.offsetMax = new Vector2(-extraRight, -extraTop);
            }
            else
            {
                containerObj = UIContext.CreateObject(
                    "Container",
                    parent
                );
                containerRect = UIContext.GetRect(containerObj);
                ApplyDefaults(containerRect);

                layoutElement = containerObj.AddComponent<LayoutElement>();

                if (_width.IsFull)
                {
                    ApplyWidth(containerRect);
                    layoutElement.flexibleWidth = 1f;
                }
                else if (_width.Value > 0)
                {
                    ApplyWidth(containerRect);
                    layoutElement.preferredWidth = _width.Value;
                    layoutElement.minWidth = _width.Value;
                    layoutElement.flexibleWidth = 0f;
                }

                if (_height.IsFull)
                {
                    ApplyHeight(containerRect);
                    layoutElement.flexibleHeight = 1f;
                }
                else if (_height.Value > 0)
                {
                    ApplyHeight(containerRect);
                    layoutElement.preferredHeight = _height.Value;
                    layoutElement.minHeight = _height.Value;
                    layoutElement.flexibleHeight = 0f;
                }
            }

            if (_color.HasValue)
            {
                Color finalColor = _color.Value;
                if (_opacity.HasValue)
                {
                    finalColor.a *= Mathf.Clamp01(_opacity.Value);
                }

                UIContext.AddImage(
                    containerObj,
                    finalColor,
                    _radius
                );
            }

            if (_clip)
            {
                UIContext.ApplyClip(
                    containerObj,
                    _radius,
                    _color.HasValue
                );
            }

            if (_border.HasValue && _border.Thickness > 0f)
            {
                UIContext.AddBorder(
                    containerObj,
                    _border,
                    _radius
                );
            }

            if (_child != null)
            {
                GameObject child =
                    _child.Build(containerObj.transform);

                RectTransform childRect =
                    child.GetComponent<RectTransform>();

                ApplyChildAlignment(childRect);

                bool isAutoWidth = !_width.IsFull && _width.Value <= 0f;
                bool isAutoHeight = !_height.IsFull && _height.Value <= 0f;

                if (isAutoWidth || isAutoHeight)
                {
                    UIAutoContainerSize autoSize = containerObj.AddComponent<UIAutoContainerSize>();
                    autoSize.autoWidth = isAutoWidth;
                    autoSize.autoHeight = isAutoHeight;
                    autoSize.padding = _padding;
                    autoSize.margin = _margin;
                    autoSize.border = _border;
                    autoSize.marginObj = (hasMargin || hasBorder) ? containerObj.transform.parent.gameObject : null;
                    autoSize.UpdateSize();
                }
            }

            GameObject finalRoot = (hasMargin || hasBorder) ? containerObj.transform.parent.gameObject : containerObj;

            return finalRoot;
        }

        private void ApplyDefaults(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void ApplyWidth(RectTransform rect, float extraSize = 0f)
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
                    _width.Value + extraSize
                );
            }
        }

        private void ApplyHeight(RectTransform rect, float extraSize = 0f)
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
                    _height.Value + extraSize
                );
            }
        }

        private void ApplyChildAlignment(
            RectTransform child)
        {
            TMPro.TextMeshProUGUI tmp = child.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmp != null && child.GetComponent<TextExplicitAlignMarker>() == null)
            {
                Text.ApplyAlignment(tmp, _alignment);
            }

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

            float padLeft = _padding.Left;
            float padRight = _padding.Right;
            float padTop = _padding.Top;
            float padBottom = _padding.Bottom;

            if (isStretchX && isStretchY)
            {
                child.offsetMin = new Vector2(padLeft, padBottom);
                child.offsetMax = new Vector2(-padRight, -padTop);
            }
            else if (isStretchX)
            {
                child.offsetMin = new Vector2(padLeft, child.offsetMin.y);
                child.offsetMax = new Vector2(-padRight, child.offsetMax.y);

                float yOffset = 0f;
                if (anchor.y == 1f) yOffset = -padTop;
                else if (anchor.y == 0f) yOffset = padBottom;
                else yOffset = (padBottom - padTop) * 0.5f;

                child.anchoredPosition = new Vector2(0f, yOffset);
            }
            else if (isStretchY)
            {
                child.offsetMin = new Vector2(child.offsetMin.x, padBottom);
                child.offsetMax = new Vector2(child.offsetMax.x, -padTop);

                float xOffset = 0f;
                if (anchor.x == 0f) xOffset = padLeft;
                else if (anchor.x == 1f) xOffset = -padRight;
                else xOffset = (padLeft - padRight) * 0.5f;

                child.anchoredPosition = new Vector2(xOffset, 0f);
            }
            else
            {
                float xOffset = 0f;
                if (anchor.x == 0f) xOffset = padLeft;
                else if (anchor.x == 1f) xOffset = -padRight;
                else xOffset = (padLeft - padRight) * 0.5f;

                float yOffset = 0f;
                if (anchor.y == 1f) yOffset = -padTop;
                else if (anchor.y == 0f) yOffset = padBottom;
                else yOffset = (padBottom - padTop) * 0.5f;

                child.anchoredPosition = new Vector2(xOffset, yOffset);
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