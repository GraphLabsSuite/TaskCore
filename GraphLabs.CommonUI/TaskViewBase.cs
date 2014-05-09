using System.Windows.Controls;

namespace GraphLabs.CommonUI
{
    /// <summary> Базовая View задания </summary>
    /// <typeparam name="TViewModel"> Вьюмодель задания </typeparam>
    public abstract class TaskViewBase<TViewModel> : UserControl
        where TViewModel: TaskViewModelBase
    {
        /// <summary> Вьюмодель </summary>
        public TViewModel ViewModel
        {
            get { return DataContext as TViewModel; }
            set { DataContext = value; }
        }
    }
}