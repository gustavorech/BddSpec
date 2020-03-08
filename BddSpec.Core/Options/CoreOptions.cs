using System;
using CommandLine;

namespace BddSpec.Core.Options {

    public enum VerbosityLevel
    {
        errors = 0,
        e = 0,
        verbose = 1,
        v = 1,
        summary = 2,
        s = 2
    }

    public class CoreOptions
    {
        const string VERBOSITY_HELP_TEXT = 
@"set the verbosity of the output
- e[rrors]  : at the end of execution only show the errors
- v[erbose] : print every step during the execution -- this disables paralel execution
- s[ummary] : print every step only after end of execution";

        [Option('v', "verbosity", Default = VerbosityLevel.errors, HelpText = VERBOSITY_HELP_TEXT)]
        public VerbosityLevel VerbosityLevel { get; set; }
    }
}