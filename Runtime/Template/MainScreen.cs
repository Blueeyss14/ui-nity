using UnityEngine;
using Uinity;

public class MainScreen : UinityScreen
{
    public override UIElement Build()
    {
        return new Vertical(
            new UIElement[]
            {
                Template.Build()
            }
        );
    }
}
