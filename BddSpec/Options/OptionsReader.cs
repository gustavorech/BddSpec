
using CommandLine;
using BddSpec.Printer;
using System.Text.RegularExpressions;
using System;

namespace BddSpec.Options
{
    public class OptionsReader
    {
        public bool CreateExecuteConfigurationFromOptions(string[] args)
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

        private void PopulateExecutionConfiguration(Options options)
        {
            Configuration.SpecFilter = options.Filter;
            Configuration.ShowLine = options.ShowLine;
            Configuration.ShowTime = options.ShowTime;
            Configuration.ExecuteAsynchronous = options.ExecuteAsynchronous;

            PopulateVerbosityLevel(options);

            SeparateSpecificLineFromFilter();
        }

        public void PopulateVerbosityLevel(Options options)
        {
            switch (options.VerbosityLevel)
            {
                case OptionVerbosityLevel.errors:
                    Configuration.Verbosity = PrinterVerbosity.OnlyShowErrors;
                    break;
                case OptionVerbosityLevel.verbose:
                    Configuration.Verbosity = PrinterVerbosity.VerboseSteps;
                    break;
                case OptionVerbosityLevel.summary:
                    Configuration.Verbosity = PrinterVerbosity.VerboseAfterCompletion;
                    break;
            }
        }

        private void SeparateSpecificLineFromFilter()
        {
            if (string.IsNullOrEmpty(Configuration.SpecFilter))
                return;

            // ClassToTest:135 -- pos 1: ClassToTest; pos 3: 135
            Regex splitFilterAndNumber = new Regex("([^:]*)([:](.*))?");

            Match result = splitFilterAndNumber.Match(Configuration.SpecFilter);

            Configuration.SpecFilter = result.Groups[1].Value;

            failSilentlyConvertingTheLineNumber();
            void failSilentlyConvertingTheLineNumber()
            {
                if (int.TryParse(result.Groups[3].Value, out int value))
                    Configuration.SpecificLine = value;
            }

            AddMoreVerbosityIfIsSpecificLine();
        }

        public void AddMoreVerbosityIfIsSpecificLine()
        {
            if (Configuration.IsSpecificLine
                && Configuration.Verbosity == PrinterVerbosity.OnlyShowErrors)
            {
                Configuration.Verbosity = PrinterVerbosity.VerboseAfterCompletion;
                Configuration.ShowLine = true;
            }
        }
    }
}