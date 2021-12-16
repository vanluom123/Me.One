namespace Me.One.Core.Security
{
    public static class AppDataProtectionDefaults
    {
        public static string RedisDataProtectionKey => "App.DataProtectionKeys";

        public static string AzureDataProtectionKeyFile => "DataProtectionKeys.xml";

        public static string DataProtectionKeysPath => "~/App_Data/DataProtectionKeys";
    }
}