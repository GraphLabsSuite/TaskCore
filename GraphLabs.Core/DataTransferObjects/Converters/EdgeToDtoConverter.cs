using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Tasks.Core.DataTransferObjects.Converters
{
    /// <summary> Конвертер из ребра в соответствующую ДТО и обратно </summary>
    internal static class EdgeToDtoConverter
    {
        /// <summary> Из ребра в ДТО </summary>
        public static EdgeDto Convert(IEdge value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new EdgeDto
                {
                    Vertex1 = VertexToDtoConverter.Convert(value.Vertex1),
                    Vertex2 = VertexToDtoConverter.Convert(value.Vertex2),
                    Directed = value.Directed
                };
        }

        /// <summary> Из ДТО в ребра </summary>
        public static Edge ConvertBack(EdgeDto value, IEnumerable<IVertex> vertices)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            var vertex1 = vertices.Single(v => v.Name == value.Vertex1.Name);
            var vertex2 = vertices.Single(v => v.Name == value.Vertex2.Name);
            return value.Directed
                       ? (Edge)new DirectedEdge(vertex1, vertex2)
                       : (Edge)new UndirectedEdge(vertex1, vertex2);
        }
    }
}
