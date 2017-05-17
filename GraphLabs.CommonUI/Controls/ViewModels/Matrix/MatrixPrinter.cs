using GraphLabs.Utils;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace GraphLabs.CommonUI.Controls.ViewModels.Matrix
{
    /// <summary> Представляет матрицу в виде строки </summary>
    public class MatrixPrinter
    {
        /// <summary> Представляет матрицу в виде строки </summary>
        public string MatrixToString(ObservableCollection<ViewModels.MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"({string.Join(", ", row).Remove(0,1)})")
            );
            return $"({string.Join("; ", rows)})";
        }

        /// <summary> Представляет матрицу в виде нескольких строк </summary>
        public string MatrixToStrings(ObservableCollection<ViewModels.MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"{string.Join(" ", row).Remove(0,1)}")
            );
            return $"{string.Join("\n", rows)}";
        }
        
    }
}
