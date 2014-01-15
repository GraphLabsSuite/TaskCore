using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Common.Controls.ViewModels
{
    /// <summary> Мгновенная команда </summary>
    public class ToolBarInstantCommand : ToolBarCommandBase
    {

        /// <summary> Активна ли команда? </summary>
        public virtual bool CanExecute()
        {
            return _canExecute();
        }

        /// <summary> Выполнить команду. </summary>
        public virtual void Execute()
        {
            if (_canExecute())
            {
                _action();
            }
            else
            {
                throw new InvalidOperationException("Can't execute action (canExecute == false)");
            }

        }

        /// <summary> Конструктор </summary>
        public ToolBarInstantCommand(Action action, Func<bool> canExecute)
        {
            Contract.Requires<ArgumentNullException>(action != null);
            Contract.Requires<ArgumentNullException>(canExecute != null);

            _action = action;
            _canExecute = canExecute;
        }

        private readonly Action _action;
        private readonly Func<bool> _canExecute; 
    }
}
