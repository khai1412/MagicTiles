namespace GameCore.Extensions
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using VContainer;
    using VContainer.Internal;
    using VContainer.Unity;
    using Object = UnityEngine.Object;

    public static class DiExtension
    {
        public static LifetimeScope? CurrentSceneContext;

        [Obsolete("Obsolete")]
        public static IObjectResolver GetCurrentContainer()
        {
            var lifetimeScopes = Object.FindObjectsOfType<LifetimeScope>();

            foreach (var scope in lifetimeScopes)
            {
                // Check if the LifetimeScope is the scene-specific one
                if (scope.Parent != null) // Scene containers typically have a parent scope
                {
                    // Debug.Log("Scene-specific LifetimeScope found.");
                    var sceneContainer = scope.Container;

                    return sceneContainer;
                }
            }

            throw new Exception("Can not find current scene context");
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