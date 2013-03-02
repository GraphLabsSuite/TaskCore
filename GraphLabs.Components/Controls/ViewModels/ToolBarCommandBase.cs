using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GraphLabs.Tasks.Components.Controls.ViewModels
{
    /// <summary> Команда тулбара </summary>
    public abstract class ToolBarCommandBase : DependencyObject
    {
        /// <summary> Картинка на кнопке </summary>
        public DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(BitmapImage), typeof(ToolBarCommandBase), new PropertyMetadata(null));

        /// <summary> Картинка на кнопке </summary>
        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        
        /// <summary> Описание </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(ToolBarCommandBase), new PropertyMetadata(string.Empty));

        /// <summary> Описание </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>  ToolBar, на котором лежит команда </summary>
        public ToolBar ToolBar { get; protected internal set; }

        /// <summary> Принудительно выстреливает событие StatusChanged </summary>
        public void RefreshState()
        {
            OnStatusChanged();
        }

        /// <summary> Выстреливает, когда происходят изменения, потенциально способные изменить доступность команды </summary>
        public event EventHandler StatusChanged;

        /// <summary> Реакция на возможное изменение доступности/недоступности команды </summary>
        protected virtual void OnStatusChanged()
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, EventArgs.Empty);
            }
        }

    }
}
