using UnityEngine;
using UnityEngine.UI;

namespace Uinity
{
    [ExecuteAlways]
    public class UILayoutGapAdapter : MonoBehaviour
    {
        public float targetGap;
        public bool isVertical;

        private HorizontalOrVerticalLayoutGroup _layoutGroup;
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            UpdateSpacing();
        }

        private void Update()
        {
            UpdateSpacing();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateSpacing();
        }

        public void UpdateSpacing()
        {
            if (_layoutGroup == null)
                _layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_layoutGroup == null || _rectTransform == null)
                return;

            int childCount = 0;
            float totalChildrenSize = 0f;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (!childTransform.gameObject.activeSelf)
                    continue;

                RectTransform childRect = childTransform as RectTransform;
                if (childRect == null)
                    continue;

                childCount++;
                if (isVertical)
                {
                    float h = LayoutUtility.GetPreferredHeight(childRect);
                    if (h <= 0f) h = childRect.rect.height;
                    totalChildrenSize += h;
                }
                else
                {
                    float w = LayoutUtility.GetPreferredWidth(childRect);
                    if (w <= 0f) w = childRect.rect.width;
                    totalChildrenSize += w;
                }
            }

            if (childCount <= 1)
            {
                _layoutGroup.spacing = 0f;
                return;
            }

            float parentSize = isVertical ? _rectTransform.rect.height : _rectTransform.rect.width;
            if (parentSize <= 0f)
            {
                parentSize = isVertical ? Screen.height : Screen.width;
            }

            float availableSpace = parentSize - totalChildrenSize;
            float maxGap = availableSpace > 0f ? availableSpace / (childCount - 1) : 0f;

            _layoutGroup.spacing = Mathf.Clamp(targetGap, 0f, maxGap);
        }
    }
}
