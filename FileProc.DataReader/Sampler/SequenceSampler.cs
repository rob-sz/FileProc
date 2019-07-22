namespace FileProc.DataReader.Sampler
{
    /// <summary>Sequence value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class SequenceSampler : Sampler, ISampler
    {
        #region Private Members

        private long nextValue;

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="SequenceSampler"/> class.</summary>
        /// <param name="nextValue">The next value.</param>
        public SequenceSampler(long nextValue)
        {
            this.nextValue = nextValue;
        }

        #endregion

        #region Public Methods

        /// <summary>Inserts sample sequence value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            string sampleValue = string.Format("{0}", nextValue++);

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
