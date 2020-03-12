
using CommandLine;
using BddSpec.Printer;
using BddSpec.Configuration;
using System.Text.RegularExpressions;
using System;

namespace BddSpec.Options
{
    public class OptionsProcessor
    {
        public static bool CreateExecuteConfigurationFromOptions(string[] args)
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

                    VerifyAndSeparateSpecificLine();
                })
                .WithNotParsed(c =>
                {
                    isSuccess = false;
                });

            return isSuccess;
        }

        private static void VerifyAndSeparateSpecificLine()
        {
            if (string.IsNullOrEmpty(ExecutionConfiguration.SpecSelector))
                return;

            // ClassToTest:135 -- pos 1: ClassToTest; pos 3: 135
            Regex splitFilterAndNumber = new Regex("([^:]*)([:](.*))?");

            Match result = splitFilterAndNumber.Match(ExecutionConfiguration.SpecSelector);

            ExecutionConfiguration.SpecSelector = result.Groups[1].Value;

            failSilentlyConvertingTheLineNumber();
            void failSilentlyConvertingTheLineNumber()
            {
                if (int.TryParse(result.Groups[3].Value, out int value))
                    ExecutionConfiguration.SpecificLine = value;
            }

            if (ExecutionConfiguration.IsSpecificLine
                && ExecutionConfiguration.Verbosity == PrinterVerbosity.OnlyShowErrors)
                ExecutionConfiguration.Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        }
    }
}