using System;
using System.Windows.Controls;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Простой диалог-сообщение </summary>
    public sealed partial class SimpleDialog : ChildWindow
    {
        /// <summary> Простой диалог-сообщение </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="text">Сопроводительный текст</param>
        public SimpleDialog(string title, string text)
        {
            InitializeComponent();

            Title = title;
            Info.Text = text;
        }
    }
}

