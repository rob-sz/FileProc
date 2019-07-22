using System.Text;

namespace FileProc.DataReader.Parser
{
    /// <summary>Parser to extract number value from record.</summary>
    /// <seealso cref="FileProc.DataReader.Parser.Parser" />
    internal class NumberParser : Parser
    {
        /// <summary>Extracts the value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        /// <param name="fieldParts">The field parts.</param>
        internal void ExtractValue(
            char[] sourceRecord,
            StringBuilder targetBuffer,
            FieldPart[] fieldParts)
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
            }
        }
    }
}
