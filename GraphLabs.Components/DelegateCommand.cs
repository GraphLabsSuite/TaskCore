using System;
using System.Windows.Input;

namespace GraphLabs.Tasks.Components
{
    /// <summary> Простая команда, которая инвочит делегат </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary> Occurs when changes occur that affect whether the command should execute. </summary>
        public event EventHandler CanExecuteChanged;

        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _executeAction;
        private bool _canExecuteCache;

        /// <summary> Initializes a new instance of the <see cref="DelegateCommand"/> class. </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateCommand(Action<object> executeAction,
                               Func<object, bool> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        #region ICommand Members
        /// <summary> Defines the method that determines whether the command 
        /// can execute in its current state. </summary>
        /// <param name="parameter">
        /// Data used by the command. 
        /// If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns> true if this command can be executed; otherwise, false. </returns>
        public bool CanExecute(object parameter)
        {
            bool tempCanExecute = _canExecute(parameter);

            if (_canExecuteCache != tempCanExecute)
            {
                _canExecuteCache = tempCanExecute;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return _canExecuteCache;
        }

        /// <summary> Defines the method to be called when the command is invoked. </summary>
        /// <param name="parameter">
        /// Data used by the command. 
        /// If the command does not require data to be passed, 
        /// this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
        #endregion
    }
}
