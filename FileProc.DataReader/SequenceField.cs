using FileProc.DataReader.Sampler;
using System.Text;
using System.Threading;

namespace FileProc.DataReader
{
    /// <summary>Sequence field specification.</summary>
    /// <seealso cref="FileProc.DataReader.Field" />
    public class SequenceField : Field
    {
        #region Private Members

        private long nextValue;
        private readonly object nextValueLock = new object();

        #endregion

        #region Public Properties

        /// <summary>Gets the next value in sequence.</summary>
        /// <value>The next value in sequence.</value>
        public long NextValue { get { return nextValue; } }

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="SequenceField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        public SequenceField(string name)
            : this(name, 1)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SequenceField"/> class.</summary>
        /// <param name="name">Destination table column name.</param>
        /// <param name="nextValue">The next value.</param>
        /// <exception cref="ArgumentException">Field name cannot be empty.</exception>
        public SequenceField(string name, long nextValue)
            : base(name, null)
        {
            this.nextValue = nextValue;
            Sampler = new SequenceSampler(nextValue);
        }

        #endregion

        #region Internal Methods

        /// <summary>Extracts the sequence value into target buffer and increments next value.</summary>
        /// <param name="sourceRecord">The source record.</param>
        /// <param name="targetBuffer">The target buffer.</param>
        internal override void ExtractValue(char[] sourceRecord, StringBuilder targetBuffer)
        {
            lock (nextValueLock)
            {
                targetBuffer.Clear();
                targetBuffer.Append(nextValue);

                Interlocked.Increment(ref nextValue);
            }
        }

        #endregion
    }
}
