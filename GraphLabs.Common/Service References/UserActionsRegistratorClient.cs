namespace GraphLabs.Common.UserActionsRegistrator
{
    public partial class UserActionsRegistratorClient
    {
        public UserActionsRegistratorClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}