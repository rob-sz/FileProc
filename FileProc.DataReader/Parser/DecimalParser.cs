using System.Text;

namespace FileProc.DataReader.Parser
{
    /// <summary>Parser to extract decimal value from record.</summary>
    /// <seealso cref="FileProc.DataReader.Parser.Parser" />
    internal class DecimalParser : Parser
    {
        /// <summary>Extracts the value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="impliedScale">The implied scale.</param>
        internal void ExtractValue(
            char[] sourceRecord,
            StringBuilder targetBuffer,
            FieldPart[] fieldParts,
            int impliedScale)
        {
            ExtractRawValue(sourceRecord, targetBuffer, fieldParts);

            if (targetBuffer.Length == 1)
            {
                if (targetBuffer[0] == '+' || targetBuffer[0] == '-')
                {
                    targetBuffer.Length = 0;
                }
            }
            else if (targetBuffer.Length > 0)
            {
                // remove spaces
                targetBuffer.Replace(" ", "");

                if (impliedScale > 0)
                {
                    if (impliedScale > targetBuffer.Length)
                    {
                        // pad out value to scale
                        targetBuffer.Insert(0, "0", impliedScale - targetBuffer.Length);
                    }

                    // insert decimal point
                    targetBuffer.Insert(targetBuffer.Length - impliedScale, '.');
                }
            }
        }
    }
}
