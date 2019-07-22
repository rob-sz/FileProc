namespace FileProc.DataReader.Sampler
{
    /// <summary>Sampler interface.</summary>
    public interface ISampler
    {
        /// <summary>Inserts sample value into record according to field part specification and format.</summary>
        /// <param name="targetRecord">The target record.</param>
        /// <param name="fieldParts">The field parts.</param>
        /// <param name="format">The format.</param>
        void InsertSample(char[] targetRecord, FieldPart[] fieldParts, string format);
    }
}
