using CommandLine;

namespace BddSpec.Core.Options {
    public class VerbosityOption
    {
        [Option("only-errors", SetName="only-errors", HelpText = "Set verbosity level to only show the erros at the end")]
        public bool OnlyErrors {get;set;}

        [Option("verbose", SetName="verbose", HelpText = "Set verbosity level to show every step on the process - this disables paralel execution")]
        public bool Verbose {get;set;}

        [Option("summary", SetName="summary", HelpText = "Set verbosity level to show a summary os all steps executed at the end")]
        public bool Summary {get;set;}
    }
}