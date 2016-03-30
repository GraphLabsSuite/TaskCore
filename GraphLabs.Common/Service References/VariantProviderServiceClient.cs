namespace GraphLabs.Common.VariantProviderService
{
    /// <summary> Клиент сервиса предоставления варианта </summary>
    public partial class VariantProviderServiceClient
    {
        /// <summary> Сеовис получения варианта </summary>
        public VariantProviderServiceClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}