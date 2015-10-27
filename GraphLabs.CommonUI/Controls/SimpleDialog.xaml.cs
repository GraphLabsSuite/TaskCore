using System;
using System.Windows.Controls;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Простой диалог-сообщение </summary>
    public sealed partial class SimpleDialog : ChildWindow
    {
        /// <summary>
        /// Простой диалог-сообщение
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="text">Сопроводительный текст</param>
        public SimpleDialog(String name, String text)
        {
            InitializeComponent();

            Title = name;
            Info.Text = text;
        }
    }
}

