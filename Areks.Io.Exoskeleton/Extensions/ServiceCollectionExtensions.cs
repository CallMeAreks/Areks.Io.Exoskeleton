using System;
using System.Collections.Generic;
using System.Linq;
using Areks.Io.Exoskeleton.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Areks.Io.Exoskeleton.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AutoRegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var settingTypes = GetSettingTypes();

            foreach (var settingType in settingTypes)
            {
                services.Configure(settingType, configuration.GetSection(settingType.Name));
            }
        }

        private static List<Type> GetSettingTypes()
        {
            return typeof(Startup).Assembly.ExportedTypes
                .Where(IsSettingSection)
                .ToList();
        }

        private static bool IsSettingSection(Type type) => MatchesType(typeof(BaseSettings), type);
        
        private static bool MatchesType(Type objectType, Type targetType)
        {
            return objectType.IsAssignableFrom(targetType) 
                   && targetType.IsClass
                   && !targetType.IsAbstract;
        }
        
        private static IServiceCollection Configure(this IServiceCollection services, Type type, IConfiguration configuration)
        {
            // Static type that contains the extension method
            var extMethodType = typeof(OptionsConfigurationServiceCollectionExtensions);

            // IConfiguration config
            
            // Find the overload for Configure<TOptions>(IServiceCollection, Action<TOptions>)
            // This could be more specific to make sure that all type arguments are exactly correct.
            // As it stands, this returns the correct overload but future updates to OptionsServiceCollectionExtensions
            // may add additional overloads which will require this to be updated.
            var genericConfigureMethodInfo = extMethodType.GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == "Configure")
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments() // Generic Type[] (ex [TOptions])
                })
                .Where(m => m.Args.Length == 1 && m.Params.Length == 2
                                               && m.Params[0].ParameterType == typeof(IServiceCollection))
                .Select(m => m.Method)
                .Single();

            var method = genericConfigureMethodInfo.MakeGenericMethod(type);

            // Invoke the method via reflection with our converted Action<objct> delegate
            // Since this is an extension method, it is static and services is passed
            // as the first parameter instead of the target object
            method.Invoke(null, new object[] { services, configuration });

            return services;
        }
    }
}