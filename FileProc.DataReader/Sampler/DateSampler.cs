using System;

namespace FileProc.DataReader.Sampler
{
    /// <summary>Date value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class DateSampler : Sampler, ISampler
    {
        #region Public Methods

        /// <summary>Inserts sample date value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            var random = new Random();

            var now = DateTime.Now;
            var startDate = new DateTime(1901, 1, 1,
                now.Hour, now.Minute, now.Second, now.Millisecond);
            int rangeDays = (DateTime.Today - startDate).Days;
            var randomDate = startDate.AddDays(random.Next(rangeDays));

            string sampleValue = string.Format("{0:" + format + "}", randomDate);

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
