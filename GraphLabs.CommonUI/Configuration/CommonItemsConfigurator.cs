﻿using Autofac;
using GraphLabs.Tasks.Contract;
using GraphLabs.Utils.Services;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Общие зависимости </summary>
    public sealed class CommonItemsConfigurator : IDependencyResolverConfigurator
    {
        /// <summary> Выполнить конфигурацию </summary>
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<DateTimeService>().As<IDateTimeService>();
            builder.RegisterType<InitParamsProvider>().As<IInitParamsProvider>();
        }
    }
}