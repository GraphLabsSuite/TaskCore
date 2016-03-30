using System;
using JetBrains.Annotations;

namespace GraphLabs.Common.VariantProviderService
{
    /// <summary> Сервис получения вариантов </summary>
    public interface IVariantProviderServiceClient : IWcfClient
    {
        /// <summary> Получить вариант </summary>
        void GetVariantAsync(long taskId, Guid sessionGuid);

        /// <summary> Подключение закрыто </summary>
        event EventHandler<GetVariantCompletedEventArgs> GetVariantCompleted;
    }

    /// <summary> Сервис получения вариантов </summary>=
    [CoverageExclude]
    public partial class VariantProviderServiceClient : IVariantProviderServiceClient
    {
    }

    /// <summary> EventArgs - вариант получен </summary>
    [CoverageExclude]
    public partial class GetVariantCompletedEventArgs
    {
    }

    /// <summary> Информация о варианте </summary>
    [CoverageExclude]
    public partial class TaskVariantDto
    {
    }
}