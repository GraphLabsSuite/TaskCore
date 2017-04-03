using System;
using System.Linq;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary>
    /// случайные позиции
    /// </summary>
    public class RandomPositionVisualizer : IVisualizationAlgorithm
    {
        private GraphVisualizer _g;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public RandomPositionVisualizer(GraphVisualizer g)
        {
            _g = g;
        }

        /// <inheritdoc />
        public string Name()
        {
            return "RandomPositions";
        }

        /// <inheritdoc />
        public void Visualize()
        {
            var vertices = _g.Vertices;
            if (!vertices.Any())
                return;

            var rnd = new Random();

            for (var i = 0; i < vertices.Count; ++i)
            {
                var vertex = vertices[i];
                vertex.ModelX = rnd.NextDouble() * (_g.ActualWidth - 2 * _g.DefaultVertexRadius) + _g.DefaultVertexRadius;
                vertex.ModelY = rnd.NextDouble() * (_g.ActualHeight - 2 * _g.DefaultVertexRadius) + _g.DefaultVertexRadius;
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