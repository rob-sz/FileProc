namespace FileProc.DataReader
{
    /// <summary>Field part specification.</summary>
    public class FieldPart
    {
        /// <summary>Gets the part value index.</summary>
        /// <value>Part value index within source record.</value>
        public int Index { get; }
        /// <summary>Gets the part value length.</summary>
        /// <value>Part value length.</value>
        public int Length { get; }
        /// <summary>Gets the trim specification.</summary>
        /// <seealso cref="FileProc.DataReader.FieldTrim" />
        /// <value>The trim specification.</value>
        public FieldTrim Trim { get; }

        /// <summary>Initializes a new instance of the <see cref="FieldPart"/> class.</summary>
        /// <param name="index">Part value index within source record.</param>
        /// <param name="length">Part value length.</param>
        public FieldPart(int index, int length)
            : this(index, length, FieldTrim.Trim)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldPart"/> class.</summary>
        /// <seealso cref="FileProc.DataReader.FieldTrim" />
        /// <param name="index">Part value index within source record.</param>
        /// <param name="length">Part value length.</param>
        /// <param name="trim">The trim specification.</param>
        public FieldPart(int index, int length, FieldTrim trim)
        {
            Index = index;
            Length = length;
            Trim = trim;
        }
    }
}
