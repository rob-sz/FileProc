using System.Text;

namespace FileProc.DataReader.Parser
{
    /// <summary>Parser to extract string value from record.</summary>
    /// <seealso cref="FileProc.DataReader.Parser.Parser" />
    internal class StringParser : Parser
    {
        internal void ExtractValue(
            char[] sourceRecord,
            StringBuilder targetBuffer,
            FieldPart[] fieldParts)
        {
            ExtractRawValue(sourceRecord, targetBuffer, fieldParts);
        }
    }
}
