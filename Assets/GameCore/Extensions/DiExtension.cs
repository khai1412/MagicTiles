namespace GameCore.Extensions
{
    using System;
    using System.Collections.Generic;
    using VContainer;
    using VContainer.Internal;
    using VContainer.Unity;
    using Object = UnityEngine.Object;

    public static class DiExtension
    {
        private static LifetimeScope? CurrentSceneContext;

        public static IObjectResolver GetCurrentContainer()
        {
            if (CurrentSceneContext == null) CurrentSceneContext = Object.FindObjectOfType<LifetimeScope>();
            return CurrentSceneContext!.Container;
        }

        public static IObjectResolver GetCurrentContainer(this object _)
        {
            return GetCurrentContainer();
        }

        public static object Instantiate(this IObjectResolver container, Type type, IReadOnlyList<IInjectParameter>? parameters = null)
        {
            return InjectorCache.GetOrBuild(type).CreateInstance(container, parameters);
        }

        public static RegistrationBuilder AsInterfacesAndSelf(this RegistrationBuilder registrationBuilder)
        {
            return registrationBuilder.AsImplementedInterfaces().AsSelf();
        }
    }
}