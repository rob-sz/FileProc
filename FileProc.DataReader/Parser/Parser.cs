using FileProc.DataReader.Utils;
using System;
using System.Text;

namespace FileProc.DataReader.Parser
{
    /// <summary>Parser abstract base class.</summary>
    internal abstract class Parser
    {
        /// <summary>Extracts the raw value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        /// <param name="fieldParts">The field parts.</param>
        protected void ExtractRawValue(
            char[] sourceRecord,
            StringBuilder targetBuffer,
            FieldPart[] fieldParts)
        {
            targetBuffer.Clear();

            for (int i = 0; i < fieldParts.Length; i++)
            {
                // get field index and length
                int partIndex = fieldParts[i].Index;
                int partLength = fieldParts[i].Length;
                var partTrim = fieldParts[i].Trim;

                if (partTrim != FieldTrim.None)
                {
                    // decrease field part index and field part length to trim
                    AdjustToTrim(partTrim, sourceRecord, ref partIndex, ref partLength);
                }

                // extract trimmed field part and append to field target buffer
                if (partLength > 0)
                {
                    targetBuffer.Append(sourceRecord, partIndex, partLength);
                }
            }
        }

        /// <summary>Extracts the raw value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        /// <param name="fieldParts">The field parts.</param>
        protected void ExtractRawValue(
            char[] sourceRecord,
            char[] targetBuffer,
            FieldPart[] fieldParts)
        {
            targetBuffer.Blank();

            for (int i = 0; i < fieldParts.Length; i++)
            {
                // get field index and length
                int partIndex = fieldParts[i].Index;
                int partLength = fieldParts[i].Length;
                var partTrim = fieldParts[i].Trim;

                if (partTrim != FieldTrim.None)
                {
                    // decrease field part index and field part length to trim
                    AdjustToTrim(partTrim, sourceRecord, ref partIndex, ref partLength);
                }

                // extract trimmed field part and append to field target buffer
                if (partLength > 0)
                {
                    Array.Copy(sourceRecord, partIndex, targetBuffer, 0, partLength);
                }
            }
        }

        /// <summary>Adjust part index and part length within record to trim.</summary>
        /// <param name="fieldTrim">The field trim.</param>
        /// <param name="sourceRecord">The field value.</param>
        /// <param name="partIndex">Index of the field.</param>
        /// <param name="partLength">Length of the field.</param>
        protected void AdjustToTrim(FieldTrim fieldTrim, char[] sourceRecord, ref int partIndex, ref int partLength)
        {
            // trim left as required
            if (fieldTrim == FieldTrim.Trim || fieldTrim == FieldTrim.Left)
            {
                while (partLength > 0 && char.IsWhiteSpace(sourceRecord[partIndex]))
                {
                    partIndex++;
                    partLength--;
                }
            }

            // trim right as required
            if (fieldTrim == FieldTrim.Trim || fieldTrim == FieldTrim.Right)
            {
                while (partLength > 0 && char.IsWhiteSpace(sourceRecord[partIndex + partLength - 1]))
                {
                    partLength--;
                }
            }
        }
    }
}
