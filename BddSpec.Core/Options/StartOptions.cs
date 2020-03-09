
using CommandLine;

namespace BddSpec.Core.Options
{
    public class StartOptions
    {
        const string VERBOSITY_HELP_TEXT =
@"set the verbosity of the output
- e[rrors]  : only show the errors at the end of execution
- v[erbose] : print every step during the execution -- this disables paralel execution
- s[ummary] : print every step only after end of execution";


        [Value(0, MetaName = "SpecSelector", Required = false, HelpText = "Spec class to execute")]
        public string SpecSelector { get; set; }

        [Option('v', "verbosity", Default = OptionVerbosityLevel.errors, HelpText = VERBOSITY_HELP_TEXT)]
        public OptionVerbosityLevel VerbosityLevel { get; set; }

        [Option('l', "show-line", Default = false, HelpText = "Shows the execution line after the step")]
        public bool ShowLine { get; set; }

        [Option('t', "show-time", Default = false, HelpText = "Shows the execution time after the step")]
        public bool ShowTime { get; set; }

        [Option('a', "async", Default = false, HelpText = "Executes spec classes asynchronous")]
        public bool ExecuteAsynchronous { get; set; }
    }
}