using System;

namespace GraphLabs.Graphs
{
    /// <summary> Взвешенная дуга </summary>
    public class DirectedWeightedEdge : Edge, IEquatable<DirectedWeightedEdge>
    {
        /// <summary> Сравнение рёбер </summary>
        public bool Equals(DirectedEdge other)
        {
            return Equals((IEdge)other);
        }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public override bool Directed => true;

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public override object Clone()
        {
            return new DirectedWeightedEdge((Vertex)Vertex1.Clone(), (Vertex)Vertex2.Clone(), Weight);
        }

        /// <summary> Сравниваем </summary>
        public bool Equals(DirectedWeightedEdge other)
        {
            return Equals((IEdge)other);
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return $"{Vertex1}--({Vertex2})-->{Weight}";
        }

        /// <summary> Создаёт новое ребро по указанным параметрам</summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        /// <param name="weight">Вес</param>
        public DirectedWeightedEdge(Vertex vertex1, Vertex vertex2, int? weight) :
            base(vertex1, vertex2, weight)
        {
        }
    }
}
