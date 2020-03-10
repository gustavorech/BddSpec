
namespace BddSpec
{
    internal class SpecDescription
    {
        internal string SourceFilePath { get; }
        internal int SourceFileNumber { get; }
        internal string TestDescription { get; }
        internal SpecType ContextType { get; }
        internal string ContextTypeName { get => ContextType.ToString().ToLower(); }

        internal SpecDescription(string sourceFilePath, int sourceFileNumber,
            string testDescription, SpecType contextType)
        {
            SourceFilePath = sourceFilePath;
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            ContextType = contextType;
        }
    }
}
