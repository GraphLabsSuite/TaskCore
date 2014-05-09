using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> ������������ �������� WCF-�������� </summary>
    public sealed class WcfServicesConfigurator : WcfServicesConfiguratorBase
    {
        /// <summary> �������� ������ ������������ �������� </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            return new UserActionsRegistratorClient();
        }

        /// <summary> �������� ������ ���������� ��������� </summary>
        protected override ITasksDataServiceClient GetDataServiceClient()
        {
            return new TasksDataServiceClient();
        }
    }
}