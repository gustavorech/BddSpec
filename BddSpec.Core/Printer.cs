﻿
using System;
using bddlike;

namespace bddlike
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

    public class TestExecutionStepPrinter
    {
        public static void Print(TestExecutionStep executionStep)
        {
            TestContextDescription contextDescription = executionStep.TestContextDescription;

            ConsolePrinter.WriteIdentation(executionStep.StepLevel);

            PrintDescription(executionStep, contextDescription);

            ConsolePrinter.WriteInfo($" (ln:{contextDescription.SourceFileNumber})");

            PrintStepTimeSpan(executionStep.TimeSpent);

            Console.WriteLine();
        }

        private static void PrintDescription(TestExecutionStep executionStep, TestContextDescription contextDescription)
        {
            string message = $"{contextDescription.ContextTypeName}: {contextDescription.TestDescription}";

            if (executionStep.HasExecutionError)
                ConsolePrinter.WriteError(message);
            else if (executionStep.Children.Count == 0)
                ConsolePrinter.WriteSuccess(message);
            else
                Console.Write(message);
        }

        private static void PrintStepTimeSpan(TimeSpan time)
        {
            if (time > TimeSpan.FromMinutes(1))
                ConsolePrinter.WriteInfo($" ({time.ToString("mm")}:{time.ToString("ss")}minutes)");
            else if (time > TimeSpan.FromSeconds(1))
                ConsolePrinter.WriteInfo($" ({time.ToString("ss")}:{time.ToString("fff")}s)");
            else if (time > TimeSpan.FromMilliseconds(100))
                ConsolePrinter.WriteInfo(" (" + time.ToString("fff") + "ms)");
            else if (time > TimeSpan.FromMilliseconds(10))
                ConsolePrinter.WriteInfo(" (" + time.ToString("fff").Substring(1) + "ms)");
        }
    }
}