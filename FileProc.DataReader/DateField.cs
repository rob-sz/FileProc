using System;
using System.Text;
using FileProc.DataReader.Parser;
using FileProc.DataReader.Sampler;

namespace FileProc.DataReader
{
    /// <summary>Date field specification.</summary>
    /// <seealso cref="FileProc.DataReader.Field" />
    public class DateField : Field
    {
        #region Private Members

        private DateParser Parser = new DateParser();

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="DateField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="index">The field index within record.</param>
        /// <param name="length">The field value length.</param>
        /// <param name="format">The field value format.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Hour part must have 24 hour format: HH</exception>
        /// <exception cref="ArgumentException">Year part must have format: yyyy</exception>
        /// <exception cref="ArgumentException">Month part must have format: MM</exception>
        /// <exception cref="ArgumentException">Day part must have format: dd</exception>
        /// <exception cref="ArgumentException">Hour part must have format: HH</exception>
        /// <exception cref="ArgumentException">Minute part must have format: mm</exception>
        /// <exception cref="ArgumentException">Second part must have format: ss</exception>
        public DateField(string name, int index, int length, string format)
            : this(name, new[] { new FieldPart(index, length) }, format)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DateField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The field value format.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        /// <exception cref="ArgumentException">Field part cannot be null.</exception>
        /// <exception cref="ArgumentException">Field part index cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Field part length cannot be nagative.</exception>
        /// <exception cref="ArgumentException">Hour part must have 24 hour format: HH</exception>
        /// <exception cref="ArgumentException">Year part must have format: yyyy</exception>
        /// <exception cref="ArgumentException">Month part must have format: MM</exception>
        /// <exception cref="ArgumentException">Day part must have format: dd</exception>
        /// <exception cref="ArgumentException">Hour part must have format: HH</exception>
        /// <exception cref="ArgumentException">Minute part must have format: mm</exception>
        /// <exception cref="ArgumentException">Second part must have format: ss</exception>
        public DateField(string name, FieldPart[] fieldParts, string format)
            : base(name, fieldParts, format)
        {
            if (format.Contains("h"))
                throw new ArgumentException(
                    string.Format("Hour part must have 24 hour format: HH ({0})", name), "format");
            if (format.Contains("y") && !format.Contains("yyyy"))
                throw new ArgumentException(
                    string.Format("Year part must have format: yyyy ({0})", name), "format");
            if (format.Contains("M") && !format.Contains("MM"))
                throw new ArgumentException(
                    string.Format("Month part must have format: MM ({0})", name), "format");
            if (format.Contains("d") && !format.Contains("dd"))
                throw new ArgumentException(
                    string.Format("Day part must have format: dd ({0})", name), "format");
            if (format.Contains("H") && !format.Contains("HH"))
                throw new ArgumentException(
                    string.Format("Hour part must have format: HH ({0})", name), "format");
            if (format.Contains("m") && !format.Contains("mm"))
                throw new ArgumentException(
                    string.Format("Minute part must have format: mm ({0})", name), "format");
            if (format.Contains("s") && !format.Contains("ss"))
                throw new ArgumentException(
                    string.Format("Second part must have format: ss ({0})", name), "format");

            Sampler = new DateSampler();
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the value from source record into target buffer.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal override void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer)
        {
            Parser.ExtractValue(sourceRecord, targetBuffer, FieldParts, Format);
        }

        #endregion
    }
}
