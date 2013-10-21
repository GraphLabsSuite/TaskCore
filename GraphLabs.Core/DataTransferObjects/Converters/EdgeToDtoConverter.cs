using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Core.DataTransferObjects.Converters
{
    /// <summary> Конвертер из ребра в соответствующую ДТО и обратно </summary>
    internal static class EdgeToDtoConverter
    {
        /// <summary> Из ребра в ДТО </summary>
        public static EdgeDto Convert(IEdgeBase value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new EdgeDto
                {
                    Vertex1 = VertexToDtoConverter.Convert(value.Vertex1),
                    Vertex2 = VertexToDtoConverter.Convert(value.Vertex2),
                    Directed = value.Directed,
                    Weight = (value is IWeightedEdge) ? ((IWeightedEdge)value).Weight : (int?)null
                };
        }

        /// <summary> Из ДТО в ребра </summary>
        public static Edge ConvertBack(EdgeDto value, ICollection<IVertex> vertices)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            var vertex1 = vertices.Single(v => v.Name == value.Vertex1.Name);
            var vertex2 = vertices.Single(v => v.Name == value.Vertex2.Name);
            return value.Directed
                       ? (
                            !value.Weight.HasValue
                                ? (Edge)new DirectedEdge((Vertex)vertex1, (Vertex)vertex2)
                                : (Edge)new DirectedWeightedEdge((Vertex)vertex1, (Vertex)vertex2, value.Weight.Value)
                         )
                       : (Edge)new UndirectedEdge((Vertex)vertex1, (Vertex)vertex2);
        }
    }
}
