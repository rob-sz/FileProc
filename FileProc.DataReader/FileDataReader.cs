using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;

namespace FileProc.DataReader
{
    /// <summary>File data reader used by sql bulk copy in file import.</summary>
    /// <seealso cref="FileProc.DataReader.FileImport" />
    /// <seealso cref="System.Data.SqlClient.SqlBulkCopy" />
    /// <seealso cref="System.Data.IDataReader" />
    public class FileDataReader : IDataReader
    {
        #region Constants

        private static long RecordStartFirst = 0;
        private static long RecordCountAll = -1;

        #endregion

        #region Private Members

        private readonly object readerLock = new object();
        private bool isDisposed;
        private int recordLength;
        private Field[] fields;
        private StreamReader fileReader;
        private StringBuilder fieldValue;
        private char[] record;
        private object[] recordValues;
        private long recordReadCount;
        private long recordCount;

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="FileDataReader"/> class.</summary>
        /// <param name="filePath">The file path of file to import.</param>
        /// <param name="fields">The fields specification.</param>
        /// <param name="recordLength">Length of each record (excluding CRLF).</param>
        /// <exception cref="ArgumentException">File specified by file path does not exist.</exception>
        /// <exception cref="ArgumentException">Fields cannot be null or empty.</exception>
        /// <exception cref="ArgumentException">Record length must be at least total field length.</exception>
        public FileDataReader(string filePath, Field[] fields, int recordLength)
            : this(filePath, fields, recordLength, RecordStartFirst, RecordCountAll)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FileDataReader"/> class.</summary>
        /// <param name="filePath">The file path of file to import.</param>
        /// <param name="fields">The fields specification.</param>
        /// <param name="recordLength">Length of each record (excluding CRLF).</param>
        /// <param name="recordStart">Starting record to import.</param>
        /// <param name="recordCount">Number of records to import.</param>
        /// <exception cref="ArgumentException">File specified by file path does not exist.</exception>
        /// <exception cref="ArgumentException">Fields cannot be null or empty.</exception>
        /// <exception cref="ArgumentException">Record length must be at least total field length.</exception>
        public FileDataReader(string filePath, Field[] fields, int recordLength, 
            long recordStart, long recordCount)
        {
            // assert file exists
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(
                    string.Format("File does not exist. ({0})", filePath), "filePath");
            }

            // assert fields valid
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException("Fields cannot be null or empty.", "fields");
            }

            // assert record length within bounds
            var totalFieldLength = fields.Sum(
                o => o.FieldParts == null ? 0 : o.FieldParts.Sum(p => p.Length));
            if (recordLength <= 0 || recordLength < totalFieldLength)
            {
                throw new ArgumentException(
                    "Record length must be at least total field length.", "recordLength");
            }

            if (recordStart < 0) recordStart = RecordStartFirst;
            if (recordCount < 0) recordCount = RecordCountAll;

            this.recordLength = recordLength;
            this.recordCount = recordCount;
            this.fields = fields;

            record = new char[this.recordLength];
            recordValues = new object[this.fields.Length];
            fieldValue = new StringBuilder();

            // seek stream to record start
            fileReader = new StreamReader(filePath);
            fileReader.BaseStream.Position = recordLength * recordStart;
        }

        #endregion

        #region IDataReader

        public int Depth
        {
            get { return 0; }
        }

        public bool IsClosed
        {
            // stream is closed if null or at end of stream
            get { return fileReader == null || fileReader.EndOfStream; }
        }

        public int RecordsAffected
        {
            get
            {
                // used for SELECT statements;
                //  -1 must be returned
                return -1;
            }
        }

        public void Close()
        {
            Dispose();
        }

        public DataTable GetSchemaTable()
        {
            // note: I don't think this is used

            DataTable schema = new DataTable("SchemaTable");
            schema.Locale = CultureInfo.InvariantCulture;
            schema.MinimumCapacity = fields.Length;

            schema.Columns.Add(SchemaTableColumn.AllowDBNull, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseTableName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnSize, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.DataType, typeof(object)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsAliased, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsExpression, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsKey, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsLong, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsUnique, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericPrecision, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericScale, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ProviderType, typeof(int)).ReadOnly = true;

            schema.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)).ReadOnly = true;

            // null marks columns that will change for each row
            object[] schemaRow = new object[] {
                    true,					// 00- AllowDBNull
					null,					// 01- BaseColumnName
					string.Empty,			// 02- BaseSchemaName
					string.Empty,			// 03- BaseTableName
					null,					// 04- ColumnName
					null,					// 05- ColumnOrdinal
					int.MaxValue,			// 06- ColumnSize
					typeof(string),			// 07- DataType
					false,					// 08- IsAliased
					false,					// 09- IsExpression
					false,					// 10- IsKey
					false,					// 11- IsLong
					false,					// 12- IsUnique
					DBNull.Value,			// 13- NumericPrecision
					DBNull.Value,			// 14- NumericScale
					(int) DbType.String,	// 15- ProviderType
					string.Empty,			// 16- BaseCatalogName
					string.Empty,			// 17- BaseServerName
					false,					// 18- IsAutoIncrement
					false,					// 19- IsHidden
					true,					// 20- IsReadOnly
					false					// 21- IsRowVersion
			  };

            for (int i = 0; i < fields.Length; i++)
            {
                schemaRow[1] = fields[i].Name; // Base column name
                schemaRow[4] = fields[i].Name; // Column name
                schemaRow[5] = i; // Column ordinal

                schema.Rows.Add(schemaRow);
            }

            return schema;
        }

        public bool NextResult()
        {
            // not reading batch sql statements
            return false;
        }

        public bool Read()
        {
            if (IsClosed)
                return false;

            if (recordReadCount >= recordCount)
                return false;

            recordReadCount++;

            if (fileReader.Read(record, 0, recordLength) > 0)
            {
                // parse record to populate record values
                ParseRecord();
                return true;
            }

            record = null;
            return false;
        }

        #endregion

        #region IDataRecord

        public int FieldCount
        {
            get { return fields == null ? 0 : fields.Length; }
        }

        public object this[int i]
        {
            get { return recordValues[i]; }
        }

        public object this[string name]
        {
            get
            {
                int i = this.GetOrdinal(name);
                return i >= 0 ? this[i] : DBNull.Value;
            }
        }

        public bool GetBoolean(int i)
        {
            return bool.Parse(GetString(i));
        }

        public byte GetByte(int i)
        {
            return byte.Parse(GetString(i));
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return char.Parse(GetString(i));
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            try
            {
                return DateTime.Parse(GetString(i));
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }
            return DateTime.Now;
        }

        public decimal GetDecimal(int i)
        {
            return decimal.Parse(GetString(i));
        }

        public double GetDouble(int i)
        {
            return double.Parse(GetString(i));
        }

        public Type GetFieldType(int i)
        {
            return recordValues[i].GetType();
        }

        public float GetFloat(int i)
        {
            return float.Parse(GetString(i));
        }

        public short GetInt16(int i)
        {
            return short.Parse(GetString(i));
        }

        public int GetInt32(int i)
        {
            return int.Parse(GetString(i));
        }

        public long GetInt64(int i)
        {
            return long.Parse(GetString(i));
        }

        public Guid GetGuid(int i)
        {
            return new Guid(this[i].ToString());
        }

        public string GetName(int i)
        {
            return fields[i].Name;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name.Equals(name))
                    return i;
            }

            return -1;
        }

        public string GetString(int i)
        {
            object value = recordValues[i];

            return value == DBNull.Value ? string.Empty : value as string;
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                values[i] = this.GetValue(i);
            }

            return (values == null ? -1 : values.Length);
        }

        public bool IsDBNull(int i)
        {
            return (recordValues[i] == DBNull.Value);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (isDisposed)
                return;

            try
            {
                lock (readerLock)
                {
                    if (fileReader != null)
                    {
                        fileReader.Dispose();
                        fileReader = null;
                        record = null;
                    }
                }
            }
            finally
            {
                isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        /// <summary>Parses the record.</summary>
        private void ParseRecord()
        {
            // clear previous record values
            Array.Clear(recordValues, 0, recordValues.Length);

            for (int i = 0; i < recordValues.Length; i++)
            {
                // extract field value from record
                fields[i].ExtractValue(record, fieldValue);

                // apply any action to field value
                fields[i].Action?.Invoke(fieldValue);

                // add field value to record values
                string recordValue = fieldValue.ToString();
                recordValues[i] = string.IsNullOrWhiteSpace(recordValue) 
                    ? fields[i].EmptyValue 
                    : recordValue;
            }
        }

        #endregion
    }
}
