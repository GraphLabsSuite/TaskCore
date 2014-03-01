using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.CommonUI.Controls.ViewModels
{
    /// <summary> Инструмент </summary>
    public class ToolBarToggleCommand : ToolBarCommandBase
    {
        /// <summary> Инструмент активен? </summary>
        public bool IsTurnedOn { get; private set; }

        /// <summary> Можно включить? </summary>
        public virtual bool CanBeTurnedOn()
        {
            return !IsTurnedOn && _canBeTurnedOn();
        }

        /// <summary> Можно выключить? </summary>
        public virtual bool CanBeTurnedOff()
        {
            return IsTurnedOn && _canBeTurnedOff();
        }

        /// <summary> Включить </summary>
        public virtual void TurnOn()
        {
            if (!CanBeTurnedOn())
            {
                throw new InvalidOperationException("Включить нельзя.");
            }

            _turnOnAction();
            IsTurnedOn = true;
            OnStatusChanged();
        }

        /// <summary> Выключить </summary>
        public virtual void TurnOff()
        {
            if (!CanBeTurnedOff())
            {
                throw new InvalidOperationException("Выключить нельзя.");
            }

            _turnOffAction();
            IsTurnedOn = false;
            OnStatusChanged();
        }

        private readonly Action _turnOnAction;
        private readonly Action _turnOffAction;
        private readonly Func<bool> _canBeTurnedOn;
        private readonly Func<bool> _canBeTurnedOff;

        /// <summary> Конструктор </summary>
        /// <param name="turnOnAction"> Действие на включение </param>
        /// <param name="turnOffAction"> Действие на выключение </param>
        /// <param name="canBeTurnedOn"> Функция, определяющая, можно ли включить </param>
        /// <param name="canBeTurnedOff"> Функция, определяющая, можно ли выключить </param>
        public ToolBarToggleCommand(Action turnOnAction,
            Action turnOffAction,
            Func<bool> canBeTurnedOn,
            Func<bool> canBeTurnedOff)
        {
            Contract.Requires<ArgumentNullException>(
                turnOnAction != null && turnOffAction != null && 
                canBeTurnedOn != null && canBeTurnedOff != null);

            _turnOnAction = turnOnAction;
            _turnOffAction = turnOffAction;
            _canBeTurnedOn = canBeTurnedOn;
            _canBeTurnedOff = canBeTurnedOff;
        }

    }
}
