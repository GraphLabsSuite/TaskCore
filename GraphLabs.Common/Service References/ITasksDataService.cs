using System;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Сервис получения вариантов </summary>
    public interface ITasksDataServiceClient : IWcfClient
    {
        /// <summary> Получить вариант </summary>
        void GetVariantAsync(long taskId, Guid sessionGuid);

        /// <summary> Подключение закрыто </summary>
        event EventHandler<GetVariantCompletedEventArgs> GetVariantCompleted;
    }

    /// <summary> Сервис получения вариантов </summary>=
    [CoverageExclude]
    public partial class TasksDataServiceClient : ITasksDataServiceClient
    {
    }

    /// <summary> EventArgs - вариант получен </summary>
    [CoverageExclude]
    public partial class GetVariantCompletedEventArgs
    {
    }
}