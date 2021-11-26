namespace System
{
    public static class ArrayExtensions
    {
        public static ArraySegment<T> ToSegment<T>(
                this T[]    array,
                int         offset,
                int         count)
            => new ArraySegment<T>(array, offset, count);
    }
}
