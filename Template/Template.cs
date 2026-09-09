using UnityEngine;
using Uinity;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class Template : MonoBehaviour
{
    private Vector2 _lastScreenSize;
    private bool _isRebuildPending;

    private void OnEnable()
    {
        CheckAndRebuild(force: true);
    }

    private void OnValidate()
    {
        CheckAndRebuild(force: true);
    }

    private void Update()
    {
        CheckAndRebuild(force: false);
    }

    private void OnRectTransformDimensionsChange()
    {
        CheckAndRebuild(force: false);
    }

    private void CheckAndRebuild(bool force = false)
    {
        Vector2 currentScreenSize = new Vector2(Width.screen.Value, Height.screen.Value);
        if (force || currentScreenSize != _lastScreenSize)
        {
            _lastScreenSize = currentScreenSize;
            Rebuild();
        }
    }

    private void Rebuild()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (_isRebuildPending) return;
            _isRebuildPending = true;
            EditorApplication.delayCall += () =>
            {
                _isRebuildPending = false;
                if (this == null) return;
                DoRebuild();
            };
            return;
        }
#endif
        DoRebuild();
    }

    private void DoRebuild()
    {
        Clear();
        new Container(
            alignment: Alignment.Center,
            color: Color.red,
            width: Width.full,
            height: Height.full,
            child: new Vertical(
                gap: 80,

                new UIElement[]{
                new Container(
                // padding: new SetPadding(10),
                margin: new SetMargin(20),
                clip: true,
                radius: new SetRadiusOnly(topLeft: 20, bottomRight: 100, bottomLeft: 50),
                color: Color.green,
                // width: 400,
                // height: 200,
                child: new Horizontal(
                    new UIElement[]
                    {
                        new Vertical(
                            padding: new SetPadding(0),
                            new UIElement[] {
                                new Container(
                                    color: Color.yellow,
                                    width: 100,
                                    height: 100
                                ),
                                new Container(
                                    color: Color.blue,
                                    width: 100,
                                    height: 100
                                )
                            }
                        ),
                        new Vertical(
                            new UIElement[] {
                                new Container(
                                    color: Color.blue,
                                    width: 100,
                                    height: 100
                                ),
                                new Container(
                                    color: Color.yellow,
                                    width: 100,
                                    height: 100
                                )
                            }
                        ),


                    }
                )
            ),
             new Container(
                radius: new SetRadiusX(0, 20),
                                    color: Color.yellow,
                                    width: 200,
                                    height: 100
                                ),
             new Container(
                radius: new SetRadiusY(30, 0),
                                    color: Color.blue,
                                    width: 200,
                                    height: 100
                                ),
             new Container(
                radius: new Radius(Radius.full),
                                    color: Color.blue,
                                    width: 100,
                                    height: 100
                                ),
             new Container(
                radius: new Radius(20),
                                    color: Color.yellow,
                                    width: 100,
                                    height: 100
                                ),
             new Container(
                radius: new Radius(20),
                                    color: Color.blue,
                                    width: Width.screen / 2,
                                    height: 100
                                ),
            new Horizontal(
                new UIElement[]
                {
                    new Container(
                    margin: new SetMarginX(40),
                radius: new Radius(20),
                                    color: Color.green,
                                    width: Width.screen / 3,
                                    height: 100
                                ),
                    new Container(
                radius: new Radius(20),
                                    color: Color.green,
                                    width: Width.screen / 3,
                                    height: 100
                                ),
                }
            )
                }
            )
        ).Build(transform);
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child =
                transform.GetChild(i).gameObject;

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