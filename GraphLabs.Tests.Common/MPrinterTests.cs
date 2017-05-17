using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.CommonUI.Controls.ViewModels.Matrix;
using NUnit.Framework;

namespace GraphLabs.Tests.Common
{
    class MPrinterTests
    {
        /// <summary> ... </summary>
        [Test]
        public void MPrinterTest()
        {
            var matrix = new ObservableCollection<MatrixRowViewModel<string>>();
            var line = new MatrixRowViewModel<string>();
            line.ReSize(5);

        }
    }
}