using System;
using System.Linq;
using System.Text;

namespace FileProc.DataReader.Sampler
{
    /// <summary>String value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class StringSampler : Sampler, ISampler
    {
        #region Private Members

        private StringBuilder sampleBuffer = new StringBuilder();

        #endregion

        #region Public Methods

        /// <summary>Inserts sample string value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            var random = new Random();
            var valueLength = random.Next(fieldParts.Sum(o => o.Length));

            sampleBuffer.Length = 0;
            sampleBuffer.Capacity = valueLength;

            for (int i = 0; i < valueLength; i++)
            {
                sampleBuffer.Append(
                    Convert.ToChar(Convert.ToInt32(Math.Floor(95 * random.NextDouble() + 32))));
            }

            string sampleValue = sampleBuffer.ToString();

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
