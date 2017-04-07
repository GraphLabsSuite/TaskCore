using System;
using System.Linq;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary>
    /// по кургу
    /// </summary>
    public class CirclePositionsVisualizer : IVisualizationAlgorithm
    {
        /// <summary> Визуализатор </summary>
        public GraphVisualizer Visualizer { get; set; }

        /// <summary>
        /// constr
        /// </summary>
        public CirclePositionsVisualizer()
        {
        }

        /// <inheritdoc />
        public string Name()
        {
            return "Circle";
        }

        /// <inheritdoc />
        public void Visualize()
        {
            var vertices = Visualizer.Vertices;
            if (!vertices.Any())
                return;

            var r = Math.Min(Visualizer.ActualHeight, Visualizer.ActualWidth) / 2;
            var phi = 0.0;
            var deltaPhi = 2 * Math.PI / vertices.Count;
            foreach (var vertex in vertices)
            {
                vertex.ModelX = (r - 2 * Visualizer.DefaultVertexRadius) * Math.Cos(phi) + r;
                vertex.ModelY = (r - 2 * Visualizer.DefaultVertexRadius) * Math.Sin(phi) + r;
                vertex.ScaleFactor = 1;

                phi += deltaPhi;
            }
        }
    }
}