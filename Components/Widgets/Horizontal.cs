using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    public class Horizontal : UIElement
    {
        private readonly UIElement[] _children;

        public Horizontal(params UIElement[] children)
        {
            _children = children;
        }

        public override GameObject Build(
            Transform parent)
        {
            GameObject obj =
                UIContext.CreateObject(
                    "Horizontal",
                    parent
                );

            RectTransform rect =
                UIContext.GetRect(obj);

            rect.anchorMin =
                new Vector2(0f, 1f);

            rect.anchorMax =
                new Vector2(1f, 1f);

            rect.pivot =
                new Vector2(0f, 1f);

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            HorizontalLayoutGroup layout =
                obj.AddComponent<HorizontalLayoutGroup>();

            layout.childAlignment =
                TextAnchor.UpperLeft;

            layout.childControlWidth = true;
            layout.childControlHeight = true;

            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            layout.spacing = 0f;

            ContentSizeFitter fitter =
                obj.AddComponent<ContentSizeFitter>();

            fitter.horizontalFit =
                ContentSizeFitter.FitMode.Unconstrained;

            fitter.verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            LayoutElement layoutElement =
                obj.AddComponent<LayoutElement>();
            layoutElement.flexibleWidth = 1f;

            foreach (UIElement child in _children)
            {
                if (child != null)
                {
                    child.Build(obj.transform);
                }
            }

            return obj;
        }
    }
}