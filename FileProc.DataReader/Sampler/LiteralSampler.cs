namespace FileProc.DataReader.Sampler
{
    /// <summary>Literal value sampler.</summary>
    /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
    /// <seealso cref="FileProc.DataReader.Sampler.ISampler" />
    public class LiteralSampler : Sampler, ISampler
    {
        #region Private Members

        private string literalValue;

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="LiteralSampler"/> class.</summary>
        /// <param name="literalValue">The literal value.</param>
        public LiteralSampler(string literalValue)
        {
            this.literalValue = literalValue;
        }

        #endregion

        #region Public Methods

        /// <summary>Inserts sample literal value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        public void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format)
        {
            string sampleValue = literalValue;

            InsertSampleChars(targetRecord, fieldParts, sampleValue.ToCharArray());
        }

        #endregion
    }
}
