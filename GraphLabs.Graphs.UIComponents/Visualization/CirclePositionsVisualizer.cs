using System;
using System.Linq;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary>
    /// по кургу
    /// </summary>
    public class CirclePositionsVisualizer : IVisualizationAlgorithm
    {
        private GraphVisualizer g_;

        /// <summary>
        /// constr
        /// </summary>
        public CirclePositionsVisualizer(GraphVisualizer g)
        {
            g_ = g;
        }

        /// <inheritdoc />
        public string Name()
        {
            return "Circle";
        }

        /// <inheritdoc />
        public void Visualize()
        {
            var vertices = g_.Vertices;
            if (!vertices.Any())
                return;

            var r = Math.Min(g_.ActualHeight, g_.ActualWidth) / 2;
            var phi = 0.0;
            var deltaPhi = 2 * Math.PI / vertices.Count;
            foreach (var vertex in vertices)
            {
                vertex.ModelX = (r - 2 * g_.DefaultVertexRadius) * Math.Cos(phi) + r;
                vertex.ModelY = (r - 2 * g_.DefaultVertexRadius) * Math.Sin(phi) + r;
                vertex.ScaleFactor = 1;

                phi += deltaPhi;
            }
        }
    }
}