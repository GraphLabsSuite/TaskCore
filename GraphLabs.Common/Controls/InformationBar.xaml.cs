using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GraphLabs.Common.Controls.ViewModels;

namespace GraphLabs.Common.Controls
{
    /// <summary> Панель информации </summary>
    public partial class InformationBar : UserControl
    {
        #region Константы

        private const double DEFAULT_SCORE_COL_WIDTH = 70.0d;

        #endregion


        /// <summary> Ширина колонки с оценкой </summary>
        public static readonly DependencyProperty ScoreColumnWidthProperty =
            DependencyProperty.Register("ScoreColumnWidth", typeof(double), typeof(InformationBar), new PropertyMetadata(DEFAULT_SCORE_COL_WIDTH));

        /// <summary> Ширина колонки с оценкой </summary>
        public double ScoreColumnWidth
        {
            get { return (double)GetValue(ScoreColumnWidthProperty); }
            set { SetValue(ScoreColumnWidthProperty, value); }
        }

        /// <summary> Сообщения </summary>
        public static readonly DependencyProperty LogProperty =
            DependencyProperty.Register("Log", typeof(ReadOnlyObservableCollection<LogEventViewModel>), typeof(InformationBar), new PropertyMetadata(default(ReadOnlyObservableCollection<LogEventViewModel>)));

        /// <summary> Сообщения </summary>
        public ReadOnlyObservableCollection<LogEventViewModel> Log
        {
            get { return (ReadOnlyObservableCollection<LogEventViewModel>)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        /// <summary> Текущий балл </summary>
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(InformationBar), new PropertyMetadata(UserActionsManager.STARTING_SCORE));

        /// <summary> Текущий балл </summary>
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        /// <summary> Ctor. </summary>
        public InformationBar()
        {
            InitializeComponent();

            SetBindings();
        }

        private void SetBindings()
        {
            SetBinding(ScoreProperty, new Binding("Score"));
            SetBinding(LogProperty, new Binding("Log"));
        }
    }
}
