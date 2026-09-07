namespace Uinity
{
    public readonly struct Width
    {
        public readonly float Value;
        public readonly bool IsFull;

        private Width(float value, bool isFull)
        {
            Value = value;
            IsFull = isFull;
        }

        public static Width Full =>
            new Width(0f, true);

        public static implicit operator Width(float value)
        {
            return new Width(value, false);
        }
    }
}