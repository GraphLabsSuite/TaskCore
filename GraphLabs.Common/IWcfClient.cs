using System;
using System.ComponentModel;

namespace GraphLabs.Common
{
    /// <summary> Клиент WCF-сервиса </summary>
    public interface IWcfClient
    {
        /// <summary> Закрыть подключение </summary>
        void CloseAsync();

        /// <summary> Подключение закрыто </summary>
        event EventHandler<AsyncCompletedEventArgs> CloseCompleted;
    }
}