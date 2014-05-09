namespace GraphLabs.CommonUI
{
    /// <summary> Постритель View </summary>
    public interface IViewBuilder
    {
        /// <summary> Построить View </summary>
        TaskViewBase BuildView(StartupParameters startupParameters, bool sendReportOnEveryAction);
    }
}