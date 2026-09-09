namespace Uinity
{
    public enum ImageFit
    {
        fill,
        contain,
        cover,
        fitWidth,
        fitHeight,
        none,
        scaleDown,

        Fill = fill,
        Contain = contain,
        Cover = cover,
        FitWidth = fitWidth,
        FitHeight = fitHeight,
        None = none,
        ScaleDown = scaleDown
    }

    public static class BoxFit
    {
        public static ImageFit fill => ImageFit.fill;
        public static ImageFit contain => ImageFit.contain;
        public static ImageFit cover => ImageFit.cover;
        public static ImageFit fitWidth => ImageFit.fitWidth;
        public static ImageFit fitHeight => ImageFit.fitHeight;
        public static ImageFit none => ImageFit.none;
        public static ImageFit scaleDown => ImageFit.scaleDown;

        public static ImageFit Fill => ImageFit.fill;
        public static ImageFit Contain => ImageFit.contain;
        public static ImageFit Cover => ImageFit.cover;
        public static ImageFit FitWidth => ImageFit.fitWidth;
        public static ImageFit FitHeight => ImageFit.fitHeight;
        public static ImageFit None => ImageFit.none;
        public static ImageFit ScaleDown => ImageFit.scaleDown;
    }
}
