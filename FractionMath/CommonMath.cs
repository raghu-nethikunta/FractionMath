using System;

namespace FractionMath
{
    public static class CommonMath
    {
        /// <summary>
        /// Find the greatest common divisor for integers.
        /// </summary>
        /// <param name="first">First integer.</param>
        /// <param name="second">Second integer.</param>
        /// <returns>The greatest common divisor for <paramref name="first"/> and <paramref name="second"/>.</returns>
        public static int GreatestCommonDivisor(int first, int second)
        {
            first = Math.Abs(first);
            second = Math.Abs(second);

            while (second != 0)
            {
                int temp = second;
                second = first % second;
                first = temp;
            }

            return first;
        }

        /// <summary>
        /// Find the least common multiple for two integers.
        /// </summary>
        /// <param name="first">First integer.</param>
        /// <param name="second">Second integer.</param>
        /// <returns>The least common multiple for <paramref name="first"/> and <paramref name="second"/>.</returns>
        public static int LeastCommonMultiple(int first, int second)
        {
            if (first == 0 && second == 0)
                return 0;

            return Math.Abs(first * second) / GreatestCommonDivisor(first, second);
        }
    }
}