
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public class SpecDiscoverer
    {
        public static List<Type> AllSpecClassesTypes() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                {
                    return typeof(SpecClass).IsAssignableFrom(x)
                    && !x.IsInterface
                    && !x.IsAbstract;
                })
                .ToList();

        public static List<Type> FilteredSpecClassesTypes()
        {
            bool isSpecFiltered =
                !string.IsNullOrEmpty(Configuration.SpecFilter);

            if (isSpecFiltered)
                return FilteredByRegexMatchOrClassName();

            return AllSpecClassesTypes();
        }

        private static List<Type> FilteredByRegexMatchOrClassName()
        {
            if (Configuration.SpecFilter.Contains("%"))
                return FilteredByRegexMatch();
            else
                return FilteredByClassName();
        }

        private static List<Type> FilteredByClassName()
        {
            ExecutionPrinter.PrintSpecDiscovererFilter(Configuration.SpecFilter);

            return AllSpecClassesTypes()
                .Where(c => c.Name.Equals(Configuration.SpecFilter, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private static List<Type> FilteredByRegexMatch()
        {
            string namespaceFilter = "^" + Regex.Escape(Configuration.SpecFilter)
                .Replace("%", ".*") + "$";

            Regex regex = new Regex(namespaceFilter, RegexOptions.IgnoreCase);

            ExecutionPrinter.PrintSpecDiscovererFilter(namespaceFilter);

            return AllSpecClassesTypes()
                .Where(c => regex.IsMatch(c.FullName))
                .ToList();
        }
    }
}