﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public class TestDiscoverer
    {
        public static void DiscoverAndExecute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            IEnumerable<Type> testTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                typeof(BddLike).IsAssignableFrom(x)
                && !x.IsInterface
                && !x.IsAbstract);

            List<TestClassExecutor> testExecutors = Sincronous(testTypes);

            VerifyPrintAll(testExecutors);

            testExecutors.ToList().ForEach(c => c.PrintOnlyErrors());

            Console.WriteLine("ACABEI: " + timer.Elapsed.ToString());
        }

        private static List<TestClassExecutor> Sincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .Select(type =>
                {
                    TestClassExecutor testExecutor = new TestClassExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static List<TestClassExecutor> Asincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .AsParallel()
                .Select(type =>
                {
                    TestClassExecutor testExecutor = new TestClassExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static void VerifyPrintAll(List<TestClassExecutor> testExecutors)
        {
            if (CentralizedPrinter.Strategy == PrinterStrategy.VerboseAfterCompletion)
                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.Print();
                    });
        }
    }
}