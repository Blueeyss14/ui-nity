using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Uinity
{
    public class TextExplicitAlignMarker : MonoBehaviour { }

    public class Text : UIElement
    {
        private readonly string _content;
        private readonly float _size;
        private readonly Color? _color;
        private readonly FontWeight _weight;
        private readonly TextOverflow _overflow;
        private readonly TextBorder _border;
        private readonly TextAlign? _align;
        private readonly Alignment? _alignment;
        private readonly Width _width;
        private readonly Height _height;
        private readonly string _name;

        // Static aliases so users can write TextOverflow.ellipsis or Text.ellipsis
        public static TextOverflow clip => TextOverflow.clip;
        public static TextOverflow ellipsis => TextOverflow.ellipsis;
        public static TextOverflow fade => TextOverflow.fade;
        public static TextOverflow visible => TextOverflow.visible;

        public static TextOverflow Clip => TextOverflow.clip;
        public static TextOverflow Ellipsis => TextOverflow.ellipsis;
        public static TextOverflow Fade => TextOverflow.fade;
        public static TextOverflow Visible => TextOverflow.visible;

        // Static aliases for FontWeight so users can write FontWeight.bold or Text.bold
        public static FontWeight thin => FontWeight.thin;
        public static FontWeight extraLight => FontWeight.extraLight;
        public static FontWeight light => FontWeight.light;
        public static FontWeight normal => FontWeight.normal;
        public static FontWeight medium => FontWeight.medium;
        public static FontWeight semiBold => FontWeight.semiBold;
        public static FontWeight bold => FontWeight.bold;
        public static FontWeight extraBold => FontWeight.extraBold;
        public static FontWeight heavy => FontWeight.heavy;
        public static FontWeight black => FontWeight.black;

        public Text(
            string content,
            float size = 16f,
            Color? color = null,
            FontWeight weight = FontWeight.normal,
            TextOverflow overflow = TextOverflow.clip,
            TextBorder border = default,
            TextAlign? align = null,
            Alignment? alignment = null,
            Width width = default,
            Height height = default,
            string name = "Text")
        {
            _content = content ?? string.Empty;
            _size = size <= 0f ? 16f : size;
            _color = color;
            _weight = weight;
            _overflow = overflow;
            _border = border;
            _align = align;
            _alignment = alignment;
            _width = width;
            _height = height;
            _name = string.IsNullOrEmpty(name) ? "Text" : name;
        }

        public override GameObject Build(Transform parent)
        {
            GameObject obj = UIContext.CreateObject(_name, parent);
            RectTransform rect = UIContext.GetRect(obj);

            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            LayoutElement layoutElement = obj.AddComponent<LayoutElement>();

            bool isAutoWidth = !_width.IsFull && _width.Value <= 0f;
            bool isAutoHeight = !_height.IsFull && _height.Value <= 0f;

            if (_width.IsFull)
            {
                rect.anchorMin = new Vector2(0f, rect.anchorMin.y);
                rect.anchorMax = new Vector2(1f, rect.anchorMax.y);
                layoutElement.flexibleWidth = 1f;
            }
            else if (_width.Value > 0f)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width.Value);
                layoutElement.preferredWidth = _width.Value;
                layoutElement.minWidth = _width.Value;
                layoutElement.flexibleWidth = 0f;
            }

            if (_height.IsFull)
            {
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0f);
                rect.anchorMax = new Vector2(rect.anchorMax.x, 1f);
                layoutElement.flexibleHeight = 1f;
            }
            else if (_height.Value > 0f)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _height.Value);
                layoutElement.preferredHeight = _height.Value;
                layoutElement.minHeight = _height.Value;
                layoutElement.flexibleHeight = 0f;
            }

            if (isAutoWidth || isAutoHeight)
            {
                ContentSizeFitter csf = obj.AddComponent<ContentSizeFitter>();
                if (isAutoWidth) csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                if (isAutoHeight) csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
            tmp.text = _content;
            tmp.fontSize = _size;
            if (_color.HasValue)
            {
                tmp.color = _color.Value;
            }

            switch (_weight)
            {
                case FontWeight.thin:
                    tmp.fontWeight = TMPro.FontWeight.Thin;
                    break;
                case FontWeight.extraLight:
                    tmp.fontWeight = TMPro.FontWeight.ExtraLight;
                    break;
                case FontWeight.light:
                    tmp.fontWeight = TMPro.FontWeight.Light;
                    break;
                case FontWeight.medium:
                    tmp.fontWeight = TMPro.FontWeight.Medium;
                    break;
                case FontWeight.semiBold:
                    tmp.fontWeight = TMPro.FontWeight.SemiBold;
                    break;
                case FontWeight.bold:
                    tmp.fontWeight = TMPro.FontWeight.Bold;
                    tmp.fontStyle |= FontStyles.Bold;
                    break;
                case FontWeight.extraBold:
                case FontWeight.heavy:
                    tmp.fontWeight = TMPro.FontWeight.Heavy;
                    tmp.fontStyle |= FontStyles.Bold;
                    break;
                case FontWeight.black:
                    tmp.fontWeight = TMPro.FontWeight.Black;
                    tmp.fontStyle |= FontStyles.Bold;
                    break;
                case FontWeight.normal:
                default:
                    tmp.fontWeight = TMPro.FontWeight.Regular;
                    break;
            }

            switch (_overflow)
            {
                case TextOverflow.ellipsis:
                    tmp.overflowMode = TextOverflowModes.Ellipsis;
                    tmp.textWrappingMode = TextWrappingModes.NoWrap;
                    break;
                case TextOverflow.fade:
                    tmp.overflowMode = TextOverflowModes.Masking;
                    break;
                case TextOverflow.visible:
                    tmp.overflowMode = TextOverflowModes.Overflow;
                    break;
                case TextOverflow.clip:
                default:
                    tmp.overflowMode = TextOverflowModes.Truncate;
                    break;
            }

            if (_align.HasValue || _alignment.HasValue)
            {
                obj.AddComponent<TextExplicitAlignMarker>();

                if (_align.HasValue)
                {
                    switch (_align.Value)
                    {
                        case TextAlign.center:
                            tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                            break;
                        case TextAlign.right:
                            tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
                            break;
                        case TextAlign.justify:
                            tmp.horizontalAlignment = HorizontalAlignmentOptions.Justified;
                            break;
                        case TextAlign.left:
                        default:
                            tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                            break;
                    }
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                }
                else if (_alignment.HasValue)
                {
                    ApplyAlignment(tmp, _alignment.Value);
                }
            }
            else
            {
                tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
            }

            if (_border.HasValue && _border.Thickness > 0f)
            {
                float normalizedThickness = Mathf.Clamp01(_border.Thickness * 0.1f);

                if (tmp.fontMaterial != null)
                {
                    tmp.fontMaterial = new Material(tmp.fontMaterial);
                    tmp.fontMaterial.EnableKeyword(ShaderUtilities.Keyword_Outline);
                    tmp.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, normalizedThickness);
                    tmp.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, _border.Color);
                }

                tmp.outlineWidth = normalizedThickness;
                tmp.outlineColor = _border.Color;
            }

            return obj;
        }

        public static void ApplyAlignment(TextMeshProUGUI tmp, Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.topLeft:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Top;
                    break;
                case Alignment.topCenter:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Top;
                    break;
                case Alignment.topRight:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Top;
                    break;

                case Alignment.centerLeft:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                    break;
                case Alignment.center:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                    break;
                case Alignment.centerRight:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                    break;

                case Alignment.bottomLeft:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Bottom;
                    break;
                case Alignment.bottomCenter:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Bottom;
                    break;
                case Alignment.bottomRight:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Bottom;
                    break;
            }
        }
    }
}
