using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Kledex.Extensions;
using Kledex.Store.EF.InMemory.Extensions;
using Me.One.Core.DependencyInjection;
using MediatorTutorials.Core.Configs;
using MediatorTutorials.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bootstrapper = MediatorTutorials.CommandHander.Bootstrapper;

namespace MediatorTutorials
{
    internal static class Program
    {
        private const string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dotnet-efcore;Integrated Security=True";

        //static void Main(string[] args)
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<Mediator>().As<IMediator>()
        //        .InstancePerLifetimeScope();

        //    MapperConfig.Init();

        //    builder.Register<ServiceFactory>(ctx =>
        //    {
        //        var c = ctx.Resolve<IComponentContext>();
        //        return t => c.Resolve(t);
        //    });

        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsImplementedInterfaces();

        //    var container = builder.Build();
        //    var mediator = container.Resolve<IMediator>();

        //    var response = mediator.Send(new PingCommand()).Result;
        //    Console.WriteLine($"We got a response at {response.Timestamp}");

        //    ConfigureContainer(builder);
        //}

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.StartAsync();
            await host.WaitForShutdownAsync();
            host.Dispose();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(_ =>
                    Console.WriteLine("Configure action for AutofacServiceProviderFactory")
                ))
                .ConfigureServices((_, services) => ConfigServices(services))
                .ConfigureContainer<ContainerBuilder>(ConfigContainer);
        }

        private static void ConfigContainer(ContainerBuilder builder)
        {
            var container = WireUp(builder);
            var mapper = MapperConfig.Init();
            container.RegisterInstance(mapper);
        }

        private static void ConfigServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext, CoreDataContext>(options =>
                options.UseSqlServer(ConnectionString, o =>
                    o.MigrationsAssembly("MediatorTutorials")));

            services.AddKledex(typeof(Bootstrapper),
                typeof(QueryHandler.Bootstrapper)).AddInMemoryStore();

            services.AddHostedService<WorkService>();
        }

        private static CoreContainerAdapter WireUp(ContainerBuilder builder)
        {
            var container = new CoreContainerAdapter(builder);

            container.RegisterInstance<IRegisterDependencies>(container);
            container.RegisterInstance<IResolveDependencies>(container);

            Business.Bootstrapper.CreateBootstrapper(container).WireUp();

            Data.Bootstrapper.CreateBootstrapper(container).WireUp();
            
            DependencyResolver.SetResolver(container);

            builder.RegisterBuildCallback(autofacContainer =>
            {
                container.SetContainerInstance(autofacContainer as IContainer);
            });

            return container;
        }
    }
}