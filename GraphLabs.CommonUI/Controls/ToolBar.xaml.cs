using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using GraphLabs.CommonUI.Controls.ViewModels;
using GraphLabs.Utils;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Панель инструментов </summary>
    public partial class ToolBar : UserControl
    {
        /// <summary> Отступ между кнопками </summary>
        private const double BUTTON_MARGIN = 5;

        /// <summary> Ориентация панели инструментов </summary>
        public Orientation Orientation
        {
            get { return ButtonsPanel.Orientation; }
            set
            {
                if (value == ButtonsPanel.Orientation)
                {
                    return;
                }

                ButtonsPanel.Orientation = value;
                SetOrientation();
            }
        }

        /// <summary> Ctor. </summary>
        public ToolBar()
        {
            InitializeComponent();

            _commandsAndButtons = new Dictionary<ToolBarCommandBase, ButtonBase>();
            ButtonsPanel.SizeChanged += AdjustButtonsSize;
            SetOrientation();
        }

        /// <summary> Команды </summary>
        public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register(
            "Commands", 
            typeof(ObservableCollection<ToolBarCommandBase>), 
            typeof(ToolBar), 
            new PropertyMetadata(default(ObservableCollection<ToolBarCommandBase>), CommandsCollectionReplaced));


        /// <summary> Команды </summary>
        public ObservableCollection<ToolBarCommandBase> Commands
        {
            get { return (ObservableCollection<ToolBarCommandBase>)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        private Dictionary<ToolBarCommandBase, ButtonBase> _commandsAndButtons;


        #region Вспомагательные методы

        /// <summary> Добавляет кнопку команды на тулбар </summary>
        /// <param name="command"> Команда </param>
        /// <returns> Кнопка, добавленная на панель. 
        /// Либо <see cref="ButtonBase"/>, либо <see cref="ToggleButton"/>. </returns>
        private ButtonBase AddCommandButton(ToolBarCommandBase command)
        {
            Contract.Assert(command != null);
            Contract.Assert(command.ToolBar == null);

            ButtonBase button;
            if (command is ToolBarInstantCommand)
            {
                button = new Button();
                var instantCommand = (ToolBarInstantCommand)command;
                command.StatusChanged += (sender, args) => button.IsEnabled = instantCommand.CanExecute();
                button.Click += (sender, args) => instantCommand.Execute();
            }
            else if (command is ToolBarToggleCommand)
            {
                button = new ToggleButton();
                var toggleButton = (ToggleButton)button;
                var toggleCommand = (ToolBarToggleCommand)command;

                // ReSharper disable ImplicitlyCapturedClosure
                command.StatusChanged += (sender, args) =>
                {
                    toggleButton.IsEnabled = !toggleCommand.IsTurnedOn
                        ? toggleCommand.CanBeTurnedOn()
                        : toggleCommand.CanBeTurnedOff();
                };
                toggleButton.Click += (sender, args) =>
                {
                    if (!toggleCommand.IsTurnedOn)
                    {
                        toggleCommand.TurnOn();
                    }
                    else
                    {
                        toggleCommand.TurnOff();
                    }
                    Commands.ForEach(c => c.RefreshState());
                };
                command.RefreshState();
                // ReSharper restore ImplicitlyCapturedClosure
            }
            else
            {
                throw new NotSupportedException(
                    string.Format("Поддержка команд типа {0} ещё не реализована.", command.GetType()));
            }

            button.SetValue(ToolTipService.ToolTipProperty, command.Description);
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Content = new Image
            {
                Source = command.Image,
                Stretch = Stretch.None,
            };
            command.ToolBar = this;
            ButtonsPanel.Children.Add(button);

            return button;
        }

        /// <summary> Переприсвоена коллекция комманд </summary>
        private static void CommandsCollectionReplaced(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Contract.Assert(d != null);

            var toolBar = (ToolBar)d;
            toolBar.ButtonsPanel.Children.Clear();
            toolBar._commandsAndButtons.Clear();
            var newCollection = (ObservableCollection<ToolBarCommandBase>)args.NewValue;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += toolBar.CommandsCollectionChanged;
                newCollection.ForEach(c => toolBar._commandsAndButtons.Add(c, toolBar.AddCommandButton(c)));
            }
        }

        /// <summary> Изменился набор комманд в коллекции </summary>
        private void CommandsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Contract.Assert(sender == Commands);

            if (args.OldItems != null && args.Action != NotifyCollectionChangedAction.Add)
            {
                args.OldItems.Cast<ToolBarCommandBase>().ForEach(c =>
                    {
                        ButtonsPanel.Children.Remove(_commandsAndButtons[c]);
                        _commandsAndButtons.Remove(c);
                    });
            }

            if (args.NewItems != null && args.Action != NotifyCollectionChangedAction.Remove)
            {
                args.NewItems.Cast<ToolBarCommandBase>().ForEach(c => _commandsAndButtons.Add(c, AddCommandButton(c)));
            }
        }

        /// <summary> Изменение расположения панели инструментов </summary>
        private void SetOrientation()
        {
            if (Orientation == Orientation.Vertical)
            {
                ButtonsPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                ButtonsPanel.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                ButtonsPanel.HorizontalAlignment = HorizontalAlignment.Center;
                ButtonsPanel.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }

        /// <summary> Подгон размера кнопки под размер тулбара </summary>
        private void AdjustButtonsSize(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (Orientation == Orientation.Vertical)
            {
                ButtonsPanel
                    .Children
                    .Cast<ButtonBase>()
                    .ForEach(b =>
                        {
                            b.Width = b.Height = ButtonsPanel.ActualWidth - BUTTON_MARGIN * 2;
                            b.Margin = new Thickness(BUTTON_MARGIN, BUTTON_MARGIN / 2, BUTTON_MARGIN, BUTTON_MARGIN / 2);
                        });
            }
            else
            {
                ButtonsPanel
                    .Children
                    .Cast<ButtonBase>()
                    .ForEach(b =>
                        {
                            b.Width = b.Height = ButtonsPanel.ActualHeight - BUTTON_MARGIN * 2;
                            b.Margin = new Thickness(BUTTON_MARGIN / 2, BUTTON_MARGIN, BUTTON_MARGIN / 2, BUTTON_MARGIN);
                        });
            }
        }

        #endregion
    }
}
