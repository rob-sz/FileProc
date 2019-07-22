using System;
using System.Linq;

namespace FileProc.DataReader.Sampler
{
    /// <summary>Sampler abstract base class.</summary>
    public abstract class Sampler
    {
        #region Protected Methods

        /// <summary>Inserts sample chars into record according to field part specification.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="sampleChars">The sample chars.</param>
        protected void InsertSampleChars(char[] targetRecord, FieldPart[] fieldParts, char[] sampleChars)
        {
            int sampleCharsIndex = 0;
            foreach (var fieldPart in fieldParts ?? Enumerable.Empty<FieldPart>())
            {
                if (fieldPart.Index < 0 ||
                    fieldPart.Index >= targetRecord.Length)
                    continue;

                if (fieldPart.Length <= 0 ||
                    fieldPart.Index + fieldPart.Length > targetRecord.Length)
                    continue;

                int sampleCharsLength = Math.Min(fieldPart.Length, sampleChars.Length - sampleCharsIndex);
                sampleCharsLength = Math.Min(sampleCharsLength, targetRecord.Length - fieldPart.Index);

                Array.Copy(sampleChars, sampleCharsIndex, targetRecord, fieldPart.Index, sampleCharsLength);

                sampleCharsIndex += sampleCharsLength;
                if (sampleCharsIndex >= sampleChars.Length)
                    break;
            }
        }

        #endregion
    }
}
