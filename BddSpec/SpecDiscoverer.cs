
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BddSpec.Configuration;

namespace BddSpec
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
                return FilteredByClassOrNamespace();

            return AllSpecClassesTypes();
        }

        private static List<Type> FilteredByClassOrNamespace()
        {
            if (ExecutionConfiguration.SpecSelector.Contains("%"))
            {
                string namespaceFilter = "^" + Regex.Escape(ExecutionConfiguration.SpecSelector)
                    .Replace("%", ".*") + "$";

                Regex regex = new Regex(namespaceFilter, RegexOptions.IgnoreCase);

                Console.WriteLine("searching for: " + namespaceFilter);

                return AllSpecClassesTypes()
                    .Where(c => regex.IsMatch(c.FullName))
                    .ToList();
            }

            return AllSpecClassesTypes()
                .Where(c => c.Name.Equals(ExecutionConfiguration.SpecSelector, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}