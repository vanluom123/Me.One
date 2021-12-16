using System;
using System.Linq;
using System.Reflection;

namespace Me.One.Core.Cryptography.Extensions
{
    public static class KeyVaultExtension
    {
        private static AzureKeyVault Vault;

        public static void InitVault(string client, string clientSecret, string name)
        {
            Vault = new AzureKeyVault(client, clientSecret, name);
        }

        public static void ApplyVault(this IConfigableSetting setting, string sectionName)
        {
            if (setting == null)
                throw new NullReferenceException("Appsettings null");
            if (Vault == null)
                throw new NullReferenceException("Need to call InitVault before apply vault to setting");
            ApplyKeyVaultProperties(sectionName, setting.GetType(), setting);
        }

        private static void ApplyKeyVaultProperties(string prefix, Type type, object instance)
        {
            var list = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            for (var index = 0; index < list.Count; ++index)
                try
                {
                    var propertyType = list[index].PropertyType;
                    if (!propertyType.IsArray && list[index].GetCustomAttributes<KeyVaultAttribute>().Any() &&
                        instance != null)
                    {
                        var result = Vault.GetKey(prefix + "-" + list[index].Name).Result;
                        if (result != null)
                            list[index].SetValue(instance, Convert.ChangeType(result, propertyType));
                    }

                    if (propertyType.IsClass)
                        if (!propertyType.FullName.StartsWith("System."))
                            ApplyKeyVaultProperties(prefix + "-" + list[index].Name, propertyType,
                                list[index].GetValue(instance));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }
        }
    }
}