using FileProc.DataReader.Utils;
using System;
using System.Linq;

namespace FileProc.DataReader.Sampler
{
    /// <summary>Literal value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class NumberSampler : Sampler, ISampler
    {
        #region Public Methods

        /// <summary>Inserts sample literal value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            int signCharLength = fieldParts.Length > 1 && fieldParts[0].Length == 1 ? 1 : 0;
            int digitCount = fieldParts.Sum(o => o.Length) - signCharLength;
            int randomValue = new Random().Next(
                Convert.ToInt32(Math.Min(int.MaxValue, 10.Pow(digitCount))));

            string signCharValue = signCharLength == 1
                ? new Random().Next(2) == 1 ? "+" : "-"
                : string.Empty;

            string sampleValue = string.Format(
                "{0}{1:" + "".PadRight(digitCount, '0') + "}", signCharValue, randomValue);

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
