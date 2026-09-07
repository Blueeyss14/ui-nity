using UnityEngine;
using Uinity;

[ExecuteAlways]
public class Home : MonoBehaviour
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
            child: new Container(
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