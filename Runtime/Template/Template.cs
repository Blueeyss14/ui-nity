using UnityEngine;
using Uinity;

public static class Template
{
    public static UIElement Build()
    {
        return new Container(
            opacity: 0.5,
            alignment: Alignment.center,
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
                        alignment: Alignment.center,
                    margin: new SetMarginX(40),
                radius: new Radius(20),
                                    color: Color.green,
                                    width: Width.screen / 3,
                                    height: 100,
                                    child: new Text("This is a text",
                                    size: 50f,
                                    weight: FontWeight.bold,
                                    border: new TextBorder(
                                        thickness: 2f,
                                        Color: Color.black
                                    )

                                    )
                                ),
                    new Container(
                        alignment: Alignment.center,
                radius: new Radius(20),
                                    color: Color.green,
                                    width: Width.screen / 3,
                                    height: 100,
                                    child: new Image(
                                        name: "Image Name",
                                        path: "Assets/Sprites/Hotkeys",
                                        fit: Image.contain
                                    )
                                ),
                }
            )
                }
            )
        );
    }
}