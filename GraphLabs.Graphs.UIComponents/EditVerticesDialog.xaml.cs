using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GraphLabs.Utils;

namespace GraphLabs.Graphs.UIComponents
{
    /// <summary> Окно диалога редактирования вершин </summary>
    public sealed partial class EditVerticesDialog : ChildWindow
    {
        private readonly IGraph _graph;

        /// <summary>
        /// Окно диалога редактирования вершин
        /// </summary>
        /// <param name="currentGraph">Редактируемый граф</param>
        /// <param name="verticesFullCollection">Полная коллекция вершин, возможных в графе</param>
        /// <param name="description">Текст, сопровождающий диалог</param>
        public EditVerticesDialog(IGraph currentGraph, IEnumerable<IVertex> verticesFullCollection, String description = "Выберите вершины, входящие в граф")
        {
            InitializeComponent();

            Info.Text = description;

            _graph = currentGraph;
            verticesFullCollection.ForEach(v =>
            {
                var cb = new CheckBox
                {
                    Content = "Вершина [" + v.Name + "]",
                    IsChecked = currentGraph.Vertices.SingleOrDefault(s => s.Equals(v)) != null
                };
                VerticesList.Children.Add(cb);
            });
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            VerticesList.Children.ForEach(ch =>
            {
                var cb = ch as CheckBox;
                var name = cb.Content.ToString().Substring(9);
                name = name.Substring(0, name.Length - 1);
                if (_graph.Vertices.SingleOrDefault(v => v.Name == name) == null && cb.IsChecked == true)
                    _graph.AddVertex(new Vertex(name));
                if (_graph.Vertices.SingleOrDefault(v => v.Name == name) != null && cb.IsChecked == false)
                    _graph.RemoveVertex(_graph.Vertices.Single(v => v.Name == name));
            });
            DialogResult = true;
        }
    }
}