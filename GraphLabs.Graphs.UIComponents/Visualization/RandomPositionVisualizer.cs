using System;
using System.Linq;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary>
    /// случайные позиции
    /// </summary>
    public class RandomPositionVisualizer : IVisualizationAlgorithm
    {
        /// <inheritdoc />
        public GraphVisualizer Visualizer { get; set; }

        /// <inheritdoc />
        public string Name()
        {
            return "RandomPositions";
        }

        /// <inheritdoc />
        public void Visualize()
        {
            var vertices = Visualizer.Vertices;
            if (!vertices.Any())
                return;

            var rnd = new Random();

            for (var i = 0; i < vertices.Count; ++i)
            {
                var vertex = vertices[i];
                vertex.ModelX = rnd.NextDouble() * (Visualizer.ActualWidth - 2 * Visualizer.DefaultVertexRadius) + Visualizer.DefaultVertexRadius;
                vertex.ModelY = rnd.NextDouble() * (Visualizer.ActualHeight - 2 * Visualizer.DefaultVertexRadius) + Visualizer.DefaultVertexRadius;
                vertex.ScaleFactor = 1;

                var j = 0;
                for (; j < i; j++)
                {
                    if (GraphVisualizer.AreIntersecting(vertex, (Vertex)vertices[j]))
                        break;
                }
                if (j < i)
                    --i;
            }
        }
    }
}