namespace GraphLabs.Common.TasksDataService
{
    public partial class TasksDataServiceClient
    {
        public TasksDataServiceClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}