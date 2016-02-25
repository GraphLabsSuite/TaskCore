using System.ServiceModel;
using GraphLabs.Common.TasksDataService;
using GraphLabs.Common.UserActionsRegistrator;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> ������������ �������� WCF-�������� </summary>
    public sealed class WcfServicesConfigurator : WcfServicesConfiguratorBase
    {
        /// <summary> �������� ������ ������������ �������� </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            return UserActionsRegistratorAddress == null ?
                new UserActionsRegistratorClient() :
                new UserActionsRegistratorClient(UserActionsRegistratorAddress);
        }

        /// <summary> �������� ������ ���������� ��������� </summary>
        protected override ITasksDataServiceClient GetDataServiceClient()
        {
            return DataServiceClientAddress == null ?
                new TasksDataServiceClient() :
                new TasksDataServiceClient(DataServiceClientAddress);
        }

        /// <summary> ����� ��� ������� ������������ �������� </summary>
        public string UserActionsRegistratorAddress { get; set; }

        /// <summary> ����� ��� ������� ���������� ��������� </summary>
        public string DataServiceClientAddress { get; set; }
    }
}