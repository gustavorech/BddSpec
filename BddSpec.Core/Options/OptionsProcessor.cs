using System.Runtime.CompilerServices;
using System;
using CommandLine;

namespace BddSpec.Core.Options {
    public class OptionsProcessor
    {
        public static CoreOptions CoreOptions{ get; private set; }
        public static bool ReadOptions(string[] args)
        {
            bool _isSuccess = true;

            Parser.Default.ParseArguments<CoreOptions>(args)
                .WithParsed<CoreOptions>(c => OptionsProcessor.CoreOptions = c)
                .WithNotParsed(c => {
                    _isSuccess = false;
                });

            return _isSuccess;
        }
    }
}