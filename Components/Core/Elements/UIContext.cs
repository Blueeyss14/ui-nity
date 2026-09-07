using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public static class UIContext
    {
        public static GameObject CreateObject(
            string name,
            Transform parent)
        {
            GameObject obj = new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer)
            );

            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

            return obj;
        }

        public static RectTransform GetRect(GameObject obj)
        {
            return obj.GetComponent<RectTransform>();
        }

        public static void ApplySize(
            RectTransform rect,
            Width width,
            Height height)
        {
            ApplyWidth(rect, width);
            ApplyHeight(rect, height);
        }

        private static void ApplyWidth(
            RectTransform rect,
            Width width)
        {
            if (width.IsFull)
            {
                rect.anchorMin = new Vector2(
                    rect.anchorMin.x,
                    rect.anchorMin.y
                );

                rect.anchorMax = new Vector2(
                    1f,
                    rect.anchorMax.y
                );

                rect.offsetMin = new Vector2(
                    rect.offsetMin.x,
                    rect.offsetMin.y
                );

                rect.offsetMax = new Vector2(
                    0f,
                    rect.offsetMax.y
                );
            }
            else
            {
                rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    width.Value
                );
            }
        }

        private static void ApplyHeight(
            RectTransform rect,
            Height height)
        {
            if (height.IsFull)
            {
                rect.anchorMin = new Vector2(
                    rect.anchorMin.x,
                    rect.anchorMin.y
                );

                rect.anchorMax = new Vector2(
                    rect.anchorMax.x,
                    1f
                );

                rect.offsetMin = new Vector2(
                    rect.offsetMin.x,
                    rect.offsetMin.y
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
                    height.Value
                );
            }
        }

        public static void AddImage(
            GameObject obj,
            Color color,
            Radius radius = default)
        {
            if (radius.HasValue && (radius.IsFull || radius.TopLeft > 0f || radius.TopRight > 0f || radius.BottomRight > 0f || radius.BottomLeft > 0f))
            {
                UIRoundedRectangle graphic = obj.AddComponent<UIRoundedRectangle>();
                graphic.color = color;
                graphic.radius = radius;
            }
            else
            {
                Image image = obj.AddComponent<Image>();
                image.color = color;
            }
        }
    }
}