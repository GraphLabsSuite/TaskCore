using System;
using System.ComponentModel;
using GraphLabs.Utils;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Сообщение лога </summary>
    public class LogEventViewModel : INotifyPropertyChanged
    {
        private string _message;
        private DateTime _timeStamp;
        private int _penalty;

        /// <summary> Сообщение </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => Message));
            }
        }

        /// <summary> Временная отметка </summary>
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => TimeStamp));
            }
        }

        /// <summary> Штрафные баллы </summary>
        public int Penalty
        {
            get { return _penalty; }
            set
            {
                _penalty = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => Penalty));
            }
        }

        /// <summary> Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a property value changes. </summary>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}