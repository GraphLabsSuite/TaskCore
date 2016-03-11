namespace GraphLabs.Common.UserActionsRegistrator
{
    public partial class UserActionsRegistratorClient
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="remoteAddress">Задание</param>
        public UserActionsRegistratorClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}