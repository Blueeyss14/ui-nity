using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Uinity
{
    [ExecuteAlways]
    public abstract class UinityScreen : MonoBehaviour
    {
        private Vector2 _lastScreenSize;
        private bool _isRebuildPending;

        protected virtual void OnEnable()
        {
            CheckAndRebuild(force: true);
        }

        protected virtual void OnValidate()
        {
            CheckAndRebuild(force: true);
        }

        protected virtual void Update()
        {
            CheckAndRebuild(force: false);
        }

        protected virtual void OnRectTransformDimensionsChange()
        {
            CheckAndRebuild(force: false);
        }

        public void CheckAndRebuild(bool force = false)
        {
            Vector2 currentScreenSize = new Vector2(Width.screen.Value, Height.screen.Value);
            if (force || currentScreenSize != _lastScreenSize)
            {
                _lastScreenSize = currentScreenSize;
                Rebuild();
            }
        }

        public void Rebuild()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.delayCall -= ExecuteEditModeRebuild;
                EditorApplication.delayCall += ExecuteEditModeRebuild;
                return;
            }
#endif
            DoRebuild();
        }

#if UNITY_EDITOR
        private void ExecuteEditModeRebuild()
        {
            if (this == null) return;
            DoRebuild();
        }
#endif

        private void DoRebuild()
        {
            Clear();
            UIElement root = Build();
            if (root != null)
            {
                GameObject obj = root.Build(transform);
#if UNITY_EDITOR
                if (!Application.isPlaying && obj != null)
                {
                    Canvas.ForceUpdateCanvases();
                    RectTransform rootRect = obj.GetComponent<RectTransform>();
                    if (rootRect != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);
                    }
                    UIAutoContainerSize[] autoSizes = obj.GetComponentsInChildren<UIAutoContainerSize>();
                    foreach (var autoSize in autoSizes)
                    {
                        autoSize.UpdateSize();
                    }
                    Canvas.ForceUpdateCanvases();
                }
#endif
            }
        }

        public abstract UIElement Build();

        public void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DestroyImmediate(child);
                }
                else
                {
                    Destroy(child);
                }
#else
                Destroy(child);
#endif
            }
        }
    }
}
