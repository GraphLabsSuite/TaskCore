using System.Runtime.Serialization;

namespace GraphLabs.Core.DataTransferObjects
{
    /// <summary> Класс для пересылки ребра </summary>
    [DataContract]
    public class EdgeDto
    {
        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        [DataMember]
        public VertexDto Vertex1 { get; set; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        [DataMember]
        public VertexDto Vertex2 { get; set; }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        [DataMember]
        public bool Directed { get; set; }
    }
}
