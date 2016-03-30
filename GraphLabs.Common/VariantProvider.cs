using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using GraphLabs.Common.VariantProviderService;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Поставщик вариантов задания </summary>
    public sealed class VariantProvider : INotifyPropertyChanged, IUiBlockerAsyncProcessor
    {
        private readonly long _taskId;
        private readonly Guid _sessionGuid;
        private readonly Version[] _allowedVariantGenerationVersions;

        /// <summary> Сервис получения вариантов </summary>
        private readonly IVariantProviderServiceClient _providerServiceClient;


        #region INotifyPropertyChanged

        /// <summary> Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a property value changes. </summary>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            Interlocked.CompareExchange(ref PropertyChanged, null, null)
                ?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary> Выполняется бокирующая UI операция </summary>
        public bool IsBusy 
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value)
                {
                    return;
                }
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        private bool _isBusy = false;

        #endregion


        /// <summary> Поставщик вариантов задания </summary>
        /// <param name="taskId">Id задания</param>
        /// <param name="sessionGuid">Guid сессии</param>
        /// <param name="allowedVariantGenerationVersions">Допустимые версии генераторов. АХТУНГ! Имеет смысл только мажорная часть версии!</param>
        /// <param name="client">Клиент</param>
        public VariantProvider(long taskId, Guid sessionGuid, Version[] allowedVariantGenerationVersions, 
            DisposableWcfClientWrapper<IVariantProviderServiceClient> client)
        {
            Contract.Requires(client != null);
            Contract.Requires(sessionGuid != null);
            Contract.Requires(allowedVariantGenerationVersions != null && allowedVariantGenerationVersions.Any());
            
            _taskId = taskId;
            _sessionGuid = sessionGuid;
            _allowedVariantGenerationVersions = allowedVariantGenerationVersions;
            

            _providerServiceClient = client.Instance;
            _providerServiceClient.GetVariantCompleted += ProviderServiceClientOnGetVariantCompleted;
        }


        #region Получение варианта

        private bool _getVariantInvoked = false;

        /// <summary> Запустить получение варианта </summary>
        public void DownloadVariantAsync()
        {
            if (_getVariantInvoked)
                throw new InvalidOperationException("Получение варианта уже было запущено.");

            _getVariantInvoked = true;
            IsBusy = true;
            _providerServiceClient.GetVariantAsync(_taskId, _sessionGuid);
        }

        private void ProviderServiceClientOnGetVariantCompleted(object sender, GetVariantCompletedEventArgs getVariantCompletedEventArgs)
        {
            Contract.Requires(getVariantCompletedEventArgs != null);
            Contract.Requires(!getVariantCompletedEventArgs.Cancelled);

            if (getVariantCompletedEventArgs.Error != null)
            {
                throw getVariantCompletedEventArgs.Error;
            }

            var result = getVariantCompletedEventArgs.Result;
            Contract.Assert(result != null);

            var version = new Version(result.GeneratorVersion);
            CheckVersion(version);

            OnVariantDownloaded(result.Data, result.Number, result.Version);
            IsBusy = false;
        }

        private void CheckVersion(Version generatorVersion)
        {
            var success = _allowedVariantGenerationVersions.Any(v => v.Major == generatorVersion.Major);

            if (!success)
                throw new Exception("Полученный вариант сгенерирован неизвестной версией генератора. Продолжение работы невозможно.");
        }

        /// <summary> Вариант загружен </summary>
        public event EventHandler<VariantDownloadedEventArgs> VariantDownloaded;

        /// <summary> Вариант загружен. Событие может быть вызвано не из UI-потока </summary>
        private void OnVariantDownloaded(byte[] data, string number, long? version)
        {
            Interlocked.CompareExchange(ref VariantDownloaded, null, null)
                ?.Invoke(this, new VariantDownloadedEventArgs { Data = data, Number = number, Version = version });
        }

        #endregion
    }
}
