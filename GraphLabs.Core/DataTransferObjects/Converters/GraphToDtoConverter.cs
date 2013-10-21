using System;
using System.Linq;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Core.DataTransferObjects.Converters
{
    /// <summary> Конвертер из графа в соответствующую ДТО и обратно </summary>
    internal static class GraphToDtoConverter
    {
        /// <summary> Из графа в ДТО </summary>
        public static GraphDto Convert(IGraphBase value)
        {
            return new GraphDto
                {
                    Vertices = value.Vertices.Select(VertexToDtoConverter.Convert).ToArray(),
                    Edges = value.Edges.Select(EdgeToDtoConverter.Convert).ToArray(),
                    Directed = value.Directed,
                    IsWeighted = (value is DirectedWeightedGraph),
                    AllowMultipleEdges = value.AllowMultipleEdges
                };
        }

        /// <summary> Из ДТО в граф </summary>
        public static IGraphBase ConvertBack(GraphDto value)
        {
            if (value.AllowMultipleEdges)
            {
                throw new InvalidOperationException("Данный тип графов не поддерживается.");
            }
            var graph = value.Directed
                            ? (
                                  !value.IsWeighted
                                      ? (IGraphBase)new DirectedGraph()
                                      : (IGraphBase)new DirectedWeightedGraph()
                              )
                            : (IGraphBase)new UndirectedGraph();
            value.Vertices.ForEach(v => graph.AddVertex(VertexToDtoConverter.ConvertBack(v)));
            value.Edges.ForEach(e => graph.AddEdge(EdgeToDtoConverter.ConvertBack(e, graph.Vertices)));

            return graph;
        }
    }
}
