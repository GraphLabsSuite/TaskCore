namespace GraphLabs.CommonUI
{
    /// <summary> Постритель View </summary>
    public class ViewBuilder<TView, TViewModel> : IViewBuilder
        where TView: TaskViewBase, new()
        where TViewModel: TaskViewModelBase<TView>, new()
    {
        /// <summary> Постритель View </summary>
        public TaskViewBase BuildView(StartupParameters startupParameters, bool sendReportOnEveryAction)
        {
            var view = new TView();
            var viewModel = new TViewModel();
            viewModel.Initialize(view, startupParameters, sendReportOnEveryAction);

            return view;
        }
    }
}