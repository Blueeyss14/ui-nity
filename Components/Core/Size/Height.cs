namespace Uinity
{
    public readonly struct Height
    {
        public readonly float Value;
        public readonly bool IsFull;

        private Height(float value, bool isFull)
        {
            Value = value;
            IsFull = isFull;
        }

        public static Height Full =>
            new Height(0f, true);

        public static implicit operator Height(float value)
        {
            return new Height(value, false);
        }
    }
}