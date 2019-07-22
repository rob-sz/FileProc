using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileProc.DataReader
{
    /// <summary>File import.</summary>
    public class FileImport
    {
        #region Private Members

        private class ImportArgs
        {
            internal string FilePath { get; set; }
            internal Field[] Fields { get; set; }
            internal int RecordLength { get; set; }
            internal long RecordStart { get; set; }
            internal long RecordCount { get; set; }
            internal string ConnectionString { get; set; }
            internal string DestinationTable { get; set; }
        }

        private string filePath;
        private Field[] fields;
        private int recordLength;

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="FileImport"/> class.</summary>
        /// <param name="filePath">The file path of file to import.</param>
        /// <param name="fields">The fields specification.</param>
        /// <param name="recordLength">Length of each record (excluding CRLF).</param>
        public FileImport(string filePath, Field[] fields, int recordLength)
        {
            this.filePath = filePath;
            this.fields = fields;
            this.recordLength = recordLength + 2; // including CRLF
        }

        #endregion

        #region Public Methods

        /// <summary>Imports file into destination table according to field specifications.</summary>
        /// <summary>Multi-threaded import utilising as many threads as there are processors.</summary>
        /// <summary>Order of records imported is not quaranteed.</summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="destinationTable">The destination table.</param>
        public void Import(string connectionString, string destinationTable)
        {
            Import(connectionString, destinationTable, Environment.ProcessorCount);
        }

        /// <summary>Imports file into destination table according to field specifications.</summary>
        /// <summary>Multi-threaded import utilising as many threads as there are processors.</summary>
        /// <summary>Order of records imported is quaranteed, only if one thread specified.</summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="destinationTable">The destination table.</param>
        /// <param name="threadCount">The thread count.</param>
        public void Import(string connectionString, string destinationTable, int threadCount)
        {
            var fileInfo = new FileInfo(filePath);

            long recordCount = fileInfo.Length / recordLength;
            long recordCountPartial = fileInfo.Length % recordLength;
            if (recordCountPartial > 0) recordCount++;

            long recordCountPerThread = recordCount / threadCount;
            long recordCountPerThreadPartial = recordCount % threadCount;

            if (recordCountPerThread <= 0)
                return;

            var importArgs = new ImportArgs[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                long fileRecordStart = i * recordCountPerThread;
                long fileRecordCount = recordCountPerThread;

                if (i == threadCount - 1)
                {
                    // add any remaining records to last thread
                    fileRecordCount += recordCountPerThreadPartial;
                }

                importArgs[i] = new ImportArgs
                {
                    FilePath = filePath,
                    Fields = fields,
                    RecordLength = recordLength,
                    RecordStart = fileRecordStart,
                    RecordCount = fileRecordCount,
                    ConnectionString = connectionString,
                    DestinationTable = destinationTable
                };
            }

            Thread[] threads = (
                from n in Enumerable.Range(0, threadCount)
                select new Thread(ImportFile)
            ).ToArray();

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(importArgs[i]);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Imports the file.</summary>
        /// <param name="args">Import arguments.</param>
        private static void ImportFile(object args)
        {
            var importArgs = args as ImportArgs;
            if (importArgs == null)
                return;

            try
            {
                using (var sqlConnection = 
                    new SqlConnection(importArgs.ConnectionString))
                {
                    sqlConnection.Open();

                    using (var bulkCopy = new SqlBulkCopy(
                        sqlConnection, SqlBulkCopyOptions.TableLock, null))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = importArgs.DestinationTable;

                        using (var dataReader =
                            new FileDataReader(
                                importArgs.FilePath,
                                importArgs.Fields,
                                importArgs.RecordLength,
                                importArgs.RecordStart,
                                importArgs.RecordCount))
                        {
                            bulkCopy.WriteToServer(dataReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
