
using System;

namespace BddSpec.Core.Printer
{
    public class ConsolePrinter
    {
        public static void WriteIdentation(int level) =>
            Console.Write(new string(' ', level * 2));

        public static void WriteMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteError(string message) =>
            WriteMessage(message, ConsoleColor.DarkRed);

        public static void WriteSuccess(string message) =>
            WriteMessage(message, ConsoleColor.Green);

        public static void WriteInfo(string message) =>
            WriteMessage(message, ConsoleColor.DarkGray);
    }
}
