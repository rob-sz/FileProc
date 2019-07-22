using FileProc.DataReader.Parser;
using FileProc.DataReader.Sampler;
using System.Text;

namespace FileProc.DataReader
{
    /// <summary>String field specification.</summary>
    /// <seealso cref="FileProc.DataReader.Field" />
    public class StringField : Field
    {
        #region Private Members

        private StringParser Parser = new StringParser();

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="StringField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="index">The field index within record.</param>
        /// <param name="length">The field value length.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        public StringField(string name, int index, int length)
            : this(name, new[] { new FieldPart(index, length) })
        {
        }

        /// <summary>Initializes a new instance of the <see cref="StringField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        public StringField(string name, FieldPart[] fieldParts)
            : base(name, fieldParts)
        {
            Sampler = new StringSampler();
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the value from source record into target buffer.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal override void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer)
        {
            Parser.ExtractValue(sourceRecord, targetBuffer, FieldParts);
        }

        #endregion
    }
}
