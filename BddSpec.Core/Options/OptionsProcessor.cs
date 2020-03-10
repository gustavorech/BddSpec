
using CommandLine;
using BddSpec.Core.Printer;

namespace BddSpec.Core.Options
{
    internal class OptionsProcessor
    {
        internal static bool ReadOptions(string[] args)
        {
            bool isSuccess = true;

            Parser.Default.ParseArguments<StartOptions>(args)
                .WithParsed<StartOptions>(options =>
                {
                    ExecutionConfiguration.SpecSelector = options.SpecSelector;
                    ExecutionConfiguration.ShowLine = options.ShowLine;
                    ExecutionConfiguration.ShowTime = options.ShowTime;
                    ExecutionConfiguration.ExecuteAsynchronous = options.ExecuteAsynchronous;

                    switch (options.VerbosityLevel)
                    {
                        case OptionVerbosityLevel.errors:
                            ExecutionConfiguration.Verbosity = PrinterVerbosity.OnlyShowErrors;
                            break;
                        case OptionVerbosityLevel.verbose:
                            ExecutionConfiguration.Verbosity = PrinterVerbosity.VerboseSteps;
                            break;
                        case OptionVerbosityLevel.summary:
                            ExecutionConfiguration.Verbosity = PrinterVerbosity.VerboseAfterCompletion;
                            break;
                    }

                    if (options.VerbosityLevel == OptionVerbosityLevel.errors)
                        ExecutionConfiguration.Verbosity = PrinterVerbosity.OnlyShowErrors;
                })
                .WithNotParsed(c =>
                {
                    isSuccess = false;
                });

            return isSuccess;
        }
    }
}