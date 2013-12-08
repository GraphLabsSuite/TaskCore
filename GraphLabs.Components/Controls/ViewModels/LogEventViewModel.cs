using System;

namespace GraphLabs.Components.Controls.ViewModels
{
    /// <summary> Сообщение лога </summary>
    public class LogEventViewModel
    {
        /// <summary> Сообщение </summary>
        public string Message { get; set; }

        /// <summary> Временная отметка </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary> Штрафные баллы </summary>
        public int Penalty { get; set; }
    }
}