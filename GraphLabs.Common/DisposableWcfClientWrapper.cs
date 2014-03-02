using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

namespace GraphLabs.Common
{
    /// <summary> Disposable-обёртка для Wcf-клиента (чтобы не делать каждый раз Close вручную) </summary>
    /// <typeparam name="T">Wcf-клиент</typeparam>
    public class DisposableWcfClientWrapper<T> : IDisposable
        where T: class, IWcfClient
    {
        private const int CloseWaitingTimeoutMs = 1000;

        /// <summary> Экземпляр клиента </summary>
        public T Instance { get; private set; }

        /// <summary> Disposable-обёртка для Wcf-клиента </summary>
        public DisposableWcfClientWrapper(T instance)
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
                closedEvent.WaitOne(CloseWaitingTimeoutMs);
                if (!closed)
                {
                    Debug.WriteLine("Не удалось дождаться закрытия подключения {0}", Instance);
                }
                Instance.CloseCompleted -= closeCompleted;
                Instance = null;
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
