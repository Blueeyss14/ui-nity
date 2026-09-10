using UnityEngine;
using Uinity;

public class MainUinity : UinityScreen
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
