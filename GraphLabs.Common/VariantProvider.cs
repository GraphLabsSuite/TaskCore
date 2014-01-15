using System;

namespace GraphLabs.Common
{
    /// <summary> Поставщик вариантов </summary>
    /// <typeparam name="T">Класс варианта задания</typeparam>
    public class VariantProvider<T> where T: class
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; private set; }

        /// <summary> Id сессии </summary>
        public Guid SessionGuid { get; private set; }

        /// <summary> Поставщик вариантов </summary>
        internal VariantProvider(long taskId, Guid sessionGuid)
        {
            TaskId = taskId;
            SessionGuid = sessionGuid;
        }

        /// <summary> Загрузить вариант и зафиксировать начало выполнения </summary>
        public T LoadAndRegisterVariant()
        {
            throw new NotImplementedException();
        }
    }
}
