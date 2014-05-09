using Autofac;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Конфигуратор IOC-контейнера </summary>
    public interface IDependencyResolverConfigurator
    {
        /// <summary> Выполнить конфигурацию </summary>
        void Configure(ContainerBuilder builder);
    }
}