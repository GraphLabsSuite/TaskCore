using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.Utils;

namespace GraphLabs.CommonUI.Controls.ViewModels
{
    /// <summary> Представляет матрицу в виде строки </summary>
    public class MatrixPrinter
    {
        /// <summary> Представляет матрицу в виде строки </summary>
        public string MatrixToString(IEnumerable<MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"({string.Join(", ", row.Skip(1))})")
            );
            return $"({string.Join("; ", rows)})";
        }

        /// <summary> Представляет матрицу в виде нескольких строк </summary>
        public string MatrixToStrings(IEnumerable<MatrixRowViewModel<string>> matrix)
        {
            var rows = new List<string>();
            matrix.ForEach(row =>
                rows.Add($"{string.Join(" ", row.Skip(1))}")
            );
            return $"{string.Join(Environment.NewLine, rows)}";
        }
        
    }
}
