
using CommandLine;
using BddSpec.Printer;
using BddSpec.Configuration;
using System.Text.RegularExpressions;
using System;

namespace BddSpec.Options
{
    public class OptionsReader
    {
        public static bool CreateExecuteConfigurationFromOptions(string[] args)
        {
            bool isSuccess = true;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(options =>
                {
                    PopulateExecutionConfiguration(options);
                })
                .WithNotParsed(c =>
                {
                    isSuccess = false;
                });

            return isSuccess;
        }

        private static void PopulateExecutionConfiguration(Options options)
        {
            ExecutionConfiguration.SpecFilter = options.Filter;
            ExecutionConfiguration.ShowLine = options.ShowLine;
            ExecutionConfiguration.ShowTime = options.ShowTime;
            ExecutionConfiguration.ExecuteAsynchronous = options.ExecuteAsynchronous;

            PopulateVerbosityLevel(options);

            SeparateSpecificLineFromFilter();
        }

        public static void PopulateVerbosityLevel(Options options)
        {
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
        }

        private static void SeparateSpecificLineFromFilter()
        {
            if (string.IsNullOrEmpty(ExecutionConfiguration.SpecFilter))
                return;

            // ClassToTest:135 -- pos 1: ClassToTest; pos 3: 135
            Regex splitFilterAndNumber = new Regex("([^:]*)([:](.*))?");

            Match result = splitFilterAndNumber.Match(ExecutionConfiguration.SpecFilter);

            ExecutionConfiguration.SpecFilter = result.Groups[1].Value;

            failSilentlyConvertingTheLineNumber();
            void failSilentlyConvertingTheLineNumber()
            {
                if (int.TryParse(result.Groups[3].Value, out int value))
                    ExecutionConfiguration.SpecificLine = value;
            }

            AddMoreVerbosityIfIsSpecificLine();
        }

        public static void AddMoreVerbosityIfIsSpecificLine()
        {
            if (ExecutionConfiguration.IsSpecificLine
                && ExecutionConfiguration.Verbosity == PrinterVerbosity.OnlyShowErrors)
            {
                ExecutionConfiguration.Verbosity = PrinterVerbosity.VerboseAfterCompletion;
                ExecutionConfiguration.ShowLine = true;
            }
        }
    }
}