
namespace BddSpec.Core
{
    internal class TestStepDescription
    {
        internal string SourceFilePath { get; }
        internal int SourceFileNumber { get; }
        internal string TestDescription { get; }
        internal TestStepType ContextType { get; }
        internal string ContextTypeName { get => ContextType.ToString().ToLower(); }

        internal TestStepDescription(string sourceFilePath, int sourceFileNumber,
            string testDescription, TestStepType contextType)
        {
            SourceFilePath = sourceFilePath;
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            ContextType = contextType;
        }
    }
}
