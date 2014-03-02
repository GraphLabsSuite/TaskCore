using System;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> EventArgs - Получение варианта завершено </summary>
    [CoverageExclude]
    public class VariantDownloadedEventArgs : EventArgs
    {
        /// <summary> Данные варианта </summary>
        public byte[] Data { get; set; }

        /// <summary> Номер варианта </summary>
        public string Number { get; set; }

        /// <summary> Версия варианта </summary>
        public long? Version { get; set; }
    }
}