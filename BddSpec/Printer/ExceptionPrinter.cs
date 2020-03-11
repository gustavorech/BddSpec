using System;
using System.Linq;

namespace BddSpec.Printer
{
    public class ExceptionPrinter
    {
        public static void Print(Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine();
            ConsolePrinter.WriteErrorLine("Failure: ", 1);
            exception.Message
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    ConsolePrinter.WriteErrorLine(messageLine, 2);
                });

            Console.WriteLine();
            ConsolePrinter.WriteInfoLine("StackTrace: ", 1);
            exception.StackTrace
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    ConsolePrinter.WriteInfoLine(messageLine, 2);
                });
        }
    }
}