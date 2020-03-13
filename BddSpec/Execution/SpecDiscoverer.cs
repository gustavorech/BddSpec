
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BddSpec.Configuration;
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
                !string.IsNullOrEmpty(ExecutionConfiguration.SpecSelector);

            if (isSpecFiltered)
                return FilteredByRegexMatchOrClassName();

            return AllSpecClassesTypes();
        }

        private static List<Type> FilteredByRegexMatchOrClassName()
        {
            if (ExecutionConfiguration.SpecSelector.Contains("%"))
                return FilteredByRegexMatch();
            else
                return FilteredByClassName();
        }

        private static List<Type> FilteredByClassName()
        {
            ExecutionPrinter.NotifySpecDiscovererFilter(ExecutionConfiguration.SpecSelector);

            return AllSpecClassesTypes()
                .Where(c => c.Name.Equals(ExecutionConfiguration.SpecSelector, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private static List<Type> FilteredByRegexMatch()
        {
            string namespaceFilter = "^" + Regex.Escape(ExecutionConfiguration.SpecSelector)
                .Replace("%", ".*") + "$";

            Regex regex = new Regex(namespaceFilter, RegexOptions.IgnoreCase);

            ExecutionPrinter.NotifySpecDiscovererFilter(namespaceFilter);

            return AllSpecClassesTypes()
                .Where(c => regex.IsMatch(c.FullName))
                .ToList();
        }
    }
}