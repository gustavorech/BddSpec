
using CommandLine;

namespace BddSpec.Options
{
    public class Options
    {
        const string FILTER_HELP_TEXT =
@"Filter to execute a subset of the spec suite
- executes only one spec class. ex.: NameOfClassSpec
- executes a specific line of a spec class. ex.: NameOfClassSpec:120
- executes all spec from one namespace. ex.: BddSpec.Core.%
- any filter utilizing '%' for anything in between. ex.: %.Domain.%Client";

        const string VERBOSITY_HELP_TEXT =
@"Set the verbosity of the output
- e[rrors]  : only show the errors (if have) at the end of execution
- v[erbose] : print every step during the execution -- this disables paralel execution
- s[ummary] : print a summary (verbose) after the end of execution";

        [Value(0, MetaName = "Filter", Required = false, HelpText = FILTER_HELP_TEXT)]
        public string Filter { get; set; }

        [Option('v', "verbosity", Default = OptionVerbosityLevel.errors, HelpText = VERBOSITY_HELP_TEXT)]
        public OptionVerbosityLevel VerbosityLevel { get; set; }

        [Option('l', "show-line", Default = false, HelpText = "Shows the execution line of every step")]
        public bool ShowLine { get; set; }

        [Option('t', "show-time", Default = false, HelpText = "Shows the time spent on every step")]
        public bool ShowTime { get; set; }

        [Option('a', "async", Default = false, HelpText = "Executes spec classes asynchronous")]
        public bool ExecuteAsynchronous { get; set; }
    }
}