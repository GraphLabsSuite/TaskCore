using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Tasks.Core.DataTransferObjects.Converters
{
    /// <summary> Конвертер из вершины в соответствующую ДТО и обратно </summary>
    internal static class VertexToDtoConverter
    {
        /// <summary> Из вершины в ДТО </summary>
        public static VertexDto Convert(IVertex value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new VertexDto {Name = (value).Name};
        }

        /// <summary> Из ДТО в вершину </summary>
        public static Vertex ConvertBack(VertexDto value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new Vertex(value.Name);
        }
    }
}
