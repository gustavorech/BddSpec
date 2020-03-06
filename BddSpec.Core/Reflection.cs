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

            Asincronous(testTypes);

            Console.WriteLine("ACABEI: " + timer.Elapsed.ToString());
        }

        private static void Sincronous(IEnumerable<Type> testTypes)
        {
            IEnumerable<TestExecutor> testExecutors = testTypes
            .Select(type =>
                {
                    TestExecutor testExecutor = new TestExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                });

            VerifyPrintAll(testExecutors);
        }

        private static void Asincronous(IEnumerable<Type> testTypes)
        {
            IEnumerable<TestExecutor> testExecutors = testTypes
            .AsParallel()
            .Select(type =>
                {
                    TestExecutor testExecutor = new TestExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                });

            VerifyPrintAll(testExecutors);
        }

        private static void VerifyPrintAll(IEnumerable<TestExecutor> testExecutors)
        {
            if (CentralizedPrinter.Strategy == PrinterStrategy.VerboseAfterCompletion)
                testExecutors.ToList()
                .ForEach(testExecutor =>
                {
                    testExecutor.Print();
                });
        }
    }
}
