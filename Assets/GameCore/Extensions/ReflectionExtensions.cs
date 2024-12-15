namespace GameCore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => !asm.IsDynamic)
                .SelectMany(baseType.GetDerivedTypes);
        }

        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly assembly)
        {
            return assembly.GetTypes().Where(type => !type.IsAbstract && baseType.IsAssignableFrom(type));
        }
    }
}