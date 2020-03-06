using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace bddlike
{
    public class Reflection
    {
        public static void Execute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            IEnumerable<Type> testTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                typeof(BddLike).IsAssignableFrom(x)
                && !x.IsInterface
                && !x.IsAbstract);

            List<TestExecutor> testExecutors = Asincronous(testTypes);

            VerifyPrintAll(testExecutors);

            testExecutors.ToList().ForEach(c => c.PrintOnlyErrors());

            Console.WriteLine("ACABEI: " + timer.Elapsed.ToString());
        }

        private static List<TestExecutor> Sincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .Select(type =>
                {
                    TestExecutor testExecutor = new TestExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static List<TestExecutor> Asincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .AsParallel()
                .Select(type =>
                {
                    TestExecutor testExecutor = new TestExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static void VerifyPrintAll(List<TestExecutor> testExecutors)
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
