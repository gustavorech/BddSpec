
namespace BddSpec
{
    internal class SpecDescription
    {
        internal string SourceFilePath { get; }
        internal int SourceFileNumber { get; }
        internal string TestDescription { get; }
        internal string Verb { get; }

        internal SpecDescription(string sourceFilePath, int sourceFileNumber,
            string testDescription, string verb)
        {
            SourceFilePath = sourceFilePath;
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            Verb = verb;
        }
    }
}
