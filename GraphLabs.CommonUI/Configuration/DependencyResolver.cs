using System;
using System.Diagnostics.Contracts;
using Autofac;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> IOC-контейнер </summary>
    internal static class DependencyResolver
    {
        /// <summary> Экземпляр IOC-контейнера </summary>
        public static IContainer Current
        {
            get { return _current; }
            private set
            {
                CheckNotBuilt();
                _current = value;
            }
        }

        private static bool _built = false;

        private static readonly Lazy<ContainerBuilder> _builder = new Lazy<ContainerBuilder>();
        private static IContainer _current;

        /// <summary> Получить построитель контейнера (чтобы зарегистрировать в нём ещё что-нибудь) </summary>
        /// <remarks> Имеет смыл до тех пор, пока контейнер не был сконфигурирован. </remarks>
        internal static ContainerBuilder GetBuilder()
        {
            CheckNotBuilt();

            return _builder.Value;
        }

        /// <summary> Сконфигурировать IOC-контейнер </summary>
        internal static void Setup()
        {
            CheckNotBuilt();

            var builder = _builder.Value;
            
            Current = builder.Build();
            _built = true;
        }

        private static void CheckNotBuilt()
        {
            Contract.Requires<InvalidOperationException>(!_built, "IOC-контейнер уже сконфигурирован.");
        }
    }
}
