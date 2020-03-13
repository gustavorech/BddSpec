using System;
using System.Linq;

namespace BddSpec.Printer
{
    public class ExceptionPrinter
    {
        public static void Print(Exception exception)
        {
            Console.WriteLine();
            PrinterHelper.WriteErrorLine("Failure: ", 1);
            exception.Message
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    PrinterHelper.WriteErrorLine(messageLine, 2);
                });

            Console.WriteLine();
            PrinterHelper.WriteInfoLine("StackTrace: ", 1);
            exception.StackTrace
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    PrinterHelper.WriteInfoLine(messageLine, 2);
                });
            Console.WriteLine();
        }
    }
}