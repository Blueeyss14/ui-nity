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
    height: Width.screen / 2)

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