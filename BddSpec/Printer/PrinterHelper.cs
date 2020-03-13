
using System;

namespace BddSpec.Printer
{
    public class PrinterHelper
    {
        public static void WriteIdentation(int level) =>
            Console.Write(new string(' ', level * 2));

        public static void WriteMessage(string message, ConsoleColor color, int level)
        {
            if (level > 0)
                WriteIdentation(level);

            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteMessageLine(string message, ConsoleColor color, int level)
        {
            if (level > 0)
                WriteIdentation(level);

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteError(string message, int level = 0) =>
            WriteMessage(message, ConsoleColor.DarkRed, level);

        public static void WriteSuccess(string message, int level = 0) =>
            WriteMessage(message, ConsoleColor.Green, level);

        public static void WriteInfo(string message, int level = 0) =>
            WriteMessage(message, ConsoleColor.DarkGray, level);

        public static void WriteErrorLine(string message, int level = 0) =>
            WriteMessageLine(message, ConsoleColor.DarkRed, level);

        public static void WriteSuccessLine(string message, int level = 0) =>
            WriteMessageLine(message, ConsoleColor.Green, level);

        public static void WriteInfoLine(string message, int level = 0) =>
            WriteMessageLine(message, ConsoleColor.DarkGray, level);
    }
}
