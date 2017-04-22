using GraphLabs.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace GraphLabs.Graphs
{
    /// <summary> Представляет граф в виде строки </summary>
    public class GraphPrinter
    {
        /// <summary> Представляет весь граф в виде строки </summary>
        public string GraphToString (IGraph graph) => $"({VerticesToString(graph)}, {EdgesToString(graph)})";
   
        /// <summary> Представляет вершины графа в виде строки </summary>
        public string VerticesToString (IGraph graph)
        {
            Contract.Requires<ArgumentNullException>(graph != null);
            var verticesListStr = string.Join("; ", graph.Vertices);
            if (verticesListStr.Length != 0)
                return $"{{{verticesListStr}}}";
            else
                return $"{{{'\x00D8'}}}";
        }

        /// <summary> Представляет ребра графа в виде строки </summary>
        public string EdgesToString(IGraph graph)
        {
            Contract.Requires<ArgumentNullException>(graph != null);
            var edgesListStr = string.Join("; ", graph.Edges.Select(e => $"({e.Vertex1.Name}, {e.Vertex2.Name})"));
            if (edgesListStr.Length != 0) 
                return $"{{{edgesListStr}}}";
            else
                return $"{{{'\x00D8'}}}";
        }
    }
}
