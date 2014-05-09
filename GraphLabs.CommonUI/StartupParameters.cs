using System;

namespace GraphLabs.CommonUI
{
    /// <summary> Параметры запуска задания </summary>
    public struct StartupParameters
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; private set; }

        /// <summary> Guid сессии </summary>
        public Guid SessionGuid { get; private set; }

        /// <summary> Параметры запуска задания </summary>
        public StartupParameters(long taskId, Guid sessionGuid)
            : this()
        {
            TaskId = taskId;
            SessionGuid = sessionGuid;
        }
    }
}