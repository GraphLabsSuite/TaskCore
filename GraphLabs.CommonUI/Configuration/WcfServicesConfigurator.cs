using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Конфигуратор реальных WCF-сервисов </summary>
    public sealed class WcfServicesConfigurator : WcfServicesConfiguratorBase
    {
        /// <summary> Получить клиент регистратора действий </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            return new UserActionsRegistratorClient();
        }

        /// <summary> Получить клиент поставщика вариантов </summary>
        protected override ITasksDataServiceClient GetDataServiceClient()
        {
            return new TasksDataServiceClient();
        }
    }
}