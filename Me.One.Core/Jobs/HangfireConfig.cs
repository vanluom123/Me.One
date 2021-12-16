using System;
using Autofac;
using Hangfire;
using Hangfire.Azure;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Me.One.Core.Jobs
{
    public class HangfireConfig
    {
        public IServiceCollection Init(
            string type,
            string connection,
            IServiceCollection services)
        {
            switch (type)
            {
                case "Cosmos":
                    var strArray = connection.Split(';');
                    var url = strArray[0];
                    var secretkey = strArray[1];
                    var database = strArray[2];
                    var collection = strArray[3];
                    var options = new DocumentDbStorageOptions
                    {
                        RequestTimeout = TimeSpan.FromSeconds(30.0),
                        ExpirationCheckInterval = TimeSpan.FromMinutes(2.0),
                        CountersAggregateInterval = TimeSpan.FromMinutes(2.0),
                        QueuePollInterval = TimeSpan.FromSeconds(15.0),
                        ConnectionMode = ConnectionMode.Direct,
                        ConnectionProtocol = Protocol.Tcp,
                        EnablePartition = true
                    };
                    services.AddHangfire(
                        o => o.UseAzureDocumentDbStorage(url, secretkey, database, collection, options));
                    JobStorage.Current = new DocumentDbStorage(url, secretkey, database, collection, options);
                    break;
                case "Sql":
                    services.AddHangfire(o => o.UseSqlServerStorage(connection));
                    JobStorage.Current = new SqlServerStorage(connection);
                    break;
                default:
                    services.AddHangfire(o => o.UseMemoryStorage());
                    JobStorage.Current = new MemoryStorage();
                    break;
            }

            return services;
        }

        public BackgroundJobServer StartServer(IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivatorCore(serviceProvider));
            return StartServer();
        }

        public BackgroundJobServer StartServerWithOverrideOptions(
            IServiceProvider serviceProvider,
            BackgroundJobServerOptions options)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivatorCore(serviceProvider));
            return StartServer(options);
        }

        public BackgroundJobServer StartServer(IContainer container)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireAutofacCore(container));
            return StartServer();
        }

        public BackgroundJobServer StartServerWithOverrideOptions(
            IContainer container,
            BackgroundJobServerOptions options)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireAutofacCore(container));
            return StartServer(options);
        }

        public void SendStopServer(BackgroundJobServer serverInstance)
        {
            serverInstance?.SendStop();
        }

        public void DisposeServer(BackgroundJobServer serverInstance)
        {
            serverInstance?.Dispose();
        }

        public void StartServerWithAutofacContainer(
            IContainer container,
            BackgroundJobServerOptions options)
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(container);
            var backgroundJobServer = new BackgroundJobServer(options ?? new BackgroundJobServerOptions());
        }

        private BackgroundJobServer StartServer()
        {
            return new(new BackgroundJobServerOptions());
        }

        private BackgroundJobServer StartServer(BackgroundJobServerOptions options)
        {
            return new(options);
        }
    }
}