namespace Uinity
{
    public readonly struct SetRadiusX
    {
        public readonly float Left;
        public readonly float Right;

        public SetRadiusX(float left = 0f, float right = 0f)
        {
            Left = left;
            Right = right;
        }

        public static implicit operator Radius(SetRadiusX x)
        {
            return new Radius(topLeft: x.Left, topRight: x.Right, bottomRight: x.Right, bottomLeft: x.Left);
        }
    }
}
