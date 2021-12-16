﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Me.One.Core.DependencyInjection
{
    public class CoreContainerAdapter : IRegisterDependencies, IResolveDependencies
    {
        public CoreContainerAdapter(ContainerBuilder containerBuilder)
        {
            Builder = containerBuilder;
        }

        public IContainer ContainerInstance { get; private set; }

        public ContainerBuilder Builder { get; }

        public virtual void Register(Type type, params DependencyParameter[] parameters)
        {
            var registrationBuilder = Builder.RegisterType(type);
            if (parameters.Length == 0)
                return;
            registrationBuilder.WithParameters(
                parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
        }

        public virtual void Register<TFrom, TTo>(params DependencyParameter[] parameters) where TTo : TFrom
        {
            var registrationBuilder = Builder.RegisterType<TTo>().As<TFrom>();
            if (parameters.Length == 0)
                return;
            registrationBuilder.WithParameters(
                parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
        }

        public virtual void RegisterNamed<TFrom, TTo>(
            string name,
            LifeTimeScope scope,
            params DependencyParameter[] parameters)
            where TTo : TFrom
        {
            var registrationBuilder = Builder.RegisterType<TTo>();
            switch (scope)
            {
                case LifeTimeScope.InstancePerLifeTimeScoped:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case LifeTimeScope.InstancePerDependency:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            if (parameters.Length != 0)
                registrationBuilder.WithParameters(parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
            registrationBuilder.Named<TFrom>(name);
        }

        public virtual void RegisterSingleton<TFrom, TTo>(params DependencyParameter[] parameters) where TTo : TFrom
        {
            var registrationBuilder = Builder.RegisterType<TTo>().As<TFrom>();
            if (parameters.Length != 0)
                registrationBuilder.WithParameters(parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
            registrationBuilder.SingleInstance();
        }

        public virtual void RegisterSingletonNamed<TFrom, TTo>(
            string name,
            params DependencyParameter[] parameters)
            where TTo : TFrom
        {
            var registrationBuilder = Builder.RegisterType<TTo>();
            if (parameters.Length != 0)
                registrationBuilder.WithParameters(parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
            registrationBuilder.Named<TFrom>(name).SingleInstance();
        }

        public virtual void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            Builder.RegisterInstance(instance);
        }

        public virtual void RegisterInstanceNamed<TInterface>(TInterface instance, string name) where TInterface : class
        {
            Builder.RegisterInstance(instance).Named<TInterface>(name);
        }

        public void RegisterHub<TType>()
        {
            Builder.RegisterType<TType>().ExternallyOwned();
        }

        public virtual T Resolve<T>()
        {
            return ContainerInstance.Resolve<T>();
        }

        public virtual T Resolve<T>(string name)
        {
            return ContainerInstance.ResolveNamed<T>(name);
        }

        public virtual T ResolveOptional<T>() where T : class
        {
            return ContainerInstance.ResolveOptional<T>();
        }

        public virtual T ResolveOptional<T>(string name) where T : class
        {
            return ContainerInstance.ResolveOptionalNamed<T>(name);
        }

        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return ContainerInstance.Resolve<IEnumerable<T>>();
        }

        public virtual object Resolve(Type type)
        {
            return ContainerInstance.Resolve(type);
        }

        public virtual object Resolve(Type type, string name)
        {
            return ContainerInstance.ResolveNamed(name, type);
        }

        public virtual object ResolveOptional(Type type)
        {
            return ContainerInstance.ResolveOptional(type);
        }

        public virtual object ResolveOptional(Type type, string name)
        {
            return ContainerInstance.TryResolveNamed(name, type, out var obj) ? obj : null;
        }

        public bool IsRegistered<T>()
        {
            return ContainerInstance.IsRegistered<T>();
        }

        public bool IsRegistered(Type type)
        {
            return ContainerInstance.IsRegistered(type);
        }

        public bool IsRegistered<T>(string name)
        {
            return ContainerInstance.IsRegisteredWithName<T>(name);
        }

        public bool IsRegistered(Type type, string name)
        {
            return ContainerInstance.IsRegisteredWithName(name, type);
        }

        public void SetContainerInstance(IContainer container)
        {
            ContainerInstance = container;
        }

        public virtual void RegisterPerLifetime<TFrom, TTo>(params DependencyParameter[] parameters) where TTo : TFrom
        {
            var registrationBuilder = Builder.RegisterType<TTo>().As<TFrom>().InstancePerLifetimeScope();
            if (parameters.Length == 0)
                return;
            registrationBuilder.WithParameters(
                parameters.Select(
                    (Func<DependencyParameter, NamedParameter>) (p => new NamedParameter(p.Key, p.Value))));
        }

        public void RegisterModule(string fileName)
        {
        }

        public void RegisterModule(IModule module)
        {
            Builder.RegisterModule(module);
        }
    }
}