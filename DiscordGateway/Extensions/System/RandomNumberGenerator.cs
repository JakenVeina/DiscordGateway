namespace System
{
    internal interface IRandomNumberGenerator
    {
        int Next(int minValue, int maxValue);
    }

    internal class RandomNumberGenerator
        : IRandomNumberGenerator
    {
        public RandomNumberGenerator()
            => _random = new();

        public RandomNumberGenerator(int seed)
            => _random = new(seed);

        public int Next(int minValue, int maxValue)
            => _random.Next(minValue, maxValue);

        private readonly Random _random;
    }
}
