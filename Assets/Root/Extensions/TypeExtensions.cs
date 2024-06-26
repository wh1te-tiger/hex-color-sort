using System;
using System.Collections.Generic;
using System.Linq;

namespace Root
{
    public static class TypeExtensions
    {
        public static List<Type> GetChildTypes(this Type parentType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes(),
                    (domainAssembly, assemblyType) => new { domainAssembly, assemblyType })
                .Where(t => parentType.IsAssignableFrom(t.assemblyType))
                .Select(t => t.assemblyType).ToList().ToList();
        }
    }
}