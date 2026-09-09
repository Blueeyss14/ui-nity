using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public class Image : UIElement
    {
        private readonly string _name;
        private readonly Sprite _sprite;
        private readonly Texture2D _texture;
        private readonly Color? _color;
        private readonly Width _width;
        private readonly Height _height;
        private readonly ImageFit _fit;

        public static ImageFit fill => ImageFit.fill;
        public static ImageFit contain => ImageFit.contain;
        public static ImageFit cover => ImageFit.cover;
        public static ImageFit fitWidth => ImageFit.fitWidth;
        public static ImageFit fitHeight => ImageFit.fitHeight;
        public static ImageFit none => ImageFit.none;
        public static ImageFit scaleDown => ImageFit.scaleDown;

        public static ImageFit Fill => ImageFit.fill;
        public static ImageFit Contain => ImageFit.contain;
        public static ImageFit Cover => ImageFit.cover;
        public static ImageFit FitWidth => ImageFit.fitWidth;
        public static ImageFit FitHeight => ImageFit.fitHeight;
        public static ImageFit None => ImageFit.none;
        public static ImageFit ScaleDown => ImageFit.scaleDown;

        // Flutter-style Image.asset("path") helper
        public static Image asset(string path, ImageFit fit = ImageFit.fill, Width width = default, Height height = default, string name = "Image")
        {
            return new Image(name: name, path: path, fit: fit, width: width, height: height);
        }

        public static Image Asset(string path, ImageFit fit = ImageFit.fill, Width width = default, Height height = default, string name = "Image")
        {
            return asset(path, fit, width, height, name);
        }

        public Image(
            string name = "Image",
            Sprite sprite = null,
            string path = null,
            ImageFit fit = ImageFit.fill,
            Texture2D texture = null,
            Color? color = null,
            Width width = default,
            Height height = default)
        {
            _name = string.IsNullOrEmpty(name) ? "Image" : name;
            _sprite = sprite != null ? sprite : (!string.IsNullOrEmpty(path) ? Resources.Load<Sprite>(path) : null);
            _fit = fit;
            _texture = texture;
            _color = color;
            _width = width;
            _height = height;
        }

        public Image(Sprite sprite, ImageFit fit = ImageFit.fill)
            : this(name: "Image", sprite: sprite, fit: fit)
        {
        }

        public override GameObject Build(Transform parent)
        {
            GameObject obj = UIContext.CreateObject(
                _name,
                parent
            );

            RectTransform rect = UIContext.GetRect(obj);

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            LayoutElement layoutElement = obj.AddComponent<LayoutElement>();

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

            UnityEngine.UI.Image uiImage = obj.AddComponent<UnityEngine.UI.Image>();

            if (_color.HasValue)
            {
                uiImage.color = _color.Value;
            }

            Sprite targetSprite = _sprite;

            if (targetSprite == null && UIContext.SpriteCache.TryGetValue(_name, out Sprite cachedSprite))
            {
                targetSprite = cachedSprite;
            }

            if (targetSprite == null && _texture != null)
            {
                targetSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            }

            if (targetSprite != null)
            {
                uiImage.sprite = targetSprite;
            }
            else if (!_color.HasValue)
            {
                uiImage.color = Color.clear;
            }

            ApplyFit(obj, uiImage, targetSprite, _fit);

            return obj;
        }

        private void ApplyFit(GameObject obj, UnityEngine.UI.Image uiImage, Sprite sprite, ImageFit fit)
        {
            if (fit == ImageFit.fill)
            {
                uiImage.preserveAspect = false;
                return;
            }

            if (fit == ImageFit.none)
            {
                uiImage.SetNativeSize();
                return;
            }

            float spriteAspect = 1f;
            if (sprite != null && sprite.rect.height > 0f)
            {
                spriteAspect = sprite.rect.width / sprite.rect.height;
            }

            if (fit == ImageFit.contain)
            {
                uiImage.preserveAspect = true;
                return;
            }

            AspectRatioFitter fitter = obj.AddComponent<AspectRatioFitter>();
            fitter.aspectRatio = spriteAspect;

            switch (fit)
            {
                case ImageFit.cover:
                    fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                    break;
                case ImageFit.fitWidth:
                    fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                    break;
                case ImageFit.fitHeight:
                    fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                    break;
                case ImageFit.scaleDown:
                    fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                    break;
                default:
                    uiImage.preserveAspect = false;
                    break;
            }
        }
    }
}
