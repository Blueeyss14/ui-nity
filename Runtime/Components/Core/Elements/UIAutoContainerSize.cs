using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    [ExecuteAlways]
    public class UIAutoContainerSize : MonoBehaviour
    {
        public bool autoWidth;
        public bool autoHeight;
        public Padding padding;
        public Margin margin;
        public Border border;
        public GameObject marginObj;

        private RectTransform _rectTransform;
        private LayoutElement _layoutElement;

        private void OnEnable()
        {
            UpdateSize();
        }

        private void Update()
        {
            UpdateSize();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateSize();
        }

        public void UpdateSize()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            GameObject targetObj = marginObj != null ? marginObj : gameObject;
            if (_layoutElement == null)
                _layoutElement = targetObj.GetComponent<LayoutElement>();

            if (_rectTransform == null || transform.childCount == 0)
                return;

            float maxChildWidth = 0f;
            float maxChildHeight = 0f;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (!childTransform.gameObject.activeSelf)
                    continue;

                RectTransform childRect = childTransform as RectTransform;
                if (childRect == null)
                    continue;

                LayoutElement le = childRect.GetComponent<LayoutElement>();
                if (le != null && le.ignoreLayout)
                    continue;

                float w = LayoutUtility.GetPreferredWidth(childRect);
                if (w <= 0f) w = childRect.rect.width;
                if (w > maxChildWidth) maxChildWidth = w;

                float h = LayoutUtility.GetPreferredHeight(childRect);
                if (h <= 0f) h = childRect.rect.height;
                if (h > maxChildHeight) maxChildHeight = h;
            }

            float borderExtra = (border.HasValue && border.Thickness > 0f) ? border.Thickness * 2f : 0f;

            if (autoWidth && maxChildWidth > 0f)
            {
                float innerW = maxChildWidth + padding.Left + padding.Right;
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, innerW);
                float extraW = (margin.HasValue ? margin.Left + margin.Right : 0f) + borderExtra;
                if (marginObj != null)
                {
                    RectTransform marginRect = marginObj.GetComponent<RectTransform>();
                    if (marginRect != null)
                    {
                        marginRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, innerW + extraW);
                    }
                }
                if (_layoutElement != null)
                {
                    float totalW = innerW + extraW;
                    _layoutElement.preferredWidth = totalW;
                    _layoutElement.minWidth = totalW;
                }
            }

            if (autoHeight && maxChildHeight > 0f)
            {
                float innerH = maxChildHeight + padding.Top + padding.Bottom;
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, innerH);
                float extraH = (margin.HasValue ? margin.Top + margin.Bottom : 0f) + borderExtra;
                if (marginObj != null)
                {
                    RectTransform marginRect = marginObj.GetComponent<RectTransform>();
                    if (marginRect != null)
                    {
                        marginRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, innerH + extraH);
                    }
                }
                if (_layoutElement != null)
                {
                    float totalH = innerH + extraH;
                    _layoutElement.preferredHeight = totalH;
                    _layoutElement.minHeight = totalH;
                }
            }
        }
    }
}
