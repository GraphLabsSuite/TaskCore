namespace GraphLabs.Common.TasksDataService
{
    public partial class TasksDataServiceClient
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="remoteAddress">Задание</param>
        public TasksDataServiceClient(string remoteAddress) :
               base(GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
        }
    }
}