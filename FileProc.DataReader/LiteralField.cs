using FileProc.DataReader.Sampler;
using System.Text;

namespace FileProc.DataReader
{
    /// <summary>Literal field specification.</summary>
    /// <seealso cref="FileProc.DataReader.Field" />
    public class LiteralField : Field
    {
        #region Public Properties

        /// <summary>Gets the literal value.</summary>
        /// <value>The literal value.</value>
        public string LiteralValue { get; }

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="LiteralField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        public LiteralField(string name)
            : this(name, string.Empty)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LiteralField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="literalValue">The literal value.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        public LiteralField(string name, string literalValue)
            : base(name, null)
        {
            LiteralValue = literalValue;
            Sampler = new LiteralSampler(literalValue);
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the literal value into target buffer.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal override void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer)
        {
            targetBuffer.Clear();
            targetBuffer.Append(LiteralValue);
        }

        #endregion
    }
}
