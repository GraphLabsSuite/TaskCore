namespace GraphLabs.Common.VariantProviderService
{
    public partial class VariantProviderServiceClient
    {
        /// <summary> Сеовис получения варианта </summary>
        public VariantProviderServiceClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}