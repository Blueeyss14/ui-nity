# Uinity

## Installation
Open Unity Package Manager.

Copy and paste this to download the package.

```
https://github.com/Blueeyss14/ui-nity.git
```

## Container

How to make a simple container

```bash

new Container(
    color: Color.blue,
    width: 100,
    height: 100
    child: <child>),

```

### Container Attributes

```bash

new Container(
    clip: true,
    opacity: 0.5,
    padding: new SetPadding(10),
    margin: new SetMargin(10),
    alignment: Alignment.center,
    width: 100,
    height: 100,
    color: Color.blue,
    border: new Border(
        thickness: 10f,
        color: Color.black,    
    radius: new Radius(Radius.full),
    )
    child: <child>)

```

### Container (Size)

Define container size in pixel

```bash

new Container(
    width: 100,
    height: 100)

```

Define container screen size

```bash

new Container(
    width: Width.screen,
    height: Height.screen / 2)

```

### Container (Padding and Margin)

```bash

new Container(
    padding: new SetPadding(10),
    margin: new SetMargin(10)
),

```

Define horizontal(X) or vertical(Y):

```bash

new Container(
    padding: new SetPaddingX(value),
    margin: new SetMarginY(value)
),

```

Make it specific

```bash

new Container(
    padding: new SetPaddingX(left, right),
    margin: new SetMarginY(top, bottom)
),

```

Or

```bash

new Container(
    padding: new SetPaddingOnly(left: 10, right: 2, top: 12, bottom: 20),
    margin: new SetMarginOnly(left: 10, right: 2, top: 12, bottom: 20)
),

```

## Direction (Vertical & Horizontal)

How to use:

```bash

new Vertical(
    padding: new SetPadding(10),
    new UIElement[]
    {
        new <child>,
        new <child>,
        new <child>
    }
)

```

```bash

new Horizontal(
    padding: new SetPadding(10),
    new UIElement[]
    {
        new <child>,
        new <child>,
        new <child>
    }
)

```

## Text

Text Attributes:

```bash
new Text(
    "This is a text",
    size: 50f,
    color: Color.black,
    weight: FontWeight.bold,
    border: new TextBorder(
        thickness: 2f,
        color: Color.black
    )
)
```

### Text (Alignment & Overflow)

```bash
new Text(
    "Long text example...",
    size: 20f,
    color: Color.white,
    weight: FontWeight.medium,
    align: TextAlign.center,          // TextAlign.left, center, right, justify
    overflow: TextOverflow.ellipsis   // TextOverflow.clip, ellipsis, fade, visible
)
```

### Font Weight Options
Available `FontWeight` options:
- `FontWeight.thin` (w100)
- `FontWeight.extraLight` (w200)
- `FontWeight.light` (w300)
- `FontWeight.normal` (w400)
- `FontWeight.medium` (w500)
- `FontWeight.semiBold` (w600)
- `FontWeight.bold` (w700)
- `FontWeight.extraBold` (w800)
- `FontWeight.heavy`
- `FontWeight.black` (w900)

## Image

How to use:

```bash
new Image(
    name: "HotkeysIcon",
    path: "Assets/Sprites/Hotkeys",
    fit: Image.contain
)
```

Using Flutter-style `Image.asset`:

```bash
Image.asset(
    "Assets/Sprites/Hotkeys",
    fit: ImageFit.contain,
    width: 100,
    height: 100
)
```

Using `Sprite` directly:

```bash
new Image(
    sprite: mySprite,
    fit: ImageFit.cover
)
```

### Image Attributes

```bash
new Image(
    name: "ImageName",
    path: "Assets/Sprites/Icon",
    sprite: null,              // or provide Sprite reference
    color: Color.white,        // tint color
    width: 100,
    height: 100,
    fit: Image.contain         // Image.contain, cover, fill, fitWidth, fitHeight, none, scaleDown
)
```

### Image Fit Options
- `Image.contain` (`ImageFit.contain`) - Preserves aspect ratio, fits entirely within parent.
- `Image.cover` (`ImageFit.cover`) - Scales to fill, preserving aspect ratio (may crop).
- `Image.fill` (`ImageFit.fill`) - Stretches to fill bounds without preserving aspect ratio.
- `Image.fitWidth` (`ImageFit.fitWidth`) - Adjusts height to match width aspect ratio.
- `Image.fitHeight` (`ImageFit.fitHeight`) - Adjusts width to match height aspect ratio.
- `Image.none` (`ImageFit.none`) - Native image size.
- `Image.scaleDown` (`ImageFit.scaleDown`) - Scales down only if larger than container.

## Radius (Rounded Corners)

Uinity provides flexible ways to round corners on Containers:

### Uniform Radius
Apply the same radius to all 4 corners:

```bash
new Container(
    radius: new Radius(20), // or simply: radius: 20
    color: Color.blue,
    width: 100,
    height: 100
)
```

### Full / Circular Radius
Create a pill or circular container:

```bash
new Container(
    radius: new Radius(Radius.full), // or Radius.full
    color: Color.blue,
    width: 100,
    height: 100
)
```

### Axis-based Radius
Set corner radii horizontally or vertically:

```bash
// Left & Right corners
new Container(
    radius: new SetRadiusX(left: 0, right: 20)
)

// Top & Bottom corners
new Container(
    radius: new SetRadiusY(top: 30, bottom: 0)
)
```

### Specific Corner Radius
Control each corner individually:

```bash
new Container(
    radius: new SetRadiusOnly(
        topLeft: 20,
        topRight: 0,
        bottomRight: 100,
        bottomLeft: 50
    )
)
```

## Direction Spacing (Gap)

Add spacing/gap between children inside `Vertical` or `Horizontal`:

```bash
new Vertical(
    gap: 80,
    new UIElement[]
    {
        new Container(color: Color.yellow, width: 100, height: 100),
        new Container(color: Color.blue, width: 100, height: 100)
    }
)
```

```bash
new Horizontal(
    gap: 20,
    padding: new SetPadding(10),
    new UIElement[]
    {
        new Container(color: Color.yellow, width: 100, height: 100),
        new Container(color: Color.blue, width: 100, height: 100)
    }
)
```

## Alignment

Container child alignment options:
- `Alignment.topLeft`
- `Alignment.topCenter`
- `Alignment.topRight`
- `Alignment.centerLeft`
- `Alignment.center`
- `Alignment.centerRight`
- `Alignment.bottomLeft`
- `Alignment.bottomCenter`
- `Alignment.bottomRight`

Example:

```bash
new Container(
    alignment: Alignment.center,
    width: Width.screen / 2,
    height: 100,
    child: new Text("Centered Text", size: 24f)
)
```

## Responsive Sizing (Width & Height)

- `Width.full` / `Height.full` : Fills available parent size.
- `Width.screen` / `Height.screen` : Matches screen resolution.

Supports mathematical operations:

```bash
new Container(
    width: Width.screen / 2, // Half of screen width
    height: Height.screen / 3
)

new Container(
    width: Width.full,
    height: Height.full
)
```

## How to Render (UinityScreen)

To display UI in your Unity scene, create a script inheriting from `UinityScreen` and implement the `Build()` method:

```csharp
using UnityEngine;
using Uinity;

public class MainUinity : UinityScreen
{
    public override UIElement Build()
    {
        return Template.Build();
    }
}
```

Attach this component to any GameObject under a `Canvas`. It will automatically rebuild in Edit Mode (`[ExecuteAlways]`) and at Runtime.
