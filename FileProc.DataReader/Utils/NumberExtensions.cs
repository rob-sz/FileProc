namespace FileProc.DataReader.Utils
{
    /// <summary>Number extension class.</summary>
    internal static class NumberExtensions
    {
        /// <summary>Compute base number to the power of exponent.</summary>
        /// <param name="baseNumber">The base number.</param>
        /// <param name="exponent">The exponent.</param>
        /// <returns>Return base number to the power of exponent as a long.</returns>
        internal static long Pow(this int baseNumber, int exponent)
        {
            long result = 1;
            for (long i = 0; i < exponent; i++)
                result *= baseNumber;
            return result;
        }
    }
}
