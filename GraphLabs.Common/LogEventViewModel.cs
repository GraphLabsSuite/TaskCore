using System;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Сообщение лога </summary>
    [CoverageExclude]
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