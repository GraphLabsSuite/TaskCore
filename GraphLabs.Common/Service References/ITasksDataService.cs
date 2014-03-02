using System;
using JetBrains.Annotations;

namespace GraphLabs.Common.TasksDataService
{
    /// <summary> Сервис регистрации действий клиента </summary>
    public interface ITasksDataServiceClient
    {
        /// <summary> Получить вариант </summary>
        void GetVariantAsync(long taskId, Guid sessionGuid);

        /// <summary> Подключение закрыто </summary>
        event EventHandler<GetVariantCompletedEventArgs> GetVariantCompleted;

        /// <summary> Закрыть подключение </summary>
        void CloseAsync();
    }

    /// <summary> Сервис регистрации действий клиента </summary>
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
