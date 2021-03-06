﻿using System;
using System.ComponentModel;
using GraphLabs.Common;
using JetBrains.Annotations;

namespace GraphLabs.Common.UserActionsRegistrator
{
    /// <summary> Сервис регистрации действий клиента </summary>
    public interface IUserActionsRegistratorClient : IWcfClient
    {
        /// <summary> Регистрация действий завершена </summary>
        event EventHandler<RegisterUserActionsCompletedEventArgs> RegisterUserActionsCompleted;

        /// <summary> Зарегистрировать действия </summary>
        void RegisterUserActionsAsync(long taskId, Guid sessionGuid, ActionDescription[] actions, bool isTaskFinished);
    }

    /// <summary> Сервис регистрации действий клиента </summary>
    [CoverageExclude]
    public partial class UserActionsRegistratorClient : IUserActionsRegistratorClient
    {
    }

    /// <summary> Описание действия </summary>
    [CoverageExclude]
    public partial class ActionDescription
    {
    }

    /// <summary> EventArgs для завершения регистрации действий </summary>
    [CoverageExclude]
    public partial class RegisterUserActionsCompletedEventArgs
    {
    }
}
