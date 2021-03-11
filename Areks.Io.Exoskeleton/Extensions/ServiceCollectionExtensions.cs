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
            return typeof(Startup).Assembly.ExportedTypes.Where(TypeInheritsBaseSettings).ToList();
        }

        private static bool TypeInheritsBaseSettings(Type type) => typeof(BaseSettings).InheritsType(type);
        
        private static bool InheritsType(this Type objectType, Type targetType)
        {
            return objectType.IsAssignableFrom(targetType) 
                   && targetType.IsClass
                   && !targetType.IsInterface
                   && !targetType.IsAbstract;
        }
        
        private static void Configure(this IServiceCollection services, Type type, IConfiguration configuration)
        {
            // Static type that contains the extension method
            var extMethodType = typeof(OptionsConfigurationServiceCollectionExtensions);
            
            // Find the overload for Configure<TOptions>(IServiceCollection, IConfiguration)
            var genericConfigureMethodInfo = extMethodType.GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == "Configure")
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments() // Generic Type[] (ex [TOptions])
                })
                .Where(m => m.Args.Length == 1 && m.Params.Length == 2
                                               && m.Params[0].ParameterType == typeof(IServiceCollection)
                                               && m.Params[1].ParameterType == typeof(IConfiguration))
                .Select(m => m.Method)
                .Single();

            var method = genericConfigureMethodInfo.MakeGenericMethod(type);

            // Invoke the method via reflection with the IServiceCollection and IConfiguration objects
            method.Invoke(null, new object[] { services, configuration });
        }
    }
}