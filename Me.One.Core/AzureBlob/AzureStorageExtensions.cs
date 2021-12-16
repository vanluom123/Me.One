using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Me.One.Core.AzureBlob
{
    public static class AzureStorageExtensions
    {
        public static IServiceCollection AddStorageService(
            this IServiceCollection services,
            IConfiguration config,
            string configSection = "StorageSettings")
        {
            var str = config.GetValue<string>(configSection + ":StorageMode");
            var section = config.GetSection(configSection + ":" + str);
            var type = Type.GetType(section.GetValue<string>("ServiceType"));
            services.AddSingleton(typeof(ICloudStorage), Activator.CreateInstance(type, section));
            return services;
        }

        public static IServiceCollection AddAzureStorageService(
            this IServiceCollection services,
            AzureBlobSettings settings)
        {
            var type = Type.GetType(settings.ServiceType);
            services.AddSingleton(typeof(ICloudStorage), Activator.CreateInstance(type, settings));
            return services;
        }
    }
}