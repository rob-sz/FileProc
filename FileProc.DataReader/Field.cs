using System;
using System.Text;
using FileProc.DataReader.Sampler;

namespace FileProc.DataReader
{
    /// <summary>Field abstract base class.</summary>
    public abstract class Field
    {
        #region Public Properties

        /// <summary>Gets the field name.</summary>
        /// <value>Destination table column name.</value>
        public string Name { get; }
        /// <summary>Gets the field parts.</summary>
        /// <value>The field parts.</value>
        public FieldPart[] FieldParts { get; }
        /// <summary>Gets or sets the sampler used to generate sample values when creating sample file.</summary>
        /// <seealso cref="FileProc.DataReader.Sampler.SampleFile" />
        /// <value>The sampler.</value>
        public ISampler Sampler { get; set; }
        /// <summary>Gets or sets the empty value used when value in source file is empty.</summary>
        /// <value>The empty value.</value>
        public object EmptyValue { get; set; } = DBNull.Value;
        /// <summary>Gets or sets any action applied to field value after it is extracted from record.</summary>
        /// <value>The action.</value>
        public Action<StringBuilder> Action { get; set; }

        #endregion

        #region Internal Properties

        /// <summary>Gets the source record value format.</summary>
        /// <value>The format.</value>
        internal string Format { get; }

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="Field"/> class.</summary>
        /// <param name="name">The field name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        protected Field(string name, FieldPart[] fieldParts)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Field name cannot be empty.", "name");
            }

            if (fieldParts != null)
            {
                for (int i = 0; i < fieldParts.Length; i++)
                {
                    if (fieldParts[i] == null)
                        throw new ArgumentException(
                            string.Format("Field part cannot be null. ({0}, {1})", name, i),
                            "fieldParts");
                    if (fieldParts[i].Index < 0)
                        throw new ArgumentException(
                            string.Format("Field part index cannot be nagative. ({0}, {1})", name, i),
                            "fieldParts");
                    if (fieldParts[i].Length < 0)
                        throw new ArgumentException(
                            string.Format("Field part length cannot be nagative. ({0}, {1})", name, i),
                            "fieldParts");
                }
            }

            Name = name;
            FieldParts = fieldParts;
        }

        /// <summary>Initializes a new instance of the <see cref="Field"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The field value format.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        protected Field(string name, FieldPart[] fieldParts, string format)
            : this(name, fieldParts)
        {
            Format = format;
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the value from source record into target buffer.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal abstract void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer);

        /// <summary>Inserts the sample into target record using available sampler.</summary>
        /// <seealso cref="FileProc.DataReader.Sampler.Sampler" />
        /// <param name="targetRecord">The target record.</param>
        internal void InsertSample(char[] targetRecord)
        {
            if (Sampler != null)
                Sampler.InsertSample(targetRecord, FieldParts, Format);
        }

        #endregion
    }
}
