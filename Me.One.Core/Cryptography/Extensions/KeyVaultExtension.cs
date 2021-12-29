using System;
using System.Linq;
using System.Reflection;

namespace Me.One.Core.Cryptography.Extensions
{
    public static class KeyVaultExtension
    {
        private static AzureKeyVault _vault;

        public static void InitVault(string client, string clientSecret, string name)
        {
            _vault = new AzureKeyVault(client, clientSecret, name);
        }

        public static void ApplyVault(this IConfigableSetting setting, string sectionName)
        {
            if (setting == null)
                throw new NullReferenceException("Appsettings null");
            if (_vault == null)
                throw new NullReferenceException("Need to call InitVault before apply vault to setting");
            ApplyKeyVaultProperties(sectionName, setting.GetType(), setting);
        }

        private static void ApplyKeyVaultProperties(string prefix, Type type, object instance)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            foreach (var info in propertyInfos)
            {
                try
                {
                    var propertyType = info.PropertyType;
                    if (!propertyType.IsArray && info.GetCustomAttributes<KeyVaultAttribute>().Any() &&
                        instance != null)
                    {
                        var result = _vault.GetKey(prefix + "-" + info.Name).Result;
                        if (result != null)
                        {
                            info.SetValue(instance, Convert.ChangeType(result, propertyType));
                        }
                    }

                    if (!propertyType.IsClass)
                        continue;
                    
                    if (!propertyType.FullName!.StartsWith("System."))
                    {
                        ApplyKeyVaultProperties(prefix + "-" + info.Name, propertyType,
                            info.GetValue(instance));
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}