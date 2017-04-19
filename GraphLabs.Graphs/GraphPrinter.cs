using GraphLabs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphLabs.Graphs
{
    /// <summary> Представляет граф в виде строки </summary>
    public static class GraphPrinter
    {
        /// <summary> Представляет весь граф в виде строки </summary>
        public static string GraphToString (IGraph graph) => $"({VerticesToString(graph)},{EdgesToString(graph)})";
   
        /// <summary> Представляет вершины графа в виде строки </summary>
        public static string VerticesToString (IGraph graph)
        {
            var verticesListStr = string.Join("; ", graph.Vertices);
            return $"{{verticesListStr}}";
        }

        /// <summary> Представляет ребра графа в виде строки </summary>
        public static string EdgesToString(IGraph graph)
        {
            var edgesListStr = string.Join("; ", graph.Edges.Select(e => $"({e.Vertex1.Name}, {e.Vertex2.Name})"));
            return $"{{edgesListStr}}";
        }
    }
}
