using FileProc.DataReader.Parser;
using FileProc.DataReader.Sampler;
using System;
using System.Text;

namespace FileProc.DataReader
{
    /// <summary>Decimal field specification.</summary>
    /// <seealso cref="FileProc.DataReader.Field" />
    public class DecimalField : Field
    {
        #region Private Members

        private DecimalParser Parser = new DecimalParser();

        #endregion

        #region Public Members

        /// <summary>Gets the implied scale.</summary>
        /// <value>Number of implied deciaml places in value.</value>
        public int ImpliedScale { get; }

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="DecimalField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="index">The field index within record.</param>
        /// <param name="length">The field value length.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Implied scale cannot be nagative.</exception>
        public DecimalField(string name, int index, int length)
            : this(name, new[] { new FieldPart(index, length) })
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DecimalField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="index">The field index within record.</param>
        /// <param name="length">The field value length.</param>
        /// <param name="impliedScale">The implied scale.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Implied scale cannot be nagative.</exception>
        public DecimalField(string name, int index, int length, int impliedScale)
            : this(name, new[] { new FieldPart(index, length) }, impliedScale)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DecimalField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Implied scale cannot be nagative.</exception>
        public DecimalField(string name, FieldPart[] fieldParts)
            : this(name, fieldParts, 0)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DecimalField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="impliedScale">Number of implied deciaml places in value.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Implied scale cannot be nagative.</exception>
        public DecimalField(string name, FieldPart[] fieldParts, int impliedScale)
            : base(name, fieldParts)
        {
            if (impliedScale < 0)
                throw new ArgumentException(
                    string.Format("Implied scale cannot be nagative. ({0})", name),
                    "impliedScale");

            ImpliedScale = impliedScale;
            Sampler = new DecimalSampler(impliedScale);
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the value from source record into target buffer.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal override void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer)
        {
            Parser.ExtractValue(sourceRecord, targetBuffer, FieldParts, ImpliedScale);
        }

        #endregion
    }
}
