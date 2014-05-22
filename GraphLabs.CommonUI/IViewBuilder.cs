using GraphLabs.Tasks.Contract;

namespace GraphLabs.CommonUI
{
    /// <summary> Постритель View </summary>
    public interface IViewBuilder
    {
        /// <summary> Построить View </summary>
        TaskViewBase BuildView(InitParams startupParameters, bool sendReportOnEveryAction);
    }
}