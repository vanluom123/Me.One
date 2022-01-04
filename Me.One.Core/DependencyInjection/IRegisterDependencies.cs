using System;

namespace Me.One.Core.DependencyInjection
{
    public interface IRegisterDependencies
    {
        void Register(Type type, params DependencyParameter[] parameters);

        void Register<TFrom, TTo>(params DependencyParameter[] parameters) where TTo : TFrom;

        void RegisterNamed<TFrom, TTo>(
            string name,
            LifeTimeScope scope,
            params DependencyParameter[] parameters)
            where TTo : TFrom;

        void RegisterSingleton<TFrom, TTo>(params DependencyParameter[] parameters) where TTo : TFrom;

        void RegisterSingletonNamed<TFrom, TTo>(string name, params DependencyParameter[] parameters) where TTo : TFrom;

        void RegisterInstance<TInterface>(TInterface instance) where TInterface : class;

        void RegisterInstanceNamed<TInterface>(TInterface instance, string name) where TInterface : class;

        void RegisterHub<TType>();

        Autofac.Builder.IRegistrationBuilder<object, Autofac.Features.Scanning.ScanningActivatorData, Autofac.Builder.DynamicRegistrationStyle> RegisterAssemblyTypes(params System.Reflection.Assembly[] assemblies);

        Autofac.Builder.IRegistrationBuilder<object, Autofac.Features.Scanning.OpenGenericScanningActivatorData, Autofac.Builder.DynamicRegistrationStyle> RegisterAssemblyOpenGenericTypes(params System.Reflection.Assembly[] assemblies);

    }
}