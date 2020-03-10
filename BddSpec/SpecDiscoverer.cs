
using System;
using System.Collections.Generic;
using System.Linq;
using BddSpec.Configuration;

namespace BddSpec
{
    internal class SpecDiscoverer
    {
        internal static List<Type> AllSpecClassesTypes() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                {
                    return typeof(SpecClass).IsAssignableFrom(x)
                    && !x.IsInterface
                    && !x.IsAbstract;
                })
                .ToList();

        internal static List<Type> FilteredSpecClassesTypes()
        {
            bool isSpecFiltered =
                !string.IsNullOrEmpty(ExecutionConfiguration.SpecSelector);

            if (isSpecFiltered)
                return AllSpecClassesTypes()
                    .Where(c => c.Name == ExecutionConfiguration.SpecSelector)
                    .ToList();

            return AllSpecClassesTypes();
        }
    }
}