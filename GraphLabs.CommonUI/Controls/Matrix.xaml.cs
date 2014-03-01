using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GraphLabs.CommonUI.Controls.ViewModels;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Матрица смежности </summary>
    public partial class Matrix : UserControl
    {
        /// <summary> Модель матрицы </summary>
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(
                "DataSource",
                typeof(ObservableCollection<MatrixRowViewModel<string>>),
                typeof(Matrix),
                new PropertyMetadata(default(ObservableCollection<MatrixRowViewModel<string>>), BindDataGrid));

        /// <summary> Модель матрицы </summary>
        public ObservableCollection<MatrixRowViewModel<string>> DataSource
        {
            get { return (ObservableCollection<MatrixRowViewModel<string>>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        /// <summary> DataGrid с матрицей </summary>
        public DataGrid MatrixDataGrid
        {
            get { return MatrixGrid; }
        }

        private static void BindDataGrid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Contract.Requires(d != null);
            Contract.Requires(e.OldValue == null);
            Contract.Requires(e.NewValue != null);

            var page = (Matrix)d;
            var dataSource = (ObservableCollection<MatrixRowViewModel<string>>)e.NewValue;
            
            page.MatrixGrid.Columns.Clear();
            page.OnMatrixChanged(dataSource);
        }

        /// <summary> Происходит при изменении отображаемой матрицы </summary>
        protected virtual void OnMatrixChanged(ObservableCollection<MatrixRowViewModel<string>> dataSource)
        {
            Contract.Requires(dataSource != null);

            var headerColumn = new DataGridTextColumn
            {
                Header = "",
                Binding = new Binding("[0]"),
                IsReadOnly = true,
                Width = new DataGridLength(25),
                CellStyle = (Style)Resources["GridRowHeaderStyle"]
            };

            if (headerColumn.CellStyle == null)
                ThrowOnStyleNotFound();

            MatrixGrid.RowHeight = 25;
            MatrixGrid.ColumnHeaderHeight = 25;
            MatrixGrid.Columns.Add(headerColumn);

            var cellStyle = (Style)Resources["CellStyle"];
            if (cellStyle == null)
                ThrowOnStyleNotFound();

            for (var i = 0; i < dataSource.Count; ++i)
            {
                var dataColumn = new DataGridTextColumn
                {
                    Header = dataSource[i][0],
                    Binding = new Binding(string.Format("[{0}]", i + 1)),
                    Width = new DataGridLength(MatrixGrid.RowHeight),
                    CellStyle = cellStyle,
                };
                MatrixGrid.Columns.Add(dataColumn);
            }

            MatrixGrid.ItemsSource = dataSource;
        }

        private void ThrowOnStyleNotFound()
        {
            throw new InvalidOperationException("Не удалось найти один или несколько стилей, используемых матрицей. Вероятно, это вызвано тем, что вы используете собственный ResourceDictionaty в классе-наследнике.");
        }


        #region Изменили значение в ячейке

        /// <summary> Завершилось редактирование значения </summary>
        private void OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            OnCellEdited();
        }

        /// <summary> Отредактировано значение в ячейке (просто оповещение) </summary>
        public event EventHandler CellEdited;

        /// <summary> Отредактировано значение в ячейке (просто оповещение) </summary>
        private void OnCellEdited()
        {
            var handler = CellEdited;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion


        /// <summary> Ctor. </summary>
        public Matrix()
        {
            InitializeComponent();

            MatrixGrid.CellEditEnded += OnCellEditEnded;
        }
    }
}
