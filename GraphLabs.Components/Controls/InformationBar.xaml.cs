using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GraphLabs.Components.Controls.ViewModels;

namespace GraphLabs.Components.Controls
{
    /// <summary> Панель информации </summary>
    public partial class InformationBar : UserControl
    {
        #region Константы

        private const double DEFAULT_SCORE_COL_WIDTH = 70.0d;
        private const int STARTING_SCORE = 100;

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
            DependencyProperty.Register("Log", typeof(ObservableCollection<LogEventViewModel>), typeof(InformationBar), new PropertyMetadata(default(ObservableCollection<LogEventViewModel>)));

        /// <summary> Сообщения </summary>
        public ObservableCollection<LogEventViewModel> Log
        {
            get { return (ObservableCollection<LogEventViewModel>)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        /// <summary> Текущий балл </summary>
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(InformationBar), new PropertyMetadata(STARTING_SCORE));

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
        }
    }
}
