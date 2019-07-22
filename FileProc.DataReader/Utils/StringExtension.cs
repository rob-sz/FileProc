namespace FileProc.DataReader.Utils
{
    /// <summary>String extension class.</summary>
    internal static class StringExtension
    {
        /// <summary>Fills the specified string with spaces.</summary>
        /// <param name="str">The string.</param>
        internal static void Blank(this char[] str)
        {
            if (str == null)
                return;

            for (int i = 0; i < str.Length; i++)
                str[i] = ' ';
        }
    }
}
