using GraphLabs.Utils;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GraphLabs.CommonUI.Controls.ViewModels.Matrix
{
    /// <summary> Представляет матрицу в виде строки </summary>
    public class MatrixPrinter
    {
        /// <summary> Представляет матрицу в виде строки </summary>
        public string MatrixToString(IEnumerable<ViewModels.MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"({string.Join(", ", row.Skip(1))})")
            );
            return $"({string.Join("; ", rows)})";
        }

        /// <summary> Представляет матрицу в виде нескольких строк </summary>
        public string MatrixToStrings(IEnumerable<ViewModels.MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"{string.Join(" ", row.Skip(1))}")
            );
            return $"{string.Join(Environment.NewLine, rows)}";
        }
        
    }
}
