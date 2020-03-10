
using System;

namespace BddSpec.Printer
{
    internal class ConsolePrinter
    {
        internal static void WriteIdentation(int level) =>
            Console.Write(new string(' ', level * 2));

        internal static void WriteMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        internal static void WriteError(string message) =>
            WriteMessage(message, ConsoleColor.DarkRed);

        internal static void WriteSuccess(string message) =>
            WriteMessage(message, ConsoleColor.Green);

        internal static void WriteInfo(string message) =>
            WriteMessage(message, ConsoleColor.DarkGray);
    }
}
