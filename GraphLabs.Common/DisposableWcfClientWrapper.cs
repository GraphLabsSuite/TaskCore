using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

namespace GraphLabs.Common
{
    /// <summary> Disposable-обёртка для Wcf-клиента (чтобы не делать каждый раз Close вручную) </summary>
    public static class DisposableWcfClientWrapper
    {
        /// <summary> Обернуть </summary>
        /// <typeparam name="T">Wcf-клиент</typeparam>
        public static DisposableWcfClientWrapper<T> Create<T>(T client)
            where T : class, IWcfClient
        {
            return new DisposableWcfClientWrapper<T>(client);
        }
    }

    /// <summary> Disposable-обёртка для Wcf-клиента (чтобы не делать каждый раз Close вручную) </summary>
    /// <typeparam name="T">Wcf-клиент</typeparam>
    public class DisposableWcfClientWrapper<T> : IDisposable
        where T: class, IWcfClient
    {
        private const int CLOSE_WAITING_TIMEOUT_MS = 1000;

        /// <summary> Экземпляр клиента </summary>
        public T Instance { get; private set; }

        /// <summary> Disposable-обёртка для Wcf-клиента </summary>
        internal DisposableWcfClientWrapper(T instance)
        {
            Contract.Requires(instance != null);

            Instance = instance;
        }

        private bool _disposed = false;
        /// <summary> Уничтожаем </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            using (var closedEvent = new AutoResetEvent(false))
            {
                var closed = false;
                EventHandler<AsyncCompletedEventArgs> closeCompleted =
                    delegate
                    {
                        closed = true;
                        closedEvent.Set();
                    };
                Instance.CloseCompleted += closeCompleted;
                Instance.CloseAsync();
                closedEvent.WaitOne(CLOSE_WAITING_TIMEOUT_MS);
                Instance.CloseCompleted -= closeCompleted;
                Instance = null;

                if (!closed)
                {
                    Debug.WriteLine("Не удалось дождаться закрытия подключения {0}", Instance);
                }
            }
        }

        /// <summary> Финализатор </summary>
        ~DisposableWcfClientWrapper()
        {
            if (_disposed)
            {
                Debug.WriteLine("Disposable-обёртка для {0} не была уничтожена как следует!!!", Instance);
            }
        }
    }
}
