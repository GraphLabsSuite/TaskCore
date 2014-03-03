using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace GraphLabs.CommonUI
{
    /// <summary> Вспомогательные методы для получения параметров задания </summary>
    public static class StartupHelper
    {
        /// <summary> Имя параметра "ID задания" </summary>
        public const string TaskIdParam = "task_id";

        /// <summary> Имя параметра "GUID сессии" </summary>
        public const string SessionGuidParam = "session_guid";

        /// <summary> Получить ID задания </summary>
        public static long GetTaskId(this StartupEventArgs args)
        {
            Contract.Requires(args != null);

            long result;
            if (args.InitParams.ContainsKey(TaskIdParam) &&
                long.TryParse(args.InitParams[TaskIdParam], out result))
            {
                return result;
            }

            throw new Exception("Не удалось получить параметр модуля-задания.");
        }

        /// <summary> Получить GUID сессии </summary>
        public static Guid GetSessionGuid(this StartupEventArgs args)
        {
            Contract.Requires(args != null);

            Guid result;
            if (args.InitParams.ContainsKey(SessionGuidParam) &&
                Guid.TryParse(args.InitParams[SessionGuidParam], out result))
            {
                return result;
            }

            throw new Exception("Не удалось получить параметр модуля-задания.");
        }
    }
}
