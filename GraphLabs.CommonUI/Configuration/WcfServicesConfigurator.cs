using System.ServiceModel;
using GraphLabs.Common.TasksDataService;
using GraphLabs.Common.UserActionsRegistrator;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Конфигуратор реальных WCF-сервисов </summary>
    public sealed class WcfServicesConfigurator : WcfServicesConfiguratorBase
    {
        /// <summary> Получить клиент регистратора действий </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            return string.IsNullOrEmpty(UserActionsRegistratorAddress) ?
                new UserActionsRegistratorClient() :
                new UserActionsRegistratorClient(UserActionsRegistratorAddress);
        }

        /// <summary> Получить клиент поставщика вариантов </summary>
        protected override ITasksDataServiceClient GetDataServiceClient()
        {
            return string.IsNullOrEmpty(DataServiceClientAddress) ?
                new TasksDataServiceClient() :
                new TasksDataServiceClient(DataServiceClientAddress);
        }

        /// <summary> Адрес для клиента регистратора действий </summary>
        public string UserActionsRegistratorAddress { get; set; }

        /// <summary> Адрес для клиента поставщика вариантов </summary>
        public string DataServiceClientAddress { get; set; }
    }
}