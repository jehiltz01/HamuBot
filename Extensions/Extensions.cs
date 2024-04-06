namespace HamuBot.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Looping itteration; used to cycle the calendar days to the new year 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="from"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<T> Iterate<T>(this IList<T> input, int from, int length)
        {
            for (int i = from; i < from + length; i++) {
                yield return input[i % input.Count];
            }
        }
    }
}
