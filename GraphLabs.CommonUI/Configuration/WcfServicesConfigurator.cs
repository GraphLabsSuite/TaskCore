using System.ServiceModel;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Common.VariantProviderService;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> ������������ �������� WCF-�������� </summary>
    public sealed class WcfServicesConfigurator : WcfServicesConfiguratorBase
    {
        /// <summary> �������� ������ ������������ �������� </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            return string.IsNullOrEmpty(UserActionsRegistratorAddress) ?
                new UserActionsRegistratorClient() :
                new UserActionsRegistratorClient(UserActionsRegistratorAddress);
        }

        /// <summary> �������� ������ ���������� ��������� </summary>
        protected override IVariantProviderServiceClient GetDataServiceClient()
        {
            return string.IsNullOrEmpty(VariantProviderServiceClientAddress) ?
                new VariantProviderServiceClient() : 
                new VariantProviderServiceClient(VariantProviderServiceClientAddress);
        }

        /// <summary> ����� ��� ������� ������������ �������� </summary>
        public string UserActionsRegistratorAddress { get; set; }

        /// <summary> ����� ��� ������� ���������� ��������� </summary>
        public string VariantProviderServiceClientAddress { get; set; }
    }
}