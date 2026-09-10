using UnityEngine;

namespace Uinity
{
    public abstract class UIElement
    {
        public abstract GameObject Build(Transform parent);
    }
}