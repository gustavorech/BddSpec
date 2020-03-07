
namespace BddSpec.Core
{
    public class TestStepDescription
    {
        public string SourceFilePath { get; }
        public int SourceFileNumber { get; }
        public string TestDescription { get; }
        public TestStepType ContextType { get; }
        public string ContextTypeName { get => ContextType.ToString().ToLower(); }

        public TestStepDescription(string sourceFilePath, int sourceFileNumber,
            string testDescription, TestStepType contextType)
        {
            SourceFilePath = sourceFilePath;
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            ContextType = contextType;
        }
    }
}
