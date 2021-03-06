﻿using System.Runtime.Serialization;

namespace GraphLabs.Graphs.DataTransferObjects
{
    /// <summary> Класс для пересылки графа </summary>
    [DataContract]
    public struct GraphDto
    {
        /// <summary> Вершины </summary>
        [DataMember]
        public VertexDto[] Vertices { get; set; }

        /// <summary> Рёбра </summary>
        [DataMember]
        public EdgeDto[] Edges { get; set; }

        /// <summary> Граф ориентированный? </summary>
        [DataMember]
        public bool Directed { get; set; }

        /// <summary> Граф взвешенный? </summary>
        [DataMember]
        public bool IsWeighted { get; set; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        [DataMember]
        public bool AllowMultipleEdges { get; set; }
    }
}
