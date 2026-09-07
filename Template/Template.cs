using UnityEngine;
using Uinity;

[ExecuteAlways]
public class Template : MonoBehaviour
{
    private void OnEnable()
    {
        Rebuild();
    }

    private void OnValidate()
    {
        Rebuild();
    }

    private void Rebuild()
    {
        Clear();
        new Container(
            alignment: Alignment.Center,
            color: Color.red,
            width: Width.Full,
            height: Height.Full,
            child: new Vertical(
                new UIElement[]{
                new Container(
                clip: true,
                radius: new SetRadiusOnly(topLeft: 20, bottomRight: 100, bottomLeft: 50),
                color: Color.green,
                width: 400,
                height: 200,
                child: new Horizontal(
                    new UIElement[]
                    {
                        new Vertical(
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
                        new Container(
                            width: 100,
                            height: 200
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