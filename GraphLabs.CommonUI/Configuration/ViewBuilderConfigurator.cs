using Autofac;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Регистрирует в IOC-контейнере реализацию IViewBuilder </summary>
    public class ViewBuilderConfigurator<T> : IDependencyResolverConfigurator
        where T: IViewBuilder
    {
        /// <summary> Выполнить конфигурацию </summary>
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<T>().As<IViewBuilder>();
        }
    }
}