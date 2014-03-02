using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using GraphLabs.Utils;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Поставщик вариантов задания </summary>
    public class VariantProvider : INotifyPropertyChanged, IUiBlockerAsyncProcessor
    {
        /// <summary> Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a property value changes. </summary>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
                OnPropertyChanged(ExpressionUtility.NameForMember(() => IsBusy));
            }
        }
        private bool _isBusy = false;


        /// <summary> Сервис получения вариантов </summary>
        protected ITasksDataServiceClient TasksDataServiceClient { get; private set; }

        /// <summary> Поставщик вариантов задания </summary>
        public VariantProvider(DisposableWcfClientWrapper<ITasksDataServiceClient> client)
        {
            Contract.Requires(client != null);

            TasksDataServiceClient = client.Instance;
        }
        
    }
}
