using System.IO;
using FileProc.DataReader.Utils;

namespace FileProc.DataReader.Sampler
{
    /// <summary>Sample file creator.</summary>
    public class SampleFile
    {
        /// <summary>Create sample file accoring to field specifications.</summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="recordLength">Length of the record.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="fields">The fields.</param>
        public void Create(string filePath, int recordLength, long recordCount, Field[] fields)
        {
            using (var fileWriter = new StreamWriter(filePath, false))
            {
                char[] dataRecord = new char[recordLength];
                for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
                {
                    dataRecord.Blank();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fields[i].InsertSample(dataRecord);
                    }
                    fileWriter.WriteLine(dataRecord);
                }
            }
        }
    }
}
