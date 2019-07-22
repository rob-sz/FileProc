using FileProc.DataReader.Utils;
using System;
using System.Linq;

namespace FileProc.DataReader.Sampler
{
    /// <summary>Decimal value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class DecimalSampler : Sampler, ISampler
    {
        #region Private Members

        private int impliedScale;

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="DecimalSampler"/> class.</summary>
        /// <param name="impliedScale">The implied scale.</param>
        public DecimalSampler(int impliedScale)
        {
            this.impliedScale = impliedScale;
        }

        #endregion

        #region Public Methods

        /// <summary>Inserts sample decimal value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            var random = new Random();
            int signCharLength = fieldParts.Length > 1 && fieldParts[0].Length == 1 ? 1 : 0;
            int decimalPointLength = impliedScale > 0 ? 0 : 1;
            int digitCount = fieldParts.Sum(o => o.Length) - signCharLength - decimalPointLength;
            int randomValue = random.Next(
                Convert.ToInt32(Math.Min(int.MaxValue, 10.Pow(digitCount))));

            string signCharValue = signCharLength == 1
                ? random.Next(2) == 1 ? "+" : "-"
                : string.Empty;
            
            string sampleValue = string.Format(
                "{0}{1:" + "".PadRight(digitCount, '0') + "}", signCharValue, randomValue);

            if (impliedScale <= 0)
            {
                // scale not implied: insert random decimal point
                int scale = random.Next(1, sampleValue.Length);
                sampleValue = sampleValue.Insert(sampleValue.Length - scale, ".");
            }

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
