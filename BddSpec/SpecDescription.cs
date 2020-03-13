
namespace BddSpec
{
    public class SpecDescription
    {
        public int SourceFileNumber { get; }
        public string TestDescription { get; }
        public string Verb { get; }

        public SpecDescription(int sourceFileNumber, string testDescription, string verb)
        {
            SourceFileNumber = sourceFileNumber;
            TestDescription = testDescription;
            Verb = verb;
        }
    }
}
