using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Me.One.Core.Cryptography
{
    public class AzureKeyVault : IDisposable
    {
        private const string KEY_URL_PATTERN = "https://{0}.vault.azure.net/";
        private KeyVaultClient _keyClient;
        private string _keyUrl = "";

        public AzureKeyVault(string clientId, string clientSecret, string name)
        {
            _keyUrl = string.Format("https://{0}.vault.azure.net/", name);
            var serviceTokenProvider = new AzureServiceTokenProvider();
            _keyClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var authenticationContext = new AuthenticationContext(authority);
                var clientCredential1 = new ClientCredential(clientId, clientSecret);
                var resource1 = resource;
                var clientCredential2 = clientCredential1;
                return (await authenticationContext.AcquireTokenAsync(resource1, clientCredential2)).AccessToken;
            }, Array.Empty<DelegatingHandler>());
        }

        public void Dispose()
        {
            _keyClient = null;
            _keyUrl = "";
        }

        public async Task SaveKey(string name, string value)
        {
            var secretBundle = await _keyClient.SetSecretAsync(_keyUrl ?? "", name, value);
        }

        public async Task<string> GetKey(string name)
        {
            try
            {
                return (await _keyClient.GetSecretAsync(_keyUrl ?? "", name)).Value;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }
    }
}