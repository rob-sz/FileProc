using System.Text;

namespace FileProc.DataReader.Parser
{
    /// <summary>Parser to extract date value from record.</summary>
    /// <seealso cref="FileProc.DataReader.Parser.Parser" />
    internal class DateParser : Parser
    {
        // yyyy-MM-dd HH:mm:ss
        private readonly (string Mask, string Separator, string Default)[] targetMap = new[]
            {
                ("yyyy", "", "1900"),
                ("MM", "-", "01"),
                ("dd", "-", "01"),
                ("HH", " ", "00"),
                ("mm", ":", "00"),
                ("ss", ":", "00")
            };

        /// <summary>Extracts the value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        internal void ExtractValue(
            char[] sourceRecord,
            StringBuilder targetBuffer,
            FieldPart[] fieldParts,
            string format)
        {
            char[] rawValueBuffer = new char[19];
            ExtractRawValue(sourceRecord, rawValueBuffer, fieldParts);

            targetBuffer.Length = 0;
            targetBuffer.Capacity = rawValueBuffer.Length;

            foreach (var mapPart in targetMap)
            {
                int dateIndex = format.IndexOf(mapPart.Mask);

                targetBuffer.Append(mapPart.Separator);

                if (dateIndex >= 0)
                    targetBuffer.Append(rawValueBuffer, dateIndex, mapPart.Mask.Length);
                else
                    targetBuffer.Append(mapPart.Default);
            }
        }
    }
}
