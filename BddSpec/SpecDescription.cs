
namespace BddSpec
{
    public class SpecDescription
    {
        public string SourceFilePath { get; }
        public int SourceFileNumber { get; }
        public string TestDescription { get; }
        public string Verb { get; }

        public SpecDescription(string sourceFilePath, int sourceFileNumber,
            string testDescription, string verb)
        {
            SourceFilePath = sourceFilePath;
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            Verb = verb;
        }
    }
}
