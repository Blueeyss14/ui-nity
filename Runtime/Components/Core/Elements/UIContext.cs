using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public static class UIContext
    {
        public static readonly Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>();

        public static void PreserveSprites(Transform parent)
        {
            if (parent == null) return;
            foreach (Transform child in parent)
            {
                UnityEngine.UI.Image img = child.GetComponent<UnityEngine.UI.Image>();
                if (img != null && img.sprite != null)
                {
                    SpriteCache[child.name] = img.sprite;
                }
                PreserveSprites(child);
            }
        }

        public static GameObject CreateObject(
            string name,
            Transform parent)
        {
            if (parent != null)
            {
                Transform existingChild = parent.Find(name);
                if (existingChild != null)
                {
                    UnityEngine.UI.Image existingImg = existingChild.GetComponent<UnityEngine.UI.Image>();
                    if (existingImg != null && existingImg.sprite != null)
                    {
                        SpriteCache[name] = existingImg.sprite;
                    }
                }
            }

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
                UnityEngine.UI.Image image = obj.AddComponent<UnityEngine.UI.Image>();
                image.color = color;
            }
        }

        public static void ApplyClip(
            GameObject obj,
            Radius radius = default,
            bool hasColor = false)
        {
            bool hasRadius = radius.HasValue && (radius.IsFull || radius.TopLeft > 0f || radius.TopRight > 0f || radius.BottomRight > 0f || radius.BottomLeft > 0f);

            if (hasRadius)
            {
                if (!hasColor)
                {
                    AddImage(obj, Color.clear, radius);
                }
                Mask mask = obj.AddComponent<Mask>();
                mask.showMaskGraphic = hasColor;
            }
            else if (hasColor)
            {
                Mask mask = obj.AddComponent<Mask>();
                mask.showMaskGraphic = true;
            }
            else
            {
                obj.AddComponent<RectMask2D>();
            }
        }

        public static void AddBorder(
            GameObject obj,
            Border border,
            Radius radius = default)
        {
            if (!border.HasValue || border.Thickness <= 0f) return;

            GameObject borderObj = CreateObject("Border", obj.transform);
            RectTransform borderRect = GetRect(borderObj);
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.offsetMin = Vector2.zero;
            borderRect.offsetMax = Vector2.zero;

            LayoutElement le = borderObj.AddComponent<LayoutElement>();
            le.ignoreLayout = true;

            UIRoundedBorder graphic = borderObj.AddComponent<UIRoundedBorder>();
            graphic.color = border.Color;
            graphic.radius = radius;
            graphic.thickness = border.Thickness;
            graphic.raycastTarget = false;
            graphic.maskable = false;
        }
    }
}